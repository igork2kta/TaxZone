using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaxZone.DTO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TaxZone
{
    public partial class F_Relatorio_Importacao : Form
    {
        TaxContext taxContext;
        public F_Relatorio_Importacao(TaxContext context)
        {
            InitializeComponent();
            taxContext = context;

            List<string> estabelecimentos = ["TODOS"];
            estabelecimentos.AddRange(Empresa.GetEstabelecimentos(taxContext.Empresa).Select(a => a.ToString()));
            cb_estabelecimento.DataSource = estabelecimentos;
            tb_usuario.Text = ConfigManager.UsuarioTax;
        }

        private async void bt_pesquisar_Click(object sender, EventArgs e)
        {
            pb_loading.Visible = true;
            lbl_loading_percentage.Visible = true;

            string status = string.Empty;
            string estabelecimento = string.Empty;

            if (cb_status.SelectedIndex == 0 || cb_status.SelectedIndex == -1) status = " ";
            else if (cb_status.SelectedIndex == 1) status = "O";
            else if (cb_status.SelectedIndex == 2) status = "E";

            if (cb_estabelecimento.SelectedIndex != 0) estabelecimento = cb_estabelecimento.Text;

            ParametrosRelatorioImportacao parametros = new(
                    dtp_inicio.Value,
                    dtp_fim.Value,
                    status,
                    tb_usuario.Text,
                    estabelecimento,
                    "");

            var progresso = new Progress<Progresso>(p =>
            {
                lbl_loading_percentage.Text = p.Mensagem;
                if (p.Valor == 100) {
                    pb_loading.Visible = false;
                    lbl_loading_percentage.Visible = false;
                }
            });

            var retorno = await ApiTax.ObterLogsProcessosCustomizados(taxContext, parametros, progresso);

            if (!retorno.Success)
                MessageBox.Show(retorno.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            dgv_relatorio_importacao.DataSource = retorno.ProcessosImportacao;



        }
    }
}
