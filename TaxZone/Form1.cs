using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using TaxZone.DTO;

namespace TaxZone
{
    public partial class Form1 : Form
    {

        List<TaxContext> contextos = new();
        private readonly CookieRenewService _cookieRenew = new();
        private bool _formCarregado = false;

        public Form1()
        {
            InitializeComponent();
            ConfigManager.Load();

            NotificationService.QtdNotasProgressChanged += AtualizarStatusQtdNotas;
            NotificationService.StatusTaxChanged += AtualizarStatusTax;

            tb_usuario_banco_far.Text = ConfigManager.DatabaseUserFar;
            tb_senha_banco_far.Text = ConfigManager.DatabasePasswordFar;
            tb_usuario_banco_msa.Text = ConfigManager.DatabaseUserMsa;
            tb_senha_banco_msa.Text = ConfigManager.DatabasePasswordMsa;
            tb_usuario_tax.Text = ConfigManager.UsuarioTax;
            tb_senha_tax.Text = ConfigManager.SenhaTax;

            //Data source dos combobox
            cb_banco.DataSource = Empresa.ListaEmpresas;
            cb_empresa.DataSource = Empresa.ListaEmpresas; ;
            //cb_empresa_tax_api.DataSource = Empresa.ListaEmpresas; ;

            cb_empresa_qtd_notas.Items.Add("TODAS");
            cb_empresa_qtd_notas.Items.AddRange(Empresa.ListaEmpresas.ToArray());
            cb_empresa_qtd_notas.SelectedIndex = 0;

            cb_local_qtd_notas.SelectedIndex = 0;
            cb_status.SelectedIndex = 0;

            //se não fizer isso fica vinculado com os combo box e se alterar la altera aqui tambem
            //lbox_empresas.DataSource = new List<string>(Empresa.ListaEmpresas);
            lbox_empresas.DataSource = new List<string>(Empresa.ListaEmpresas);


            //Preenchimento das datas
            DateTime referenciaAnterior = DateTime.Now.AddMonths(-1);

            tb_ano.Text = referenciaAnterior.Year.ToString();
            tb_mes.Text = referenciaAnterior.Month.ToString();

            //Sempre o mês fechado
            dtp_periodo_ini_qtd_notas.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, 01);
            dtp_periodo_fin_qtd_notas.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, DateTime.DaysInMonth(referenciaAnterior.Year, referenciaAnterior.Month));

