namespace YLWService.AutoUpdater
{
    partial class frmLauncher
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
            this.components = new System.ComponentModel.Container();
            this.prbProgress = new System.Windows.Forms.ProgressBar();
            this.lblLength = new System.Windows.Forms.Label();
            this.lblFile = new System.Windows.Forms.Label();
            this.lblUpdatableList = new System.Windows.Forms.Label();
            this.lsbFiles = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.autoUpdater = new YLWService.AutoUpdater.AutoUpdater(this.components);
            this.SuspendLayout();
            // 
            // prbProgress
            // 
            this.prbProgress.Dock = System.Windows.Forms.DockStyle.Top;
            this.prbProgress.Location = new System.Drawing.Point(3, 57);
            this.prbProgress.Name = "prbProgress";
            this.prbProgress.Size = new System.Drawing.Size(588, 23);
            this.prbProgress.TabIndex = 5;
            // 
            // lblLength
            // 
            this.lblLength.AutoSize = true;
            this.lblLength.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLength.Location = new System.Drawing.Point(3, 39);
            this.lblLength.Name = "lblLength";
            this.lblLength.Padding = new System.Windows.Forms.Padding(3);
            this.lblLength.Size = new System.Drawing.Size(62, 18);
            this.lblLength.TabIndex = 3;
            this.lblLength.Text = "lblLength";
            this.lblLength.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFile.Location = new System.Drawing.Point(3, 21);
            this.lblFile.Name = "lblFile";
            this.lblFile.Padding = new System.Windows.Forms.Padding(3);
            this.lblFile.Size = new System.Drawing.Size(44, 18);
            this.lblFile.TabIndex = 4;
            this.lblFile.Text = "lblFile";
            this.lblFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUpdatableList
            // 
            this.lblUpdatableList.AutoSize = true;
            this.lblUpdatableList.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUpdatableList.Location = new System.Drawing.Point(3, 3);
            this.lblUpdatableList.Name = "lblUpdatableList";
            this.lblUpdatableList.Padding = new System.Windows.Forms.Padding(3);
            this.lblUpdatableList.Size = new System.Drawing.Size(100, 18);
            this.lblUpdatableList.TabIndex = 6;
            this.lblUpdatableList.Text = "lblUpdatableList";
            this.lblUpdatableList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lsbFiles
            // 
            this.lsbFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsbFiles.ItemHeight = 12;
            this.lsbFiles.Location = new System.Drawing.Point(3, 85);
            this.lsbFiles.Name = "lsbFiles";
            this.lsbFiles.Size = new System.Drawing.Size(588, 133);
            this.lsbFiles.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(588, 5);
            this.panel1.TabIndex = 8;
            // 
            // autoUpdater
            // 
            this.autoUpdater.LastUpdateFileName = null;
            this.autoUpdater.LocalRoot = "";
            this.autoUpdater.RootUri = "";
            this.autoUpdater.UpdateListFileName = null;
            this.autoUpdater.UpdateCompleted += new System.EventHandler<YLWService.AutoUpdater.AutoUpdater.UpdateCompletedEventArgs>(this.autoUpdater_UpdateCompleted);
            this.autoUpdater.UpdatableListFound += new System.EventHandler<YLWService.AutoUpdater.AutoUpdater.UpdatableListFoundEventArgs>(this.autoUpdater_UpdatableListFound);
            this.autoUpdater.FileTransfering += new System.EventHandler<YLWService.AutoUpdater.AutoUpdater.FileTransferingEventArgs>(this.autoUpdater_FileTransfering);
            this.autoUpdater.FileTransfered += new System.EventHandler<YLWService.AutoUpdater.AutoUpdater.FileTransferedEventArgs>(this.autoUpdater_FileTransfered);
            this.autoUpdater.UpdateProgressChanged += new System.EventHandler<YLWService.AutoUpdater.AutoUpdater.UpdateProgressChangedEventArgs>(this.autoUpdater_UpdateProgressChanged);
            // 
            // frmLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(594, 221);
            this.Controls.Add(this.lsbFiles);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.prbProgress);
            this.Controls.Add(this.lblLength);
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.lblUpdatableList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmLauncher";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NoM 자동 업데이터";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLength;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.ProgressBar prbProgress;
        private System.Windows.Forms.Label lblUpdatableList;
        private System.Windows.Forms.ListBox lsbFiles;
        private System.Windows.Forms.Panel panel1;
        private AutoUpdater autoUpdater;

    }
}