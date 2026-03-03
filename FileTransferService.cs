using System;
using System.IO;

namespace FileTransferApp
{
    public class FileTransferService
    {
        public void Execute(TransferItem item)
        {
            try
            {
                string src = item.SourcePath.Trim('\"');
                string dest = item.DestPath.Trim('\"');

                if (string.IsNullOrWhiteSpace(src) || string.IsNullOrWhiteSpace(dest))
                    throw new Exception("パスが空です。");

                if (File.Exists(src))
                {
                    string finalDest = Directory.Exists(dest) ? Path.Combine(dest, Path.GetFileName(src)) : dest;
                    CopyFile(src, finalDest, item.DeleteAfter, item.UseTimestamp);
                }
                else if (Directory.Exists(src))
                {
                    if (File.Exists(dest)) throw new Exception("フォルダからファイルへの転送は不可。");
                    CopyDirectory(src, dest, item.EntireFolder, item.DeleteAfter, item.UseTimestamp);
                }
                else throw new Exception("転送元が存在しません。");

                item.Status = "完了";
            }
            catch (Exception ex)
            {
                item.Status = "エラー";
                throw ex; // 呼び出し元でMessageBoxを表示
            }
        }

        private void CopyFile(string src, string dest, bool delete, bool useTs)
        {
            string target = dest;
            if (useTs && File.Exists(dest))
            {
                string dir = Path.GetDirectoryName(dest);
                string name = Path.GetFileNameWithoutExtension(dest);
                string ext = Path.GetExtension(dest);
                target = Path.Combine(dir, name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ext);
            }

            string targetDir = Path.GetDirectoryName(target);
            if (!string.IsNullOrEmpty(targetDir)) Directory.CreateDirectory(targetDir);

            File.Copy(src, target, true);
            if (delete) File.Delete(src);
        }

        private void CopyDirectory(string src, string dest, bool entire, bool delete, bool useTs)
        {
            // 転送先の決定
            string baseDest = entire ? Path.Combine(dest, new DirectoryInfo(src).Name) : dest;

            // 全ファイルのコピー
            foreach (string filePath in Directory.GetFiles(src, "*.*", SearchOption.AllDirectories))
            {
                string rel = filePath.Substring(src.Length).TrimStart(Path.DirectorySeparatorChar);
                CopyFile(filePath, Path.Combine(baseDest, rel), false, useTs);
            }

            // 削除処理
            if (delete)
            {
                if (entire)
                {
                    // フォルダごと削除
                    Directory.Delete(src, true);
                }
                else
                {
                    // 中身だけ削除（親フォルダは残す）
                    foreach (string file in Directory.GetFiles(src))
                    {
                        File.Delete(file);
                    }
                    foreach (string dir in Directory.GetDirectories(src))
                    {
                        Directory.Delete(dir, true);
                    }
                }
            }
        }
    }
}