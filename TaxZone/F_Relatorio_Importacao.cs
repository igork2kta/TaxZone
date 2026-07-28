
using System.Data;
using System.Diagnostics;
using TaxZone.DTO;

namespace TaxZone
{
    public partial class F_Relatorio_Importacao : Form
    {
        bool primeiro_carregamento = true;
        TaxContext taxContext;
        public F_Relatorio_Importacao(TaxContext context)
        {
            InitializeComponent();
            taxContext = context;

            List<string> estabelecimentos = ["TODOS"];
            estabelecimentos.AddRange(Empresa.GetEstabelecimentos(taxContext.Empresa).Select(a => a.ToString()));
            cb_estabelecimento.DataSource = estabelecimentos;
            tb_usuario.Text = ConfigManager.UsuarioTax;

            cb_acao_botao_relatorio.SelectedIndex = 0;
        }

        private async void bt_pesquisar_Click(object sender, EventArgs e)
        {
            dgv_relatorio_importacao.DataSource = null;
            pb_loading.Visible = true;
            lbl_loading_percentage.Visible = true;

            string status = " ";
            string estabelecimento = string.Empty;

            /*
            if (cb_status.SelectedIndex == 0 || cb_status.SelectedIndex == -1) status = " ";
            else if (cb_status.SelectedIndex == 1) status = "O";
            else if (cb_status.SelectedIndex == 2) status = "E";
            */
            if (cb_estabelecimento.SelectedIndex != 0) estabelecimento = cb_estabelecimento.Text;

            ParametrosRelatorioImportacao parametros = new(
                    dtp_inicio.Value,
                    dtp_fim.Value,
                    status,
                    tb_usuario.Text,
                    estabelecimento,
                    tb_descricao.Text);

            var progresso = new Progress<Progresso>(p =>
            {
                lbl_loading_percentage.Text = p.Mensagem;
                if (p.Valor == 100)
                {
                    pb_loading.Visible = false;
                    lbl_loading_percentage.Visible = false;
                }
            });

            var retorno = await ApiTax.ObterLogsProcessosImportacao(taxContext, parametros, progresso);

            if (!retorno.Success)
                MessageBox.Show(retorno.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            dgv_relatorio_importacao.DataSource = retorno.ProcessosImportacao;

            dgv_relatorio_importacao.Columns["CodEmpresa"].Visible = false;


            if (primeiro_carregamento)
            {

                var btn_relatorio = new DataGridViewButtonColumn
                {
                    HeaderText = "Ação",
                    Text = "Relatório",
                    UseColumnTextForButtonValue = true,
                    Name = "btnRelatorio"
                };

                dgv_relatorio_importacao.Columns.Add(btn_relatorio);
                primeiro_carregamento = false;

                // Move a coluna diferença para antes do status

                dgv_relatorio_importacao.Columns["btnRelatorio"].DisplayIndex =
                    dgv_relatorio_importacao.Columns["NumProcesso"].DisplayIndex;
            }

        }

        private void dgv_relatorio_importacao_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == DBNull.Value) return;

            string coluna = dgv_relatorio_importacao.Columns[e.ColumnIndex].Name;

            if (coluna == "QtdErr")
            {
                if (Convert.ToInt16(e.Value) > 0)
                {
                    e.CellStyle.BackColor = Color.Salmon;
                    e.CellStyle.Font = new Font(dgv_relatorio_importacao.Font, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.BackColor = Color.LightGreen;
                    e.CellStyle.Font = new Font(dgv_relatorio_importacao.Font, FontStyle.Bold);
                }
            }

            else if (coluna == "QtdIns")
            {
                e.CellStyle.BackColor = Color.LightGreen;
                e.CellStyle.Font = new Font(dgv_relatorio_importacao.Font, FontStyle.Bold);
            }

            else if (coluna == "QtdIgn")
            {
                e.CellStyle.BackColor = Color.LightGray;
                e.CellStyle.Font = new Font(dgv_relatorio_importacao.Font, FontStyle.Bold);
            }
        }

        private async void dgv_relatorio_importacao_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dgv_relatorio_importacao.Columns[e.ColumnIndex].Name == "btnRelatorio")
            {

                int opcao = cb_acao_botao_relatorio.SelectedIndex;
                TaxApiResponse response = null;

                if (opcao == 0)
                { //Visualisar arquivo
                    response = await ApiTax.BaixarRelatorioProcessoImportacao(taxContext, e.RowIndex + 1, Config.PathArquivoTemporario + "\\Relatorio.pdf");
                    Process.Start(new ProcessStartInfo(Config.PathArquivoTemporario + "\\Relatorio.pdf") { UseShellExecute = true });
                }
                else if (opcao == 1)
                {
                    using SaveFileDialog saveDialog = new SaveFileDialog();

                    saveDialog.Title = "Salvar arquivo";
                    saveDialog.Filter = "Arquivo PDF (*.pdf)|*.pdf|Todos os arquivos (*.*)|*.*";
                    saveDialog.DefaultExt = "pdf";
                    saveDialog.AddExtension = true;
                    saveDialog.FileName = "Relatorio.pdf";
                    saveDialog.OverwritePrompt = true;

                    if (saveDialog.ShowDialog() != DialogResult.OK)
                        return;

                    response = await ApiTax.BaixarRelatorioProcessoImportacao(taxContext, e.RowIndex + 1, saveDialog.FileName);

                }
                else if (opcao == 2)
                {
                    if (dgv_relatorio_importacao.Rows[e.RowIndex].Cells["descricao"].Value.ToString() == "IMPX42")
                    {
                        response = await ApiTax.BaixarRelatorioProcessoImportacao(taxContext, e.RowIndex + 1, Config.PathArquivoTemporario + "\\Relatorio.pdf");
                        FuncoesTax.ImportarPessoaFisicaJuridica(ckb_gerar_arquivo.Checked, ckb_fracionar.Checked, false, Config.PathArquivoTemporario + "\\Relatorio.pdf");
                    }
                    else
                    {
                        MessageBox.Show("Processamento do arquivo disponível apenas para SAFX42", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                MessageBox.Show(response?.Message, "Atenção");
            }

        }

        private void cb_acao_botao_relatorio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cb_acao_botao_relatorio.SelectedIndex == 2)
            {
                ckb_fracionar.Visible = true;
                ckb_gerar_arquivo.Visible = true;
            }
            else
            {
                ckb_fracionar.Visible = false;
                ckb_gerar_arquivo.Visible = false;
            }
        }
    }
}
