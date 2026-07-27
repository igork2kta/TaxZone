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
            ((System.ComponentModel.ISupportInitialize)dgv_relatorio_importacao).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pb_loading).BeginInit();
            SuspendLayout();
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Location = new Point(171, 17);
            label29.Name = "label29";
            label29.Size = new Size(13, 15);
            label29.TabIndex = 54;
            label29.Text = "à";
            // 
            // dtp_fim
            // 
            dtp_fim.Format = DateTimePickerFormat.Short;
            dtp_fim.Location = new Point(190, 13);
            dtp_fim.Name = "dtp_fim";
            dtp_fim.Size = new Size(84, 23);
            dtp_fim.TabIndex = 53;
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(23, 17);
            label30.Name = "label30";
            label30.Size = new Size(51, 15);
            label30.TabIndex = 52;
            label30.Text = "Período:";
            // 
            // dtp_inicio
            // 
            dtp_inicio.Format = DateTimePickerFormat.Short;
            dtp_inicio.Location = new Point(80, 13);
            dtp_inicio.Name = "dtp_inicio";
            dtp_inicio.Size = new Size(85, 23);
            dtp_inicio.TabIndex = 51;
            // 
            // cb_status
            // 
            cb_status.FormattingEnabled = true;
            cb_status.Items.AddRange(new object[] { " ", "Finalizado com sucesso", "Finalizado com erros" });
            cb_status.Location = new Point(339, 13);
            cb_status.Name = "cb_status";
            cb_status.Size = new Size(194, 23);
            cb_status.TabIndex = 55;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(291, 17);
            label1.Name = "label1";
            label1.Size = new Size(42, 15);
            label1.TabIndex = 56;
            label1.Text = "Status:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(23, 57);
            label2.Name = "label2";
            label2.Size = new Size(50, 15);
            label2.TabIndex = 57;
            label2.Text = "Usuario:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(552, 17);
            label3.Name = "label3";
            label3.Size = new Size(38, 15);
            label3.TabIndex = 58;
            label3.Text = "Estab:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(211, 57);
            label4.Name = "label4";
            label4.Size = new Size(61, 15);
            label4.TabIndex = 59;
            label4.Text = "Descrição:";
            // 
            // tb_usuario
            // 
            tb_usuario.Location = new Point(79, 53);
            tb_usuario.Name = "tb_usuario";
            tb_usuario.Size = new Size(126, 23);
            tb_usuario.TabIndex = 60;
            // 
            // cb_estabelecimento
            // 
            cb_estabelecimento.FormattingEnabled = true;
            cb_estabelecimento.Items.AddRange(new object[] { "Finalizado com sucesso", "Finalizado com erros" });
            cb_estabelecimento.Location = new Point(596, 13);
            cb_estabelecimento.Name = "cb_estabelecimento";
            cb_estabelecimento.Size = new Size(67, 23);
            cb_estabelecimento.TabIndex = 61;
            // 
            // tb_descricao
            // 
            tb_descricao.Location = new Point(275, 53);
            tb_descricao.Name = "tb_descricao";
            tb_descricao.Size = new Size(100, 23);
            tb_descricao.TabIndex = 62;
            // 
            // bt_pesquisar
            // 
            bt_pesquisar.Location = new Point(394, 53);
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
            dgv_relatorio_importacao.Location = new Point(11, 91);
            dgv_relatorio_importacao.Name = "dgv_relatorio_importacao";
            dgv_relatorio_importacao.ReadOnly = true;
            dgv_relatorio_importacao.RowHeadersVisible = false;
            dgv_relatorio_importacao.Size = new Size(777, 347);
            dgv_relatorio_importacao.TabIndex = 64;
            // 
            // lbl_loading_percentage
            // 
            lbl_loading_percentage.AutoSize = true;
            lbl_loading_percentage.Location = new Point(364, 282);
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
            pb_loading.Location = new Point(339, 217);
            pb_loading.Name = "pb_loading";
            pb_loading.Size = new Size(69, 62);
            pb_loading.SizeMode = PictureBoxSizeMode.Zoom;
            pb_loading.TabIndex = 65;
            pb_loading.TabStop = false;
            pb_loading.Visible = false;
            // 
            // F_Relatorio_Importacao
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
    }
}