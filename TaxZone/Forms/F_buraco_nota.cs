using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaxZone
{
    public partial class F_buraco_nota : Form
    {
        List<(int NumDocfis, int Proximo)> pairs = new List<(int NumDocfis, int Proximo)>();

        int total_inicio, total;
        public F_buraco_nota(ref List<(int NumDocfis, int Proximo)> pairs)
        {
            InitializeComponent();

            this.pairs = pairs;

            foreach ((int NumDocfis, int Proximo) in pairs)
            {
                int buraco = (Proximo - 1) - NumDocfis;
                dw_buraco_nota.Rows.Add(NumDocfis, Proximo, buraco);

                total += buraco;
            }

            total_inicio = total;

            lbl_total.Text = total.ToString();
        }

        private void bt_cancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void bt_ok_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            //se nada mudou, não precisa fazer o foreach
            if (total_inicio == total) return;

            pairs.Clear();

            foreach (DataGridViewRow row in dw_buraco_nota.Rows)
            {
                // Ignora a última linha vazia (quando AllowUserToAddRows = true)
                if (row.IsNewRow) continue;

                int numDocfis = Convert.ToInt32(row.Cells[0].Value);
                int proximo = Convert.ToInt32(row.Cells[1].Value);

                pairs.Add((numDocfis, proximo));
            }

        }

        private void dw_buraco_nota_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.IsNewRow) return;

            int numDocfis = Convert.ToInt32(e.Row.Cells[0].Value);
            int proximo = Convert.ToInt32(e.Row.Cells[1].Value);

            total -= (proximo - 1) - numDocfis;

            lbl_total.Text = total.ToString(); 
        }
    }
}
