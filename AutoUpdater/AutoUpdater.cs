using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;

namespace YLWService.AutoUpdater
{
    /// <summary>
    /// HTTP�� ���� �ڵ� ������Ʈ�� �����ϴ� ���۳�Ʈ
    /// </summary>
    [DefaultEvent("UpdateCompleted")]
    [Description("HTTP�� ���� �ڵ� ������Ʈ�� �����ϴ� ���۳�Ʈ")]
    public partial class AutoUpdater : Component
    {
        public AutoUpdater()
        {
            InitializeComponent();
        }

        public AutoUpdater(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// ������Ʈ ���� �ð�
        /// </summary>
        private DateTime _startedAt;

        /// <summary>
        /// ���� ����
        /// </summary>
        private TransferingInfo _transferingInfo;

        /// <summary>
        /// WebClient
        /// </summary>
        private WebClient _webClient;

        /// <summary>
        /// ��Ʈ URI. 
        /// </summary>
        [Description("��Ʈ URI. ��) http://svc.metrosoft.co.kr/haesungHASP")]
        public string RootUri { get; set; }

        /// <summary>
        /// ������Ʈ ��� ������ �̸�. 
        /// </summary>
        [Description("������Ʈ ��� ������ �̸�. ��) UpdatingList.xml")]
        public string UpdateListFileName { get; set; }

        /// <summary>
        /// ���� ������Ʈ�� ����
        /// </summary>
        [Description("���� ������Ʈ�� ����(��¥)")]
        public string LastUpdateFileName { get; set; }

        private string _localRoot;

        /// <summary>
        /// �ٿ�ε� ���� ��Ʈ ����. ��) c:\program files\NoM
        /// </summary>
        [Description("�ٿ�ε� ���� ��Ʈ ����")]
        public string LocalRoot
        {
            get { return _localRoot; }
            set
            {
                _localRoot = value;
                if (_localRoot.EndsWith("\\") == false)
                    _localRoot += "\\";
            }
        }

        /// <summary>
        /// ������Ʈ �ؾ� �� ������ ����� ���� �� ������Ʈ�� �����Ѵ�.
        /// </summary>
        public void Run()
        {
            // TransferingInfo�� �غ��Ѵ�.
            _transferingInfo = new TransferingInfo();

            try
            {
                _startedAt = DateTime.Now;

                var updatableFiles = GetUpdatableFiles();

                OnUpdatableListFound(updatableFiles);

                LinkedList<RemoteFile> remoteFiles = new LinkedList<RemoteFile>(updatableFiles);

                _transferingInfo.TotalFileCount = remoteFiles.Count;
                foreach (var remoteFile in remoteFiles)
                    _transferingInfo.TotalLength += remoteFile.ContentLength;

                if (remoteFiles.Count == 0)
                {
                    OnUpdateCompleted(_transferingInfo, LocalRoot);
                    return;
                }

                // WebClient�� �غ��Ѵ�.
                _webClient = new WebClient();
                _webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                _webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;

                // ù��° ������ �ٿ�ε带 �����Ѵ�.
                var node = remoteFiles.First;
                DownloadFile(node);
            }
            catch (Exception ex)
            {
                OnUpdateCompleted(_transferingInfo, LocalRoot);
                return;
            }
        }

        /// <summary>
        /// ������Ʈ �ؾ� �� ������ ����� ��´�.
        /// </summary>
        /// <returns></returns>
        private List<RemoteFile> GetUpdatableFiles()
        {
            if (RootUri.EndsWith("/") == false)
                RootUri += "/";

            string listText = GetUpdatingText(RootUri + UpdateListFileName);

            UpdateListDataSet updateList = new UpdateListDataSet();
            StringReader reader = new StringReader(listText);
            updateList.ReadXml(reader);
            reader.Close();

            List<RemoteFile> remoteFiles = new List<RemoteFile>();

            foreach (UpdateListDataSet.FileRow file in updateList.File)
            {
                RemoteFile remoteFile = new RemoteFile();
                remoteFile.Uri = RootUri + file.Path.Replace('\\', '/');
                remoteFile.LocalPath = LocalRoot + file.Path;
                remoteFile.ContentLength = file.Length;
                remoteFile.LastModified = file.LastWriteTime;

                bool updatable = IsUpdatable(remoteFile);
                if (updatable)
                {
                    remoteFiles.Add(remoteFile);

                    OnUpdatableFileFound(remoteFile);
                }
            }

            return remoteFiles;
        }

        /// <summary>
        ///  ������Ʈ �ؾ� �� ���������� �˻��Ѵ�.
        /// </summary>
        /// <param name="remoteFile"></param>
        /// <returns></returns>
        private bool IsUpdatable(RemoteFile remoteFile)
        {
            //// ���ÿ� ���� �����̶�� �ٿ�ε��Ѵ�.
            //if (File.Exists(remoteFile.LocalPath) == false)
            //    return true;

            //// ������ ������¥�� �ٸ��ٸ� �ٿ�ε��Ѵ�.
            //if (File.GetLastWriteTime(remoteFile.LocalPath) != remoteFile.LastModified)
            //    return true;

            //���� ������ �̸��� ���� ������Ʈ�� ���� ���ĸ� true
            string filename = Path.GetFileName(remoteFile.LocalPath);
            if (filename.CompareTo(LastUpdateFileName) > 0) return true;

            // ���ÿ� �����ϰ�, ������ ������¥�� ���ٸ� �ٿ�ε����� �ʴ´�.
            return false;
        }

        /// <summary>
        /// ������Ʈ ��� ������ ������ �д´�.
        /// </summary>
        /// <param name="listFileUri">������Ʈ ��� ������ URI</param>
        /// <returns>������Ʈ ���</returns>
        private string GetUpdatingText(string listFileUri)
        {
            WebClient webClient = new WebClient();

            Stream stream = webClient.OpenRead(listFileUri);
            StreamReader reader = new StreamReader(stream);

            string text = reader.ReadToEnd();

            stream.Close();

            return text;
        }

        /// <summary>
        /// ������ �ٿ�ε� �Ѵ�.
        /// </summary>
        /// <param name="node">RemoteFile ��ü�� ��� �ִ� ���� ����Ʈ ���</param>
        private void DownloadFile(LinkedListNode<RemoteFile> node)
        {
            OnFileTransfering(_transferingInfo, node.Value);

            // ���丮�� ������ ���� ���丮�� �����.
            string directory = Path.GetDirectoryName(node.Value.LocalPath);
            if (directory.Length != 0 && Directory.Exists(directory) == false)
                Directory.CreateDirectory(directory);

            lock (_transferingInfo)
            {
                _transferingInfo.CurrentFilePath = node.Value.LocalPath;
            }

            _webClient.DownloadFileAsync(new Uri(node.Value.Uri), node.Value.LocalPath, node);
        }

        private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            LinkedListNode<RemoteFile> node = (LinkedListNode<RemoteFile>)e.UserState;

            //���� ������ ������¥�� ���� ������ ������¥�� �����Ѵ�.
            File.SetLastWriteTime(node.Value.LocalPath, node.Value.LastModified);

            lock (_transferingInfo)
            {
                _transferingInfo.TransferedFileCount++;
                _transferingInfo.TransferedLength += _transferingInfo.TransferingLength;
                _transferingInfo.TransferingLength = 0;
            }

            OnFileTransfered(_transferingInfo, node.Value);

            LinkedListNode<RemoteFile> nextNode = node.Next;
            if (nextNode != null)
            {
                DownloadFile(nextNode);
            }
            else
            {
                OnUpdateCompleted(_transferingInfo, LocalRoot);
            }
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            lock (_transferingInfo)
            {
                _transferingInfo.TransferingLength = e.BytesReceived;

                double deltaSenconds = (DateTime.Now - _startedAt).TotalSeconds;
                double totalSeconds = deltaSenconds * 100 / _transferingInfo.LengthPercent;

                _transferingInfo.RemainingSeconds = totalSeconds - deltaSenconds;
            }

            OnUpdateProgressChanged(_transferingInfo);
        }

