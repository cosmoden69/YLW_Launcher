namespace HubornCRM
{
    partial class UpdateListEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateListEditorForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbReadDirectory = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbHelp = new System.Windows.Forms.ToolStripButton();
            this.fbdFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.ofdOpen = new System.Windows.Forms.OpenFileDialog();
            this.sfdSave = new System.Windows.Forms.SaveFileDialog();
            this.bdsFile = new System.Windows.Forms.BindingSource(this.components);
            this._dataSet = new UpdateListDataSet();
            this.pathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lengthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastWriteTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsbMarkAsNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToResizeColumns = false;
            this.dgvList.AllowUserToResizeRows = false;
            this.dgvList.AutoGenerateColumns = false;
            this.dgvList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pathDataGridViewTextBoxColumn,
            this.lengthDataGridViewTextBoxColumn,
            this.lastWriteTimeDataGridViewTextBoxColumn});
            this.dgvList.DataSource = this.bdsFile;
            this.dgvList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvList.Location = new System.Drawing.Point(0, 25);
            this.dgvList.Name = "dgvList";
            this.dgvList.ReadOnly = true;
            this.dgvList.RowTemplate.Height = 23;
            this.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvList.Size = new System.Drawing.Size(878, 582);
            this.dgvList.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbOpen,
            this.toolStripSeparator4,
            this.tsbReadDirectory,
            this.toolStripSeparator3,
            this.tsbMarkAsNew,
            this.toolStripSeparator1,
            this.tsbSave,
            this.toolStripSeparator2,
            this.tsbHelp});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(878, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbOpen
            // 
            this.tsbOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbOpen.Image = ((System.Drawing.Image)(resources.GetObject("tsbOpen.Image")));
            this.tsbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpen.Name = "tsbOpen";
            this.tsbOpen.Size = new System.Drawing.Size(52, 22);
            this.tsbOpen.Text = "열기(&O)";
            this.tsbOpen.ToolTipText = "이전에 만들어 둔 업데이터 리스트 파일을 불러옵니다.";
            this.tsbOpen.Click += new System.EventHandler(this.tsbOpen_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbReadDirectory
            // 
            this.tsbReadDirectory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbReadDirectory.Image = ((System.Drawing.Image)(resources.GetObject("tsbReadDirectory.Image")));
            this.tsbReadDirectory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReadDirectory.Name = "tsbReadDirectory";
            this.tsbReadDirectory.Size = new System.Drawing.Size(103, 22);
            this.tsbReadDirectory.Text = "디렉토리 읽기(&D)";
            this.tsbReadDirectory.ToolTipText = "디렉토리와 하위에 모든 파일을 읽어 들입니다.";
            this.tsbReadDirectory.Click += new System.EventHandler(this.tsbReadDirectory_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbSave.Image = ((System.Drawing.Image)(resources.GetObject("tsbSave.Image")));
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(75, 22);
            this.tsbSave.Text = "저장하기(&S)";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbHelp
            // 
            this.tsbHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbHelp.Image = ((System.Drawing.Image)(resources.GetObject("tsbHelp.Image")));
            this.tsbHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbHelp.Name = "tsbHelp";
            this.tsbHelp.Size = new System.Drawing.Size(63, 22);
            this.tsbHelp.Text = "도움말(&H)";
            this.tsbHelp.Click += new System.EventHandler(this.tsbHelp_Click);
            // 
            // ofdOpen
            // 
            this.ofdOpen.FileName = "UpdatingList.xml";
            this.ofdOpen.Filter = "UpdatingList files|*.xml";
            // 
            // sfdSave
            // 
            this.sfdSave.FileName = "UpdatingList.xml";
            this.sfdSave.Filter = "UpdatingList files|*.xml";
            // 
            // bdsFile
            // 
            this.bdsFile.DataMember = "File";
            this.bdsFile.DataSource = this._dataSet;
            // 
            // _dataSet
            // 
            this._dataSet.DataSetName = "UpdateListDataSet";
            this._dataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pathDataGridViewTextBoxColumn
            // 
            this.pathDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.pathDataGridViewTextBoxColumn.DataPropertyName = "Path";
            this.pathDataGridViewTextBoxColumn.HeaderText = "경로";
            this.pathDataGridViewTextBoxColumn.Name = "pathDataGridViewTextBoxColumn";
            // 
            // lengthDataGridViewTextBoxColumn
            // 
            this.lengthDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.lengthDataGridViewTextBoxColumn.DataPropertyName = "Length";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = null;
            this.lengthDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.lengthDataGridViewTextBoxColumn.HeaderText = "파일 크기";
            this.lengthDataGridViewTextBoxColumn.Name = "lengthDataGridViewTextBoxColumn";
            this.lengthDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastWriteTimeDataGridViewTextBoxColumn
            // 
            this.lastWriteTimeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.lastWriteTimeDataGridViewTextBoxColumn.DataPropertyName = "LastWriteTime";
            dataGridViewCellStyle4.Format = "yyyy-MM-dd HH:mm:ss";
            dataGridViewCellStyle4.NullValue = null;
            this.lastWriteTimeDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.lastWriteTimeDataGridViewTextBoxColumn.HeaderText = "마지막 수정 시각";
            this.lastWriteTimeDataGridViewTextBoxColumn.Name = "lastWriteTimeDataGridViewTextBoxColumn";
            this.lastWriteTimeDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastWriteTimeDataGridViewTextBoxColumn.Width = 150;
            // 
            // tsbMarkAsNew
            // 
            this.tsbMarkAsNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbMarkAsNew.Image = ((System.Drawing.Image)(resources.GetObject("tsbMarkAsNew.Image")));
            this.tsbMarkAsNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMarkAsNew.Name = "tsbMarkAsNew";
            this.tsbMarkAsNew.Size = new System.Drawing.Size(164, 22);
            this.tsbMarkAsNew.Text = "마지막 수정 시각 1초 증가(&I)";
            this.tsbMarkAsNew.ToolTipText = "선택된 항목들의 마지막 수정 시각을 1초 증가시켜 새버전으로 표시합니다.";
            this.tsbMarkAsNew.Click += new System.EventHandler(this.tsbMarkAsNew_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // UpdateListEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 607);
            this.Controls.Add(this.dgvList);
            this.Controls.Add(this.toolStrip1);
            this.Name = "UpdateListEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "업데이트 리스트 에디터";
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.BindingSource bdsFile;
        private UpdateListDataSet _dataSet;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbOpen;
        private System.Windows.Forms.ToolStripButton tsbReadDirectory;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbHelp;
        private System.Windows.Forms.FolderBrowserDialog fbdFolder;
        private System.Windows.Forms.OpenFileDialog ofdOpen;
        private System.Windows.Forms.SaveFileDialog sfdSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.DataGridViewTextBoxColumn pathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lengthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastWriteTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbMarkAsNew;
    }
}