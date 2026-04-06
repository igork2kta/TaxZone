namespace TaxZone
{
    partial class F_buraco_nota
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
            dw_buraco_nota = new DataGridView();
            INICIO = new DataGridViewTextBoxColumn();
            FIM = new DataGridViewTextBoxColumn();
            BURACO = new DataGridViewTextBoxColumn();
            bt_ok = new Button();
            bt_cancelar = new Button();
            label1 = new Label();
            lbl_total = new Label();
            ((System.ComponentModel.ISupportInitialize)dw_buraco_nota).BeginInit();
            SuspendLayout();
            // 
            // dw_buraco_nota
            // 
            dw_buraco_nota.AllowUserToAddRows = false;
            dw_buraco_nota.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dw_buraco_nota.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dw_buraco_nota.Columns.AddRange(new DataGridViewColumn[] { INICIO, FIM, BURACO });
            dw_buraco_nota.Location = new Point(12, 12);
            dw_buraco_nota.Name = "dw_buraco_nota";
            dw_buraco_nota.ReadOnly = true;
            dw_buraco_nota.Size = new Size(342, 401);
            dw_buraco_nota.TabIndex = 0;
            dw_buraco_nota.UserDeletingRow += dw_buraco_nota_UserDeletingRow;
            // 
            // INICIO
            // 
            INICIO.HeaderText = "INICIO";
            INICIO.Name = "INICIO";
            INICIO.ReadOnly = true;
            // 
            // FIM
            // 
            FIM.HeaderText = "FIM";
            FIM.Name = "FIM";
            FIM.ReadOnly = true;
            // 
            // BURACO
            // 
            BURACO.HeaderText = "BURACO";
            BURACO.Name = "BURACO";
            BURACO.ReadOnly = true;
            // 
            // bt_ok
            // 
            bt_ok.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            bt_ok.Location = new Point(279, 419);
            bt_ok.Name = "bt_ok";
            bt_ok.Size = new Size(75, 23);
            bt_ok.TabIndex = 1;
            bt_ok.Text = "OK";
            bt_ok.UseVisualStyleBackColor = true;
            bt_ok.Click += bt_ok_Click;
            // 
            // bt_cancelar
            // 
            bt_cancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            bt_cancelar.Location = new Point(189, 419);
            bt_cancelar.Name = "bt_cancelar";
            bt_cancelar.Size = new Size(75, 23);
            bt_cancelar.TabIndex = 2;
            bt_cancelar.Text = "CANCELAR";
            bt_cancelar.UseVisualStyleBackColor = true;
            bt_cancelar.Click += bt_cancelar_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new Point(12, 427);
            label1.Name = "label1";
            label1.Size = new Size(35, 15);
            label1.TabIndex = 3;
            label1.Text = "Total:";
            // 
            // lbl_total
            // 
            lbl_total.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lbl_total.AutoSize = true;
            lbl_total.Location = new Point(53, 427);
            lbl_total.Name = "lbl_total";
            lbl_total.Size = new Size(31, 15);
            lbl_total.TabIndex = 4;
            lbl_total.Text = "total";
            // 
            // F_buraco_nota
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(366, 450);
            Controls.Add(lbl_total);
            Controls.Add(label1);
            Controls.Add(bt_cancelar);
            Controls.Add(bt_ok);
            Controls.Add(dw_buraco_nota);
            Name = "F_buraco_nota";
            ShowIcon = false;
            ((System.ComponentModel.ISupportInitialize)dw_buraco_nota).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dw_buraco_nota;
        private DataGridViewTextBoxColumn INICIO;
        private DataGridViewTextBoxColumn FIM;
        private DataGridViewTextBoxColumn BURACO;
        private Button bt_ok;
        private Button bt_cancelar;
        private Label label1;
        private Label lbl_total;
    }
}