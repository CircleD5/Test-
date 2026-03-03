using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization; // 参照の追加: System.Web.Extensions
using System.Windows.Forms;

namespace FileTransferApp
{
    public partial class MainForm : Form
    {
        private BindingList<TransferItem> items = new BindingList<TransferItem>();
        private int currentSlot = 1;
        private FileTransferService service = new FileTransferService();

        public MainForm()
        {
            // DataGridViewの初期設定
            InitializeDataGridView();
            LoadSettings(1);
        }

        private void InitializeDataGridView()
        {
            dgv.AutoGenerateColumns = false;
            dgv.DataSource = items;
            // 列の追加はデザイナーで行うか、コードで以下のように定義します
            // (SourcePath, DeleteAfter, DestPath, EntireFolder, UseTimestamp, Status)
        }

        private void btnAdd_Click(object sender, EventArgs e) => items.Add(new TransferItem());

        private void btnSave_Click(object sender, EventArgs e) => SaveSettings(currentSlot);

        private void btnRunAll_Click(object sender, EventArgs e)
        {
            foreach (var item in items)
            {
                service.Execute(item);
                dgv.Refresh();
            }
            MessageBox.Show("全処理終了");
        }

        private void btnSlot_Click(object sender, EventArgs e)
        {
            SaveSettings(currentSlot);
            currentSlot = int.Parse(((Button)sender).Tag.ToString());
            LoadSettings(currentSlot);
        }

        private void SaveSettings(int slot)
        {
            var json = new JavaScriptSerializer().Serialize(items.ToList());
            File.WriteAllText($"settings_{slot}.json", json);
        }

        private void LoadSettings(int slot)
        {
            string path = $"settings_{slot}.json";
            items.Clear();
            if (File.Exists(path))
            {
                var list = new JavaScriptSerializer().Deserialize<List<TransferItem>>(File.ReadAllText(path));
                foreach (var i in list) items.Add(i);
            }
        }
    }
}