using System;
using System.Collections.Generic;
using System.Text;

namespace YLWService.AutoUpdater
{
    /// <summary>
    /// ���� ����
    /// </summary>
    public class TransferingInfo
    {
        /// <summary>
        /// ������ �������� ��ü ũ��
        /// </summary>
        public long TotalLength { get; set; }

        /// <summary>
        /// ���� ������ ���۵� �������� ũ�� 
        /// </summary>
        public long TransferedLength { get; set; }

        /// <summary>
        /// ���� ������ ���� ���� �������� ũ��
        /// </summary>
        public long TransferingLength { get; set; }

        /// <summary>
        /// ���۵� �������� ��ü ũ��
        /// </summary>
        public long TotalTransferedLength { get { return TransferedLength + TransferingLength; } }

        /// <summary>
        /// ������ ������ ��ü ����
        /// </summary>
        public int TotalFileCount { get; set; }

        /// <summary>
        /// ���۵� ������ ����
        /// </summary>
        public int TransferedFileCount { get; set; }

        /// <summary>
        /// ���۵� �������� �ۼ�Ʈ
        /// </summary>
        public int LengthPercent
        {
            get { return (int)(TotalTransferedLength * 100.0 / TotalLength); }
        }

        /// <summary>
        /// ���۵� ���� ������ �ۼ�Ʈ
        /// </summary>
        public int FileCountPercent
        {
            get { return (int)(TransferedFileCount * 100.0 / TotalFileCount); }
        }

        /// <summary>
        /// ���� ���� ���� ������ ���� ���
        /// </summary>
        public string CurrentFilePath { get; set; }

        /// <summary>
        /// ���� ���� �ð� (��)
        /// </summary>
        public double RemainingSeconds { get; set; }
    }
}