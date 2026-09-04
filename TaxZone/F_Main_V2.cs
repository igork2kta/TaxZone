using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using TaxZone.DTO;

namespace TaxZone
{
    public partial class F_Main_V2 : Form
    {
        private readonly CookieRenewService _cookieRenew = new();
        //List<TaxContext> contextos = new();
        bool _formCarregado = false;

        public F_Main_V2()
        {
            InitializeComponent();

            cb_local_qtd_notas.SelectedIndex = 0;
            cb_status.SelectedIndex = 0;
            cb_ferramentas.SelectedIndex = 0;

            //se não fizer isso fica vinculado com os combo box e se alterar la altera aqui tambem
            //lbox_empresas.DataSource = new List<string>(Empresa.ListaEmpresas);
            lbox_empresas.DataSource = new List<string>(Empresa.ListaEmpresas);

            //Preenchimento das datas
            DateTime referenciaAnterior = DateTime.Now.AddMonths(-1);

            //Sempre o mês fechado
            dtp_periodo_inicio.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, 01);
            dtp_periodo_fim.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, DateTime.DaysInMonth(referenciaAnterior.Year, referenciaAnterior.Month));

            //Configução das variáveis globais utilizadas por muitas funções
            Globais.gerarArquivo = ckb_gerar_arquivo.Checked;
            Globais.fracionarValores = ckb_fracionar_valores.Checked;
            Globais.mesAberto = ckb_mes_aberto.Checked;

            dgv_comparativo_notas.EnableHeadersVisualStyles = false;
            // 1. Define o estilo de borda do cabeçalho como simples
            dgv_comparativo_notas.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            // 2. Modifica a cor das divisórias superiores, esquerdas e direitas para "Inset" ou "Outset" 
            // e define a cor da linha de grade (inferior/direita) para a mesma cor suave do grid
            dgv_comparativo_notas.AdvancedColumnHeadersBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
            dgv_comparativo_notas.AdvancedColumnHeadersBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.Single;
            dgv_comparativo_notas.GridColor = Color.FromArgb(224, 224, 224); // Cinza bem suave

