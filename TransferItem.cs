using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer
{
    public class TransferItem
    {
        public string SourcePath { get; set; } = "";
        public string DestPath { get; set; } = "";
        public bool DeleteAfterTransfer { get; set; }
        public bool TransferEntireFolder { get; set; }
        public bool UseTimestamp { get; set; } // 追加: チェックありで日付付与
        public string Status { get; set; } = "待機中";
    }
}
