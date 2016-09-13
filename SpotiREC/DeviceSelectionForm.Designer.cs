namespace SpotiREC
{
    partial class DeviceSelectionForm
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
            this.BT_OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_OK.Location = new System.Drawing.Point(158, 134);
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
            this.BT_Cancel.Location = new System.Drawing.Point(239, 134);
            this.BT_Cancel.Name = "BT_Cancel";
            this.BT_Cancel.Size = new System.Drawing.Size(75, 27);
            this.BT_Cancel.TabIndex = 3;
            this.BT_Cancel.Text = "Annuler";
            this.BT_Cancel.UseVisualStyleBackColor = true;
            // 
            // DeviceSelectionForm
            // 
            this.AcceptButton = this.BT_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BT_Cancel;
            this.ClientSize = new System.Drawing.Size(330, 170);
            this.Controls.Add(this.BT_Cancel);
            this.Controls.Add(this.BT_OK);
            this.Controls.Add(this.LB_DeviceSelection);
            this.Controls.Add(this.LV_DeviceList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DeviceSelectionForm";
            this.ShowInTaskbar = false;
            this.Text = "Device selection";
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
    }
}