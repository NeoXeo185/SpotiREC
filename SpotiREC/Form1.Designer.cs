namespace SpotiREC
{
    partial class Main
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.MainTimer = new System.Windows.Forms.Timer(this.components);
            this.LB_Status = new System.Windows.Forms.Label();
            this.LB_Position = new System.Windows.Forms.Label();
            this.PlayingProgress = new System.Windows.Forms.ProgressBar();
            this.btnRefreshDevices = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnConvert = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.BT_RecordOnPlay = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.BT_Settings = new System.Windows.Forms.Button();
            this.LV_DeviceList = new System.Windows.Forms.ListView();
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnChannels = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // MainTimer
            // 
            this.MainTimer.Interval = 400;
            // 
            // LB_Status
            // 
            this.LB_Status.AutoSize = true;
            this.LB_Status.BackColor = System.Drawing.Color.Transparent;
            this.LB_Status.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LB_Status.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_Status.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LB_Status.Location = new System.Drawing.Point(325, 9);
            this.LB_Status.Name = "LB_Status";
            this.LB_Status.Size = new System.Drawing.Size(77, 18);
            this.LB_Status.TabIndex = 0;
            this.LB_Status.Text = "Loading ...";
            // 
            // LB_Position
            // 
            this.LB_Position.AutoSize = true;
            this.LB_Position.BackColor = System.Drawing.Color.Transparent;
            this.LB_Position.Font = new System.Drawing.Font("Roboto Condensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_Position.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LB_Position.Location = new System.Drawing.Point(325, 27);
            this.LB_Position.Name = "LB_Position";
            this.LB_Position.Size = new System.Drawing.Size(16, 15);
            this.LB_Position.TabIndex = 0;
            this.LB_Position.Text = "//";
            // 
            // PlayingProgress
            // 
            this.PlayingProgress.Location = new System.Drawing.Point(321, 199);
            this.PlayingProgress.Name = "PlayingProgress";
            this.PlayingProgress.Size = new System.Drawing.Size(344, 10);
            this.PlayingProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.PlayingProgress.TabIndex = 1;
            // 
            // btnRefreshDevices
            // 
            this.btnRefreshDevices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshDevices.Location = new System.Drawing.Point(207, -119);
            this.btnRefreshDevices.Name = "btnRefreshDevices";
            this.btnRefreshDevices.Size = new System.Drawing.Size(107, 23);
            this.btnRefreshDevices.TabIndex = 6;
            this.btnRefreshDevices.Text = "Refresh devices";
            this.btnRefreshDevices.UseVisualStyleBackColor = true;
            this.btnRefreshDevices.Click += new System.EventHandler(this.btnRefreshDevices_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(227, 177);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(87, 23);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "Stop recording";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(209, 9);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(105, 23);
            this.btnConvert.TabIndex = 9;
            this.btnConvert.Text = "Convert to MP3";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(134, 177);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(87, 23);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Start recording";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // BT_RecordOnPlay
            // 
            this.BT_RecordOnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BT_RecordOnPlay.BackColor = System.Drawing.Color.Transparent;
            this.BT_RecordOnPlay.BackgroundImage = global::SpotiREC.Properties.Resources.record_on_play_white;
            this.BT_RecordOnPlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BT_RecordOnPlay.Enabled = false;
            this.BT_RecordOnPlay.FlatAppearance.BorderSize = 0;
            this.BT_RecordOnPlay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
            this.BT_RecordOnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_RecordOnPlay.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BT_RecordOnPlay.Location = new System.Drawing.Point(328, 170);
            this.BT_RecordOnPlay.Name = "BT_RecordOnPlay";
            this.BT_RecordOnPlay.Size = new System.Drawing.Size(26, 23);
            this.BT_RecordOnPlay.TabIndex = 8;
            this.BT_RecordOnPlay.UseVisualStyleBackColor = false;
            this.BT_RecordOnPlay.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(321, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(344, 211);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // BT_Settings
            // 
            this.BT_Settings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BT_Settings.BackColor = System.Drawing.Color.Transparent;
            this.BT_Settings.BackgroundImage = global::SpotiREC.Properties.Resources.settings_white;
            this.BT_Settings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BT_Settings.FlatAppearance.BorderSize = 0;
            this.BT_Settings.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.BT_Settings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.BT_Settings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_Settings.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BT_Settings.Location = new System.Drawing.Point(635, 7);
            this.BT_Settings.Name = "BT_Settings";
            this.BT_Settings.Size = new System.Drawing.Size(23, 20);
            this.BT_Settings.TabIndex = 8;
            this.BT_Settings.UseVisualStyleBackColor = false;
            // 
            // LV_DeviceList
            // 
            this.LV_DeviceList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnChannels});
            this.LV_DeviceList.Location = new System.Drawing.Point(12, 38);
            this.LV_DeviceList.MultiSelect = false;
            this.LV_DeviceList.Name = "LV_DeviceList";
            this.LV_DeviceList.Size = new System.Drawing.Size(302, 133);
            this.LV_DeviceList.TabIndex = 10;
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
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 209);
            this.Controls.Add(this.LV_DeviceList);
            this.Controls.Add(this.BT_Settings);
            this.Controls.Add(this.LB_Position);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.BT_RecordOnPlay);
            this.Controls.Add(this.btnRefreshDevices);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.PlayingProgress);
            this.Controls.Add(this.LB_Status);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Main";
            this.Text = "SpotiREC";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer MainTimer;
        private System.Windows.Forms.Label LB_Status;
        private System.Windows.Forms.Label LB_Position;
        private System.Windows.Forms.ProgressBar PlayingProgress;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnRefreshDevices;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button BT_RecordOnPlay;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button BT_Settings;
        private System.Windows.Forms.ListView LV_DeviceList;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnChannels;
    }
}

