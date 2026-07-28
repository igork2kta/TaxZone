namespace TaxZone
{
    partial class F_Relatorio_Importacao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_Relatorio_Importacao));
            label29 = new Label();
            dtp_fim = new DateTimePicker();
            label30 = new Label();
            dtp_inicio = new DateTimePicker();
            cb_status = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            tb_usuario = new TextBox();
            cb_estabelecimento = new ComboBox();
            tb_descricao = new TextBox();
            bt_pesquisar = new Button();
            dgv_relatorio_importacao = new DataGridView();
            lbl_loading_percentage = new Label();
            pb_loading = new PictureBox();
            cb_acao_botao_relatorio = new ComboBox();
            label5 = new Label();
            ckb_gerar_arquivo = new CheckBox();
            ckb_fracionar = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)dgv_relatorio_importacao).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pb_loading).BeginInit();
            SuspendLayout();
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Location = new Point(171, 21);
            label29.Name = "label29";
            label29.Size = new Size(13, 15);
            label29.TabIndex = 54;
            label29.Text = "à";
            // 
            // dtp_fim
            // 
            dtp_fim.Format = DateTimePickerFormat.Short;
            dtp_fim.Location = new Point(190, 17);
            dtp_fim.Name = "dtp_fim";
            dtp_fim.Size = new Size(84, 23);
            dtp_fim.TabIndex = 53;
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(23, 21);
            label30.Name = "label30";
            label30.Size = new Size(51, 15);
            label30.TabIndex = 52;
            label30.Text = "Período:";
            // 
            // dtp_inicio
            // 
            dtp_inicio.Format = DateTimePickerFormat.Short;
            dtp_inicio.Location = new Point(80, 17);
            dtp_inicio.Name = "dtp_inicio";
            dtp_inicio.Size = new Size(85, 23);
            dtp_inicio.TabIndex = 51;
            // 
            // cb_status
            // 
            cb_status.FormattingEnabled = true;
            cb_status.Items.AddRange(new object[] { " ", "Finalizado com sucesso", "Finalizado com erros" });
            cb_status.Location = new Point(823, 49);
            cb_status.Name = "cb_status";
            cb_status.Size = new Size(194, 23);
            cb_status.TabIndex = 55;
            cb_status.Visible = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(775, 53);
            label1.Name = "label1";
            label1.Size = new Size(42, 15);
            label1.TabIndex = 56;
            label1.Text = "Status:";
            label1.Visible = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(420, 21);
            label2.Name = "label2";
            label2.Size = new Size(50, 15);
            label2.TabIndex = 57;
            label2.Text = "Usuario:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(291, 21);
            label3.Name = "label3";
            label3.Size = new Size(38, 15);
            label3.TabIndex = 58;
            label3.Text = "Estab:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(608, 21);
            label4.Name = "label4";
            label4.Size = new Size(61, 15);
            label4.TabIndex = 59;
            label4.Text = "Descrição:";
            // 
            // tb_usuario
            // 
            tb_usuario.Location = new Point(476, 17);
            tb_usuario.Name = "tb_usuario";
            tb_usuario.Size = new Size(126, 23);
            tb_usuario.TabIndex = 60;
            // 
            // cb_estabelecimento
            // 
            cb_estabelecimento.FormattingEnabled = true;
            cb_estabelecimento.Items.AddRange(new object[] { "Finalizado com sucesso", "Finalizado com erros" });
            cb_estabelecimento.Location = new Point(335, 17);
            cb_estabelecimento.Name = "cb_estabelecimento";
            cb_estabelecimento.Size = new Size(67, 23);
            cb_estabelecimento.TabIndex = 61;
            // 
            // tb_descricao
            // 
            tb_descricao.Location = new Point(672, 17);
            tb_descricao.Name = "tb_descricao";
            tb_descricao.Size = new Size(100, 23);
            tb_descricao.TabIndex = 62;
            // 
            // bt_pesquisar
            // 
            bt_pesquisar.Location = new Point(791, 17);
            bt_pesquisar.Name = "bt_pesquisar";
            bt_pesquisar.Size = new Size(75, 23);
            bt_pesquisar.TabIndex = 63;
            bt_pesquisar.Text = "Pesquisar";
            bt_pesquisar.UseVisualStyleBackColor = true;
            bt_pesquisar.Click += bt_pesquisar_Click;
            // 
            // dgv_relatorio_importacao
            // 
            dgv_relatorio_importacao.AllowUserToAddRows = false;
            dgv_relatorio_importacao.AllowUserToDeleteRows = false;
            dgv_relatorio_importacao.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgv_relatorio_importacao.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv_relatorio_importacao.BackgroundColor = SystemColors.ButtonFace;
            dgv_relatorio_importacao.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgv_relatorio_importacao.DefaultCellStyle = dataGridViewCellStyle1;
            dgv_relatorio_importacao.Location = new Point(11, 91);
            dgv_relatorio_importacao.Name = "dgv_relatorio_importacao";
            dgv_relatorio_importacao.ReadOnly = true;
            dgv_relatorio_importacao.RowHeadersVisible = false;
            dgv_relatorio_importacao.Size = new Size(1034, 347);
            dgv_relatorio_importacao.TabIndex = 64;
            dgv_relatorio_importacao.CellContentClick += dgv_relatorio_importacao_CellContentClick;
            dgv_relatorio_importacao.CellFormatting += dgv_relatorio_importacao_CellFormatting;
            // 
            // lbl_loading_percentage
            // 
            lbl_loading_percentage.AutoSize = true;
            lbl_loading_percentage.Location = new Point(511, 285);
            lbl_loading_percentage.Name = "lbl_loading_percentage";
            lbl_loading_percentage.Size = new Size(69, 15);
            lbl_loading_percentage.TabIndex = 66;
            lbl_loading_percentage.Text = "Carregando";
            lbl_loading_percentage.TextAlign = ContentAlignment.MiddleCenter;
            lbl_loading_percentage.Visible = false;
            // 
            // pb_loading
            // 
            pb_loading.ErrorImage = null;
            pb_loading.Image = (Image)resources.GetObject("pb_loading.Image");
            pb_loading.InitialImage = null;
            pb_loading.Location = new Point(487, 220);
            pb_loading.Name = "pb_loading";
            pb_loading.Size = new Size(69, 62);
            pb_loading.SizeMode = PictureBoxSizeMode.Zoom;
            pb_loading.TabIndex = 65;
            pb_loading.TabStop = false;
            pb_loading.Visible = false;
            // 
            // cb_acao_botao_relatorio
            // 
            cb_acao_botao_relatorio.FormattingEnabled = true;
            cb_acao_botao_relatorio.Items.AddRange(new object[] { "Visualisar Arquivo", "Baixar Arquivo", "Processar (SAFX42 apenas)" });
            cb_acao_botao_relatorio.Location = new Point(147, 54);
            cb_acao_botao_relatorio.Name = "cb_acao_botao_relatorio";
            cb_acao_botao_relatorio.Size = new Size(168, 23);
            cb_acao_botao_relatorio.TabIndex = 67;
            cb_acao_botao_relatorio.SelectedIndexChanged += cb_acao_botao_relatorio_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(23, 58);
            label5.Name = "label5";
            label5.Size = new Size(118, 15);
            label5.TabIndex = 68;
            label5.Text = "Ação botão relatório:";
            // 
            // ckb_gerar_arquivo
            // 
            ckb_gerar_arquivo.AutoSize = true;
            ckb_gerar_arquivo.Location = new Point(326, 56);
            ckb_gerar_arquivo.Name = "ckb_gerar_arquivo";
            ckb_gerar_arquivo.Size = new Size(99, 19);
            ckb_gerar_arquivo.TabIndex = 69;
            ckb_gerar_arquivo.Text = "Gerar Arquivo";
            ckb_gerar_arquivo.UseVisualStyleBackColor = true;
            ckb_gerar_arquivo.Visible = false;
            // 
            // ckb_fracionar
            // 
            ckb_fracionar.AutoSize = true;
            ckb_fracionar.Location = new Point(431, 56);
            ckb_fracionar.Name = "ckb_fracionar";
            ckb_fracionar.Size = new Size(75, 19);
            ckb_fracionar.TabIndex = 70;
            ckb_fracionar.Text = "Fracionar";
            ckb_fracionar.UseVisualStyleBackColor = true;
            ckb_fracionar.Visible = false;
            // 
            // F_Relatorio_Importacao
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1057, 450);
            Controls.Add(ckb_fracionar);
            Controls.Add(ckb_gerar_arquivo);
            Controls.Add(label5);
            Controls.Add(cb_acao_botao_relatorio);
            Controls.Add(lbl_loading_percentage);
            Controls.Add(pb_loading);
            Controls.Add(dgv_relatorio_importacao);
            Controls.Add(bt_pesquisar);
            Controls.Add(tb_descricao);
            Controls.Add(cb_estabelecimento);
            Controls.Add(tb_usuario);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(cb_status);
            Controls.Add(label29);
            Controls.Add(dtp_fim);
            Controls.Add(label30);
            Controls.Add(dtp_inicio);
            Name = "F_Relatorio_Importacao";
            Text = "F_Relatorio_Importacao";
            ((System.ComponentModel.ISupportInitialize)dgv_relatorio_importacao).EndInit();
            ((System.ComponentModel.ISupportInitialize)pb_loading).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label29;
        private DateTimePicker dtp_fim;
        private Label label30;
        private DateTimePicker dtp_inicio;
        private ComboBox cb_status;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox tb_usuario;
        private ComboBox cb_estabelecimento;
        private TextBox tb_descricao;
        private Button bt_pesquisar;
        private DataGridView dgv_relatorio_importacao;
        private Label lbl_loading_percentage;
        private PictureBox pb_loading;
        private ComboBox cb_acao_botao_relatorio;
        private Label label5;
        private CheckBox ckb_gerar_arquivo;
        private CheckBox ckb_fracionar;
    }
}