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
                // 이미 1초가 증가된 로우는 통과한다.
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

            MessageBox.Show(string.Format("{0}이 저장되었습니다.", sfdSave.FileName));
        }

        private void tsbHelp_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 지정된 로컬 폴더와 그 하위 폴더에 있는 파일들을 대상으로 업데이트 리스트를 만든다. 정규식을 전달하여 특정 파일을 제외할 수 있다.
        /// </summary>
        /// <param name="rootPath">탐색을 시작할 루트</param>
        /// <param name="excludingRegex">리스트에 포함시키지 않을 파일명을 표현하는 정규식</param>
        /// <returns>업데이트 리스트의 XML (UpdateListDataSet에서 사용)</returns>
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