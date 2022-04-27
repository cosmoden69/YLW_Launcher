using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;

namespace YLWService.AutoUpdater
{
    /// <summary>
    /// HTTP를 통한 자동 업데이트를 구현하는 컴퍼넌트
    /// </summary>
    [DefaultEvent("UpdateCompleted")]
    [Description("HTTP를 통한 자동 업데이트를 구현하는 컴퍼넌트")]
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
        /// 업데이트 시작 시각
        /// </summary>
        private DateTime _startedAt;

        /// <summary>
        /// 전송 정보
        /// </summary>
        private TransferingInfo _transferingInfo;

        /// <summary>
        /// WebClient
        /// </summary>
        private WebClient _webClient;

        /// <summary>
        /// 루트 URI. 
        /// </summary>
        [Description("루트 URI. 예) http://svc.metrosoft.co.kr/haesungHASP")]
        public string RootUri { get; set; }

        /// <summary>
        /// 업데이트 목록 파일의 이름. 
        /// </summary>
        [Description("업데이트 목록 파일의 이름. 예) UpdatingList.xml")]
        public string UpdateListFileName { get; set; }

        /// <summary>
        /// 최종 업데이트된 파일
        /// </summary>
        [Description("최종 업데이트된 파일(날짜)")]
        public string LastUpdateFileName { get; set; }

        private string _localRoot;

        /// <summary>
        /// 다운로드 받을 루트 폴더. 예) c:\program files\NoM
        /// </summary>
        [Description("다운로드 받을 루트 폴더")]
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
        /// 업데이트 해야 할 파일의 목록을 얻은 후 업데이트를 시작한다.
        /// </summary>
        public void Run()
        {
            // TransferingInfo를 준비한다.
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

                // WebClient를 준비한다.
                _webClient = new WebClient();
                _webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                _webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;

                // 첫번째 노드부터 다운로드를 시작한다.
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
        /// 업데이트 해야 할 파일의 목록을 얻는다.
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
        ///  업데이트 해야 할 파일인지를 검사한다.
        /// </summary>
        /// <param name="remoteFile"></param>
        /// <returns></returns>
        private bool IsUpdatable(RemoteFile remoteFile)
        {
            //// 로컬에 없는 파일이라면 다운로드한다.
            //if (File.Exists(remoteFile.LocalPath) == false)
            //    return true;

            //// 파일의 수정날짜가 다르다면 다운로드한다.
            //if (File.GetLastWriteTime(remoteFile.LocalPath) != remoteFile.LastModified)
            //    return true;

            //서버 파일의 이름이 최종 업데이트된 파일 이후면 true
            string filename = Path.GetFileName(remoteFile.LocalPath);
            if (filename.CompareTo(LastUpdateFileName) > 0) return true;

            // 로컬에 존재하고, 파일의 수정날짜가 같다면 다운로드하지 않는다.
            return false;
        }

        /// <summary>
        /// 업데이트 목록 파일의 내용을 읽는다.
        /// </summary>
        /// <param name="listFileUri">업데이트 목록 파일의 URI</param>
        /// <returns>업데이트 목록</returns>
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
        /// 파일을 다운로드 한다.
        /// </summary>
        /// <param name="node">RemoteFile 객체가 들어 있는 연결 리스트 노드</param>
        private void DownloadFile(LinkedListNode<RemoteFile> node)
        {
            OnFileTransfering(_transferingInfo, node.Value);

            // 디렉토리가 없으면 먼저 디렉토리를 만든다.
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

            //로컬 파일의 수정날짜를 서버 파일의 수정날짜로 변경한다.
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
        /// 업데이트가 끝났음.
        /// </summary>
        [Description("업데이트가 끝났음.")]
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
        /// 업데이트 할 파일의 목록이 발견되었음.
        /// </summary>
        [Description("업데이트 할 파일의 목록이 발견되었음.")]
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
        /// 업데이트 할 파일이 발견되었음.
        /// </summary>
        [Description("업데이트 할 파일이 발견되었음.")]
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
        /// 개별 파일의 전송이 시작되려고 함.
        /// </summary>
        [Description("개별 파일의 전송이 시작되려고 함.")]
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
            /// 전송 정보
            /// </summary>
            public TransferingInfo TransferingInfo { get; set; }

            /// <summary>
            /// 업데이트 할 파일의 리스트
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
        /// 개별 파일의 전송이 완료되었음.
        /// </summary>
        [Description("개별 파일의 전송이 완료되었음.")]
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
            /// 전송 정보
            /// </summary>
            public TransferingInfo TransferingInfo { get; set; }

            /// <summary>
            /// 업데이트 할 파일의 리스트
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
        /// 업데이트 진행 상황이 변경되었음.
        /// </summary>
        [Description("업데이트 진행 상황이 변경되었음.")]
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
            /// 전송 정보
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
