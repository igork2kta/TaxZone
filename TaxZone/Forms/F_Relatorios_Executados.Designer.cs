namespace TaxZone
{
    partial class F_Relatorios_Executados
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_Relatorios_Executados));
            dgv_relatorios = new DataGridView();
            bt_atualizar = new Button();
            pb_loading = new PictureBox();
            lbl_loading_percentage = new Label();
            ((System.ComponentModel.ISupportInitialize)dgv_relatorios).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pb_loading).BeginInit();
            SuspendLayout();
            // 
            // dgv_relatorios
            // 
            dgv_relatorios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgv_relatorios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv_relatorios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_relatorios.Location = new Point(12, 12);
            dgv_relatorios.Name = "dgv_relatorios";
            dgv_relatorios.Size = new Size(689, 243);
            dgv_relatorios.TabIndex = 0;
            dgv_relatorios.Visible = false;
            // 
            // bt_atualizar
            // 
            bt_atualizar.Location = new Point(626, 263);
            bt_atualizar.Name = "bt_atualizar";
            bt_atualizar.Size = new Size(75, 23);
            bt_atualizar.TabIndex = 1;
            bt_atualizar.Text = "Atualizar";
            bt_atualizar.UseVisualStyleBackColor = true;
            bt_atualizar.Click += bt_atualizar_Click;
            // 
            // pb_loading
            // 
            pb_loading.ErrorImage = null;
            pb_loading.Image = (Image)resources.GetObject("pb_loading.Image");
            pb_loading.InitialImage = null;
            pb_loading.Location = new Point(309, 91);
            pb_loading.Name = "pb_loading";
            pb_loading.Size = new Size(69, 62);
            pb_loading.SizeMode = PictureBoxSizeMode.Zoom;
            pb_loading.TabIndex = 2;
            pb_loading.TabStop = false;
            // 
            // lbl_loading_percentage
            // 
            lbl_loading_percentage.AutoSize = true;
            lbl_loading_percentage.Location = new Point(334, 156);
            lbl_loading_percentage.Name = "lbl_loading_percentage";
            lbl_loading_percentage.Size = new Size(69, 15);
            lbl_loading_percentage.TabIndex = 3;
            lbl_loading_percentage.Text = "Carregando";
            lbl_loading_percentage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // F_Relatorios_Executados
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(713, 298);
            Controls.Add(lbl_loading_percentage);
            Controls.Add(pb_loading);
            Controls.Add(bt_atualizar);
            Controls.Add(dgv_relatorios);
            Name = "F_Relatorios_Executados";
            Text = "F_Relatorios_Executados";
            ((System.ComponentModel.ISupportInitialize)dgv_relatorios).EndInit();
            ((System.ComponentModel.ISupportInitialize)pb_loading).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgv_relatorios;
        private Button bt_atualizar;
        private PictureBox pb_loading;
        private Label lbl_loading_percentage;
    }
}