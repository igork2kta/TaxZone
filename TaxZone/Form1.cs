using System.Data;
using System.Diagnostics;
using TaxZone.DTO;

namespace TaxZone
{
    public partial class Form1 : Form
    {

        List<TaxContext> contextos = new();
        private readonly CookieRenewService _cookieRenew = new();

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
            cb_empresa_tax_api.DataSource = Empresa.ListaEmpresas; ;

            cb_empresa_qtd_notas.Items.Add("TODAS");
            cb_empresa_qtd_notas.Items.AddRange(Empresa.ListaEmpresas.ToArray());
            cb_empresa_qtd_notas.SelectedIndex = 0;

            cb_local_qtd_notas.SelectedIndex = 0;

            //Preenchimento das datas
            DateTime referenciaAnterior = DateTime.Now.AddMonths(-1);

            tb_ano.Text = referenciaAnterior.Year.ToString();
            tb_mes.Text = referenciaAnterior.Month.ToString();

            //Sempre o mês fechado
            dtp_periodo_ini_qtd_notas.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, 01);
            dtp_periodo_fin_qtd_notas.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, DateTime.DaysInMonth(referenciaAnterior.Year, referenciaAnterior.Month));

            dtp_tax_data_inicio.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, 01);
            dtp_tax_data_fim.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, DateTime.DaysInMonth(referenciaAnterior.Year, referenciaAnterior.Month));

            tb_referenciaBuracoNota.Text = $"{referenciaAnterior.Month}_{referenciaAnterior.Year}";

            //Configução das variáveis globais utilizadas por muitas funções
            Globais.gerarArquivo = ckb_gerar_arquivo.Checked;
            Globais.fracionarValores = ckb_fracionar_valores.Checked;
            Globais.mesAberto = ckb_mes_aberto.Checked;
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
            Banco banco = Empresa.GetBancoFar(cb_banco.Text);
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

        private void bt_qtd_notas_Click(object sender, EventArgs e)
        {
            DateTime periodoIni = dtp_periodo_ini_qtd_notas.Value;
            DateTime periodoFin = dtp_periodo_fin_qtd_notas.Value;
            string empresa = cb_empresa_qtd_notas.Text;
            bool mostrarNaTela = ckb_mostrar_na_tela.Checked;
            bool arquivoTemporario = ckb_arquivo_temp.Checked;
            string local = cb_local_qtd_notas.Text;
            bool incluidasHoje = ckb_incluidas_hoje.Checked;
            bool mesAberto = ckb_mes_aberto.Checked;

            FuncoesTax.GetQuantidadeNotas(periodoIni, periodoFin, empresa, mostrarNaTela, arquivoTemporario, local, incluidasHoje, mesAberto);

        }

        private void bt_pessoa_fisica_juridica_Click(object sender, EventArgs e)
            => FuncoesTax.ImportarPessoaFisicaJuridica(ckb_gerar_arquivo.Checked, ckb_fracionar_valores.Checked, ckb_codFisJur.Checked);

        private void bt_produtos_taxas_Click(object sender, EventArgs e)
            => FuncoesTax.ImportarProdutos();
        

        private void bt_tax_automation_Click(object sender, EventArgs e)
            => ApiTax.ProgramarTaxAutomation(cb_empresa_tax_api.Text);

        private void bt_status_tax_automation_Click(object sender, EventArgs e)
            => ApiTax.VerificarStatusExecucao(cb_empresa_tax_api.Text);

        private void bt_executar_relatorio_Click(object sender, EventArgs e)
        {
            ApiTax.param_empresa = "*";
            if (cb_estab.Text == "TODOS")
                ApiTax.param_estab = "*";
            else
                ApiTax.param_estab = cb_estab.Text;

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

            TaxContext context = GetContext(cb_empresa_tax_api.Text);

            ApiTax.ProgramarRelatorio(cb_empresa_tax_api.Text, context, string.IsNullOrEmpty(context.StorageId));
        }



        private void bt_relatorios_Click(object sender, EventArgs e)
        {

            F_Relatorios_Executados form = new(GetContext(cb_empresa_tax_api.Text));
            form.Show();
            form.BuscarDados(cb_empresa_tax_api.Text);
        }

        private async void bt_login_Click(object sender, EventArgs e)
        {
            //Renova os contextos para a nova sessão
            contextos = new();
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
                if (!Directory.Exists(Path.GetDirectoryName(Config.PathScriptTemporario)))
                    Directory.CreateDirectory(Path.GetDirectoryName(Config.PathScriptTemporario));


                if (!File.Exists(Config.PathScriptTemporario))
                    File.Create(Config.PathScriptTemporario).Close();

                Process.Start("notepad.exe", Config.PathScriptTemporario);
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
            }
            else
            {
                DateTime referenciaAnterior = DateTime.Now.AddMonths(-1);
                dtp_periodo_ini_qtd_notas.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, 01);
                dtp_periodo_fin_qtd_notas.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, DateTime.DaysInMonth(referenciaAnterior.Year, referenciaAnterior.Month));

                dtp_tax_data_inicio.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, 01);
                dtp_tax_data_fim.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, DateTime.DaysInMonth(referenciaAnterior.Year, referenciaAnterior.Month));

            }
        }

        private void tb_cookie_TextChanged(object sender, EventArgs e)
            => ConfigManager.Cookie = tb_cookie.Text;
        

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

        private void cb_empresa_tax_api_SelectedIndexChanged(object sender, EventArgs e)
        {
            var listEstab = Empresa.GetEstabelecimentos(cb_empresa_tax_api.Text);
            var datasource = new List<string> { "TODOS" };
            datasource.AddRange(listEstab.Select(x => x.ToString()));
            cb_estab.DataSource = datasource;
        }
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


    }
}
