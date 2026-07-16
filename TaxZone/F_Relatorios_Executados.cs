using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaxZone.DTO;

namespace TaxZone
{
    public partial class F_Relatorios_Executados : Form
    {

        ApiTax.TaxContext taxContext;
        public F_Relatorios_Executados(List<ProcessoRelatorio> processos, ApiTax.TaxContext taxContext)
        {
            InitializeComponent();
            dgv_relatorios.DataSource = processos;

            dgv_relatorios.Columns["Detalhes"].Visible = false;

            var btn_detalhes = new DataGridViewButtonColumn
            {
                HeaderText = "Detalhes",
                Text = "Mostrar",
                UseColumnTextForButtonValue = true,
                Name = "btnDetalhes"
            };

            dgv_relatorios.Columns.Add(btn_detalhes);

            var btn_baixar = new DataGridViewButtonColumn
            {
                HeaderText = "Ação",
                Text = "Baixar",
                UseColumnTextForButtonValue = true,
                Name = "btnBaixar"
            };

            dgv_relatorios.Columns.Add(btn_baixar);

            dgv_relatorios.CellContentClick += dgv_relatorios_CellContentClick;
            this.taxContext = taxContext;
        }

        private async void dgv_relatorios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dgv_relatorios.Columns[e.ColumnIndex].Name == "btnDetalhes")
            {
                ProcessoRelatorio processo = (ProcessoRelatorio)dgv_relatorios.Rows[e.RowIndex].DataBoundItem;

                MessageBox.Show($"Detalhes: {processo.Detalhes}");
            }

            if (dgv_relatorios.Columns[e.ColumnIndex].Name == "btnBaixar")
            {
                bool sucesso = await ApiTax.BaixarRelatorio(taxContext, e.RowIndex+1);
                if(sucesso) 
                    MessageBox.Show("Arquivos baixados com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information );
                else
                    MessageBox.Show("Falha ao baixar arquivos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