        #region events

        #region UpdateCompleted event things for C# 3.0
        /// <summary>
        /// ������Ʈ�� ������.
        /// </summary>
        [Description("������Ʈ�� ������.")]
        public event EventHandler<UpdateCompletedEventArgs> UpdateCompleted;

        protected virtual void OnUpdateCompleted(UpdateCompletedEventArgs e)
        {
            if (UpdateCompleted != null)
                UpdateCompleted(this, e);
        }

        protected virtual void OnUpdateCompleted(TransferingInfo transferingInfo, string localRoot)
        {
            if (UpdateCompleted != null)
                UpdateCompleted(this, new UpdateCompletedEventArgs(transferingInfo, localRoot));
        }

        public class UpdateCompletedEventArgs : EventArgs
        {

            public TransferingInfo TransferingInfo { get; set; }
            public string LocalRoot { get; set; }

            public UpdateCompletedEventArgs(TransferingInfo transferingInfo, string localRoot)
            {
                TransferingInfo = transferingInfo;
                LocalRoot = localRoot;
            }
        }
        #endregion

        #region UpdatableListFound event things for C# 3.0
        /// <summary>
        /// ������Ʈ �� ������ ����� �߰ߵǾ���.
        /// </summary>
        [Description("������Ʈ �� ������ ����� �߰ߵǾ���.")]
        public event EventHandler<UpdatableListFoundEventArgs> UpdatableListFound;

        protected virtual void OnUpdatableListFound(UpdatableListFoundEventArgs e)
        {
            if (UpdatableListFound != null)
                UpdatableListFound(this, e);
        }

        protected virtual void OnUpdatableListFound(List<RemoteFile> remoteFiles)
        {
            if (UpdatableListFound != null)
                UpdatableListFound(this, new UpdatableListFoundEventArgs(remoteFiles));
        }

