namespace TaxZone
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            bt_diferenca_capa_item = new Button();
            label1 = new Label();
            bt_notas_sem_item = new Button();
            label2 = new Label();
            bt_notas_canceladas = new Button();
            label3 = new Label();
            tb_estabelecimento = new TextBox();
            label4 = new Label();
            label5 = new Label();
            tb_senha_banco_far = new TextBox();
            label6 = new Label();
            tb_usuario_banco_far = new TextBox();
            label7 = new Label();
            label9 = new Label();
            tb_usuario_banco_msa = new TextBox();
            label10 = new Label();
            tb_senha_banco_msa = new TextBox();
            cb_banco = new ComboBox();
            cb_empresa = new ComboBox();
            label8 = new Label();
            bt_pendencia_processamento = new Button();
            label11 = new Label();
            cb_pendencia_processamento = new ComboBox();
            label14 = new Label();
            bt_comparar_icms = new Button();
            groupBox1 = new GroupBox();
            label13 = new Label();
            tb_ano = new TextBox();
            label12 = new Label();
            tb_mes = new TextBox();
            groupBox2 = new GroupBox();
            label20 = new Label();
            comboBox1 = new ComboBox();
            bt_qtd_notas = new Button();
            cb_empresa_qtd_notas = new ComboBox();
            label17 = new Label();
            label16 = new Label();
            dtp_periodo_fin_qtd_notas = new DateTimePicker();
            label15 = new Label();
            dtp_periodo_ini_qtd_notas = new DateTimePicker();
            label18 = new Label();
            bt_buraco_nota = new Button();
            ckb_buraco_notas_hardcore = new CheckBox();
            bt_pessoa_fisica_juridica = new Button();
            label19 = new Label();
            tb_referenciaBuracoNota = new MaskedTextBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // bt_diferenca_capa_item
            // 
            bt_diferenca_capa_item.Location = new Point(156, 28);
            bt_diferenca_capa_item.Name = "bt_diferenca_capa_item";
            bt_diferenca_capa_item.Size = new Size(86, 23);
            bt_diferenca_capa_item.TabIndex = 0;
            bt_diferenca_capa_item.Text = "Obter notas";
            bt_diferenca_capa_item.UseVisualStyleBackColor = true;
            bt_diferenca_capa_item.Click += bt_diferenca_capa_item_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(25, 32);
            label1.Name = "label1";
            label1.Size = new Size(114, 15);
            label1.TabIndex = 1;
            label1.Text = "Diferença capa-item";
            // 
            // bt_notas_sem_item
            // 
            bt_notas_sem_item.Location = new Point(156, 68);
            bt_notas_sem_item.Name = "bt_notas_sem_item";
            bt_notas_sem_item.Size = new Size(86, 23);
            bt_notas_sem_item.TabIndex = 2;
            bt_notas_sem_item.Text = "Obter notas";
            bt_notas_sem_item.UseVisualStyleBackColor = true;
            bt_notas_sem_item.Click += bt_notas_sem_item_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(49, 72);
            label2.Name = "label2";
            label2.Size = new Size(90, 15);
            label2.TabIndex = 3;
            label2.Text = "Notas sem item";
            // 
            // bt_notas_canceladas
            // 
            bt_notas_canceladas.Location = new Point(144, 34);
            bt_notas_canceladas.Name = "bt_notas_canceladas";
            bt_notas_canceladas.Size = new Size(86, 23);
            bt_notas_canceladas.TabIndex = 4;
            bt_notas_canceladas.Text = "Obter notas";
            bt_notas_canceladas.UseVisualStyleBackColor = true;
            bt_notas_canceladas.Click += bt_notas_canceladas_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(7, 38);
            label3.Name = "label3";
            label3.Size = new Size(120, 15);
            label3.TabIndex = 5;
            label3.Text = "Diferença Canceladas";
            // 
            // tb_estabelecimento
            // 
            tb_estabelecimento.Location = new Point(452, 35);
            tb_estabelecimento.Name = "tb_estabelecimento";
            tb_estabelecimento.Size = new Size(38, 23);
            tb_estabelecimento.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(352, 38);
            label4.Name = "label4";
            label4.Size = new Size(94, 15);
            label4.TabIndex = 7;
            label4.Text = "Estabelecimento";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(34, 414);
            label5.Name = "label5";
            label5.Size = new Size(98, 15);
            label5.TabIndex = 9;
            label5.Text = "Senha Banco FAR";
            // 
            // tb_senha_banco_far
            // 
            tb_senha_banco_far.Location = new Point(142, 414);
            tb_senha_banco_far.Name = "tb_senha_banco_far";
            tb_senha_banco_far.PasswordChar = '*';
            tb_senha_banco_far.Size = new Size(100, 23);
            tb_senha_banco_far.TabIndex = 8;
            tb_senha_banco_far.TextChanged += tb_senha_banco_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(26, 388);
            label6.Name = "label6";
            label6.Size = new Size(106, 15);
            label6.TabIndex = 11;
            label6.Text = "Usuário Banco FAR";
            // 
            // tb_usuario_banco_far
            // 
            tb_usuario_banco_far.Location = new Point(142, 385);
            tb_usuario_banco_far.Name = "tb_usuario_banco_far";
            tb_usuario_banco_far.Size = new Size(100, 23);
            tb_usuario_banco_far.TabIndex = 10;
            tb_usuario_banco_far.TextChanged += tb_usuario_banco_TextChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(508, 38);
            label7.Name = "label7";
            label7.Size = new Size(40, 15);
            label7.TabIndex = 13;
            label7.Text = "Banco";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(289, 385);
            label9.Name = "label9";
            label9.Size = new Size(111, 15);
            label9.TabIndex = 19;
            label9.Text = "Usuário Banco MSA";
            // 
            // tb_usuario_banco_msa
            // 
            tb_usuario_banco_msa.Location = new Point(404, 382);
            tb_usuario_banco_msa.Name = "tb_usuario_banco_msa";
            tb_usuario_banco_msa.Size = new Size(100, 23);
            tb_usuario_banco_msa.TabIndex = 18;
            tb_usuario_banco_msa.TextChanged += tb_usuario_banco_msa_TextChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(295, 414);
            label10.Name = "label10";
            label10.Size = new Size(103, 15);
            label10.TabIndex = 17;
            label10.Text = "Senha Banco MSA";
            // 
            // tb_senha_banco_msa
            // 
            tb_senha_banco_msa.Location = new Point(404, 411);
            tb_senha_banco_msa.Name = "tb_senha_banco_msa";
            tb_senha_banco_msa.PasswordChar = '*';
            tb_senha_banco_msa.Size = new Size(100, 23);
            tb_senha_banco_msa.TabIndex = 16;
            tb_senha_banco_msa.TextChanged += tb_senha_banco_msa_TextChanged;
            // 
            // cb_banco
            // 
            cb_banco.FormattingEnabled = true;
            cb_banco.Location = new Point(554, 35);
            cb_banco.Name = "cb_banco";
            cb_banco.Size = new Size(105, 23);
            cb_banco.TabIndex = 20;
            // 
            // cb_empresa
            // 
            cb_empresa.FormattingEnabled = true;
            cb_empresa.Location = new Point(406, 239);
            cb_empresa.Name = "cb_empresa";
            cb_empresa.Size = new Size(53, 23);
            cb_empresa.TabIndex = 22;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(348, 243);
            label8.Name = "label8";
            label8.Size = new Size(52, 15);
            label8.TabIndex = 21;
            label8.Text = "Empresa";
            // 
            // bt_pendencia_processamento
            // 
            bt_pendencia_processamento.Location = new Point(465, 240);
            bt_pendencia_processamento.Name = "bt_pendencia_processamento";
            bt_pendencia_processamento.Size = new Size(72, 23);
            bt_pendencia_processamento.TabIndex = 23;
            bt_pendencia_processamento.Text = "Verificar";
            bt_pendencia_processamento.UseVisualStyleBackColor = true;
            bt_pendencia_processamento.Click += bt_pendencia_processamento_Click;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(26, 244);
            label11.Name = "label11";
            label11.Size = new Size(146, 15);
            label11.TabIndex = 24;
            label11.Text = "Pendencia Processamento";
            // 
            // cb_pendencia_processamento
            // 
            cb_pendencia_processamento.FormattingEnabled = true;
            cb_pendencia_processamento.Location = new Point(178, 240);
            cb_pendencia_processamento.Name = "cb_pendencia_processamento";
            cb_pendencia_processamento.Size = new Size(139, 23);
            cb_pendencia_processamento.TabIndex = 25;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(40, 80);
            label14.Name = "label14";
            label14.Size = new Size(87, 15);
            label14.TabIndex = 27;
            label14.Text = "Compara ICMS";
            // 
            // bt_comparar_icms
            // 
            bt_comparar_icms.Location = new Point(143, 76);
            bt_comparar_icms.Name = "bt_comparar_icms";
            bt_comparar_icms.Size = new Size(87, 23);
            bt_comparar_icms.TabIndex = 26;
            bt_comparar_icms.Text = "Executar";
            bt_comparar_icms.UseVisualStyleBackColor = true;
            bt_comparar_icms.Click += bt_comparar_icms_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label13);
            groupBox1.Controls.Add(tb_ano);
            groupBox1.Controls.Add(label12);
            groupBox1.Controls.Add(tb_mes);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label14);
            groupBox1.Controls.Add(bt_notas_canceladas);
            groupBox1.Controls.Add(bt_comparar_icms);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(tb_estabelecimento);
            groupBox1.Controls.Add(cb_banco);
            groupBox1.Controls.Add(label7);
            groupBox1.Location = new Point(12, 97);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(700, 117);
            groupBox1.TabIndex = 28;
            groupBox1.TabStop = false;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(519, 71);
            label13.Name = "label13";
            label13.Size = new Size(29, 15);
            label13.TabIndex = 31;
            label13.Text = "Ano";
            // 
            // tb_ano
            // 
            tb_ano.Location = new Point(554, 68);
            tb_ano.Name = "tb_ano";
            tb_ano.Size = new Size(50, 23);
            tb_ano.TabIndex = 30;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(417, 71);
            label12.Name = "label12";
            label12.Size = new Size(29, 15);
            label12.TabIndex = 29;
            label12.Text = "Mês";
            // 
            // tb_mes
            // 
            tb_mes.Location = new Point(452, 68);
            tb_mes.Name = "tb_mes";
            tb_mes.Size = new Size(38, 23);
            tb_mes.TabIndex = 28;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label20);
            groupBox2.Controls.Add(comboBox1);
            groupBox2.Controls.Add(bt_qtd_notas);
            groupBox2.Controls.Add(cb_empresa_qtd_notas);
            groupBox2.Controls.Add(label17);
            groupBox2.Controls.Add(label16);
            groupBox2.Controls.Add(dtp_periodo_fin_qtd_notas);
            groupBox2.Controls.Add(label15);
            groupBox2.Controls.Add(dtp_periodo_ini_qtd_notas);
            groupBox2.Location = new Point(12, 287);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(700, 71);
            groupBox2.TabIndex = 31;
            groupBox2.TabStop = false;
            groupBox2.Text = "Quantidade notas MSA";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(14, 34);
            label20.Name = "label20";
            label20.Size = new Size(35, 15);
            label20.TabIndex = 37;
            label20.Text = "Local";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "MSA", "SIFAR" });
            comboBox1.Location = new Point(55, 29);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(72, 23);
            comboBox1.TabIndex = 36;
            // 
            // bt_qtd_notas
            // 
            bt_qtd_notas.Location = new Point(575, 28);
            bt_qtd_notas.Name = "bt_qtd_notas";
            bt_qtd_notas.Size = new Size(75, 23);
            bt_qtd_notas.TabIndex = 35;
            bt_qtd_notas.Text = "Executar";
            bt_qtd_notas.UseVisualStyleBackColor = true;
            bt_qtd_notas.Click += bt_qtd_notas_Click;
            // 
            // cb_empresa_qtd_notas
            // 
            cb_empresa_qtd_notas.FormattingEnabled = true;
            cb_empresa_qtd_notas.Location = new Point(492, 27);
            cb_empresa_qtd_notas.Name = "cb_empresa_qtd_notas";
            cb_empresa_qtd_notas.Size = new Size(73, 23);
            cb_empresa_qtd_notas.TabIndex = 33;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(434, 31);
            label17.Name = "label17";
            label17.Size = new Size(52, 15);
            label17.TabIndex = 32;
            label17.Text = "Empresa";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(298, 31);
            label16.Name = "label16";
            label16.Size = new Size(13, 15);
            label16.TabIndex = 34;
            label16.Text = "à";
            // 
            // dtp_periodo_fin_qtd_notas
            // 
            dtp_periodo_fin_qtd_notas.Format = DateTimePickerFormat.Short;
            dtp_periodo_fin_qtd_notas.Location = new Point(317, 26);
            dtp_periodo_fin_qtd_notas.Name = "dtp_periodo_fin_qtd_notas";
            dtp_periodo_fin_qtd_notas.Size = new Size(85, 23);
            dtp_periodo_fin_qtd_notas.TabIndex = 33;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(153, 31);
            label15.Name = "label15";
            label15.Size = new Size(48, 15);
            label15.TabIndex = 32;
            label15.Text = "Período";
            // 
            // dtp_periodo_ini_qtd_notas
            // 
            dtp_periodo_ini_qtd_notas.Format = DateTimePickerFormat.Short;
            dtp_periodo_ini_qtd_notas.Location = new Point(207, 26);
            dtp_periodo_ini_qtd_notas.Name = "dtp_periodo_ini_qtd_notas";
            dtp_periodo_ini_qtd_notas.Size = new Size(85, 23);
            dtp_periodo_ini_qtd_notas.TabIndex = 31;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(327, 32);
            label18.Name = "label18";
            label18.Size = new Size(71, 15);
            label18.TabIndex = 32;
            label18.Text = "Buraco nota";
            // 
            // bt_buraco_nota
            // 
            bt_buraco_nota.Location = new Point(416, 28);
            bt_buraco_nota.Name = "bt_buraco_nota";
            bt_buraco_nota.Size = new Size(86, 23);
            bt_buraco_nota.TabIndex = 33;
            bt_buraco_nota.Text = "Obter notas";
            bt_buraco_nota.UseVisualStyleBackColor = true;
            bt_buraco_nota.Click += bt_buraco_nota_Click;
            // 
            // ckb_buraco_notas_hardcore
            // 
            ckb_buraco_notas_hardcore.AutoSize = true;
            ckb_buraco_notas_hardcore.Location = new Point(508, 31);
            ckb_buraco_notas_hardcore.Name = "ckb_buraco_notas_hardcore";
            ckb_buraco_notas_hardcore.Size = new Size(110, 19);
            ckb_buraco_notas_hardcore.TabIndex = 34;
            ckb_buraco_notas_hardcore.Text = "Modo Hardcore";
            ckb_buraco_notas_hardcore.UseVisualStyleBackColor = true;
            ckb_buraco_notas_hardcore.CheckedChanged += ckb_buraco_notas_hardcore_CheckedChanged;
            // 
            // bt_pessoa_fisica_juridica
            // 
            bt_pessoa_fisica_juridica.Location = new Point(416, 68);
            bt_pessoa_fisica_juridica.Name = "bt_pessoa_fisica_juridica";
            bt_pessoa_fisica_juridica.Size = new Size(86, 23);
            bt_pessoa_fisica_juridica.TabIndex = 36;
            bt_pessoa_fisica_juridica.Text = "Obter notas";
            bt_pessoa_fisica_juridica.UseVisualStyleBackColor = true;
            bt_pessoa_fisica_juridica.Click += bt_pessoa_fisica_juridica_Click;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(283, 72);
            label19.Name = "label19";
            label19.Size = new Size(117, 15);
            label19.TabIndex = 35;
            label19.Text = "Pessoa fisica/juridica";
            // 
            // tb_referenciaBuracoNota
            // 
            tb_referenciaBuracoNota.Location = new Point(624, 29);
            tb_referenciaBuracoNota.Mask = "00_0000";
            tb_referenciaBuracoNota.Name = "tb_referenciaBuracoNota";
            tb_referenciaBuracoNota.Size = new Size(52, 23);
            tb_referenciaBuracoNota.TabIndex = 37;
            tb_referenciaBuracoNota.ValidatingType = typeof(DateTime);
            tb_referenciaBuracoNota.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(750, 457);
            Controls.Add(tb_referenciaBuracoNota);
            Controls.Add(bt_pessoa_fisica_juridica);
            Controls.Add(label19);
            Controls.Add(ckb_buraco_notas_hardcore);
            Controls.Add(bt_buraco_nota);
            Controls.Add(label18);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(cb_pendencia_processamento);
            Controls.Add(label11);
            Controls.Add(bt_pendencia_processamento);
            Controls.Add(cb_empresa);
            Controls.Add(label8);
            Controls.Add(label9);
            Controls.Add(tb_usuario_banco_msa);
            Controls.Add(label10);
            Controls.Add(tb_senha_banco_msa);
            Controls.Add(label6);
            Controls.Add(tb_usuario_banco_far);
            Controls.Add(label5);
            Controls.Add(tb_senha_banco_far);
            Controls.Add(label2);
            Controls.Add(bt_notas_sem_item);
            Controls.Add(label1);
            Controls.Add(bt_diferenca_capa_item);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Tax Zone";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button bt_diferenca_capa_item;
        private Label label1;
        private Button bt_notas_sem_item;
        private Label label2;
        private Button bt_notas_canceladas;
        private Label label3;
        private TextBox tb_estabelecimento;
        private Label label4;
        private Label label5;
        private TextBox tb_senha_banco_far;
        private Label label6;
        private TextBox tb_usuario_banco_far;
        private Label label7;
        private Label label9;
        private TextBox tb_usuario_banco_msa;
        private Label label10;
        private TextBox tb_senha_banco_msa;
        private ComboBox cb_banco;
        private ComboBox cb_empresa;
        private Label label8;
        private Button bt_pendencia_processamento;
        private Label label11;
        private ComboBox cb_pendencia_processamento;
        private Label label14;
        private Button bt_comparar_icms;
        private GroupBox groupBox1;
        private Label label13;
        private TextBox tb_ano;
        private Label label12;
        private TextBox tb_mes;
        private CheckBox checkBox1;
        private GroupBox groupBox2;
        private Label label16;
        private DateTimePicker dtp_periodo_fin_qtd_notas;
        private Label label15;
        private DateTimePicker dtp_periodo_ini_qtd_notas;
        private ComboBox cb_empresa_qtd_notas;
        private Label label17;
        private Button bt_qtd_notas;
        private Label label18;
        private Button bt_buraco_nota;
        private CheckBox ckb_buraco_notas_hardcore;
        private Button bt_pessoa_fisica_juridica;
        private Label label19;
        private Label label20;
        private ComboBox comboBox1;
        private MaskedTextBox tb_referenciaBuracoNota;
    }
}
