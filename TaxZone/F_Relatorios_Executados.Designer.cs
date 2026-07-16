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
            dgv_relatorios = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgv_relatorios).BeginInit();
            SuspendLayout();
            // 
            // dgv_relatorios
            // 
            dgv_relatorios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgv_relatorios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv_relatorios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_relatorios.Location = new Point(12, 12);
            dgv_relatorios.Name = "dgv_relatorios";
            dgv_relatorios.Size = new Size(700, 274);
            dgv_relatorios.TabIndex = 0;
            // 
            // F_Relatorios_Executados
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(724, 298);
            Controls.Add(dgv_relatorios);
            Name = "F_Relatorios_Executados";
            Text = "F_Relatorios_Executados";
            ((System.ComponentModel.ISupportInitialize)dgv_relatorios).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgv_relatorios;
    }
}