        public class UpdatableListFoundEventArgs : EventArgs
        {
            public List<RemoteFile> RemoteFiles { get; set; }

            public UpdatableListFoundEventArgs(List<RemoteFile> remoteFiles)
            {
                RemoteFiles = remoteFiles;
            }
        }
        #endregion

        #region UpdatableFileFound event things for C# 3.0
        /// <summary>
        /// ������Ʈ �� ������ �߰ߵǾ���.
        /// </summary>
        [Description("������Ʈ �� ������ �߰ߵǾ���.")]
        public event EventHandler<UpdatableFileFoundEventArgs> UpdatableFileFound;

        protected virtual void OnUpdatableFileFound(UpdatableFileFoundEventArgs e)
        {
            if (UpdatableFileFound != null)
                UpdatableFileFound(this, e);
        }

        protected virtual void OnUpdatableFileFound(RemoteFile remoteFile)
        {
            if (UpdatableFileFound != null)
                UpdatableFileFound(this, new UpdatableFileFoundEventArgs(remoteFile));
        }

        public class UpdatableFileFoundEventArgs : EventArgs
        {
            public RemoteFile RemoteFile { get; set; }

            public UpdatableFileFoundEventArgs(RemoteFile remoteFile)
            {
                RemoteFile = remoteFile;
            }
        }
        #endregion

        #region FileTransfering event things for C# 3.0
        /// <summary>
        /// ���� ������ ������ ���۵Ƿ��� ��.
        /// </summary>
        [Description("���� ������ ������ ���۵Ƿ��� ��.")]
        public event EventHandler<FileTransferingEventArgs> FileTransfering;

        protected virtual void OnFileTransfering(FileTransferingEventArgs e)
        {
            if (FileTransfering != null)
                FileTransfering(this, e);
        }

        protected virtual void OnFileTransfering(TransferingInfo ransferingInfo, RemoteFile remoteFile)
        {
            if (FileTransfering != null)
                FileTransfering(this, new FileTransferingEventArgs(ransferingInfo, remoteFile));
        }

        public class FileTransferingEventArgs : EventArgs
        {
            /// <summary>
            /// ���� ����
            /// </summary>
            public TransferingInfo TransferingInfo { get; set; }

            /// <summary>
            /// ������Ʈ �� ������ ����Ʈ
            /// </summary>
            public RemoteFile RemoteFile { get; set; }

            public FileTransferingEventArgs(TransferingInfo ransferingInfo, RemoteFile remoteFile)
            {
                TransferingInfo = ransferingInfo;
                RemoteFile = remoteFile;
            }
        }
        #endregion

        #region FileTransfered event things for C# 3.0
        /// <summary>
        /// ���� ������ ������ �Ϸ�Ǿ���.
        /// </summary>
        [Description("���� ������ ������ �Ϸ�Ǿ���.")]
        public event EventHandler<FileTransferedEventArgs> FileTransfered;

        protected virtual void OnFileTransfered(FileTransferedEventArgs e)
        {
            if (FileTransfered != null)
                FileTransfered(this, e);
        }

        protected virtual void OnFileTransfered(TransferingInfo transferingInfo, RemoteFile remoteFile)
        {
            if (FileTransfered != null)
                FileTransfered(this, new FileTransferedEventArgs(transferingInfo, remoteFile));
        }

        public class FileTransferedEventArgs : EventArgs
        {
            /// <summary>
            /// ���� ����
            /// </summary>
            public TransferingInfo TransferingInfo { get; set; }

            /// <summary>
            /// ������Ʈ �� ������ ����Ʈ
            /// </summary>
            public RemoteFile RemoteFile { get; set; }

            public FileTransferedEventArgs(TransferingInfo transferingInfo, RemoteFile remoteFile)
            {
                TransferingInfo = transferingInfo;
                RemoteFile = remoteFile;
            }
        }
        #endregion

        #region UpdateProgressChanged event things for C# 3.0
        /// <summary>
        /// ������Ʈ ���� ��Ȳ�� ����Ǿ���.
        /// </summary>
        [Description("������Ʈ ���� ��Ȳ�� ����Ǿ���.")]
        public event EventHandler<UpdateProgressChangedEventArgs> UpdateProgressChanged;

        protected virtual void OnUpdateProgressChanged(UpdateProgressChangedEventArgs e)
        {
            if (UpdateProgressChanged != null)
                UpdateProgressChanged(this, e);
        }

        protected virtual void OnUpdateProgressChanged(TransferingInfo transferingInfo)
        {
            if (UpdateProgressChanged != null)
                UpdateProgressChanged(this, new UpdateProgressChangedEventArgs(transferingInfo));
        }

        public class UpdateProgressChangedEventArgs : EventArgs
        {
            /// <summary>
            /// ���� ����
            /// </summary>
            public TransferingInfo TransferingInfo { get; set; }

            public UpdateProgressChangedEventArgs(TransferingInfo transferingInfo)
            {
                TransferingInfo = transferingInfo;
            }
        }
        #endregion
        #endregion
    }
}
