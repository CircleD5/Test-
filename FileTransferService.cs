using FileTransfer;
using System.IO;
public class FileTransferService
{
    public async Task ExecuteTransfer(TransferItem item, Action<string> logAction)
    {
        try
        {
            string src = item.SourcePath.Trim('\"');
            string dest = item.DestPath.Trim('\"');

            bool isSrcDir = Directory.Exists(src);
            bool isSrcFile = File.Exists(src);
            bool isDestDir = Directory.Exists(dest);

            if (!isSrcDir && !isSrcFile) throw new Exception("転送元が存在しません。");

            if (isSrcFile)
            {
                string finalDest = isDestDir ? Path.Combine(dest, Path.GetFileName(src)) : dest;
                CopyFileLogic(src, finalDest, item.DeleteAfterTransfer, item.UseTimestamp);
            }
            else if (isSrcDir && isDestDir)
            {
                CopyDirectoryLogic(src, dest, item.TransferEntireFolder, item.DeleteAfterTransfer, item.UseTimestamp);
            }
            else if (isSrcDir && !isDestDir)
            {
                throw new Exception("フォルダからファイルへの転送は不可。");
            }

            item.Status = "完了";
            logAction($"成功: {src}");
        }
        catch (Exception ex)
        {
            item.Status = "エラー";
            logAction($"失敗: {ex.Message}");
        }
    }

    private void CopyFileLogic(string src, string dest, bool delete, bool useTimestamp)
    {
        string targetPath = dest;

        // チェックあり（useTimestamp = true）かつファイル存在時のみリネーム
        if (useTimestamp && File.Exists(dest))
        {
            string dir = Path.GetDirectoryName(dest) ?? "";
            string name = Path.GetFileNameWithoutExtension(dest);
            string ext = Path.GetExtension(dest);
            targetPath = Path.Combine(dir, $"{name}_{DateTime.Now:yyyyMMddHHmmss}{ext}");
        }

        string destDir = Path.GetDirectoryName(targetPath);
        if (!string.IsNullOrEmpty(destDir)) Directory.CreateDirectory(destDir);

        File.Copy(src, targetPath, true); // デフォルト（useTimestamp=false）ならここが実行され上書きされる
        if (delete) File.Delete(src);
    }

    private void CopyDirectoryLogic(string src, string dest, bool entire, bool delete, bool useTimestamp)
    {
        string targetBase = entire ? Path.Combine(dest, new DirectoryInfo(src).Name) : dest;
        foreach (string file in Directory.GetFiles(src, "*.*", SearchOption.AllDirectories))
        {
            string relPath = Path.GetRelativePath(src, file);
            CopyFileLogic(file, Path.Combine(targetBase, relPath), false, useTimestamp);
        }
        if (delete) Directory.Delete(src, true);
    }
}