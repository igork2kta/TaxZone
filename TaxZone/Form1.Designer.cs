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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            bt_diferenca_capa_item = new Button();
            label1 = new Label();
            bt_notas_canceladas = new Button();
            label3 = new Label();
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
            bt_obter_icms_sifar = new Button();
            groupBox1 = new GroupBox();
            label13 = new Label();
            tb_ano = new TextBox();
            label12 = new Label();
            tb_mes = new TextBox();
            groupBox2 = new GroupBox();
            groupBox5 = new GroupBox();
            rb_popular_tabela = new RadioButton();
            rb_arquivo_temp = new RadioButton();
            rb_mostrar_na_tela = new RadioButton();
            lbl_status_qtd_notas = new Label();
            ckb_incluidas_hoje = new CheckBox();
            label20 = new Label();
            progress_bar_qtd_notas = new ProgressBar();
            bt_qtd_notas = new Button();
            cb_local_qtd_notas = new ComboBox();
            cb_empresa_qtd_notas = new ComboBox();
            label17 = new Label();
            label16 = new Label();
            dtp_periodo_fin_qtd_notas = new DateTimePicker();
            label15 = new Label();
            dtp_periodo_ini_qtd_notas = new DateTimePicker();
            ckb_mes_aberto = new CheckBox();
            label18 = new Label();
            bt_buraco_nota = new Button();
            ckb_buraco_notas_hardcore = new CheckBox();
            bt_pessoa_fisica_juridica = new Button();
            label19 = new Label();
            tb_referenciaBuracoNota = new MaskedTextBox();
            ckb_fracionar_valores = new CheckBox();
            ckb_gerar_arquivo = new CheckBox();
            ckb_arq_temporario = new CheckBox();
            ckb_codFisJur = new CheckBox();
            label2 = new Label();
            bt_produtos_taxas = new Button();
            groupBox3 = new GroupBox();
            lbox_empresas = new ListBox();
            lbl_status_tax = new Label();
            ckb_renew_task = new CheckBox();
            progress_bar_tax = new ProgressBar();
            label28 = new Label();
            label27 = new Label();
            tb_usuario_tax = new TextBox();
            tb_senha_tax = new TextBox();
            bt_login = new Button();
            bt_relatorios = new Button();
            ckb_extracao_canceladas = new CheckBox();
            ckb_qtd_canceladas = new CheckBox();
            ckb_qtd_notas = new CheckBox();
            ckb_qtd_itens = new CheckBox();
            ckb_notas_sem_item = new CheckBox();
            ckb_icms_resumido = new CheckBox();
            ckb_diferenca_capa_item = new CheckBox();
            ckb_buraco_notas = new CheckBox();
            label26 = new Label();
            label25 = new Label();
            dtp_tax_data_fim = new DateTimePicker();
            dtp_tax_data_inicio = new DateTimePicker();
            bt_executar_relatorio = new Button();
            label23 = new Label();
            bt_status_tax_automation = new Button();
            label22 = new Label();
            tb_cookie = new TextBox();
            label21 = new Label();
            bt_tax_automation = new Button();
            label4 = new Label();
            bt_atualizar_valores_tax = new Button();
            groupBox4 = new GroupBox();
            label31 = new Label();
            bt_atualizar_comparacao = new Button();
            cb_status = new ComboBox();
            bt_alterar_status = new Button();
            label29 = new Label();
            dtp_fim_comparativo_notas = new DateTimePicker();
            label30 = new Label();
            dtp_inicio_comparativo_notas = new DateTimePicker();
            dgv_comparativo_notas = new DataGridView();
            ckb_always_on_top = new CheckBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_comparativo_notas).BeginInit();
            SuspendLayout();
            // 
            // bt_diferenca_capa_item
            // 
            bt_diferenca_capa_item.Location = new Point(105, 11);
            bt_diferenca_capa_item.Name = "bt_diferenca_capa_item";
            bt_diferenca_capa_item.Size = new Size(82, 23);
            bt_diferenca_capa_item.TabIndex = 10;
            bt_diferenca_capa_item.Text = "Obter notas";
            bt_diferenca_capa_item.UseVisualStyleBackColor = true;
            bt_diferenca_capa_item.Click += bt_diferenca_capa_item_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(67, 13);
            label1.Name = "label1";
            label1.Size = new Size(32, 15);
            label1.TabIndex = 1;
            label1.Text = "Itens";
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
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(34, 493);
            label5.Name = "label5";
            label5.Size = new Size(62, 15);
            label5.TabIndex = 9;
            label5.Text = "Senha FAR";
            // 
            // tb_senha_banco_far
            // 
            tb_senha_banco_far.Location = new Point(100, 489);
            tb_senha_banco_far.Name = "tb_senha_banco_far";
            tb_senha_banco_far.PasswordChar = '*';
            tb_senha_banco_far.Size = new Size(100, 23);
            tb_senha_banco_far.TabIndex = 10;
            tb_senha_banco_far.TextChanged += tb_senha_banco_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(26, 464);
            label6.Name = "label6";
            label6.Size = new Size(70, 15);
            label6.TabIndex = 11;
            label6.Text = "Usuário FAR";
            // 
            // tb_usuario_banco_far
            // 
            tb_usuario_banco_far.Location = new Point(100, 460);
            tb_usuario_banco_far.Name = "tb_usuario_banco_far";
            tb_usuario_banco_far.Size = new Size(100, 23);
            tb_usuario_banco_far.TabIndex = 8;
            tb_usuario_banco_far.TextChanged += tb_usuario_banco_TextChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(270, 38);
            label7.Name = "label7";
            label7.Size = new Size(52, 15);
            label7.TabIndex = 13;
            label7.Text = "Empresa";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(212, 464);
            label9.Name = "label9";
            label9.Size = new Size(75, 15);
            label9.TabIndex = 19;
            label9.Text = "Usuário MSA";
            // 
            // tb_usuario_banco_msa
            // 
            tb_usuario_banco_msa.Location = new Point(288, 460);
            tb_usuario_banco_msa.Name = "tb_usuario_banco_msa";
            tb_usuario_banco_msa.Size = new Size(100, 23);
            tb_usuario_banco_msa.TabIndex = 16;
            tb_usuario_banco_msa.TextChanged += tb_usuario_banco_msa_TextChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(218, 493);
            label10.Name = "label10";
            label10.Size = new Size(67, 15);
            label10.TabIndex = 17;
            label10.Text = "Senha MSA";
            // 
            // tb_senha_banco_msa
            // 
            tb_senha_banco_msa.Location = new Point(288, 489);
            tb_senha_banco_msa.Name = "tb_senha_banco_msa";
            tb_senha_banco_msa.PasswordChar = '*';
            tb_senha_banco_msa.Size = new Size(100, 23);
            tb_senha_banco_msa.TabIndex = 18;
            tb_senha_banco_msa.TextChanged += tb_senha_banco_msa_TextChanged;
            // 
            // cb_banco
            // 
            cb_banco.FormattingEnabled = true;
            cb_banco.Location = new Point(328, 34);
            cb_banco.Name = "cb_banco";
            cb_banco.Size = new Size(59, 23);
            cb_banco.TabIndex = 20;
            // 
            // cb_empresa
            // 
            cb_empresa.FormattingEnabled = true;
            cb_empresa.Location = new Point(174, 265);
            cb_empresa.Name = "cb_empresa";
            cb_empresa.Size = new Size(53, 23);
            cb_empresa.TabIndex = 22;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(116, 269);
            label8.Name = "label8";
            label8.Size = new Size(52, 15);
            label8.TabIndex = 21;
            label8.Text = "Empresa";
            // 
            // bt_pendencia_processamento
            // 
            bt_pendencia_processamento.Location = new Point(334, 265);
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
            label11.Location = new Point(22, 232);
            label11.Name = "label11";
            label11.Size = new Size(146, 15);
            label11.TabIndex = 24;
            label11.Text = "Pendencia Processamento";
            // 
            // cb_pendencia_processamento
            // 
            cb_pendencia_processamento.FormattingEnabled = true;
            cb_pendencia_processamento.Items.AddRange(new object[] { "Notas", "Itens", "Canceladas" });
            cb_pendencia_processamento.Location = new Point(174, 228);
            cb_pendencia_processamento.Name = "cb_pendencia_processamento";
            cb_pendencia_processamento.Size = new Size(139, 23);
            cb_pendencia_processamento.TabIndex = 25;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(27, 75);
            label14.Name = "label14";
            label14.Size = new Size(100, 15);
            label14.TabIndex = 27;
            label14.Text = "Obter ICMS SIFAR";
            // 
            // bt_obter_icms_sifar
            // 
            bt_obter_icms_sifar.Location = new Point(143, 71);
            bt_obter_icms_sifar.Name = "bt_obter_icms_sifar";
            bt_obter_icms_sifar.Size = new Size(87, 23);
            bt_obter_icms_sifar.TabIndex = 26;
            bt_obter_icms_sifar.Text = "Executar";
            bt_obter_icms_sifar.UseVisualStyleBackColor = true;
            bt_obter_icms_sifar.Click += bt_obter_icms_sifar_Click;
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
            groupBox1.Controls.Add(bt_obter_icms_sifar);
            groupBox1.Controls.Add(cb_banco);
            groupBox1.Controls.Add(label7);
            groupBox1.Location = new Point(11, 95);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(589, 117);
            groupBox1.TabIndex = 28;
            groupBox1.TabStop = false;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(377, 75);
            label13.Name = "label13";
            label13.Size = new Size(29, 15);
            label13.TabIndex = 31;
            label13.Text = "Ano";
            // 
            // tb_ano
            // 
            tb_ano.Location = new Point(412, 71);
            tb_ano.Name = "tb_ano";
            tb_ano.Size = new Size(52, 23);
            tb_ano.TabIndex = 30;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(292, 75);
            label12.Name = "label12";
            label12.Size = new Size(29, 15);
            label12.TabIndex = 29;
            label12.Text = "Mês";
            // 
            // tb_mes
            // 
            tb_mes.Location = new Point(327, 71);
            tb_mes.Name = "tb_mes";
            tb_mes.Size = new Size(38, 23);
            tb_mes.TabIndex = 28;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(groupBox5);
            groupBox2.Controls.Add(lbl_status_qtd_notas);
            groupBox2.Controls.Add(ckb_incluidas_hoje);
            groupBox2.Controls.Add(label20);
            groupBox2.Controls.Add(progress_bar_qtd_notas);
            groupBox2.Controls.Add(bt_qtd_notas);
            groupBox2.Controls.Add(cb_local_qtd_notas);
            groupBox2.Controls.Add(cb_empresa_qtd_notas);
            groupBox2.Controls.Add(label17);
            groupBox2.Controls.Add(label16);
            groupBox2.Controls.Add(dtp_periodo_fin_qtd_notas);
            groupBox2.Controls.Add(label15);
            groupBox2.Controls.Add(dtp_periodo_ini_qtd_notas);
            groupBox2.Location = new Point(12, 310);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(588, 145);
            groupBox2.TabIndex = 31;
            groupBox2.TabStop = false;
            groupBox2.Text = "Quantidade notas";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(rb_popular_tabela);
            groupBox5.Controls.Add(rb_arquivo_temp);
            groupBox5.Controls.Add(rb_mostrar_na_tela);
            groupBox5.Location = new Point(14, 68);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(331, 53);
            groupBox5.TabIndex = 59;
            groupBox5.TabStop = false;
            groupBox5.Text = "Formato Saída";
            // 
            // rb_popular_tabela
            // 
            rb_popular_tabela.AutoSize = true;
            rb_popular_tabela.Location = new Point(222, 22);
            rb_popular_tabela.Name = "rb_popular_tabela";
            rb_popular_tabela.Size = new Size(103, 19);
            rb_popular_tabela.TabIndex = 49;
            rb_popular_tabela.TabStop = true;
            rb_popular_tabela.Text = "Popular Tabela";
            rb_popular_tabela.UseVisualStyleBackColor = true;
            // 
            // rb_arquivo_temp
            // 
            rb_arquivo_temp.AutoSize = true;
            rb_arquivo_temp.Location = new Point(6, 22);
            rb_arquivo_temp.Name = "rb_arquivo_temp";
            rb_arquivo_temp.Size = new Size(100, 19);
            rb_arquivo_temp.TabIndex = 47;
            rb_arquivo_temp.TabStop = true;
            rb_arquivo_temp.Text = "Arquivo Temp";
            rb_arquivo_temp.UseVisualStyleBackColor = true;
            // 
            // rb_mostrar_na_tela
            // 
            rb_mostrar_na_tela.AutoSize = true;
            rb_mostrar_na_tela.Location = new Point(112, 22);
            rb_mostrar_na_tela.Name = "rb_mostrar_na_tela";
            rb_mostrar_na_tela.Size = new Size(104, 19);
            rb_mostrar_na_tela.TabIndex = 48;
            rb_mostrar_na_tela.TabStop = true;
            rb_mostrar_na_tela.Text = "Mostrar na tela";
            rb_mostrar_na_tela.UseVisualStyleBackColor = true;
            // 
            // lbl_status_qtd_notas
            // 
            lbl_status_qtd_notas.AutoSize = true;
            lbl_status_qtd_notas.Location = new Point(465, 102);
            lbl_status_qtd_notas.Name = "lbl_status_qtd_notas";
            lbl_status_qtd_notas.Size = new Size(44, 15);
            lbl_status_qtd_notas.TabIndex = 42;
            lbl_status_qtd_notas.Text = "label29";
            lbl_status_qtd_notas.Visible = false;
            // 
            // ckb_incluidas_hoje
            // 
            ckb_incluidas_hoje.AutoSize = true;
            ckb_incluidas_hoje.Checked = true;
            ckb_incluidas_hoje.CheckState = CheckState.Checked;
            ckb_incluidas_hoje.Location = new Point(359, 68);
            ckb_incluidas_hoje.Name = "ckb_incluidas_hoje";
            ckb_incluidas_hoje.Size = new Size(104, 19);
            ckb_incluidas_hoje.TabIndex = 40;
            ckb_incluidas_hoje.Text = "Incluidas hoje?";
            ckb_incluidas_hoje.UseVisualStyleBackColor = true;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(14, 33);
            label20.Name = "label20";
            label20.Size = new Size(35, 15);
            label20.TabIndex = 37;
            label20.Text = "Local";
            // 
            // progress_bar_qtd_notas
            // 
            progress_bar_qtd_notas.Location = new Point(359, 98);
            progress_bar_qtd_notas.Name = "progress_bar_qtd_notas";
            progress_bar_qtd_notas.Size = new Size(100, 23);
            progress_bar_qtd_notas.TabIndex = 41;
            progress_bar_qtd_notas.Visible = false;
            // 
            // bt_qtd_notas
            // 
            bt_qtd_notas.Location = new Point(512, 29);
            bt_qtd_notas.Name = "bt_qtd_notas";
            bt_qtd_notas.Size = new Size(68, 23);
            bt_qtd_notas.TabIndex = 35;
            bt_qtd_notas.Text = "Executar";
            bt_qtd_notas.UseVisualStyleBackColor = true;
            bt_qtd_notas.Click += bt_qtd_notas_Click;
            // 
            // cb_local_qtd_notas
            // 
            cb_local_qtd_notas.FormattingEnabled = true;
            cb_local_qtd_notas.Items.AddRange(new object[] { "SIFAR", "MSA" });
            cb_local_qtd_notas.Location = new Point(55, 29);
            cb_local_qtd_notas.Name = "cb_local_qtd_notas";
            cb_local_qtd_notas.Size = new Size(55, 23);
            cb_local_qtd_notas.TabIndex = 36;
            cb_local_qtd_notas.SelectedIndexChanged += cb_local_qtd_notas_SelectedIndexChanged;
            // 
            // cb_empresa_qtd_notas
            // 
            cb_empresa_qtd_notas.FormattingEnabled = true;
            cb_empresa_qtd_notas.Location = new Point(436, 29);
            cb_empresa_qtd_notas.Name = "cb_empresa_qtd_notas";
            cb_empresa_qtd_notas.Size = new Size(68, 23);
            cb_empresa_qtd_notas.TabIndex = 33;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(378, 33);
            label17.Name = "label17";
            label17.Size = new Size(52, 15);
            label17.TabIndex = 32;
            label17.Text = "Empresa";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(265, 33);
            label16.Name = "label16";
            label16.Size = new Size(13, 15);
            label16.TabIndex = 34;
            label16.Text = "à";
            // 
            // dtp_periodo_fin_qtd_notas
            // 
            dtp_periodo_fin_qtd_notas.Format = DateTimePickerFormat.Short;
            dtp_periodo_fin_qtd_notas.Location = new Point(284, 29);
            dtp_periodo_fin_qtd_notas.Name = "dtp_periodo_fin_qtd_notas";
            dtp_periodo_fin_qtd_notas.Size = new Size(86, 23);
            dtp_periodo_fin_qtd_notas.TabIndex = 33;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(120, 33);
            label15.Name = "label15";
            label15.Size = new Size(48, 15);
            label15.TabIndex = 32;
            label15.Text = "Período";
            // 
            // dtp_periodo_ini_qtd_notas
            // 
            dtp_periodo_ini_qtd_notas.Format = DateTimePickerFormat.Short;
            dtp_periodo_ini_qtd_notas.Location = new Point(174, 29);
            dtp_periodo_ini_qtd_notas.Name = "dtp_periodo_ini_qtd_notas";
            dtp_periodo_ini_qtd_notas.Size = new Size(86, 23);
            dtp_periodo_ini_qtd_notas.TabIndex = 31;
            // 
            // ckb_mes_aberto
            // 
            ckb_mes_aberto.AutoSize = true;
            ckb_mes_aberto.Location = new Point(407, 513);
            ckb_mes_aberto.Name = "ckb_mes_aberto";
            ckb_mes_aberto.Size = new Size(85, 19);
            ckb_mes_aberto.TabIndex = 39;
            ckb_mes_aberto.Text = "Mês aberto";
            ckb_mes_aberto.UseVisualStyleBackColor = true;
            ckb_mes_aberto.CheckedChanged += ckb_mes_aberto_CheckedChanged;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(258, 16);
            label18.Name = "label18";
            label18.Size = new Size(71, 15);
            label18.TabIndex = 32;
            label18.Text = "Buraco nota";
            // 
            // bt_buraco_nota
            // 
            bt_buraco_nota.Location = new Point(339, 12);
            bt_buraco_nota.Name = "bt_buraco_nota";
            bt_buraco_nota.Size = new Size(86, 23);
            bt_buraco_nota.TabIndex = 30;
            bt_buraco_nota.Text = "Obter notas";
            bt_buraco_nota.UseVisualStyleBackColor = true;
            bt_buraco_nota.Click += bt_buraco_nota_Click;
            // 
            // ckb_buraco_notas_hardcore
            // 
            ckb_buraco_notas_hardcore.AutoSize = true;
            ckb_buraco_notas_hardcore.Location = new Point(432, 15);
            ckb_buraco_notas_hardcore.Name = "ckb_buraco_notas_hardcore";
            ckb_buraco_notas_hardcore.Size = new Size(110, 19);
            ckb_buraco_notas_hardcore.TabIndex = 40;
            ckb_buraco_notas_hardcore.Text = "Modo Hardcore";
            ckb_buraco_notas_hardcore.UseVisualStyleBackColor = true;
            ckb_buraco_notas_hardcore.CheckedChanged += ckb_buraco_notas_hardcore_CheckedChanged;
            // 
            // bt_pessoa_fisica_juridica
            // 
            bt_pessoa_fisica_juridica.Location = new Point(338, 53);
            bt_pessoa_fisica_juridica.Name = "bt_pessoa_fisica_juridica";
            bt_pessoa_fisica_juridica.Size = new Size(86, 23);
            bt_pessoa_fisica_juridica.TabIndex = 20;
            bt_pessoa_fisica_juridica.Text = "Obter";
            bt_pessoa_fisica_juridica.UseVisualStyleBackColor = true;
            bt_pessoa_fisica_juridica.Click += bt_pessoa_fisica_juridica_Click;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(209, 57);
            label19.Name = "label19";
            label19.Size = new Size(117, 15);
            label19.TabIndex = 35;
            label19.Text = "Pessoa fisica/juridica";
            // 
            // tb_referenciaBuracoNota
            // 
            tb_referenciaBuracoNota.Location = new Point(548, 13);
            tb_referenciaBuracoNota.Mask = "00_0000";
            tb_referenciaBuracoNota.Name = "tb_referenciaBuracoNota";
            tb_referenciaBuracoNota.Size = new Size(52, 23);
            tb_referenciaBuracoNota.TabIndex = 3;
            tb_referenciaBuracoNota.ValidatingType = typeof(DateTime);
            tb_referenciaBuracoNota.Visible = false;
            // 
            // ckb_fracionar_valores
            // 
            ckb_fracionar_valores.AutoSize = true;
            ckb_fracionar_valores.Checked = true;
            ckb_fracionar_valores.CheckState = CheckState.Checked;
            ckb_fracionar_valores.Location = new Point(407, 486);
            ckb_fracionar_valores.Name = "ckb_fracionar_valores";
            ckb_fracionar_valores.Size = new Size(115, 19);
            ckb_fracionar_valores.TabIndex = 38;
            ckb_fracionar_valores.Text = "Fracionar valores";
            ckb_fracionar_valores.UseVisualStyleBackColor = true;
            ckb_fracionar_valores.CheckedChanged += ckb_fracionar_valores_CheckedChanged;
            // 
            // ckb_gerar_arquivo
            // 
            ckb_gerar_arquivo.AutoSize = true;
            ckb_gerar_arquivo.Location = new Point(407, 461);
            ckb_gerar_arquivo.Name = "ckb_gerar_arquivo";
            ckb_gerar_arquivo.Size = new Size(99, 19);
            ckb_gerar_arquivo.TabIndex = 39;
            ckb_gerar_arquivo.Text = "Gerar Arquivo";
            ckb_gerar_arquivo.UseVisualStyleBackColor = true;
            ckb_gerar_arquivo.CheckedChanged += ckb_gerar_arquivo_CheckedChanged;
            // 
            // ckb_arq_temporario
            // 
            ckb_arq_temporario.AutoSize = true;
            ckb_arq_temporario.Location = new Point(334, 232);
            ckb_arq_temporario.Name = "ckb_arq_temporario";
            ckb_arq_temporario.Size = new Size(182, 19);
            ckb_arq_temporario.TabIndex = 40;
            ckb_arq_temporario.Text = "Buscar de arquivo temporário";
            ckb_arq_temporario.UseVisualStyleBackColor = true;
            ckb_arq_temporario.CheckedChanged += ckb_arq_temporario_CheckedChanged;
            // 
            // ckb_codFisJur
            // 
            ckb_codFisJur.AutoSize = true;
            ckb_codFisJur.Location = new Point(432, 55);
            ckb_codFisJur.Name = "ckb_codFisJur";
            ckb_codFisJur.Size = new Size(131, 19);
            ckb_codFisJur.TabIndex = 41;
            ckb_codFisJur.Text = "CodFisJur completo";
            ckb_codFisJur.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(11, 57);
            label2.Name = "label2";
            label2.Size = new Size(88, 15);
            label2.TabIndex = 42;
            label2.Text = "Produtos/Taxas";
            // 
            // bt_produtos_taxas
            // 
            bt_produtos_taxas.Location = new Point(105, 53);
            bt_produtos_taxas.Name = "bt_produtos_taxas";
            bt_produtos_taxas.Size = new Size(82, 23);
            bt_produtos_taxas.TabIndex = 5;
            bt_produtos_taxas.Text = "Obter";
            bt_produtos_taxas.UseVisualStyleBackColor = true;
            bt_produtos_taxas.Click += bt_produtos_taxas_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(lbox_empresas);
            groupBox3.Controls.Add(lbl_status_tax);
            groupBox3.Controls.Add(ckb_renew_task);
            groupBox3.Controls.Add(progress_bar_tax);
            groupBox3.Controls.Add(label28);
            groupBox3.Controls.Add(label27);
            groupBox3.Controls.Add(tb_usuario_tax);
            groupBox3.Controls.Add(tb_senha_tax);
            groupBox3.Controls.Add(bt_login);
            groupBox3.Controls.Add(bt_relatorios);
            groupBox3.Controls.Add(ckb_extracao_canceladas);
            groupBox3.Controls.Add(ckb_qtd_canceladas);
            groupBox3.Controls.Add(ckb_qtd_notas);
            groupBox3.Controls.Add(ckb_qtd_itens);
            groupBox3.Controls.Add(ckb_notas_sem_item);
            groupBox3.Controls.Add(ckb_icms_resumido);
            groupBox3.Controls.Add(ckb_diferenca_capa_item);
            groupBox3.Controls.Add(ckb_buraco_notas);
            groupBox3.Controls.Add(label26);
            groupBox3.Controls.Add(label25);
            groupBox3.Controls.Add(dtp_tax_data_fim);
            groupBox3.Controls.Add(dtp_tax_data_inicio);
            groupBox3.Controls.Add(bt_executar_relatorio);
            groupBox3.Controls.Add(label23);
            groupBox3.Controls.Add(bt_status_tax_automation);
            groupBox3.Controls.Add(label22);
            groupBox3.Controls.Add(tb_cookie);
            groupBox3.Controls.Add(label21);
            groupBox3.Controls.Add(bt_tax_automation);
            groupBox3.Controls.Add(label4);
            groupBox3.Location = new Point(606, 8);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(477, 526);
            groupBox3.TabIndex = 44;
            groupBox3.TabStop = false;
            groupBox3.Text = "Tax API";
            // 
            // lbox_empresas
            // 
            lbox_empresas.ColumnWidth = 40;
            lbox_empresas.FormattingEnabled = true;
            lbox_empresas.ItemHeight = 15;
            lbox_empresas.Location = new Point(121, 28);
            lbox_empresas.MultiColumn = true;
            lbox_empresas.Name = "lbox_empresas";
            lbox_empresas.SelectionMode = SelectionMode.MultiExtended;
            lbox_empresas.Size = new Size(127, 49);
            lbox_empresas.TabIndex = 59;
            lbox_empresas.SelectedIndexChanged += lbox_empresas_SelectedIndexChanged;
            // 
            // lbl_status_tax
            // 
            lbl_status_tax.AutoSize = true;
            lbl_status_tax.Location = new Point(132, 406);
            lbl_status_tax.Name = "lbl_status_tax";
            lbl_status_tax.Size = new Size(44, 15);
            lbl_status_tax.TabIndex = 44;
            lbl_status_tax.Text = "label29";
            lbl_status_tax.Visible = false;
            // 
            // ckb_renew_task
            // 
            ckb_renew_task.AutoSize = true;
            ckb_renew_task.Location = new Point(354, 266);
            ckb_renew_task.Name = "ckb_renew_task";
            ckb_renew_task.RightToLeft = RightToLeft.Yes;
            ckb_renew_task.Size = new Size(112, 19);
            ckb_renew_task.TabIndex = 58;
            ckb_renew_task.Text = "Renovar cookies";
            ckb_renew_task.UseVisualStyleBackColor = true;
            ckb_renew_task.CheckedChanged += ckb_renew_task_CheckedChanged;
            // 
            // progress_bar_tax
            // 
            progress_bar_tax.Location = new Point(21, 402);
            progress_bar_tax.Name = "progress_bar_tax";
            progress_bar_tax.Size = new Size(100, 23);
            progress_bar_tax.TabIndex = 43;
            progress_bar_tax.Visible = false;
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new Point(290, 199);
            label28.Name = "label28";
            label28.Size = new Size(66, 15);
            label28.TabIndex = 57;
            label28.Text = "Senha TAX:";
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new Point(285, 168);
            label27.Name = "label27";
            label27.Size = new Size(74, 15);
            label27.TabIndex = 56;
            label27.Text = "Usuario TAX:";
            // 
            // tb_usuario_tax
            // 
            tb_usuario_tax.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tb_usuario_tax.Location = new Point(362, 162);
            tb_usuario_tax.Name = "tb_usuario_tax";
            tb_usuario_tax.Size = new Size(100, 23);
            tb_usuario_tax.TabIndex = 55;
            tb_usuario_tax.TextChanged += tb_usuario_tax_TextChanged;
            // 
            // tb_senha_tax
            // 
            tb_senha_tax.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tb_senha_tax.Location = new Point(362, 191);
            tb_senha_tax.Name = "tb_senha_tax";
            tb_senha_tax.PasswordChar = '*';
            tb_senha_tax.Size = new Size(100, 23);
            tb_senha_tax.TabIndex = 56;
            tb_senha_tax.TextChanged += tb_senha_tax_TextChanged;
            // 
            // bt_login
            // 
            bt_login.Location = new Point(387, 229);
            bt_login.Name = "bt_login";
            bt_login.Size = new Size(75, 23);
            bt_login.TabIndex = 57;
            bt_login.Text = "Login";
            bt_login.UseVisualStyleBackColor = true;
            bt_login.Click += bt_login_Click;
            // 
            // bt_relatorios
            // 
            bt_relatorios.Location = new Point(230, 357);
            bt_relatorios.Name = "bt_relatorios";
            bt_relatorios.Size = new Size(75, 23);
            bt_relatorios.TabIndex = 53;
            bt_relatorios.Text = "Relatorios";
            bt_relatorios.UseVisualStyleBackColor = true;
            bt_relatorios.Click += bt_relatorios_Click;
            // 
            // ckb_extracao_canceladas
            // 
            ckb_extracao_canceladas.AutoSize = true;
            ckb_extracao_canceladas.Location = new Point(60, 331);
            ckb_extracao_canceladas.Name = "ckb_extracao_canceladas";
            ckb_extracao_canceladas.RightToLeft = RightToLeft.Yes;
            ckb_extracao_canceladas.Size = new Size(147, 19);
            ckb_extracao_canceladas.TabIndex = 51;
            ckb_extracao_canceladas.Text = "Extração de canceladas";
            ckb_extracao_canceladas.UseVisualStyleBackColor = true;
            // 
            // ckb_qtd_canceladas
            // 
            ckb_qtd_canceladas.AutoSize = true;
            ckb_qtd_canceladas.Checked = true;
            ckb_qtd_canceladas.CheckState = CheckState.Checked;
            ckb_qtd_canceladas.Location = new Point(8, 302);
            ckb_qtd_canceladas.Name = "ckb_qtd_canceladas";
            ckb_qtd_canceladas.RightToLeft = RightToLeft.Yes;
            ckb_qtd_canceladas.Size = new Size(199, 19);
            ckb_qtd_canceladas.TabIndex = 50;
            ckb_qtd_canceladas.Text = "Quantidade de notas Canceladas";
            ckb_qtd_canceladas.UseVisualStyleBackColor = true;
            // 
            // ckb_qtd_notas
            // 
            ckb_qtd_notas.AutoSize = true;
            ckb_qtd_notas.Checked = true;
            ckb_qtd_notas.CheckState = CheckState.Checked;
            ckb_qtd_notas.Location = new Point(71, 277);
            ckb_qtd_notas.Name = "ckb_qtd_notas";
            ckb_qtd_notas.RightToLeft = RightToLeft.Yes;
            ckb_qtd_notas.Size = new Size(136, 19);
            ckb_qtd_notas.TabIndex = 49;
            ckb_qtd_notas.Text = "Quantidade de notas";
            ckb_qtd_notas.UseVisualStyleBackColor = true;
            // 
            // ckb_qtd_itens
            // 
            ckb_qtd_itens.AutoSize = true;
            ckb_qtd_itens.Checked = true;
            ckb_qtd_itens.CheckState = CheckState.Checked;
            ckb_qtd_itens.Location = new Point(75, 252);
            ckb_qtd_itens.Name = "ckb_qtd_itens";
            ckb_qtd_itens.RightToLeft = RightToLeft.Yes;
            ckb_qtd_itens.Size = new Size(132, 19);
            ckb_qtd_itens.TabIndex = 48;
            ckb_qtd_itens.Text = "Quantidade de itens";
            ckb_qtd_itens.UseVisualStyleBackColor = true;
            // 
            // ckb_notas_sem_item
            // 
            ckb_notas_sem_item.AutoSize = true;
            ckb_notas_sem_item.Location = new Point(98, 227);
            ckb_notas_sem_item.Name = "ckb_notas_sem_item";
            ckb_notas_sem_item.RightToLeft = RightToLeft.Yes;
            ckb_notas_sem_item.Size = new Size(109, 19);
            ckb_notas_sem_item.TabIndex = 47;
            ckb_notas_sem_item.Text = "Notas sem item";
            ckb_notas_sem_item.UseVisualStyleBackColor = true;
            // 
            // ckb_icms_resumido
            // 
            ckb_icms_resumido.AutoSize = true;
            ckb_icms_resumido.Location = new Point(97, 202);
            ckb_icms_resumido.Name = "ckb_icms_resumido";
            ckb_icms_resumido.RightToLeft = RightToLeft.Yes;
            ckb_icms_resumido.Size = new Size(110, 19);
            ckb_icms_resumido.TabIndex = 46;
            ckb_icms_resumido.Text = "ICMS Resumido";
            ckb_icms_resumido.UseVisualStyleBackColor = true;
            // 
            // ckb_diferenca_capa_item
            // 
            ckb_diferenca_capa_item.AutoSize = true;
            ckb_diferenca_capa_item.Location = new Point(74, 177);
            ckb_diferenca_capa_item.Name = "ckb_diferenca_capa_item";
            ckb_diferenca_capa_item.RightToLeft = RightToLeft.Yes;
            ckb_diferenca_capa_item.Size = new Size(133, 19);
            ckb_diferenca_capa_item.TabIndex = 45;
            ckb_diferenca_capa_item.Text = "Diferença capa-item";
            ckb_diferenca_capa_item.UseVisualStyleBackColor = true;
            // 
            // ckb_buraco_notas
            // 
            ckb_buraco_notas.AutoSize = true;
            ckb_buraco_notas.Location = new Point(96, 152);
            ckb_buraco_notas.Name = "ckb_buraco_notas";
            ckb_buraco_notas.RightToLeft = RightToLeft.Yes;
            ckb_buraco_notas.Size = new Size(111, 19);
            ckb_buraco_notas.TabIndex = 44;
            ckb_buraco_notas.Text = "Buraco de notas";
            ckb_buraco_notas.UseVisualStyleBackColor = true;
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(57, 119);
            label26.Name = "label26";
            label26.Size = new Size(55, 15);
            label26.TabIndex = 43;
            label26.Text = "Data fim:";
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(46, 90);
            label25.Name = "label25";
            label25.Size = new Size(66, 15);
            label25.TabIndex = 42;
            label25.Text = "Data início:";
            // 
            // dtp_tax_data_fim
            // 
            dtp_tax_data_fim.Format = DateTimePickerFormat.Short;
            dtp_tax_data_fim.Location = new Point(121, 119);
            dtp_tax_data_fim.Name = "dtp_tax_data_fim";
            dtp_tax_data_fim.Size = new Size(86, 23);
            dtp_tax_data_fim.TabIndex = 42;
            // 
            // dtp_tax_data_inicio
            // 
            dtp_tax_data_inicio.Format = DateTimePickerFormat.Short;
            dtp_tax_data_inicio.Location = new Point(121, 86);
            dtp_tax_data_inicio.Name = "dtp_tax_data_inicio";
            dtp_tax_data_inicio.Size = new Size(86, 23);
            dtp_tax_data_inicio.TabIndex = 41;
            // 
            // bt_executar_relatorio
            // 
            bt_executar_relatorio.Location = new Point(132, 357);
            bt_executar_relatorio.Name = "bt_executar_relatorio";
            bt_executar_relatorio.Size = new Size(75, 23);
            bt_executar_relatorio.TabIndex = 52;
            bt_executar_relatorio.Text = "Executar";
            bt_executar_relatorio.UseVisualStyleBackColor = true;
            bt_executar_relatorio.Click += bt_executar_relatorio_Click;
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(255, 134);
            label23.Name = "label23";
            label23.Size = new Size(126, 15);
            label23.TabIndex = 28;
            label23.Text = "Status Tax Automation";
            // 
            // bt_status_tax_automation
            // 
            bt_status_tax_automation.Location = new Point(387, 130);
            bt_status_tax_automation.Name = "bt_status_tax_automation";
            bt_status_tax_automation.Size = new Size(75, 23);
            bt_status_tax_automation.TabIndex = 27;
            bt_status_tax_automation.Text = "Disparar";
            bt_status_tax_automation.UseVisualStyleBackColor = true;
            bt_status_tax_automation.Click += bt_status_tax_automation_Click;
            // 
            // label22
            // 
            label22.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label22.AutoSize = true;
            label22.Location = new Point(21, 441);
            label22.Name = "label22";
            label22.Size = new Size(44, 15);
            label22.TabIndex = 26;
            label22.Text = "Cookie";
            // 
            // tb_cookie
            // 
            tb_cookie.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            tb_cookie.Location = new Point(21, 463);
            tb_cookie.Multiline = true;
            tb_cookie.Name = "tb_cookie";
            tb_cookie.Size = new Size(441, 52);
            tb_cookie.TabIndex = 25;
            tb_cookie.TextChanged += tb_cookie_TextChanged;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(290, 105);
            label21.Name = "label21";
            label21.Size = new Size(91, 15);
            label21.TabIndex = 24;
            label21.Text = "Tax Automation";
            // 
            // bt_tax_automation
            // 
            bt_tax_automation.Location = new Point(387, 101);
            bt_tax_automation.Name = "bt_tax_automation";
            bt_tax_automation.Size = new Size(75, 23);
            bt_tax_automation.TabIndex = 23;
            bt_tax_automation.Text = "Disparar";
            bt_tax_automation.UseVisualStyleBackColor = true;
            bt_tax_automation.Click += bt_tax_automation_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(57, 26);
            label4.Name = "label4";
            label4.Size = new Size(55, 15);
            label4.TabIndex = 21;
            label4.Text = "Empresa:";
            // 
            // bt_atualizar_valores_tax
            // 
            bt_atualizar_valores_tax.Location = new Point(18, 204);
            bt_atualizar_valores_tax.Name = "bt_atualizar_valores_tax";
            bt_atualizar_valores_tax.Size = new Size(75, 23);
            bt_atualizar_valores_tax.TabIndex = 59;
            bt_atualizar_valores_tax.Text = "Atualizar";
            bt_atualizar_valores_tax.UseVisualStyleBackColor = true;
            bt_atualizar_valores_tax.Click += bt_atualizar_valores_tax_Click;
            // 
            // groupBox4
            // 
            groupBox4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox4.Controls.Add(label31);
            groupBox4.Controls.Add(bt_atualizar_valores_tax);
            groupBox4.Controls.Add(bt_atualizar_comparacao);
            groupBox4.Controls.Add(cb_status);
            groupBox4.Controls.Add(bt_alterar_status);
            groupBox4.Controls.Add(label29);
            groupBox4.Controls.Add(dtp_fim_comparativo_notas);
            groupBox4.Controls.Add(label30);
            groupBox4.Controls.Add(dtp_inicio_comparativo_notas);
            groupBox4.Controls.Add(dgv_comparativo_notas);
            groupBox4.Location = new Point(1089, 8);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(720, 524);
            groupBox4.TabIndex = 46;
            groupBox4.TabStop = false;
            groupBox4.Text = "Comparação Quantidades";
            // 
            // label31
            // 
            label31.AutoSize = true;
            label31.Location = new Point(6, 183);
            label31.Name = "label31";
            label31.Size = new Size(71, 15);
            label31.TabIndex = 60;
            label31.Text = "Valores TAX:";
            // 
            // bt_atualizar_comparacao
            // 
            bt_atualizar_comparacao.Location = new Point(639, 28);
            bt_atualizar_comparacao.Name = "bt_atualizar_comparacao";
            bt_atualizar_comparacao.Size = new Size(75, 23);
            bt_atualizar_comparacao.TabIndex = 53;
            bt_atualizar_comparacao.Text = "Atualizar";
            bt_atualizar_comparacao.UseVisualStyleBackColor = true;
            bt_atualizar_comparacao.Click += bt_atualizar_comparacao_Click;
            // 
            // cb_status
            // 
            cb_status.FormattingEnabled = true;
            cb_status.Items.AddRange(new object[] { "EM ANDAMENTO", "LIBERADO" });
            cb_status.Location = new Point(400, 27);
            cb_status.Name = "cb_status";
            cb_status.Size = new Size(121, 23);
            cb_status.TabIndex = 52;
            // 
            // bt_alterar_status
            // 
            bt_alterar_status.Location = new Point(527, 27);
            bt_alterar_status.Name = "bt_alterar_status";
            bt_alterar_status.Size = new Size(88, 23);
            bt_alterar_status.TabIndex = 51;
            bt_alterar_status.Text = "Alterar Status";
            bt_alterar_status.UseVisualStyleBackColor = true;
            bt_alterar_status.Click += bt_alterar_status_Click;
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Location = new Point(245, 31);
            label29.Name = "label29";
            label29.Size = new Size(13, 15);
            label29.TabIndex = 50;
            label29.Text = "à";
            // 
            // dtp_fim_comparativo_notas
            // 
            dtp_fim_comparativo_notas.Format = DateTimePickerFormat.Short;
            dtp_fim_comparativo_notas.Location = new Point(264, 27);
            dtp_fim_comparativo_notas.Name = "dtp_fim_comparativo_notas";
            dtp_fim_comparativo_notas.Size = new Size(86, 23);
            dtp_fim_comparativo_notas.TabIndex = 49;
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(100, 31);
            label30.Name = "label30";
            label30.Size = new Size(48, 15);
            label30.TabIndex = 48;
            label30.Text = "Período";
            // 
            // dtp_inicio_comparativo_notas
            // 
            dtp_inicio_comparativo_notas.Format = DateTimePickerFormat.Short;
            dtp_inicio_comparativo_notas.Location = new Point(154, 27);
            dtp_inicio_comparativo_notas.Name = "dtp_inicio_comparativo_notas";
            dtp_inicio_comparativo_notas.Size = new Size(86, 23);
            dtp_inicio_comparativo_notas.TabIndex = 47;
            // 
            // dgv_comparativo_notas
            // 
            dgv_comparativo_notas.AllowUserToAddRows = false;
            dgv_comparativo_notas.AllowUserToDeleteRows = false;
            dgv_comparativo_notas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgv_comparativo_notas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv_comparativo_notas.BackgroundColor = SystemColors.Control;
            dgv_comparativo_notas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgv_comparativo_notas.DefaultCellStyle = dataGridViewCellStyle1;
            dgv_comparativo_notas.EditMode = DataGridViewEditMode.EditOnEnter;
            dgv_comparativo_notas.Location = new Point(99, 59);
            dgv_comparativo_notas.Name = "dgv_comparativo_notas";
            dgv_comparativo_notas.RowHeadersVisible = false;
            dgv_comparativo_notas.Size = new Size(615, 456);
            dgv_comparativo_notas.TabIndex = 46;
            dgv_comparativo_notas.CellEndEdit += dgv_comparativo_notas_CellEndEdit;
            dgv_comparativo_notas.CellFormatting += dgv_comparativo_notas_CellFormatting;
            // 
            // ckb_always_on_top
            // 
            ckb_always_on_top.AutoSize = true;
            ckb_always_on_top.Location = new Point(17, 524);
            ckb_always_on_top.Name = "ckb_always_on_top";
            ckb_always_on_top.Size = new Size(101, 19);
            ckb_always_on_top.TabIndex = 47;
            ckb_always_on_top.Text = "Always on top";
            ckb_always_on_top.UseVisualStyleBackColor = true;
            ckb_always_on_top.CheckedChanged += ckb_always_on_top_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1821, 546);
            Controls.Add(ckb_always_on_top);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(bt_produtos_taxas);
            Controls.Add(label2);
            Controls.Add(ckb_codFisJur);
            Controls.Add(ckb_arq_temporario);
            Controls.Add(ckb_mes_aberto);
            Controls.Add(ckb_gerar_arquivo);
            Controls.Add(ckb_fracionar_valores);
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
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_comparativo_notas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button bt_diferenca_capa_item;
        private Label label1;
        private Button bt_notas_canceladas;
        private Label label3;
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
        private Button bt_obter_icms_sifar;
        private GroupBox groupBox1;
        private Label label13;
        private TextBox tb_ano;
        private Label label12;
        private TextBox tb_mes;
        private CheckBox ckb_mes_aberto;
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
        private MaskedTextBox tb_referenciaBuracoNota;
        private CheckBox ckb_fracionar_valores;
        private ComboBox cb_local_qtd_notas;
        private CheckBox ckb_gerar_arquivo;
        private CheckBox ckb_arq_temporario;
        private CheckBox ckb_codFisJur;
        private Label label2;
        private Button bt_produtos_taxas;
        private GroupBox groupBox3;
        private Label label4;
        private Label label21;
        private Button bt_tax_automation;
        private Label label22;
        private TextBox tb_cookie;
        private Button bt_status_tax_automation;
        private Label label23;
        private CheckBox ckb_incluidas_hoje;
        private Button bt_executar_relatorio;
        private Label label26;
        private Label label25;
        private DateTimePicker dtp_tax_data_fim;
        private DateTimePicker dtp_tax_data_inicio;
        private CheckBox ckb_buraco_notas;
        private CheckBox ckb_diferenca_capa_item;
        private CheckBox ckb_notas_sem_item;
        private CheckBox ckb_icms_resumido;
        private CheckBox ckb_extracao_canceladas;
        private CheckBox ckb_qtd_canceladas;
        private CheckBox ckb_qtd_notas;
        private CheckBox ckb_qtd_itens;
        private Button bt_relatorios;
        private Button button1;
        private Button bt_login;
        private Label label27;
        private TextBox tb_usuario_tax;
        private TextBox tb_senha_tax;
        private Label label28;
        private CheckBox ckb_renew_task;
        private Label lbl_status_qtd_notas;
        private ProgressBar progress_bar_qtd_notas;
        private Label lbl_status_tax;
        private ProgressBar progress_bar_tax;
        private GroupBox groupBox4;
        private DataGridView dgv_comparativo_notas;
        private Label label29;
        private DateTimePicker dtp_fim_comparativo_notas;
        private Label label30;
        private DateTimePicker dtp_inicio_comparativo_notas;
        private Button bt_alterar_status;
        private ComboBox cb_status;
        private GroupBox groupBox5;
        private RadioButton rb_popular_tabela;
        private RadioButton rb_arquivo_temp;
        private RadioButton rb_mostrar_na_tela;
        private Button bt_atualizar_comparacao;
        private CheckBox ckb_always_on_top;
        private Button bt_atualizar_valores_tax;
        private Label label31;
        private ListBox lbox_empresas;
    }
}
