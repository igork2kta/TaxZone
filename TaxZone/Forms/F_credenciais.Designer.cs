namespace TaxZone
{
    partial class F_Credenciais
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label9 = new Label();
            tb_usuario_banco_msa = new TextBox();
            label10 = new Label();
            tb_senha_banco_msa = new TextBox();
            label6 = new Label();
            tb_usuario_banco_far = new TextBox();
            label5 = new Label();
            tb_senha_banco_far = new TextBox();
            label28 = new Label();
            label27 = new Label();
            tb_usuario_tax = new TextBox();
            tb_senha_tax = new TextBox();
            SuspendLayout();
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(198, 14);
            label9.Name = "label9";
            label9.Size = new Size(75, 15);
            label9.TabIndex = 27;
            label9.Text = "Usuário MSA";
            // 
            // tb_usuario_banco_msa
            // 
            tb_usuario_banco_msa.Location = new Point(274, 10);
            tb_usuario_banco_msa.Name = "tb_usuario_banco_msa";
            tb_usuario_banco_msa.Size = new Size(100, 23);
            tb_usuario_banco_msa.TabIndex = 24;
            tb_usuario_banco_msa.TextChanged += tb_usuario_banco_msa_TextChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(204, 45);
            label10.Name = "label10";
            label10.Size = new Size(67, 15);
            label10.TabIndex = 25;
            label10.Text = "Senha MSA";
            // 
            // tb_senha_banco_msa
            // 
            tb_senha_banco_msa.Location = new Point(274, 41);
            tb_senha_banco_msa.Name = "tb_senha_banco_msa";
            tb_senha_banco_msa.PasswordChar = '*';
            tb_senha_banco_msa.Size = new Size(100, 23);
            tb_senha_banco_msa.TabIndex = 26;
            tb_senha_banco_msa.TextChanged += tb_senha_banco_msa_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 14);
            label6.Name = "label6";
            label6.Size = new Size(70, 15);
            label6.TabIndex = 23;
            label6.Text = "Usuário FAR";
            // 
            // tb_usuario_banco_far
            // 
            tb_usuario_banco_far.Location = new Point(86, 10);
            tb_usuario_banco_far.Name = "tb_usuario_banco_far";
            tb_usuario_banco_far.Size = new Size(100, 23);
            tb_usuario_banco_far.TabIndex = 20;
            tb_usuario_banco_far.TextChanged += tb_usuario_banco_far_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(20, 45);
            label5.Name = "label5";
            label5.Size = new Size(62, 15);
            label5.TabIndex = 21;
            label5.Text = "Senha FAR";
            // 
            // tb_senha_banco_far
            // 
            tb_senha_banco_far.Location = new Point(86, 41);
            tb_senha_banco_far.Name = "tb_senha_banco_far";
            tb_senha_banco_far.PasswordChar = '*';
            tb_senha_banco_far.Size = new Size(100, 23);
            tb_senha_banco_far.TabIndex = 22;
            tb_senha_banco_far.TextChanged += tb_senha_banco_far_TextChanged;
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new Point(401, 45);
            label28.Name = "label28";
            label28.Size = new Size(66, 15);
            label28.TabIndex = 61;
            label28.Text = "Senha TAX:";
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new Point(396, 14);
            label27.Name = "label27";
            label27.Size = new Size(74, 15);
            label27.TabIndex = 59;
            label27.Text = "Usuario TAX:";
            // 
            // tb_usuario_tax
            // 
            tb_usuario_tax.Location = new Point(476, 10);
            tb_usuario_tax.Name = "tb_usuario_tax";
            tb_usuario_tax.Size = new Size(100, 23);
            tb_usuario_tax.TabIndex = 58;
            tb_usuario_tax.TextChanged += tb_usuario_tax_TextChanged;
            // 
            // tb_senha_tax
            // 
            tb_senha_tax.Location = new Point(476, 41);
            tb_senha_tax.Name = "tb_senha_tax";
            tb_senha_tax.PasswordChar = '*';
            tb_senha_tax.Size = new Size(100, 23);
            tb_senha_tax.TabIndex = 60;
            tb_senha_tax.TextChanged += tb_senha_tax_TextChanged;
            // 
            // F_credenciais
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(594, 86);
            Controls.Add(label28);
            Controls.Add(label27);
            Controls.Add(tb_usuario_tax);
            Controls.Add(tb_senha_tax);
            Controls.Add(label9);
            Controls.Add(tb_usuario_banco_msa);
            Controls.Add(label10);
            Controls.Add(tb_senha_banco_msa);
            Controls.Add(label6);
            Controls.Add(tb_usuario_banco_far);
            Controls.Add(label5);
            Controls.Add(tb_senha_banco_far);
            Name = "F_credenciais";
            Text = "Ajuste de credenciais";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label9;
        private TextBox tb_usuario_banco_msa;
        private Label label10;
        private TextBox tb_senha_banco_msa;
        private Label label6;
        private TextBox tb_usuario_banco_far;
        private Label label5;
        private TextBox tb_senha_banco_far;
        private Label label28;
        private Label label27;
        private TextBox tb_usuario_tax;
        private TextBox tb_senha_tax;
    }
}