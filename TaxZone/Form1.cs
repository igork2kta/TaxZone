using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaxZone
{
    public partial class Form1 : Form
    {

        public class ComboValue
        {
            public string key { get; set; }   // Ex: COM_MGR_PR
            public string value { get; set; } // Ex: CFLCL

            public override string ToString()
            {
                // Define o que vai aparecer no ComboBox
                return key;
            }
        }

        public Form1()
        {
            InitializeComponent();
            ConfigManager.Load();

            tb_usuario_banco_far.Text = ConfigManager.DatabaseUserFar;
            tb_senha_banco_far.Text = ConfigManager.DatabasePasswordFar;
            tb_usuario_banco_msa.Text = ConfigManager.DatabaseUserMsa;
            tb_senha_banco_msa.Text = ConfigManager.DatabasePasswordMsa;

            var dataSourceEmpresas = new List<string> { "EMR", "ESE", "EPB", "ETO", "EMT", "EMS", "ESS", "ERO", "EAC" };
            cb_banco.DataSource = dataSourceEmpresas;
            cb_empresa.DataSource = dataSourceEmpresas;

            cb_empresa_qtd_notas.Items.Add("TODAS");
            cb_empresa_qtd_notas.Items.AddRange(dataSourceEmpresas.ToArray());
            cb_empresa_qtd_notas.SelectedIndex = 0;

            DateTime referenciaAnterior = DateTime.Now.AddMonths(-1);

            tb_ano.Text = referenciaAnterior.Year.ToString();
            tb_mes.Text = referenciaAnterior.Month.ToString();

            //Sempre o mês fechado
            dtp_periodo_ini_qtd_notas.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, 01);
            dtp_periodo_fin_qtd_notas.Value = new DateTime(referenciaAnterior.Year, referenciaAnterior.Month, DateTime.DaysInMonth(referenciaAnterior.Year, referenciaAnterior.Month));

            tb_referenciaBuracoNota.Text = $"{referenciaAnterior.Month}_{referenciaAnterior.Year}";

            cb_local_qtd_notas.SelectedIndex = 0;
        }


        private void bt_diferenca_capa_item_Click(object sender, EventArgs e)
        {
            FuncoesTax.DiferencaItens(ckb_gerar_arquivo.Checked, ckb_fracionar_valores.Checked);
        }

        private void bt_notas_sem_item_Click(object sender, EventArgs e)
        {
            //No arquivo Notas_sem_item as notas ficam na coluna 3
            CsvClass.CopiarNotasAreaTransferencia(2, ckb_fracionar_valores.Checked);
        }


        private void bt_buraco_nota_Click(object sender, EventArgs e)
        {
            FuncoesTax.BuracoDeNota(ckb_buraco_notas_hardcore.Checked, tb_referenciaBuracoNota.Text, ckb_gerar_arquivo.Checked, ckb_fracionar_valores.Checked);
        }

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

        private void Form1_Load(object sender, EventArgs e)
        {

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

            /*
            if (dt_icms is null || dt_icms.Rows.Count == 0) return;

            decimal icms = Convert.ToDecimal(dt_icms.Rows[0]["ICMS"]);


            MessageBox.Show($"ICMS encontrado no SIFAR {icms}");
            */
        }

        private void bt_qtd_notas_Click(object sender, EventArgs e)
        {
            DateTime periodoIni = dtp_periodo_ini_qtd_notas.Value;
            DateTime periodoFin = dtp_periodo_fin_qtd_notas.Value;

            SaveFileDialog salvarDialog = new SaveFileDialog();

            int taskCount = 1;
            if (cb_empresa_qtd_notas.Text == "TODAS") taskCount = 9;

            if (!ckb_mostrar_na_tela.Checked)
            {
                salvarDialog.Title = "Salvar arquivo como...";
                salvarDialog.Filter = "Arquivo separado por vírgula (*.csv)|*.csv|Todos os arquivos (*.*)|*.*";
                salvarDialog.DefaultExt = "csv";
                salvarDialog.AddExtension = true;
                if (taskCount == 1)
                    salvarDialog.FileName = $"qtd_notas_{cb_empresa_qtd_notas.Text}.csv"; // Nome padrão
                else
                    salvarDialog.FileName = $"qtd_notas.csv"; // Nome padrão

                if (salvarDialog.ShowDialog() != DialogResult.OK) return;
            }

            Task[] tasks = new Task[taskCount];
            DataTable qtd_notas = new();
            for (int i = 0; i < taskCount; i++)
            {

                Banco banco = null;
                string query = "";
                string empresa;
                string user = tb_usuario_banco_msa.Text;
                string password = tb_senha_banco_msa.Text;

                if (taskCount == 1) empresa = cb_empresa_qtd_notas.Text;
                else empresa = cb_empresa_qtd_notas.Items[i + 1].ToString();

                if (cb_local_qtd_notas.Text == "MSA")
                {
                    banco = Empresa.GetBancoMsa(empresa);

                    query = string.Format(Queries.qtdNotasMsa, periodoIni.ToString("dd/MM/yyyy"), periodoFin.ToString("dd/MM/yyyy"), empresa);
                    user = tb_usuario_banco_msa.Text;
                    password = tb_senha_banco_msa.Text;
                }
                else if (cb_local_qtd_notas.Text == "SIFAR")
                {
                    banco = Empresa.GetBancoFar(empresa);

                    if (ckb_mes_aberto.Checked)
                        query = string.Format(Queries.qtdNotasFarMesAberto, periodoIni.ToString("dd/MM/yyyy"), periodoFin.ToString("dd/MM/yyyy"), empresa);
                    else
                        query = string.Format(Queries.qtdNotasFarMesFechado, periodoIni.Month.ToString("00"), periodoFin.Year, empresa);




                    user = tb_usuario_banco_far.Text;
                    password = tb_senha_banco_far.Text;
                }

                if (banco is null) return;

                string serviceName = banco.database;
                string session = banco.owner;

                tasks[i] = Task.Run(
                    () =>
                    {
                        DataTable a = DataAccess.ExecuteQuery(user, password, serviceName, session, query);
                        if (a != null)
                            qtd_notas.Merge(a);
                    }
                    );
            }

            Task.WaitAll(tasks);


            if (ckb_mostrar_na_tela.Checked)
            {
                Util.MostrarDataTable(qtd_notas);
            }
            else
            {
                CsvClass.WriteDataTableToCsv(qtd_notas, salvarDialog.FileName);

                var resposta = MessageBox.Show("Extração Finalizada! Deseja abrir o arquivo?", "Pronto!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (resposta == DialogResult.Yes)
                {
                    Process.Start(new ProcessStartInfo(salvarDialog.FileName) { UseShellExecute = true });
                }
            }

        }

        private void bt_pessoa_fisica_juridica_Click(object sender, EventArgs e)
        {
            FuncoesTax.ImportarPessoaFisicaJuridica(ckb_gerar_arquivo.Checked, ckb_fracionar_valores.Checked);
        }

        private void ckb_buraco_notas_hardcore_CheckedChanged(object sender, EventArgs e)
        {
            tb_referenciaBuracoNota.Visible = ckb_buraco_notas_hardcore.Checked;
        }

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
    }
}
