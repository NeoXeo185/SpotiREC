namespace SpotiREC
{
    partial class SettingsForm
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
            this.LV_DeviceList = new System.Windows.Forms.ListView();
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnChannels = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LB_DeviceSelection = new System.Windows.Forms.Label();
            this.BT_OK = new System.Windows.Forms.Button();
            this.BT_Cancel = new System.Windows.Forms.Button();
            this.LB_PathExplain = new System.Windows.Forms.Label();
            this.BT_SavingPath = new System.Windows.Forms.Button();
            this.TB_SavingPath = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // LV_DeviceList
            // 
            this.LV_DeviceList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnChannels});
            this.LV_DeviceList.Location = new System.Drawing.Point(12, 25);
            this.LV_DeviceList.MultiSelect = false;
            this.LV_DeviceList.Name = "LV_DeviceList";
            this.LV_DeviceList.Size = new System.Drawing.Size(302, 103);
            this.LV_DeviceList.TabIndex = 1;
            this.LV_DeviceList.UseCompatibleStateImageBehavior = false;
            this.LV_DeviceList.View = System.Windows.Forms.View.Details;
            this.LV_DeviceList.SelectedIndexChanged += new System.EventHandler(this.LV_DeviceList_SelectedIndexChanged);
            // 
            // columnName
            // 
            this.columnName.Text = "Name";
            this.columnName.Width = 237;
            // 
            // columnChannels
            // 
            this.columnChannels.Text = "Channels";
            this.columnChannels.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LB_DeviceSelection
            // 
            this.LB_DeviceSelection.AutoSize = true;
            this.LB_DeviceSelection.Location = new System.Drawing.Point(12, 9);
            this.LB_DeviceSelection.Name = "LB_DeviceSelection";
            this.LB_DeviceSelection.Size = new System.Drawing.Size(190, 13);
            this.LB_DeviceSelection.TabIndex = 4;
            this.LB_DeviceSelection.Text = "Choose the device you want to record:";
            // 
            // BT_OK
            // 
            this.BT_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BT_OK.Enabled = false;
            this.BT_OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_OK.Location = new System.Drawing.Point(158, 191);
            this.BT_OK.Name = "BT_OK";
            this.BT_OK.Size = new System.Drawing.Size(75, 27);
            this.BT_OK.TabIndex = 2;
            this.BT_OK.Text = "OK";
            this.BT_OK.UseVisualStyleBackColor = true;
            // 
            // BT_Cancel
            // 
            this.BT_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BT_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_Cancel.Location = new System.Drawing.Point(239, 191);
            this.BT_Cancel.Name = "BT_Cancel";
            this.BT_Cancel.Size = new System.Drawing.Size(75, 27);
            this.BT_Cancel.TabIndex = 3;
            this.BT_Cancel.Text = "Annuler";
            this.BT_Cancel.UseVisualStyleBackColor = true;
            // 
            // LB_PathExplain
            // 
            this.LB_PathExplain.AutoSize = true;
            this.LB_PathExplain.Location = new System.Drawing.Point(9, 134);
            this.LB_PathExplain.Name = "LB_PathExplain";
            this.LB_PathExplain.Size = new System.Drawing.Size(283, 13);
            this.LB_PathExplain.TabIndex = 5;
            this.LB_PathExplain.Text = "Choose the directory where you want the files to be saved:";
            // 
            // BT_SavingPath
            // 
            this.BT_SavingPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_SavingPath.Location = new System.Drawing.Point(274, 155);
            this.BT_SavingPath.Name = "BT_SavingPath";
            this.BT_SavingPath.Size = new System.Drawing.Size(40, 23);
            this.BT_SavingPath.TabIndex = 7;
            this.BT_SavingPath.Text = "...";
            this.BT_SavingPath.UseVisualStyleBackColor = true;
            this.BT_SavingPath.Click += new System.EventHandler(this.BT_SavingPath_Click);
            // 
            // TB_SavingPath
            // 
            this.TB_SavingPath.Location = new System.Drawing.Point(12, 157);
            this.TB_SavingPath.Name = "TB_SavingPath";
            this.TB_SavingPath.ReadOnly = true;
            this.TB_SavingPath.Size = new System.Drawing.Size(256, 20);
            this.TB_SavingPath.TabIndex = 8;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.BT_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BT_Cancel;
            this.ClientSize = new System.Drawing.Size(330, 228);
            this.Controls.Add(this.TB_SavingPath);
            this.Controls.Add(this.BT_SavingPath);
            this.Controls.Add(this.LB_PathExplain);
            this.Controls.Add(this.BT_Cancel);
            this.Controls.Add(this.BT_OK);
            this.Controls.Add(this.LB_DeviceSelection);
            this.Controls.Add(this.LV_DeviceList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.Text = "Device selection";
            this.Load += new System.EventHandler(this.DeviceSelectionForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView LV_DeviceList;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnChannels;
        private System.Windows.Forms.Label LB_DeviceSelection;
        private System.Windows.Forms.Button BT_OK;
        private System.Windows.Forms.Button BT_Cancel;
        private System.Windows.Forms.Label LB_PathExplain;
        private System.Windows.Forms.Button BT_SavingPath;
        private System.Windows.Forms.TextBox TB_SavingPath;
    }
}