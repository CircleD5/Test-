using System;

namespace FileTransferApp
{
    public class TransferItem
    {
        public string SourcePath { get; set; } = "";
        public bool DeleteAfter { get; set; }
        public string DestPath { get; set; } = "";
        public bool EntireFolder { get; set; }
        public bool UseTimestamp { get; set; }
        public string Status { get; set; } = "待機中";
    }
}