            dtp_inicio_comparativo_notas.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, 01);
            dtp_fim_comparativo_notas.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, DateTime.DaysInMonth(referenciaAnterior.Year, referenciaAnterior.Month));

            dtp_tax_data_inicio.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, 01);
            dtp_tax_data_fim.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, DateTime.DaysInMonth(referenciaAnterior.Year, referenciaAnterior.Month));


            tb_referenciaBuracoNota.Text = $"{referenciaAnterior.Month}_{referenciaAnterior.Year}";

            //Configução das variáveis globais utilizadas por muitas funções
            Globais.gerarArquivo = ckb_gerar_arquivo.Checked;
            Globais.fracionarValores = ckb_fracionar_valores.Checked;
            Globais.mesAberto = ckb_mes_aberto.Checked;

            rb_popular_tabela.Checked = true;



            _formCarregado = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #region BOTOES

        private void bt_diferenca_capa_item_Click(object sender, EventArgs e)
            => FuncoesTax.DiferencaItens(ckb_gerar_arquivo.Checked, ckb_fracionar_valores.Checked);


        private void bt_buraco_nota_Click(object sender, EventArgs e)
            => FuncoesTax.BuracoDeNota(ckb_buraco_notas_hardcore.Checked, tb_referenciaBuracoNota.Text);

        private void bt_notas_canceladas_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_ano.Text) || string.IsNullOrEmpty(tb_mes.Text))
            {
                MessageBox.Show("Informe o Ano/Mês!");
                return;
            }

            string mes = int.Parse(tb_mes.Text).ToString("00");
            string ano = tb_ano.Text;

            FuncoesTax.GetDiferencaCanceladas(ano, mes, cb_banco.Text, ckb_mes_aberto.Checked, ckb_gerar_arquivo.Checked, ckb_fracionar_valores.Checked);
        }

        private void bt_pendencia_processamento_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cb_pendencia_processamento.Text) || string.IsNullOrEmpty(cb_empresa.Text))
            {
                MessageBox.Show("Obrigatório informar tipo de pendencia e empresa");
                return;
            }

            FuncoesTax.PendenciaProcessamento(cb_pendencia_processamento.Text, cb_empresa.Text, ckb_arq_temporario.Checked);
        }

        private void bt_obter_icms_sifar_Click(object sender, EventArgs e)
        {
            BancoDTO banco = Empresa.GetBancoFar(cb_banco.Text);
            if (banco is null)
            {
                MessageBox.Show("Informe o banco!");
                return;
            }
            if (string.IsNullOrEmpty(tb_ano.Text) || string.IsNullOrEmpty(tb_mes.Text))
            {
                MessageBox.Show("Informe o Ano/Mês!");
                return;
            }

            string mes = int.Parse(tb_mes.Text).ToString("00");
            string ano = tb_ano.Text;

            string query = string.Format(
                                Queries.queryIcmsSifar,
                                mes,
                                ano
                            );

            DataTable dt_icms = DataAccess.ExecuteQuery(tb_usuario_banco_far.Text, tb_senha_banco_far.Text, banco.database, banco.owner, query);

            Util.MostrarDataTable(dt_icms);

        }

        private async void bt_qtd_notas_Click(object sender, EventArgs e)
        {
            DateTime periodoIni = dtp_periodo_ini_qtd_notas.Value;
            DateTime periodoFin = dtp_periodo_fin_qtd_notas.Value;
            string empresa = cb_empresa_qtd_notas.Text;
            bool mostrarNaTela = rb_mostrar_na_tela.Checked;
            bool arquivoTemporario = rb_arquivo_temp.Checked;
            bool popularTabela = rb_popular_tabela.Checked;
            string local = cb_local_qtd_notas.Text;
            bool incluidasHoje = ckb_incluidas_hoje.Checked;
            bool mesAberto = ckb_mes_aberto.Checked;

            if (popularTabela)
            {
                await FuncoesTax.GetQuantidadeNotas(periodoIni, periodoFin, empresa, mostrarNaTela, arquivoTemporario, popularTabela, local, incluidasHoje, mesAberto);
                AtualizarComparativoNotas();
            }
            else
            {
                FuncoesTax.GetQuantidadeNotas(periodoIni, periodoFin, empresa, mostrarNaTela, arquivoTemporario, popularTabela, local, incluidasHoje, mesAberto);
            }



        }

        private void bt_pessoa_fisica_juridica_Click(object sender, EventArgs e)
            => FuncoesTax.ImportarPessoaFisicaJuridica(ckb_gerar_arquivo.Checked, ckb_fracionar_valores.Checked, ckb_codFisJur.Checked);

        private void bt_produtos_taxas_Click(object sender, EventArgs e)
            => FuncoesTax.ImportarProdutos();


        private async void bt_tax_automation_Click(object sender, EventArgs e)
        {
            List<string> empresasSelecionadas = lbox_empresas.SelectedItems.Cast<string>().ToList();

            int total = empresasSelecionadas.Count;
            int concluidas = 0;

            NotificationService.AtualizarStatusTax(
                $"Programando JOB 0/{total}",
                0);

            var tasks = empresasSelecionadas.Select(async empresa =>
            {

                TaxApiResponse response = await ApiTax.ProgramarTaxAutomation(empresa);
                int qtd = Interlocked.Increment(ref concluidas);

                if (!response.Success)
                    MessageBox.Show($"Falha ao programar relatório para a empresa {empresa}: {response.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                NotificationService.AtualizarStatusTax(
                    $"Programando JOB {qtd}/{total}",
                    (int)(qtd * 100.0 / total));

                return response;

            });

            TaxApiResponse[] resultados = await Task.WhenAll(tasks);

            string empresasSucesso = "";
            string empresasComFalha = "";
            foreach (var resultado in resultados)
            {
                if (resultado.Success)
                    empresasSucesso += resultado.Empresa + "/";
                else 
                    empresasComFalha += resultado.Empresa + "/";
            }

            MessageBox.Show($"Sucesso: {empresasSucesso}\nErro: {empresasComFalha}", "Status Tax Automation", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

        private async void bt_status_tax_automation_Click(object sender, EventArgs e)
        {

            List<string> empresasSelecionadas = lbox_empresas.SelectedItems.Cast<string>().ToList();

            int total = empresasSelecionadas.Count;
            int concluidas = 0;

            NotificationService.AtualizarStatusTax(
                $"Consultando status 0/{total}",
                0);

            var tasks = empresasSelecionadas.Select(async empresa =>
            {

                TaxApiResponse response = await ApiTax.VerificarStatusExecucao(empresa);
                int qtd = Interlocked.Increment(ref concluidas);

                if (!response.Success)
                    MessageBox.Show($"Falha ao programar relatório para a empresa {empresa}: {response.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);


                NotificationService.AtualizarStatusTax(
                    $"Consultando status {qtd}/{total}",
                    (int)(qtd * 100.0 / total));


                return response;

            });

            TaxApiResponse[] resultados = await Task.WhenAll(tasks);

            string empresasSucesso = "";
            string empresasPendentes = "";
            string empresasComFalha = "";
            foreach (var resultado in resultados)
            {
                if (resultado.Message.Contains("sucesso"))
                    empresasSucesso += resultado.Empresa + "/";
                else if (resultado.Message.Contains("não foi concluída"))
                    empresasPendentes += resultado.Empresa + "/";
                else
                    empresasComFalha += resultado.Empresa + "/";
            }   

            MessageBox.Show($"Cooncluídos: {empresasSucesso}\nPendentes: {empresasPendentes}\nErro: {empresasComFalha}", "Status Tax Automation", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }


        private async void bt_executar_relatorio_Click(object sender, EventArgs e)
        {
            ApiTax.param_empresa = "*";
            ApiTax.param_estab = "*";
            ApiTax.data_inicio = dtp_tax_data_inicio.Value.ToString("ddMMyyyy000000");
            ApiTax.data_fim = dtp_tax_data_fim.Value.ToString("ddMMyyyy000000");
            ApiTax.buraco_nota = ckb_buraco_notas.Checked ? "S" : "N";
            ApiTax.diferenca_capa_item = ckb_diferenca_capa_item.Checked ? "S" : "N";
            ApiTax.icms_resumido = ckb_icms_resumido.Checked ? "S" : "N";
            ApiTax.notas_sem_item = ckb_notas_sem_item.Checked ? "S" : "N";
            ApiTax.qtd_itens = ckb_qtd_itens.Checked ? "S" : "N";
            ApiTax.qtd_notas = ckb_qtd_notas.Checked ? "S" : "N";
            ApiTax.qtd_canceladas = ckb_qtd_canceladas.Checked ? "S" : "N";
            ApiTax.extracao_canceladas = ckb_extracao_canceladas.Checked ? "S" : "N";


            List<string> empresasSelecionadas = lbox_empresas.SelectedItems.Cast<string>().ToList();

            TaxContext context;
            int total = empresasSelecionadas.Count;
            int concluidas = 0;

            var tasks = empresasSelecionadas.Select(async empresa =>
            {
                TaxContext context = GetContext(empresa);

                TaxApiResponse resposta = await ApiTax.ProgramarRelatorio(empresa, context);
                int qtd = Interlocked.Increment(ref concluidas);

                if (!resposta.Success)
                    MessageBox.Show($"Falha ao programar relatório para a empresa {empresa}: {resposta.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

            });

            await Task.WhenAll(tasks);

        }



        private void bt_relatorios_Click(object sender, EventArgs e)
        {
            List<string> empresasSelecionadas = lbox_empresas.SelectedItems.Cast<string>().ToList();
            if (lbox_empresas.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selecione pelo menos uma empresa para visualizar os relatórios executados.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (lbox_empresas.SelectedItems.Count > 1)
            {
                MessageBox.Show("Selecione apenas uma empresa para visualizar os relatórios executados.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string empresa = lbox_empresas.SelectedItem!.ToString()!;

            F_Relatorios_Executados form = new(GetContext(empresa));
            form.Show();
            form.BuscarDados(empresa);
        }

        private async void bt_login_Click(object sender, EventArgs e)
        {

            string cookie = await ApiTax.GetCookie(tb_usuario_tax.Text, tb_senha_tax.Text);
            tb_cookie.Text = cookie;

            ckb_renew_task.Checked = true;
        }


        #endregion

        #region MUDANCA_DE_VALORES

        private void tb_usuario_banco_TextChanged(object sender, EventArgs e)
        {
            ConfigManager.DatabaseUserFar = tb_usuario_banco_far.Text;
            ConfigManager.Save();
        }

        private void tb_senha_banco_TextChanged(object sender, EventArgs e)
        {
            ConfigManager.DatabasePasswordFar = tb_senha_banco_far.Text;
            ConfigManager.Save();
        }

        private void tb_usuario_banco_msa_TextChanged(object sender, EventArgs e)
        {
            ConfigManager.DatabaseUserMsa = tb_usuario_banco_msa.Text;
            ConfigManager.Save();
        }

        private void tb_senha_banco_msa_TextChanged(object sender, EventArgs e)
        {
            ConfigManager.DatabasePasswordMsa = tb_senha_banco_msa.Text;
            ConfigManager.Save();
        }

        private void ckb_buraco_notas_hardcore_CheckedChanged(object sender, EventArgs e)
            => tb_referenciaBuracoNota.Visible = ckb_buraco_notas_hardcore.Checked;

        private void ckb_arq_temporario_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_arq_temporario.Checked)
            {
                string path = Config.PathArquivoTemporario + "\\scriptTemporario.csv";
                if (!Directory.Exists(Config.PathArquivoTemporario))
                    Directory.CreateDirectory(Config.PathArquivoTemporario);


                if (!File.Exists(path))
                    File.Create(path).Close();

                Process.Start("notepad.exe", path);
            }
        }

        private void ckb_gerar_arquivo_CheckedChanged(object sender, EventArgs e)
            => Globais.gerarArquivo = ckb_gerar_arquivo.Checked;


        private void ckb_fracionar_valores_CheckedChanged(object sender, EventArgs e)
            => Globais.fracionarValores = ckb_fracionar_valores.Checked;


        private void ckb_mes_aberto_CheckedChanged(object sender, EventArgs e)
        {
            Globais.mesAberto = ckb_mes_aberto.Checked;
            if (ckb_mes_aberto.Checked)
            {
                dtp_periodo_ini_qtd_notas.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
                dtp_periodo_fin_qtd_notas.Value = DateTime.Now.AddDays(-1);

                dtp_tax_data_inicio.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
                dtp_tax_data_fim.Value = DateTime.Now.AddDays(-1);

                dtp_inicio_comparativo_notas.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
                dtp_fim_comparativo_notas.Value = DateTime.Now.AddDays(-1);
            }
            else
            {
                DateTime referenciaAnterior = DateTime.Now.AddMonths(-1);
                dtp_periodo_ini_qtd_notas.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, 01);
                dtp_periodo_fin_qtd_notas.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, DateTime.DaysInMonth(referenciaAnterior.Year, referenciaAnterior.Month));

                dtp_tax_data_inicio.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, 01);
                dtp_tax_data_fim.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, DateTime.DaysInMonth(referenciaAnterior.Year, referenciaAnterior.Month));

                dtp_inicio_comparativo_notas.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, 01);
                dtp_fim_comparativo_notas.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, DateTime.DaysInMonth(referenciaAnterior.Year, referenciaAnterior.Month));

            }
        }

        private void tb_cookie_TextChanged(object sender, EventArgs e)
        {
            ConfigManager.Cookie = tb_cookie.Text;
            //Renova os contextos para a nova sessão
            contextos = new();
        }


        private void tb_usuario_tax_TextChanged(object sender, EventArgs e)
        {
            ConfigManager.UsuarioTax = tb_usuario_tax.Text;
            ConfigManager.Save();
        }

        private void tb_senha_tax_TextChanged(object sender, EventArgs e)
        {
            ConfigManager.SenhaTax = tb_senha_tax.Text;
            ConfigManager.Save();
        }

        private void cb_local_qtd_notas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_local_qtd_notas.SelectedIndex == 1) ckb_incluidas_hoje.Visible = true;
            else ckb_incluidas_hoje.Visible = false;
        }

        /*
        private void cb_empresa_tax_api_SelectedIndexChanged(object sender, EventArgs e)
        {
            var listEstab = Empresa.GetEstabelecimentos(cb_empresa_tax_api.Text);
            var datasource = new List<string> { "TODOS" };
            datasource.AddRange(listEstab.Select(x => x.ToString()));
            //cb_estab.DataSource = datasource;
        }
        */
        private async void ckb_renew_task_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_renew_task.Checked) _cookieRenew.Start();
            else await _cookieRenew.StopAsync();
        }

        #endregion


        private TaxContext GetContext(string empresa)
        {
            TaxContext context = contextos.FirstOrDefault(x => x.Empresa == empresa);

            if (context == null)
            {
                context = new TaxContext
                {
                    Empresa = empresa
                };

                contextos.Add(context);
            }
            return context;
        }

        private void AtualizarStatusQtdNotas(string texto, int progresso)
        {

            if (InvokeRequired)
            {
                Invoke(() => AtualizarStatusQtdNotas(texto, progresso));
                return;
            }

            lbl_status_qtd_notas.Text = texto;
            progress_bar_qtd_notas.Value = progresso;

            lbl_status_qtd_notas.Visible = progresso > 0 && progresso < 100;
            progress_bar_qtd_notas.Visible = progresso > 0 && progresso < 100;

        }

        private void AtualizarStatusTax(string texto, int progresso)
        {

            if (InvokeRequired)
            {
                Invoke(() => AtualizarStatusTax(texto, progresso));
                return;
            }

            lbl_status_tax.Text = texto;
            progress_bar_tax.Value = progresso;

            lbl_status_tax.Visible = progresso > 0 && progresso < 100;
            progress_bar_tax.Visible = progresso > 0 && progresso < 100;

        }

        private void dgv_comparativo_notas_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dgv_comparativo_notas.Rows[e.RowIndex].Cells["id"].Value);

            var value_sifar = dgv_comparativo_notas.Rows[e.RowIndex].Cells["QTD_SIFAR"].Value;
            if (value_sifar == DBNull.Value)
                dgv_comparativo_notas.Rows[e.RowIndex].Cells["QTD_SIFAR"].Value = 0;
            int qtdSifar = Convert.ToInt32(dgv_comparativo_notas.Rows[e.RowIndex].Cells["QTD_SIFAR"].Value);

            var value_tax = dgv_comparativo_notas.Rows[e.RowIndex].Cells["QTD_TAX"].Value;
            if (value_tax == DBNull.Value)
                dgv_comparativo_notas.Rows[e.RowIndex].Cells["QTD_TAX"].Value = 0;
            int qtdTax = Convert.ToInt32(dgv_comparativo_notas.Rows[e.RowIndex].Cells["QTD_TAX"].Value);



            Banco.AtualizarRegistro(qtdSifar, qtdTax, id);

        }

        private void lbox_empresas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_formCarregado) return;
            AtualizarComparativoNotas();
        }

        private void AtualizarComparativoNotas()
        {
            int ano = dtp_inicio_comparativo_notas.Value.Year;
            int mes = dtp_fim_comparativo_notas.Value.Month;
            List<string> empresas_selecionadas = lbox_empresas.SelectedItems.Cast<string>().ToList();
            var dt = Banco.Listar(ano, mes, empresas_selecionadas);
            if (dt.Rows.Count == 0)
            {
                var response = MessageBox.Show("Nenhum registro encontrado para o ano/mês informado. Deseja inserir registros?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (response == DialogResult.Yes)
                    InserirReferenciaBanco();

                dt = Banco.Listar(ano, mes, empresas_selecionadas);

            }

            dgv_comparativo_notas.DataSource = dt;

            dgv_comparativo_notas.Columns["id"].Visible = false;
            dgv_comparativo_notas.Columns["ANO"].Visible = false;
            dgv_comparativo_notas.Columns["MES"].Visible = false;
            foreach (DataGridViewColumn coluna in dgv_comparativo_notas.Columns)
            {
                coluna.ReadOnly = true;
            }

            dgv_comparativo_notas.Columns["QTD_SIFAR"].ReadOnly = false;
            dgv_comparativo_notas.Columns["QTD_TAX"].ReadOnly = false;

            if (!dt.Columns.Contains("DIFERENÇA"))
            {
                dt.Columns.Add("DIFERENÇA", typeof(int), "qtd_sifar - qtd_tax");
            }

            // Move a coluna diferença para antes do status
            dgv_comparativo_notas.Columns["DIFERENÇA"].DisplayIndex =
                dgv_comparativo_notas.Columns["STATUS"].DisplayIndex;
        }

        private void bt_alterar_status_Click(object sender, EventArgs e)
        {

            string status = cb_status.Text;

            foreach (DataGridViewRow row in dgv_comparativo_notas.Rows)
            {
                int id = Convert.ToInt32(row.Cells["id"].Value);
                Banco.AtualizarStatus(status, id);
            }
            AtualizarComparativoNotas();

        }

        private void InserirReferenciaBanco()
        {
            int ano = dtp_inicio_comparativo_notas.Value.Year;
            int mes = dtp_fim_comparativo_notas.Value.Month;
            foreach (var empresa in Empresa.ListaEmpresas)
            {
                foreach (int estabelecimento in Empresa.GetEstabelecimentos(empresa))
                {
                    Banco.InserirRegistro(ano, mes, empresa, estabelecimento, "NOTAS");
                    Banco.InserirRegistro(ano, mes, empresa, estabelecimento, "ITENS");
                    Banco.InserirRegistro(ano, mes, empresa, estabelecimento, "CANC");
                    Banco.InserirRegistro(ano, mes, empresa, estabelecimento, "ICMS");
                }
            }
        }

        private void dgv_comparativo_notas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == DBNull.Value) return;

            string coluna = dgv_comparativo_notas.Columns[e.ColumnIndex].Name;

            // Coluna STATUS
            if (coluna == "STATUS")
            {
                if (e.Value?.ToString() == "LIBERADO")
                {
                    e.CellStyle.BackColor = Color.LightGreen;
                    e.CellStyle.Font = new Font(dgv_comparativo_notas.Font, FontStyle.Bold);
                }
                else if (e.Value?.ToString() == "EM ANDAMENTO")
                {
                    e.CellStyle.BackColor = Color.LightYellow;
                    e.CellStyle.Font = new Font(dgv_comparativo_notas.Font, FontStyle.Bold);
                }
            }

            //Coluna DIFERENÇA
            if (coluna == "DIFERENÇA")
            {

                var status = dgv_comparativo_notas.Rows[e.RowIndex]
                    .Cells["STATUS"].Value?.ToString();

                if (status == "LIBERADO")
                {
                    e.CellStyle.BackColor = Color.LightGreen;
                    e.CellStyle.Font = new Font(dgv_comparativo_notas.Font, FontStyle.Bold);
                }
                else
                {
                    if (e.Value != null && Convert.ToInt32(e.Value) != 0)
                    {
                        e.CellStyle.BackColor = Color.Salmon;
                        e.CellStyle.Font = new Font(dgv_comparativo_notas.Font, FontStyle.Bold);
                    }
                    else
                    {
                        e.CellStyle.BackColor = Color.LightGreen;
                        e.CellStyle.Font = new Font(dgv_comparativo_notas.Font, FontStyle.Bold);

                    }
                }
            }
        }

        private void bt_atualizar_comparacao_Click(object sender, EventArgs e)
        {
            AtualizarComparativoNotas();
        }

        private void ckb_always_on_top_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = ckb_always_on_top.Checked;
        }

        private async void bt_atualizar_valores_tax_Click(object sender, EventArgs e)
        {
            List<string> empresasSelecionadas = lbox_empresas.SelectedItems.Cast<string>().ToList();

            TaxContext context;

            ApiTax.param_empresa = "*";
            ApiTax.param_estab = "*";
            ApiTax.data_inicio = dtp_inicio_comparativo_notas.Value.ToString("ddMMyyyy000000");
            ApiTax.data_fim = dtp_fim_comparativo_notas.Value.ToString("ddMMyyyy000000");
            ApiTax.buraco_nota = "N";
            ApiTax.diferenca_capa_item = "N";
            ApiTax.icms_resumido = "S";
            ApiTax.notas_sem_item = "N";
            ApiTax.qtd_itens = "S";
            ApiTax.qtd_notas = "S";
            ApiTax.qtd_canceladas = "S";
            ApiTax.extracao_canceladas = "N";


            int total = empresasSelecionadas.Count;
            int concluidas = 0;

            /*
            NotificationService.AtualizarStatusTax(
                $"Programando relatório 0/{total}",
                0);
            */

            var tasks = empresasSelecionadas.Select(async empresa =>
            {
                TaxContext context = GetContext(empresa);

                TaxApiResponse response = await ApiTax.ProgramarRelatorio(empresa, context);
                int qtd = Interlocked.Increment(ref concluidas);

                if (!response.Success)
                    MessageBox.Show($"Falha ao programar relatório para a empresa {empresa}: {response.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                /*
                NotificationService.AtualizarStatusTax(
                    $"Programando relatório {qtd}/{total}",
                    (int)(qtd * 100.0 / total));
                */
                return response.Success;
            });

            bool[] resultados = await Task.WhenAll(tasks);

            if (resultados.Any(r => !r))
            {
                //var response = MessageBox.Show("Houveram falhas ao programar alguns relatórios, deseja continuar?", "Erro", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                //if (response == DialogResult.Cancel)
                return;
            }

            await BuscarRelatoriosAsync(empresasSelecionadas);

            AtualizarStatusTax(
                $"Atualizando valores",
                0);

            int ano = dtp_inicio_comparativo_notas.Value.Year;
            int mes = dtp_fim_comparativo_notas.Value.Month;

            using var con = Banco.Conexao();
            con.Open();
            using var transaction = con.BeginTransaction();

            foreach (string arquivo in Directory.GetFiles(Config.PathArquivoTemporario, "*.pdf"))
            {
                string nome = Path.GetFileNameWithoutExtension(arquivo);

                int indice = nome.IndexOf('_');
                if (indice < 0)
                    continue;

                string tipo = nome.Substring(0, indice);
                string empresa = nome.Substring(indice + 1);

                using PdfReader reader = new PdfReader(arquivo);
                using PdfDocument pdf = new PdfDocument(reader);

                for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
                {
                    string texto = PdfTextExtractor.GetTextFromPage(pdf.GetPage(i));

                    foreach (string linha in texto.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        int estabelecimento = 0;
                        decimal quantidade = 0;
                        if (tipo == "ICMS")
                        {
                            Match match = Regex.Match(
                                linha.Trim(),
                                @"^\d+\s*\|\s*(\d+)\s*\|.*\|\s*([\d.,]+)$");

                            if (!match.Success)
                                continue;

                            estabelecimento = int.Parse(match.Groups[1].Value);

                            quantidade = decimal.Parse(
                                match.Groups[2].Value,
                                CultureInfo.GetCultureInfo("pt-BR"));

                        }
                        else
                        {
                            Match match = Regex.Match(
                                linha.Trim(),
                                @"^(\d+)\s*\|\s*(\d+)$");

                            if (!match.Success)
                                continue;

                            estabelecimento = int.Parse(match.Groups[1].Value);
                            quantidade = int.Parse(match.Groups[2].Value);


                        }
                        Thread.Sleep(1000);
                        Banco.AtualizarQtdTax(ano, mes, empresa, tipo, estabelecimento, quantidade, con, transaction);

                    }
                }
            }

            AtualizarStatusTax(
                $"Concluido",
                100);

            transaction.Commit();
            AtualizarComparativoNotas();
        }

        private async Task BuscarRelatoriosAsync(List<string> empresas)
        {
            int total = empresas.Count;
            int concluidas = 0;

            AtualizarStatusTax(
                $"Aguardando conclusão dos relatórios. Concluído 0/{total}",
                0);

            var tasks = empresas.Select(async empresa =>
            {
                bool finalizado = false;

                while (!finalizado)
                {
                    TaxContext context = GetContext(empresa);

                    finalizado = await ApiTax.VerificaUltimoRelatorioConcluido(
                        empresa,
                        context,
                        string.IsNullOrEmpty(context.StorageId));

                    try
                    {
                        if (finalizado)
                        {
                            await ApiTax.BaixarRelatorio(
                                context,
                                1,
                                Config.PathArquivoTemporario);
                        }
                        else
                        {
                            await Task.Delay(5000); // Nunca use Thread.Sleep em código async
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"Erro ao baixar relatório da empresa {empresa}: {ex.Message}",
                            "Erro",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        finalizado = true;
                    }
                }

                int qtd = Interlocked.Increment(ref concluidas);
                int porcentagem = (int)((double)qtd / total * 100);

                AtualizarStatusTax(
                    $"Aguardando conclusão dos relatórios. {qtd}/{total}",
                    porcentagem);
            });

            await Task.WhenAll(tasks);

            AtualizarStatusTax(
                "Busca concluída",
                100);

            MessageBox.Show(
                "Dados do TAX atualizados com sucesso.",
                "Informação",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
