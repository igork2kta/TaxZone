namespace TaxZone
{
    partial class F_Main_V2
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_Main_V2));
            lbox_empresas = new ListBox();
            label4 = new Label();
            groupBox1 = new GroupBox();
            bt_resetar_contexto = new Button();
            ckb_gerar_arquivo = new CheckBox();
            ckb_fracionar_valores = new CheckBox();
            ckb_renew_task = new CheckBox();
            ckb_mes_aberto = new CheckBox();
            tb_cookie = new TextBox();
            bt_login = new Button();
            dtp_periodo_fim = new DateTimePicker();
            label30 = new Label();
            dtp_periodo_inicio = new DateTimePicker();
            dgv_comparativo_notas = new DataGridView();
            groupBox2 = new GroupBox();
            label24 = new Label();
            bt_atualizar_comparacao = new Button();
            cb_status = new ComboBox();
            bt_alterar_status = new Button();
            bt_popular_tabela_sifar = new Button();
            bt_atualizar_valores_tax = new Button();
            menuStrip1 = new MenuStrip();
            configuraçõesToolStripMenuItem = new ToolStripMenuItem();
            credenciaisToolStripMenuItem = new ToolStripMenuItem();
            DiretorioPadraoEntradaToolStripMenuItem = new ToolStripMenuItem();
            DiretorioPadraoSaidaToolStripMenuItem = new ToolStripMenuItem();
            AbrirInterfaceAntigaToolStripMenuItem = new ToolStripMenuItem();
            sobreToolStripMenuItem = new ToolStripMenuItem();
            cb_ferramentas = new ComboBox();
            bt_ferramentas = new Button();
            label1 = new Label();
            groupBox3 = new GroupBox();
            ckb_mostrar_na_tela = new CheckBox();
            label14 = new Label();
            bt_obter_icms_sifar = new Button();
            label2 = new Label();
            bt_qtd_notas = new Button();
            cb_local_qtd_notas = new ComboBox();
            groupBox4 = new GroupBox();
            ckb_job_automatico = new CheckBox();
            label5 = new Label();
            dgv_pendencia_processamento = new DataGridView();
            groupBox6 = new GroupBox();
            ckb_buraco_notas = new CheckBox();
            bt_executar_relatorio = new Button();
            ckb_diferenca_capa_item = new CheckBox();
            ckb_icms_resumido = new CheckBox();
            ckb_notas_sem_item = new CheckBox();
            bt_relatorios = new Button();
            ckb_qtd_itens = new CheckBox();
            ckb_extracao_canceladas = new CheckBox();
            ckb_qtd_notas = new CheckBox();
            ckb_qtd_canceladas = new CheckBox();
            groupBox5 = new GroupBox();
            bt_executar_job = new Button();
            bt_logs_processos_importacao = new Button();
            label3 = new Label();
            textBox1 = new TextBox();
            statusStrip = new StatusStrip();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_comparativo_notas).BeginInit();
            groupBox2.SuspendLayout();
            menuStrip1.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_pendencia_processamento).BeginInit();
            groupBox6.SuspendLayout();
            groupBox5.SuspendLayout();
            SuspendLayout();
            // 
            // lbox_empresas
            // 
            lbox_empresas.ColumnWidth = 40;
            lbox_empresas.FormattingEnabled = true;
            lbox_empresas.ItemHeight = 15;
            lbox_empresas.Location = new Point(76, 31);
            lbox_empresas.MultiColumn = true;
            lbox_empresas.Name = "lbox_empresas";
            lbox_empresas.SelectionMode = SelectionMode.MultiExtended;
            lbox_empresas.Size = new Size(127, 49);
            lbox_empresas.TabIndex = 61;
            lbox_empresas.SelectedIndexChanged += lbox_empresas_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = SystemColors.MenuText;
            label4.Location = new Point(15, 31);
            label4.Name = "label4";
            label4.Size = new Size(55, 15);
            label4.TabIndex = 60;
            label4.Text = "Empresa:";
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(bt_resetar_contexto);
            groupBox1.Controls.Add(ckb_gerar_arquivo);
            groupBox1.Controls.Add(ckb_fracionar_valores);
            groupBox1.Controls.Add(ckb_renew_task);
            groupBox1.Controls.Add(ckb_mes_aberto);
            groupBox1.Controls.Add(tb_cookie);
            groupBox1.Controls.Add(bt_login);
            groupBox1.Controls.Add(dtp_periodo_fim);
            groupBox1.Controls.Add(label30);
            groupBox1.Controls.Add(dtp_periodo_inicio);
            groupBox1.Controls.Add(lbox_empresas);
            groupBox1.Controls.Add(label4);
            groupBox1.ForeColor = SystemColors.ControlText;
            groupBox1.Location = new Point(13, 27);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1059, 93);
            groupBox1.TabIndex = 62;
            groupBox1.TabStop = false;
            groupBox1.Text = "GLOBAL";
            // 
            // bt_resetar_contexto
            // 
            bt_resetar_contexto.Location = new Point(549, 63);
            bt_resetar_contexto.Name = "bt_resetar_contexto";
            bt_resetar_contexto.Size = new Size(112, 23);
            bt_resetar_contexto.TabIndex = 78;
            bt_resetar_contexto.Text = "Resetar Contexto";
            bt_resetar_contexto.UseVisualStyleBackColor = true;
            bt_resetar_contexto.Click += bt_resetar_contexto_Click;
            // 
            // ckb_gerar_arquivo
            // 
            ckb_gerar_arquivo.AutoSize = true;
            ckb_gerar_arquivo.Location = new Point(396, 43);
            ckb_gerar_arquivo.Name = "ckb_gerar_arquivo";
            ckb_gerar_arquivo.Size = new Size(99, 19);
            ckb_gerar_arquivo.TabIndex = 77;
            ckb_gerar_arquivo.Text = "Gerar Arquivo";
            ckb_gerar_arquivo.UseVisualStyleBackColor = true;
            ckb_gerar_arquivo.CheckedChanged += ckb_gerar_arquivo_CheckedChanged;
            // 
            // ckb_fracionar_valores
            // 
            ckb_fracionar_valores.AutoSize = true;
            ckb_fracionar_valores.Checked = true;
            ckb_fracionar_valores.CheckState = CheckState.Checked;
            ckb_fracionar_valores.Location = new Point(396, 64);
            ckb_fracionar_valores.Name = "ckb_fracionar_valores";
            ckb_fracionar_valores.Size = new Size(115, 19);
            ckb_fracionar_valores.TabIndex = 76;
            ckb_fracionar_valores.Text = "Fracionar valores";
            ckb_fracionar_valores.UseVisualStyleBackColor = true;
            ckb_fracionar_valores.CheckedChanged += ckb_fracionar_valores_CheckedChanged;
            // 
            // ckb_renew_task
            // 
            ckb_renew_task.AutoSize = true;
            ckb_renew_task.Location = new Point(549, 43);
            ckb_renew_task.Name = "ckb_renew_task";
            ckb_renew_task.RightToLeft = RightToLeft.No;
            ckb_renew_task.Size = new Size(112, 19);
            ckb_renew_task.TabIndex = 68;
            ckb_renew_task.Text = "Renovar cookies";
            ckb_renew_task.TextAlign = ContentAlignment.MiddleCenter;
            ckb_renew_task.UseVisualStyleBackColor = true;
            ckb_renew_task.CheckedChanged += ckb_renew_task_CheckedChanged;
            // 
            // ckb_mes_aberto
            // 
            ckb_mes_aberto.AutoSize = true;
            ckb_mes_aberto.Location = new Point(396, 22);
            ckb_mes_aberto.Name = "ckb_mes_aberto";
            ckb_mes_aberto.Size = new Size(85, 19);
            ckb_mes_aberto.TabIndex = 66;
            ckb_mes_aberto.Text = "Mês aberto";
            ckb_mes_aberto.UseVisualStyleBackColor = true;
            ckb_mes_aberto.CheckedChanged += ckb_mes_aberto_CheckedChanged;
            // 
            // tb_cookie
            // 
            tb_cookie.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tb_cookie.Location = new Point(676, 18);
            tb_cookie.Multiline = true;
            tb_cookie.Name = "tb_cookie";
            tb_cookie.Size = new Size(367, 62);
            tb_cookie.TabIndex = 71;
            tb_cookie.TextChanged += tb_cookie_TextChanged;
            // 
            // bt_login
            // 
            bt_login.Location = new Point(549, 15);
            bt_login.Name = "bt_login";
            bt_login.Size = new Size(75, 23);
            bt_login.TabIndex = 67;
            bt_login.Text = "Login TAX";
            bt_login.UseVisualStyleBackColor = true;
            bt_login.Click += bt_login_Click;
            // 
            // dtp_periodo_fim
            // 
            dtp_periodo_fim.Format = DateTimePickerFormat.Short;
            dtp_periodo_fim.Location = new Point(272, 60);
            dtp_periodo_fim.Name = "dtp_periodo_fim";
            dtp_periodo_fim.Size = new Size(84, 23);
            dtp_periodo_fim.TabIndex = 64;
            dtp_periodo_fim.ValueChanged += dtp_periodo_fim_ValueChanged;
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(218, 35);
            label30.Name = "label30";
            label30.Size = new Size(51, 15);
            label30.TabIndex = 63;
            label30.Text = "Período:";
            // 
            // dtp_periodo_inicio
            // 
            dtp_periodo_inicio.Format = DateTimePickerFormat.Short;
            dtp_periodo_inicio.Location = new Point(272, 31);
            dtp_periodo_inicio.Name = "dtp_periodo_inicio";
            dtp_periodo_inicio.Size = new Size(85, 23);
            dtp_periodo_inicio.TabIndex = 62;
            dtp_periodo_inicio.ValueChanged += dtp_periodo_inicio_ValueChanged;
            // 
            // dgv_comparativo_notas
            // 
            dgv_comparativo_notas.AllowUserToAddRows = false;
            dgv_comparativo_notas.AllowUserToDeleteRows = false;
            dgv_comparativo_notas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgv_comparativo_notas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv_comparativo_notas.BackgroundColor = SystemColors.Control;
            dgv_comparativo_notas.BorderStyle = BorderStyle.None;
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
            dgv_comparativo_notas.Location = new Point(15, 77);
            dgv_comparativo_notas.Name = "dgv_comparativo_notas";
            dgv_comparativo_notas.RowHeadersVisible = false;
            dgv_comparativo_notas.Size = new Size(563, 395);
            dgv_comparativo_notas.TabIndex = 69;
            dgv_comparativo_notas.CellEndEdit += dgv_comparativo_notas_CellEndEdit;
            dgv_comparativo_notas.CellFormatting += dgv_comparativo_notas_CellFormatting;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(label24);
            groupBox2.Controls.Add(bt_atualizar_comparacao);
            groupBox2.Controls.Add(cb_status);
            groupBox2.Controls.Add(bt_alterar_status);
            groupBox2.Controls.Add(bt_popular_tabela_sifar);
            groupBox2.Controls.Add(bt_atualizar_valores_tax);
            groupBox2.Controls.Add(dgv_comparativo_notas);
            groupBox2.Location = new Point(478, 126);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(594, 483);
            groupBox2.TabIndex = 70;
            groupBox2.TabStop = false;
            groupBox2.Text = "COMPARAÇÃO DE QUANTIDADES";
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(232, 41);
            label24.Name = "label24";
            label24.Size = new Size(42, 15);
            label24.TabIndex = 75;
            label24.Text = "Status:";
            // 
            // bt_atualizar_comparacao
            // 
            bt_atualizar_comparacao.Location = new Point(456, 37);
            bt_atualizar_comparacao.Name = "bt_atualizar_comparacao";
            bt_atualizar_comparacao.Size = new Size(75, 23);
            bt_atualizar_comparacao.TabIndex = 74;
            bt_atualizar_comparacao.Text = "Atualizar";
            bt_atualizar_comparacao.UseVisualStyleBackColor = true;
            bt_atualizar_comparacao.Click += bt_atualizar_comparacao_Click;
            // 
            // cb_status
            // 
            cb_status.FormattingEnabled = true;
            cb_status.Items.AddRange(new object[] { "EM ANDAMENTO", "LIBERADO" });
            cb_status.Location = new Point(276, 37);
            cb_status.Name = "cb_status";
            cb_status.Size = new Size(121, 23);
            cb_status.TabIndex = 73;
            // 
            // bt_alterar_status
            // 
            bt_alterar_status.Location = new Point(401, 37);
            bt_alterar_status.Name = "bt_alterar_status";
            bt_alterar_status.Size = new Size(50, 23);
            bt_alterar_status.TabIndex = 72;
            bt_alterar_status.Text = "Alterar";
            bt_alterar_status.UseVisualStyleBackColor = true;
            bt_alterar_status.Click += bt_alterar_status_Click;
            // 
            // bt_popular_tabela_sifar
            // 
            bt_popular_tabela_sifar.Location = new Point(119, 37);
            bt_popular_tabela_sifar.Name = "bt_popular_tabela_sifar";
            bt_popular_tabela_sifar.Size = new Size(90, 23);
            bt_popular_tabela_sifar.TabIndex = 71;
            bt_popular_tabela_sifar.Text = "Popular SIFAR";
            bt_popular_tabela_sifar.UseVisualStyleBackColor = true;
            bt_popular_tabela_sifar.Click += bt_popular_tabela_sifar_Click;
            // 
            // bt_atualizar_valores_tax
            // 
            bt_atualizar_valores_tax.Location = new Point(15, 37);
            bt_atualizar_valores_tax.Name = "bt_atualizar_valores_tax";
            bt_atualizar_valores_tax.Size = new Size(87, 23);
            bt_atualizar_valores_tax.TabIndex = 70;
            bt_atualizar_valores_tax.Text = "Popular TAX";
            bt_atualizar_valores_tax.UseVisualStyleBackColor = true;
            bt_atualizar_valores_tax.Click += bt_atualizar_valores_tax_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { configuraçõesToolStripMenuItem, sobreToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1084, 24);
            menuStrip1.TabIndex = 71;
            menuStrip1.Text = "menuStrip1";
            // 
            // configuraçõesToolStripMenuItem
            // 
            configuraçõesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { credenciaisToolStripMenuItem, DiretorioPadraoEntradaToolStripMenuItem, DiretorioPadraoSaidaToolStripMenuItem, AbrirInterfaceAntigaToolStripMenuItem });
            configuraçõesToolStripMenuItem.Name = "configuraçõesToolStripMenuItem";
            configuraçõesToolStripMenuItem.Size = new Size(96, 20);
            configuraçõesToolStripMenuItem.Text = "Configurações";
            // 
            // credenciaisToolStripMenuItem
            // 
            credenciaisToolStripMenuItem.Name = "credenciaisToolStripMenuItem";
            credenciaisToolStripMenuItem.Size = new Size(203, 22);
            credenciaisToolStripMenuItem.Text = "Credenciais";
            credenciaisToolStripMenuItem.Click += credenciaisToolStripMenuItem_Click;
            // 
            // DiretorioPadraoEntradaToolStripMenuItem
            // 
            DiretorioPadraoEntradaToolStripMenuItem.Name = "DiretorioPadraoEntradaToolStripMenuItem";
            DiretorioPadraoEntradaToolStripMenuItem.Size = new Size(203, 22);
            DiretorioPadraoEntradaToolStripMenuItem.Text = "Diretório Padrão Entrada";
            DiretorioPadraoEntradaToolStripMenuItem.Click += DiretorioPadraoEntradaToolStripMenuItem_Click;
            // 
            // DiretorioPadraoSaidaToolStripMenuItem
            // 
            DiretorioPadraoSaidaToolStripMenuItem.Name = "DiretorioPadraoSaidaToolStripMenuItem";
            DiretorioPadraoSaidaToolStripMenuItem.Size = new Size(203, 22);
            DiretorioPadraoSaidaToolStripMenuItem.Text = "Diretório Padrão Saída";
            DiretorioPadraoSaidaToolStripMenuItem.Click += DiretorioPadraoSaidaToolStripMenuItem_Click;
            // 
            // AbrirInterfaceAntigaToolStripMenuItem
            // 
            AbrirInterfaceAntigaToolStripMenuItem.Name = "AbrirInterfaceAntigaToolStripMenuItem";
            AbrirInterfaceAntigaToolStripMenuItem.Size = new Size(203, 22);
            AbrirInterfaceAntigaToolStripMenuItem.Text = "Abrir Interface Antiga";
            AbrirInterfaceAntigaToolStripMenuItem.Click += AbrirInterfaceAntigaToolStripMenuItem_Click;
            // 
            // sobreToolStripMenuItem
            // 
            sobreToolStripMenuItem.Name = "sobreToolStripMenuItem";
            sobreToolStripMenuItem.Size = new Size(49, 20);
            sobreToolStripMenuItem.Text = "Sobre";
            sobreToolStripMenuItem.Click += sobreToolStripMenuItem_Click;
            // 
            // cb_ferramentas
            // 
            cb_ferramentas.FormattingEnabled = true;
            cb_ferramentas.Items.AddRange(new object[] { "ITENS", "BURACO NOTAS", "PRODUTOS/TAXAS", "PESSOA FIS/JUR", "DIFERENÇA CANCELADAS" });
            cb_ferramentas.Location = new Point(93, 245);
            cb_ferramentas.Name = "cb_ferramentas";
            cb_ferramentas.Size = new Size(121, 23);
            cb_ferramentas.TabIndex = 0;
            // 
            // bt_ferramentas
            // 
            bt_ferramentas.Location = new Point(220, 245);
            bt_ferramentas.Name = "bt_ferramentas";
            bt_ferramentas.Size = new Size(75, 23);
            bt_ferramentas.TabIndex = 72;
            bt_ferramentas.Text = "Iniciar";
            bt_ferramentas.UseVisualStyleBackColor = true;
            bt_ferramentas.Click += bt_ferramentas_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(17, 248);
            label1.Name = "label1";
            label1.Size = new Size(75, 15);
            label1.TabIndex = 73;
            label1.Text = "Ferramentas:";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(ckb_mostrar_na_tela);
            groupBox3.Controls.Add(label14);
            groupBox3.Controls.Add(bt_obter_icms_sifar);
            groupBox3.Controls.Add(label2);
            groupBox3.Controls.Add(bt_qtd_notas);
            groupBox3.Controls.Add(cb_local_qtd_notas);
            groupBox3.Location = new Point(12, 126);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(460, 109);
            groupBox3.TabIndex = 74;
            groupBox3.TabStop = false;
            groupBox3.Text = "CONSULTAS";
            // 
            // ckb_mostrar_na_tela
            // 
            ckb_mostrar_na_tela.AutoSize = true;
            ckb_mostrar_na_tela.Location = new Point(273, 31);
            ckb_mostrar_na_tela.Name = "ckb_mostrar_na_tela";
            ckb_mostrar_na_tela.Size = new Size(105, 19);
            ckb_mostrar_na_tela.TabIndex = 78;
            ckb_mostrar_na_tela.Text = "Mostrar na tela";
            ckb_mostrar_na_tela.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(15, 71);
            label14.Name = "label14";
            label14.Size = new Size(70, 15);
            label14.TabIndex = 77;
            label14.Text = "ICMS SIFAR:";
            // 
            // bt_obter_icms_sifar
            // 
            bt_obter_icms_sifar.Location = new Point(95, 67);
            bt_obter_icms_sifar.Name = "bt_obter_icms_sifar";
            bt_obter_icms_sifar.Size = new Size(68, 23);
            bt_obter_icms_sifar.TabIndex = 76;
            bt_obter_icms_sifar.Text = "Executar";
            bt_obter_icms_sifar.UseVisualStyleBackColor = true;
            bt_obter_icms_sifar.Click += bt_obter_icms_sifar_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 32);
            label2.Name = "label2";
            label2.Size = new Size(106, 15);
            label2.TabIndex = 75;
            label2.Text = "Quantidade Notas:";
            // 
            // bt_qtd_notas
            // 
            bt_qtd_notas.Location = new Point(201, 29);
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
            cb_local_qtd_notas.Location = new Point(127, 29);
            cb_local_qtd_notas.Name = "cb_local_qtd_notas";
            cb_local_qtd_notas.Size = new Size(55, 23);
            cb_local_qtd_notas.TabIndex = 36;
            // 
            // groupBox4
            // 
            groupBox4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            groupBox4.Controls.Add(ckb_job_automatico);
            groupBox4.Controls.Add(label5);
            groupBox4.Controls.Add(dgv_pendencia_processamento);
            groupBox4.Controls.Add(groupBox6);
            groupBox4.Controls.Add(groupBox5);
            groupBox4.Controls.Add(label3);
            groupBox4.Controls.Add(textBox1);
            groupBox4.Location = new Point(12, 287);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(460, 322);
            groupBox4.TabIndex = 75;
            groupBox4.TabStop = false;
            groupBox4.Text = "TAX API";
            // 
            // ckb_job_automatico
            // 
            ckb_job_automatico.AutoSize = true;
            ckb_job_automatico.Checked = true;
            ckb_job_automatico.CheckState = CheckState.Checked;
            ckb_job_automatico.Location = new Point(340, 103);
            ckb_job_automatico.Name = "ckb_job_automatico";
            ckb_job_automatico.Size = new Size(110, 19);
            ckb_job_automatico.TabIndex = 67;
            ckb_job_automatico.Text = "Job Automatico";
            ckb_job_automatico.UseVisualStyleBackColor = true;
            ckb_job_automatico.CheckedChanged += ckb_job_automatico_CheckedChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(188, 107);
            label5.Name = "label5";
            label5.Size = new Size(146, 15);
            label5.TabIndex = 66;
            label5.Text = "Pendencia Processamento";
            // 
            // dgv_pendencia_processamento
            // 
            dgv_pendencia_processamento.AllowUserToAddRows = false;
            dgv_pendencia_processamento.AllowUserToDeleteRows = false;
            dgv_pendencia_processamento.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_pendencia_processamento.Location = new Point(188, 125);
            dgv_pendencia_processamento.Name = "dgv_pendencia_processamento";
            dgv_pendencia_processamento.ReadOnly = true;
            dgv_pendencia_processamento.RowHeadersVisible = false;
            dgv_pendencia_processamento.Size = new Size(266, 186);
            dgv_pendencia_processamento.TabIndex = 65;
            dgv_pendencia_processamento.CellContentClick += dgv_pendencia_processamento_CellContentClick;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(ckb_buraco_notas);
            groupBox6.Controls.Add(bt_executar_relatorio);
            groupBox6.Controls.Add(ckb_diferenca_capa_item);
            groupBox6.Controls.Add(ckb_icms_resumido);
            groupBox6.Controls.Add(ckb_notas_sem_item);
            groupBox6.Controls.Add(bt_relatorios);
            groupBox6.Controls.Add(ckb_qtd_itens);
            groupBox6.Controls.Add(ckb_extracao_canceladas);
            groupBox6.Controls.Add(ckb_qtd_notas);
            groupBox6.Controls.Add(ckb_qtd_canceladas);
            groupBox6.Location = new Point(10, 28);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(172, 283);
            groupBox6.TabIndex = 64;
            groupBox6.TabStop = false;
            groupBox6.Text = "RELATÓRIOS";
            // 
            // ckb_buraco_notas
            // 
            ckb_buraco_notas.AutoSize = true;
            ckb_buraco_notas.Location = new Point(14, 22);
            ckb_buraco_notas.Name = "ckb_buraco_notas";
            ckb_buraco_notas.Size = new Size(111, 19);
            ckb_buraco_notas.TabIndex = 44;
            ckb_buraco_notas.Text = "Buraco de notas";
            ckb_buraco_notas.UseVisualStyleBackColor = true;
            // 
            // bt_executar_relatorio
            // 
            bt_executar_relatorio.Location = new Point(6, 226);
            bt_executar_relatorio.Name = "bt_executar_relatorio";
            bt_executar_relatorio.Size = new Size(64, 23);
            bt_executar_relatorio.TabIndex = 52;
            bt_executar_relatorio.Text = "Executar";
            bt_executar_relatorio.UseVisualStyleBackColor = true;
            bt_executar_relatorio.Click += bt_executar_relatorio_Click;
            // 
            // ckb_diferenca_capa_item
            // 
            ckb_diferenca_capa_item.AutoSize = true;
            ckb_diferenca_capa_item.Location = new Point(14, 47);
            ckb_diferenca_capa_item.Name = "ckb_diferenca_capa_item";
            ckb_diferenca_capa_item.Size = new Size(133, 19);
            ckb_diferenca_capa_item.TabIndex = 45;
            ckb_diferenca_capa_item.Text = "Diferença capa-item";
            ckb_diferenca_capa_item.UseVisualStyleBackColor = true;
            // 
            // ckb_icms_resumido
            // 
            ckb_icms_resumido.AutoSize = true;
            ckb_icms_resumido.Location = new Point(14, 72);
            ckb_icms_resumido.Name = "ckb_icms_resumido";
            ckb_icms_resumido.Size = new Size(110, 19);
            ckb_icms_resumido.TabIndex = 46;
            ckb_icms_resumido.Text = "ICMS Resumido";
            ckb_icms_resumido.UseVisualStyleBackColor = true;
            // 
            // ckb_notas_sem_item
            // 
            ckb_notas_sem_item.AutoSize = true;
            ckb_notas_sem_item.Location = new Point(14, 97);
            ckb_notas_sem_item.Name = "ckb_notas_sem_item";
            ckb_notas_sem_item.Size = new Size(109, 19);
            ckb_notas_sem_item.TabIndex = 47;
            ckb_notas_sem_item.Text = "Notas sem item";
            ckb_notas_sem_item.UseVisualStyleBackColor = true;
            // 
            // bt_relatorios
            // 
            bt_relatorios.Location = new Point(76, 226);
            bt_relatorios.Name = "bt_relatorios";
            bt_relatorios.Size = new Size(77, 23);
            bt_relatorios.TabIndex = 53;
            bt_relatorios.Text = "Resultados";
            bt_relatorios.UseVisualStyleBackColor = true;
            bt_relatorios.Click += bt_relatorios_Click;
            // 
            // ckb_qtd_itens
            // 
            ckb_qtd_itens.AutoSize = true;
            ckb_qtd_itens.Checked = true;
            ckb_qtd_itens.CheckState = CheckState.Checked;
            ckb_qtd_itens.Location = new Point(14, 122);
            ckb_qtd_itens.Name = "ckb_qtd_itens";
            ckb_qtd_itens.Size = new Size(132, 19);
            ckb_qtd_itens.TabIndex = 48;
            ckb_qtd_itens.Text = "Quantidade de itens";
            ckb_qtd_itens.UseVisualStyleBackColor = true;
            // 
            // ckb_extracao_canceladas
            // 
            ckb_extracao_canceladas.AutoSize = true;
            ckb_extracao_canceladas.Location = new Point(14, 196);
            ckb_extracao_canceladas.Name = "ckb_extracao_canceladas";
            ckb_extracao_canceladas.Size = new Size(147, 19);
            ckb_extracao_canceladas.TabIndex = 51;
            ckb_extracao_canceladas.Text = "Extração de canceladas";
            ckb_extracao_canceladas.UseVisualStyleBackColor = true;
            // 
            // ckb_qtd_notas
            // 
            ckb_qtd_notas.AutoSize = true;
            ckb_qtd_notas.Checked = true;
            ckb_qtd_notas.CheckState = CheckState.Checked;
            ckb_qtd_notas.Location = new Point(14, 147);
            ckb_qtd_notas.Name = "ckb_qtd_notas";
            ckb_qtd_notas.Size = new Size(136, 19);
            ckb_qtd_notas.TabIndex = 49;
            ckb_qtd_notas.Text = "Quantidade de notas";
            ckb_qtd_notas.UseVisualStyleBackColor = true;
            // 
            // ckb_qtd_canceladas
            // 
            ckb_qtd_canceladas.AutoSize = true;
            ckb_qtd_canceladas.Checked = true;
            ckb_qtd_canceladas.CheckState = CheckState.Checked;
            ckb_qtd_canceladas.Location = new Point(14, 171);
            ckb_qtd_canceladas.Name = "ckb_qtd_canceladas";
            ckb_qtd_canceladas.Size = new Size(151, 19);
            ckb_qtd_canceladas.TabIndex = 50;
            ckb_qtd_canceladas.Text = "Quantidade Canceladas";
            ckb_qtd_canceladas.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(bt_executar_job);
            groupBox5.Controls.Add(bt_logs_processos_importacao);
            groupBox5.Location = new Point(188, 36);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(266, 63);
            groupBox5.TabIndex = 63;
            groupBox5.TabStop = false;
            groupBox5.Text = "JOB IMPORTAÇÃO";
            // 
            // bt_executar_job
            // 
            bt_executar_job.Location = new Point(18, 24);
            bt_executar_job.Name = "bt_executar_job";
            bt_executar_job.Size = new Size(66, 23);
            bt_executar_job.TabIndex = 61;
            bt_executar_job.Text = "Executar";
            bt_executar_job.UseVisualStyleBackColor = true;
            bt_executar_job.Click += bt_executar_job_Click;
            // 
            // bt_logs_processos_importacao
            // 
            bt_logs_processos_importacao.Location = new Point(100, 24);
            bt_logs_processos_importacao.Name = "bt_logs_processos_importacao";
            bt_logs_processos_importacao.Size = new Size(66, 23);
            bt_logs_processos_importacao.TabIndex = 60;
            bt_logs_processos_importacao.Text = "Logs";
            bt_logs_processos_importacao.UseVisualStyleBackColor = true;
            bt_logs_processos_importacao.Click += bt_logs_processos_importacao_Click;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(281, 663);
            label3.Name = "label3";
            label3.Size = new Size(44, 15);
            label3.TabIndex = 26;
            label3.Text = "Cookie";
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            textBox1.Location = new Point(281, 685);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(441, 52);
            textBox1.TabIndex = 25;
            // 
            // statusStrip
            // 
            statusStrip.Location = new Point(0, 612);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1084, 22);
            statusStrip.TabIndex = 76;
            statusStrip.Text = "statusStrip1";
            // 
            // F_Main_V2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1084, 634);
            Controls.Add(statusStrip);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(label1);
            Controls.Add(bt_ferramentas);
            Controls.Add(cb_ferramentas);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "F_Main_V2";
            Text = "TAX ZONE";
            Load += F_Main_V2_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_comparativo_notas).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_pendencia_processamento).EndInit();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            groupBox5.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox lbox_empresas;
        private Label label4;
        private GroupBox groupBox1;
        private DateTimePicker dtp_periodo_fim;
        private Label label30;
        private DateTimePicker dtp_periodo_inicio;
        private CheckBox ckb_renew_task;
        private Button bt_login;
        private CheckBox ckb_mes_aberto;
        private DataGridView dgv_comparativo_notas;
        private GroupBox groupBox2;
        private Button bt_popular_tabela_sifar;
        private Button bt_atualizar_valores_tax;
        private Label label24;
        private Button bt_atualizar_comparacao;
        private ComboBox cb_status;
        private Button bt_alterar_status;
        private TextBox tb_cookie;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem configuraçõesToolStripMenuItem;
        private ToolStripMenuItem credenciaisToolStripMenuItem;
        private ComboBox cb_ferramentas;
        private Button bt_ferramentas;
        private Label label1;
        private GroupBox groupBox3;
        private Label label2;
        private Button bt_qtd_notas;
        private ComboBox cb_local_qtd_notas;
        private Label label14;
        private Button bt_obter_icms_sifar;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private Button bt_executar_job;
        private Button bt_logs_processos_importacao;
        private TextBox tb_usuario_tax;
        private TextBox tb_senha_tax;
        private Button bt_relatorios;
        private CheckBox ckb_extracao_canceladas;
        private CheckBox ckb_qtd_canceladas;
        private CheckBox ckb_qtd_notas;
        private CheckBox ckb_qtd_itens;
        private CheckBox ckb_notas_sem_item;
        private CheckBox ckb_icms_resumido;
        private CheckBox ckb_diferenca_capa_item;
        private CheckBox ckb_buraco_notas;
        private Button bt_executar_relatorio;
        private Label label3;
        private TextBox textBox1;
        private GroupBox groupBox6;
        private CheckBox ckb_gerar_arquivo;
        private CheckBox ckb_fracionar_valores;
        private StatusStrip statusStrip;
        private ToolStripMenuItem DiretorioPadraoSaidaToolStripMenuItem;
        private ToolStripMenuItem DiretorioPadraoEntradaToolStripMenuItem;
        private CheckBox ckb_mostrar_na_tela;
        private ToolStripMenuItem AbrirInterfaceAntigaToolStripMenuItem;
        private ToolStripMenuItem sobreToolStripMenuItem;
        private Button bt_resetar_contexto;
        private Label label5;
        private DataGridView dgv_pendencia_processamento;
        private CheckBox ckb_job_automatico;
    }
}