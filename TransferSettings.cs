using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer
{
    public class TransferSettings
    {
        public List<TransferItem> Items { get; set; } = new();
        public bool UseOverwrite { get; set; } // true: 上書き, false: 日付付与
    }
}
