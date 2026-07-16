using TaxZone.DTO;

namespace TaxZone
{
    public partial class F_Relatorios_Executados : Form
    {

        TaxContext taxContext;
        List<ProcessoRelatorio> processos;
        bool primeiro_carregamento = true;
        public F_Relatorios_Executados(TaxContext context)
        {
            InitializeComponent();
            dgv_relatorios.CellContentClick += dgv_relatorios_CellContentClick;
            this.taxContext = context;
        }

        public async void BuscarDados(string empresa)
        {

            bool sucesso = await ApiTax.ObterRelatorio(this, empresa, taxContext, string.IsNullOrEmpty(taxContext.StorageId));
            if (!sucesso) this.Close();
        }

        public void PopulaDataGrid(TaxContext taxContext, List<ProcessoRelatorio> processos)
        {
            Text = "Relatórios " + taxContext.Empresa;
            this.processos = processos;

            lbl_loading_percentage.Visible = false;
            pb_loading.Visible = false;
            dgv_relatorios.Visible = true;

            dgv_relatorios.DataSource = this.processos;

            dgv_relatorios.Columns["Detalhes"].Visible = false;

            if (primeiro_carregamento)
            {
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
                primeiro_carregamento = false;
            }
            

            
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
                bool sucesso = await ApiTax.BaixarRelatorio(taxContext, e.RowIndex + 1);
                if (sucesso)
                    MessageBox.Show("Arquivos baixados com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Falha ao baixar arquivos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void bt_atualizar_Click(object sender, EventArgs e)
        {
            lbl_loading_percentage.Visible = true;
            pb_loading.Visible = true;
            dgv_relatorios.Visible = false;
            bool sucesso = await ApiTax.ObterRelatorio(this, taxContext.Empresa, taxContext, false);
            if (!sucesso) this.Close();
        }

        public void UpdateLoadingPercentage(string percentage)
        {
            lbl_loading_percentage.Text = percentage;
        }
    }
}