            _formCarregado = true;
            AtualizarComparativoNotas();

        }

        private void F_Main_V2_Load(object sender, EventArgs e)
        {

        }

        #region MUDANÇA DE VALORES

        private void credenciaisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f_credenciais = new F_Credenciais().ShowDialog();
        }

        private void ckb_mes_aberto_CheckedChanged(object sender, EventArgs e)
        {
            Globais.mesAberto = ckb_mes_aberto.Checked;
            if (ckb_mes_aberto.Checked)
            {
                dtp_periodo_inicio.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
                dtp_periodo_fim.Value = DateTime.Now.AddDays(-1);
            }
            else
            {
                DateTime referenciaAnterior = DateTime.Now.AddMonths(-1);
                dtp_periodo_inicio.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, 01);
                dtp_periodo_fim.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, DateTime.DaysInMonth(referenciaAnterior.Year, referenciaAnterior.Month));

            }
        }

        private void ckb_gerar_arquivo_CheckedChanged(object sender, EventArgs e)
            => Globais.gerarArquivo = ckb_gerar_arquivo.Checked;

        private void ckb_fracionar_valores_CheckedChanged(object sender, EventArgs e)
                => Globais.fracionarValores = ckb_fracionar_valores.Checked;

        private async void bt_login_Click(object sender, EventArgs e)
        {

            string cookie = await ApiTax.GetCookie(Config.UsuarioTax, Config.SenhaTax);
            tb_cookie.Text = cookie;

            ckb_renew_task.Checked = true;
        }

        private async void ckb_renew_task_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_renew_task.Checked) _cookieRenew.Start();
            else await _cookieRenew.StopAsync();
        }

        private void tb_cookie_TextChanged(object sender, EventArgs e)
        {
            Config.Cookie = tb_cookie.Text;
            //Renova os contextos para a nova sessão
            ApiTax.ResetContext();
        }


        private void dtp_periodo_inicio_ValueChanged(object sender, EventArgs e)
        {
            if (!_formCarregado)
                return;

            AtualizarComparativoNotas();
        }

        private void dtp_periodo_fim_ValueChanged(object sender, EventArgs e)
        {
            if (!_formCarregado)
                return;

            AtualizarComparativoNotas();
        }

        #endregion

        private async void bt_executar_relatorio_Click(object sender, EventArgs e)
        {

            var parametros = new ParametrosProcessosCustomizados(
                "*",
                "*",
                dtp_periodo_inicio.Value,
                dtp_periodo_fim.Value,
                ckb_buraco_notas.Checked,
                ckb_diferenca_capa_item.Checked,
                ckb_icms_resumido.Checked,
                ckb_notas_sem_item.Checked,
                ckb_qtd_itens.Checked,
                ckb_qtd_notas.Checked,
                ckb_qtd_canceladas.Checked,
                ckb_extracao_canceladas.Checked
                );

            List<string> empresasSelecionadas = lbox_empresas.SelectedItems.Cast<string>().ToList();

            int total = empresasSelecionadas.Count, concluidas = 0;

            var tasks = empresasSelecionadas.Select(async empresa =>
            {
                using var status = new StatusTask(statusStrip, $"Empresa {empresa}");
                var progresso = new Progress<Progresso>(status.Atualizar);

                TaxContext context = ApiTax.GetContext(empresa);

                TaxApiResponse resposta = await ApiTax.ProgramarRelatorio(context, parametros, progresso);
                int qtd = Interlocked.Increment(ref concluidas);

                if (!resposta.Success)
                    MessageBox.Show($"Falha ao programar relatório para a empresa {empresa}: {resposta.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

            });

            await Task.WhenAll(tasks);

            using var status = new StatusTask(statusStrip, "");
            IProgress<Progresso> progresso = new Progress<Progresso>(status.Atualizar);

            await BuscarRelatoriosAsync(empresasSelecionadas, progresso);

            MessageBox.Show($"Todos os relatórios programados foram concluídos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

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

            F_Relatorios_Executados form = new(ApiTax.GetContext(empresa));
            form.Show();
            form.BuscarDados(empresa);
        }

        private async void bt_executar_job_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> empresasSelecionadas = lbox_empresas.SelectedItems.Cast<string>().ToList();

                TaxContext context;
                int total = empresasSelecionadas.Count, concluidas = 0;

                var tasks = empresasSelecionadas.Select(async empresa =>
                {
                    using var status = new StatusTask(statusStrip, $"Empresa {empresa}");
                    var progresso = new Progress<Progresso>(status.Atualizar);

                    TaxApiResponse resposta = await ApiTax.ProgramarJob(empresa, null, progresso);
                    int qtd = Interlocked.Increment(ref concluidas);

                    if (!resposta.Success)
                        MessageBox.Show($"Falha ao programar relatório para a empresa {empresa}: {resposta.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return resposta;

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
                string mensagem = "";

                if (!string.IsNullOrEmpty(empresasSucesso)) mensagem += $"Sucesso: {empresasSucesso[..^1]}";
                if (!string.IsNullOrEmpty(empresasComFalha)) mensagem += $"\nErro: {empresasComFalha[..^1]}";

                MessageBox.Show(mensagem, "Programação JOB", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao executar JOB: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        private void lbox_empresas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_formCarregado) return;
            AtualizarComparativoNotas();
        }

        private void AtualizarComparativoNotas()
        {
            int ano = dtp_periodo_inicio.Value.Year;
            int mes = dtp_periodo_inicio.Value.Month;
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

        private void InserirReferenciaBanco()
        {
            int ano = dtp_periodo_inicio.Value.Year;
            int mes = dtp_periodo_inicio.Value.Month;
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

        private void bt_atualizar_comparacao_Click(object sender, EventArgs e)
        {
            AtualizarComparativoNotas();
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

        private void dgv_comparativo_notas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == DBNull.Value) return;

            string coluna = dgv_comparativo_notas.Columns[e.ColumnIndex].Name;

            // Coluna STATUS
            if (coluna == "STATUS")
            {
                if (e.Value?.ToString() == "LIBERADO")
                {
                    e.CellStyle.BackColor = Color.FromArgb(224, 248, 210);
                    e.CellStyle.Font = new Font(dgv_comparativo_notas.Font, FontStyle.Bold);
                }
                else if (e.Value?.ToString() == "EM ANDAMENTO")
                {
                    e.CellStyle.BackColor = Color.FromArgb(254, 240, 165);
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
                    e.CellStyle.BackColor = Color.FromArgb(224, 248, 210);
                    e.CellStyle.Font = new Font(dgv_comparativo_notas.Font, FontStyle.Bold);
                }
                else
                {
                    if (e.Value != null && Convert.ToInt32(e.Value) != 0)
                    {
                        e.CellStyle.BackColor = Color.FromArgb(254, 175, 173);
                        e.CellStyle.Font = new Font(dgv_comparativo_notas.Font, FontStyle.Bold);
                    }
                    else
                    {
                        e.CellStyle.BackColor = Color.FromArgb(224, 248, 210);
                        e.CellStyle.Font = new Font(dgv_comparativo_notas.Font, FontStyle.Bold);

                    }
                }
            }
        }

        private void bt_logs_processos_importacao_Click(object sender, EventArgs e)
        {
            F_Relatorio_Importacao form;

            List<string> empresasSelecionadas = lbox_empresas.SelectedItems.Cast<string>().ToList();
            if (lbox_empresas.SelectedItems.Count == 1)
            {
                string empresa = lbox_empresas.SelectedItem!.ToString()!;
                form = new(empresa);
            }
            else
                form = new();


            form.Show();
        }

        private async void bt_atualizar_valores_tax_Click(object sender, EventArgs e)
        {
            //Limpa arquivos temporários
            foreach (string arquivo in Directory.GetFiles(Config.PathArquivoTemporario))
                File.Delete(arquivo);

            List<string> empresasSelecionadas = lbox_empresas.SelectedItems.Cast<string>().ToList();

            TaxContext context;

            var parametros = new ParametrosProcessosCustomizados(
                "*",
                "*",
                dtp_periodo_inicio.Value,
                dtp_periodo_fim.Value,
                false,
                false,
                false,
                false,
                true,
                true,
                true,
                false
                );

            int total = empresasSelecionadas.Count, concluidas = 0;

            var tasks = empresasSelecionadas.Select(async empresa =>
            {
                using var status = new StatusTask(statusStrip, $"Empresa {empresa}");
                var progresso = new Progress<Progresso>(status.Atualizar);

                TaxContext context = ApiTax.GetContext(empresa);

                TaxApiResponse response = await ApiTax.ProgramarRelatorio(context, parametros, progresso);
                int qtd = Interlocked.Increment(ref concluidas);

                if (!response.Success)
                    MessageBox.Show($"Falha ao programar relatório para a empresa {empresa}: {response.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return response.Success;
            });

            bool[] resultados = await Task.WhenAll(tasks);

            if (resultados.Any(r => !r))
            {
                //var response = MessageBox.Show("Houveram falhas ao programar alguns relatórios, deseja continuar?", "Erro", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                //if (response == DialogResult.Cancel)
                return;
            }

            using var status = new StatusTask(statusStrip, "");
            IProgress<Progresso> progresso = new Progress<Progresso>(status.Atualizar);

            await BuscarRelatoriosAsync(empresasSelecionadas, progresso);

            int ano = dtp_periodo_inicio.Value.Year;
            int mes = dtp_periodo_inicio.Value.Month;

            await Task.Run(() =>
            {
                progresso.Report(new Progresso("Atualizando valores", 1));

                using var con = Banco.Conexao();
                con.Open();
                using var transaction = con.BeginTransaction();

                int arquivos = Directory.GetFiles(Config.PathArquivoTemporario, "*.pdf").Count();
                int concluidos = 0;
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
                    concluidos++;
                    progresso.Report(new Progresso($"Atualizando valores {concluidos}/{arquivos}", (int)((double)concluidos / arquivos * 100)));
                }

                progresso.Report(new Progresso($"Concluido", 100));

                transaction.Commit();

            });

            AtualizarComparativoNotas();

            MessageBox.Show(
                "Dados do TAX atualizados com sucesso.",
                "Informação",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private async Task BuscarRelatoriosAsync(List<string> empresas, IProgress<Progresso> progresso)
        {
            int total = empresas.Count;
            int concluidas = 0;

            progresso.Report(new Progresso($"Aguardando conclusão dos relatórios. Concluído 0/{total}", 1));


            var tasks = empresas.Select(async empresa =>
            {
                bool finalizado = false;

                while (!finalizado)
                {
                    TaxContext context = ApiTax.GetContext(empresa);

                    var response = await ApiTax.VerificaUltimoRelatorioConcluido(
                        empresa,
                        context);

                    finalizado = response.Completed;

                    try
                    {
                        if (finalizado)
                        {
                            await ApiTax.BaixarRelatorio(
                                context,
                                1,
                                0,
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

                progresso.Report(new Progresso($"Aguardando conclusão dos relatórios. Concluído {qtd}/{total}", porcentagem));
            });

            await Task.WhenAll(tasks);

            progresso.Report(new Progresso($"Busca concluída", 100));

        }

        private void bt_ferramentas_Click(object sender, EventArgs e)
        {

            if (cb_ferramentas.SelectedIndex == 0) //ITENS
                FuncoesTax.DiferencaItens(Globais.gerarArquivo, Globais.fracionarValores);

            else if (cb_ferramentas.SelectedIndex == 1) //BURACO NOTAS
                FuncoesTax.BuracoDeNota(false, null);

            else if (cb_ferramentas.SelectedIndex == 2) //PRODUTOS / TAXAS
                FuncoesTax.ImportarProdutos();

            else if (cb_ferramentas.SelectedIndex == 3) //PESSOA FIS/ JUR
                FuncoesTax.ImportarPessoaFisicaJuridica(Globais.gerarArquivo, Globais.fracionarValores, true);

            else if (cb_ferramentas.SelectedIndex == 4)//DIFERENÇA CANCELADAS
            {
                if (lbox_empresas.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Selecione pelo menos uma empresa para executar a função de diferença de canceladas.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (lbox_empresas.SelectedItems.Count > 1)
                {
                    MessageBox.Show("Selecione apenas uma empresa para executar a função de diferença de canceladas.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                FuncoesTax.GetDiferencaCanceladas(dtp_periodo_inicio.Value.Year.ToString(), dtp_periodo_inicio.Value.Month.ToString("D2"), lbox_empresas.SelectedItem.ToString(), Globais.mesAberto, Globais.gerarArquivo, Globais.fracionarValores);
            }
        }

        private async void bt_qtd_notas_Click(object sender, EventArgs e)
        {
            DateTime periodoIni = dtp_periodo_inicio.Value;
            DateTime periodoFin = dtp_periodo_fim.Value;
            List<string> empresasSelecionadas = lbox_empresas.SelectedItems.Cast<string>().ToList();
            bool mostrarNaTela = ckb_mostrar_na_tela.Checked;
            string local = cb_local_qtd_notas.Text;
            bool incluidasHoje = true;
            bool mesAberto = ckb_mes_aberto.Checked;
            bool popularTabela = false;

            using var status = new StatusTask(statusStrip, "");
            var progresso = new Progress<Progresso>(status.Atualizar);

            await FuncoesTax.GetQuantidadeNotasFar(periodoIni, periodoFin, empresasSelecionadas, mostrarNaTela, local, mesAberto, popularTabela, progresso);

        }

        private async void bt_popular_tabela_sifar_Click(object sender, EventArgs e)
        {
            DateTime periodoIni = dtp_periodo_inicio.Value;
            DateTime periodoFin = dtp_periodo_fim.Value;
            List<string> empresasSelecionadas = lbox_empresas.SelectedItems.Cast<string>().ToList();
            bool mostrarNaTela = ckb_mostrar_na_tela.Checked;
            bool incluidasHoje = true;
            bool mesAberto = ckb_mes_aberto.Checked;
            bool popularTabela = true;
            string local = mesAberto ? "MSA" : "SIFAR";

            using var status = new StatusTask(statusStrip, "");
            var progresso = new Progress<Progresso>(status.Atualizar);

            await FuncoesTax.GetQuantidadeNotasFar(periodoIni, periodoFin, empresasSelecionadas, mostrarNaTela, local, mesAberto, popularTabela, progresso);
        }

        private void DiretorioPadraoEntradaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Util.SelecionarPasta();
            Config.DiretorioPadraoEntrada = path;
            Config.Save();
        }

        private void DiretorioPadraoSaidaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Util.SelecionarPasta();
            Config.DiretorioPadraoSaida = path;
            Config.Save();
        }

        private void bt_obter_icms_sifar_Click(object sender, EventArgs e)
        {
            if (lbox_empresas.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selecione pelo menos uma empresa para executar a função de diferença de canceladas.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (lbox_empresas.SelectedItems.Count > 1)
            {
                MessageBox.Show("Selecione apenas uma empresa para executar a função de diferença de canceladas.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            BancoDTO banco = Empresa.GetBancoFar(lbox_empresas.SelectedItem.ToString());
            if (banco is null)
            {
                MessageBox.Show("Informe o banco!");
                return;
            }
            if (string.IsNullOrEmpty(dtp_periodo_inicio.Value.Month.ToString()) || string.IsNullOrEmpty(dtp_periodo_inicio.Value.Year.ToString()))
            {
                MessageBox.Show("Informe o Ano/Mês!");
                return;
            }

            string mes = int.Parse(dtp_periodo_inicio.Value.Month.ToString()).ToString("00");
            string ano = dtp_periodo_inicio.Value.Year.ToString();

            string query = string.Format(
                                ckb_mes_aberto.Checked ? Queries.queryIcmsSifarMesAberto : Queries.queryIcmsSifar,
                                mes,
                                ano
                            );

            DataTable dt_icms = DataAccess.ExecuteQuery(Config.DatabaseUserFar, Config.DatabasePasswordFar, banco.database, banco.owner, query);

            Util.MostrarDataTable(dt_icms);
        }

        private void AbrirInterfaceAntigaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new Form1();
            form.Show();
        }

        private void sobreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new AboutBox1();
            about.ShowDialog();
        }

        private void bt_resetar_contexto_Click(object sender, EventArgs e)
        {
            ApiTax.ResetContext();
        }
    }
}
