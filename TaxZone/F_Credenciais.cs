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
    public partial class F_Credenciais : Form
    {
        public F_Credenciais()
        {
            InitializeComponent();
            tb_usuario_banco_far.Text = Config.DatabaseUserFar;
            tb_senha_banco_far.Text = Config.DatabasePasswordFar;
            tb_usuario_banco_msa.Text = Config.DatabaseUserMsa;
            tb_senha_banco_msa.Text = Config.DatabasePasswordMsa;
            tb_usuario_tax.Text = Config.UsuarioTax;
            tb_senha_tax.Text = Config.SenhaTax;
        }

        private void tb_usuario_banco_far_TextChanged(object sender, EventArgs e)
        {
            Config.DatabaseUserFar = tb_usuario_banco_far.Text;
            Config.Save();
        }

        private void tb_senha_banco_far_TextChanged(object sender, EventArgs e)
        {
            Config.DatabasePasswordFar = tb_senha_banco_far.Text;
            Config.Save();
        }

        private void tb_usuario_banco_msa_TextChanged(object sender, EventArgs e)
        {
            Config.DatabaseUserMsa = tb_usuario_banco_msa.Text;
            Config.Save();
        }

        private void tb_senha_banco_msa_TextChanged(object sender, EventArgs e)
        {
            Config.DatabasePasswordMsa = tb_senha_banco_msa.Text;
            Config.Save();
        }

        private void tb_usuario_tax_TextChanged(object sender, EventArgs e)
        {
            Config.UsuarioTax = tb_usuario_tax.Text;
            Config.Save();
        }

        private void tb_senha_tax_TextChanged(object sender, EventArgs e)
        {
            Config.SenhaTax = tb_senha_tax.Text;
            Config.Save();
        }
    }
}
