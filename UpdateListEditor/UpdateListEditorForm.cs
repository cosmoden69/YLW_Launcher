using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HubornCRM
{
    public partial class UpdateListEditorForm : Form
    {
        public UpdateListEditorForm()
        {
            InitializeComponent();
        }

        private void tsbOpen_Click(object sender, EventArgs e)
        {
            if (ofdOpen.ShowDialog() != DialogResult.OK)
                return;

            UpdateListDataSet dataSet = new UpdateListDataSet();
            dataSet.ReadXml(ofdOpen.FileName);

            _dataSet.File.Clear();
            _dataSet.Merge(dataSet);
        }

        private void tsbReadDirectory_Click(object sender, EventArgs e)
        {
            fbdFolder.SelectedPath = Application.StartupPath;
            if (fbdFolder.ShowDialog() != DialogResult.OK)
                return;

            var dataSet = CreateUpdateList(fbdFolder.SelectedPath, null);

            _dataSet.File.Clear();
            _dataSet.Merge(dataSet);
        }

        private void tsbMarkAsNew_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvList.SelectedRows)
            {
                // �̹� 1�ʰ� ������ �ο�� ����Ѵ�.
                if (row.DefaultCellStyle.BackColor == Color.Beige)
                    continue;

                DateTime lastWriteTime = (DateTime) row.Cells[2].Value;
                row.Cells[2].Value = lastWriteTime.AddSeconds(1);

                row.DefaultCellStyle.BackColor = Color.Beige;
            }

            bdsFile.EndEdit();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (sfdSave.ShowDialog() != DialogResult.OK)
                return;

            bdsFile.EndEdit();

            _dataSet.WriteXml(sfdSave.FileName);

            MessageBox.Show(string.Format("{0}�� ����Ǿ����ϴ�.", sfdSave.FileName));
        }

        private void tsbHelp_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// ������ ���� ������ �� ���� ������ �ִ� ���ϵ��� ������� ������Ʈ ����Ʈ�� �����. ���Խ��� �����Ͽ� Ư�� ������ ������ �� �ִ�.
        /// </summary>
        /// <param name="rootPath">Ž���� ������ ��Ʈ</param>
        /// <param name="excludingRegex">����Ʈ�� ���Խ�Ű�� ���� ���ϸ��� ǥ���ϴ� ���Խ�</param>
        /// <returns>������Ʈ ����Ʈ�� XML (UpdateListDataSet���� ���)</returns>
        private UpdateListDataSet CreateUpdateList(string rootPath, Regex excludingRegex)
        {
            DirectoryInfo root = new DirectoryInfo(rootPath);

            UpdateListDataSet dataSet = new UpdateListDataSet();

            CreateUpdateListCore(root, excludingRegex, dataSet, rootPath);

            return dataSet;
        }

        private static void CreateUpdateListCore(DirectoryInfo directory, Regex excludingRegex, UpdateListDataSet dataSet, string rootPath)
        {
            var files = directory.GetFiles();
            foreach (var file in files)
            {
                if (file.Name == "YLWService.AutoUpdater.dll") continue;
                if (file.Name == "YLWService.Launcher.exe") continue;
                if (file.Name == "YLWService.UpdateListEditor.exe") continue;
                if (file.Name == "UpdatingList.xml") continue;
                if (excludingRegex == null || excludingRegex.IsMatch(file.Name) == false)
                    dataSet.File.AddFileRow(file.FullName.Replace(rootPath + "\\", string.Empty), file.Length, file.LastWriteTime);
            }

            //var subDirectories = directory.GetDirectories();
            //foreach (var subDirectory in subDirectories)
            //    CreateUpdateListCore(subDirectory, excludingRegex, dataSet, rootPath);
        }
    }
}