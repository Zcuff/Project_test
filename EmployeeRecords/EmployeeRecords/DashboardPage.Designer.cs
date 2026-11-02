namespace EmployeeRecords
{
    partial class DashboardPage
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlTotal = new System.Windows.Forms.Panel();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblTotalCaption = new System.Windows.Forms.Label();
            this.pnlActive = new System.Windows.Forms.Panel();
            this.lblActive = new System.Windows.Forms.Label();
            this.lblActiveCaption = new System.Windows.Forms.Label();
            this.pnlInactive = new System.Windows.Forms.Panel();
            this.lblInactive = new System.Windows.Forms.Label();
            this.lblInactiveCaption = new System.Windows.Forms.Label();
            this.lblByPosition = new System.Windows.Forms.Label();
            this.dgvByPosition = new System.Windows.Forms.DataGridView();
            this.lblRecent = new System.Windows.Forms.Label();
            this.dgvRecent = new System.Windows.Forms.DataGridView();
            this.pnlTotal.SuspendLayout();
            this.pnlActive.SuspendLayout();
            this.pnlInactive.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvByPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecent)).BeginInit();
            this.SuspendLayout();
            //
            // pnlTotal
            //
            this.pnlTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(50)))));
            this.pnlTotal.Controls.Add(this.lblTotal);
            this.pnlTotal.Controls.Add(this.lblTotalCaption);
            this.pnlTotal.Location = new System.Drawing.Point(30, 25);
            this.pnlTotal.Name = "pnlTotal";
            this.pnlTotal.Size = new System.Drawing.Size(255, 120);
            this.pnlTotal.TabIndex = 0;
            //
            // lblTotal
            //
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.lblTotal.Location = new System.Drawing.Point(12, 44);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(48, 59);
            this.lblTotal.TabIndex = 1;
            this.lblTotal.Text = "0";
            //
            // lblTotalCaption
            //
            this.lblTotalCaption.AutoSize = true;
            this.lblTotalCaption.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.lblTotalCaption.Location = new System.Drawing.Point(18, 16);
            this.lblTotalCaption.Name = "lblTotalCaption";
            this.lblTotalCaption.Size = new System.Drawing.Size(122, 17);
            this.lblTotalCaption.TabIndex = 0;
            this.lblTotalCaption.Text = "TOTAL EMPLOYEES";
            //
            // pnlActive
            //
            this.pnlActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(112)))), ((int)(((byte)(74)))));
            this.pnlActive.Controls.Add(this.lblActive);
            this.pnlActive.Controls.Add(this.lblActiveCaption);
            this.pnlActive.Location = new System.Drawing.Point(312, 25);
            this.pnlActive.Name = "pnlActive";
            this.pnlActive.Size = new System.Drawing.Size(255, 120);
            this.pnlActive.TabIndex = 1;
            //
            // lblActive
            //
            this.lblActive.AutoSize = true;
            this.lblActive.Font = new System.Drawing.Font("Segoe UI", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActive.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.lblActive.Location = new System.Drawing.Point(12, 44);
            this.lblActive.Name = "lblActive";
            this.lblActive.Size = new System.Drawing.Size(48, 59);
            this.lblActive.TabIndex = 1;
            this.lblActive.Text = "0";
            //
            // lblActiveCaption
            //
            this.lblActiveCaption.AutoSize = true;
            this.lblActiveCaption.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActiveCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.lblActiveCaption.Location = new System.Drawing.Point(18, 16);
            this.lblActiveCaption.Name = "lblActiveCaption";
            this.lblActiveCaption.Size = new System.Drawing.Size(127, 17);
            this.lblActiveCaption.TabIndex = 0;
            this.lblActiveCaption.Text = "ACTIVE EMPLOYEES";
            //
            // pnlInactive
            //
            this.pnlInactive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.pnlInactive.Controls.Add(this.lblInactive);
            this.pnlInactive.Controls.Add(this.lblInactiveCaption);
            this.pnlInactive.Location = new System.Drawing.Point(594, 25);
            this.pnlInactive.Name = "pnlInactive";
            this.pnlInactive.Size = new System.Drawing.Size(255, 120);
            this.pnlInactive.TabIndex = 2;
            //
            // lblInactive
            //
            this.lblInactive.AutoSize = true;
            this.lblInactive.Font = new System.Drawing.Font("Segoe UI", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInactive.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(50)))));
            this.lblInactive.Location = new System.Drawing.Point(12, 44);
            this.lblInactive.Name = "lblInactive";
            this.lblInactive.Size = new System.Drawing.Size(48, 59);
            this.lblInactive.TabIndex = 1;
            this.lblInactive.Text = "0";
            //
            // lblInactiveCaption
            //
            this.lblInactiveCaption.AutoSize = true;
            this.lblInactiveCaption.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInactiveCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(50)))));
            this.lblInactiveCaption.Location = new System.Drawing.Point(18, 16);
            this.lblInactiveCaption.Name = "lblInactiveCaption";
            this.lblInactiveCaption.Size = new System.Drawing.Size(141, 17);
            this.lblInactiveCaption.TabIndex = 0;
            this.lblInactiveCaption.Text = "INACTIVE EMPLOYEES";
            //
            // lblByPosition
            //
            this.lblByPosition.AutoSize = true;
            this.lblByPosition.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblByPosition.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(50)))));
            this.lblByPosition.Location = new System.Drawing.Point(26, 168);
            this.lblByPosition.Name = "lblByPosition";
            this.lblByPosition.Size = new System.Drawing.Size(177, 21);
            this.lblByPosition.TabIndex = 3;
            this.lblByPosition.Text = "Employees by position";
            //
            // dgvByPosition
            //
            this.dgvByPosition.AllowUserToAddRows = false;
            this.dgvByPosition.AllowUserToDeleteRows = false;
            this.dgvByPosition.AllowUserToResizeRows = false;
            this.dgvByPosition.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvByPosition.BackgroundColor = System.Drawing.Color.White;
            this.dgvByPosition.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvByPosition.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvByPosition.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvByPosition.Location = new System.Drawing.Point(30, 200);
            this.dgvByPosition.MultiSelect = false;
            this.dgvByPosition.Name = "dgvByPosition";
            this.dgvByPosition.ReadOnly = true;
            this.dgvByPosition.RowHeadersVisible = false;
            this.dgvByPosition.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvByPosition.Size = new System.Drawing.Size(520, 350);
            this.dgvByPosition.TabIndex = 4;
            //
            // lblRecent
            //
            this.lblRecent.AutoSize = true;
            this.lblRecent.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(50)))));
            this.lblRecent.Location = new System.Drawing.Point(576, 168);
            this.lblRecent.Name = "lblRecent";
            this.lblRecent.Size = new System.Drawing.Size(117, 21);
            this.lblRecent.TabIndex = 5;
            this.lblRecent.Text = "Recently hired";
            //
            // dgvRecent
            //
            this.dgvRecent.AllowUserToAddRows = false;
            this.dgvRecent.AllowUserToDeleteRows = false;
            this.dgvRecent.AllowUserToResizeRows = false;
            this.dgvRecent.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRecent.BackgroundColor = System.Drawing.Color.White;
            this.dgvRecent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRecent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecent.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvRecent.Location = new System.Drawing.Point(580, 200);
            this.dgvRecent.MultiSelect = false;
            this.dgvRecent.Name = "dgvRecent";
            this.dgvRecent.ReadOnly = true;
            this.dgvRecent.RowHeadersVisible = false;
            this.dgvRecent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecent.Size = new System.Drawing.Size(270, 350);
            this.dgvRecent.TabIndex = 6;
            //
            // DashboardPage
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.dgvRecent);
            this.Controls.Add(this.lblRecent);
            this.Controls.Add(this.dgvByPosition);
            this.Controls.Add(this.lblByPosition);
            this.Controls.Add(this.pnlInactive);
            this.Controls.Add(this.pnlActive);
            this.Controls.Add(this.pnlTotal);
            this.Name = "DashboardPage";
            this.Size = new System.Drawing.Size(880, 580);
            this.Load += new System.EventHandler(this.DashboardPage_Load);
            this.pnlTotal.ResumeLayout(false);
            this.pnlTotal.PerformLayout();
            this.pnlActive.ResumeLayout(false);
            this.pnlActive.PerformLayout();
            this.pnlInactive.ResumeLayout(false);
            this.pnlInactive.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvByPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlTotal;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblTotalCaption;
        private System.Windows.Forms.Panel pnlActive;
        private System.Windows.Forms.Label lblActive;
        private System.Windows.Forms.Label lblActiveCaption;
        private System.Windows.Forms.Panel pnlInactive;
        private System.Windows.Forms.Label lblInactive;
        private System.Windows.Forms.Label lblInactiveCaption;
        private System.Windows.Forms.Label lblByPosition;
        private System.Windows.Forms.DataGridView dgvByPosition;
        private System.Windows.Forms.Label lblRecent;
        private System.Windows.Forms.DataGridView dgvRecent;
    }
}
