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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.BT_Record = new System.Windows.Forms.Button();
            this.BT_Stop = new System.Windows.Forms.Button();
            this.BT_Settings = new System.Windows.Forms.Button();
            this.BT_RecordOnPlay = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.BT_Convert = new System.Windows.Forms.Button();
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
            this.LB_Status.Location = new System.Drawing.Point(4, 10);
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
            this.LB_Position.Location = new System.Drawing.Point(4, 28);
            this.LB_Position.Name = "LB_Position";
            this.LB_Position.Size = new System.Drawing.Size(16, 15);
            this.LB_Position.TabIndex = 0;
            this.LB_Position.Text = "//";
            // 
            // PlayingProgress
            // 
            this.PlayingProgress.Location = new System.Drawing.Point(0, 245);
            this.PlayingProgress.Name = "PlayingProgress";
            this.PlayingProgress.Size = new System.Drawing.Size(435, 10);
            this.PlayingProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.PlayingProgress.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // BT_Record
            // 
            this.BT_Record.BackColor = System.Drawing.Color.Transparent;
            this.BT_Record.BackgroundImage = global::SpotiREC.Properties.Resources.ic_play_arrow_white_18dp;
            this.BT_Record.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BT_Record.Enabled = false;
            this.BT_Record.FlatAppearance.BorderSize = 0;
            this.BT_Record.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
            this.BT_Record.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_Record.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BT_Record.Location = new System.Drawing.Point(33, 216);
            this.BT_Record.Name = "BT_Record";
            this.BT_Record.Size = new System.Drawing.Size(26, 23);
            this.BT_Record.TabIndex = 11;
            this.BT_Record.UseVisualStyleBackColor = false;
            this.BT_Record.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // BT_Stop
            // 
            this.BT_Stop.BackColor = System.Drawing.Color.Transparent;
            this.BT_Stop.BackgroundImage = global::SpotiREC.Properties.Resources.ic_stop_white_18dp;
            this.BT_Stop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BT_Stop.Enabled = false;
            this.BT_Stop.FlatAppearance.BorderSize = 0;
            this.BT_Stop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
            this.BT_Stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_Stop.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BT_Stop.Location = new System.Drawing.Point(59, 216);
            this.BT_Stop.Name = "BT_Stop";
            this.BT_Stop.Size = new System.Drawing.Size(26, 23);
            this.BT_Stop.TabIndex = 10;
            this.BT_Stop.UseVisualStyleBackColor = false;
            this.BT_Stop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // BT_Settings
            // 
            this.BT_Settings.BackColor = System.Drawing.Color.Transparent;
            this.BT_Settings.BackgroundImage = global::SpotiREC.Properties.Resources.settings_white;
            this.BT_Settings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BT_Settings.FlatAppearance.BorderSize = 0;
            this.BT_Settings.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.BT_Settings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.BT_Settings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_Settings.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BT_Settings.Location = new System.Drawing.Point(404, 8);
            this.BT_Settings.Name = "BT_Settings";
            this.BT_Settings.Size = new System.Drawing.Size(23, 20);
            this.BT_Settings.TabIndex = 8;
            this.BT_Settings.UseVisualStyleBackColor = false;
            this.BT_Settings.Click += new System.EventHandler(this.BT_Settings_Click);
            // 
            // BT_RecordOnPlay
            // 
            this.BT_RecordOnPlay.BackColor = System.Drawing.Color.Transparent;
            this.BT_RecordOnPlay.BackgroundImage = global::SpotiREC.Properties.Resources.ic_surround_sound_white_18dp;
            this.BT_RecordOnPlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BT_RecordOnPlay.Enabled = false;
            this.BT_RecordOnPlay.FlatAppearance.BorderSize = 0;
            this.BT_RecordOnPlay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
            this.BT_RecordOnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_RecordOnPlay.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BT_RecordOnPlay.Location = new System.Drawing.Point(7, 216);
            this.BT_RecordOnPlay.Name = "BT_RecordOnPlay";
            this.BT_RecordOnPlay.Size = new System.Drawing.Size(26, 23);
            this.BT_RecordOnPlay.TabIndex = 8;
            this.BT_RecordOnPlay.UseVisualStyleBackColor = false;
            this.BT_RecordOnPlay.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(435, 255);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // BT_Convert
            // 
            this.BT_Convert.BackColor = System.Drawing.Color.Transparent;
            this.BT_Convert.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BT_Convert.FlatAppearance.BorderSize = 0;
            this.BT_Convert.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
            this.BT_Convert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_Convert.Font = new System.Drawing.Font("Roboto Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BT_Convert.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BT_Convert.Location = new System.Drawing.Point(388, 216);
            this.BT_Convert.Name = "BT_Convert";
            this.BT_Convert.Size = new System.Drawing.Size(42, 23);
            this.BT_Convert.TabIndex = 12;
            this.BT_Convert.Text = "MP3";
            this.BT_Convert.UseVisualStyleBackColor = false;
            this.BT_Convert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 255);
            this.Controls.Add(this.BT_Convert);
            this.Controls.Add(this.BT_Record);
            this.Controls.Add(this.BT_Stop);
            this.Controls.Add(this.BT_Settings);
            this.Controls.Add(this.LB_Position);
            this.Controls.Add(this.BT_RecordOnPlay);
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
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button BT_RecordOnPlay;
        private System.Windows.Forms.Button BT_Settings;
        private System.Windows.Forms.Button BT_Stop;
        private System.Windows.Forms.Button BT_Record;
        private System.Windows.Forms.Button BT_Convert;
    }
}

