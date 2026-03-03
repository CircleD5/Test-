# .NET Framework 4.8 WinForms版 ファイル転送ツール 構築手順

## 1. プロジェクトの準備
1. Visual Studioで「Windows フォーム アプリ (.NET Framework)」を新規作成。
2. フレームワークに **.NET Framework 4.8** を選択。
3. **参照の追加**:
   - ソリューションエクスプローラーの「参照」を右クリック。
   - 「参照の追加」から **System.Web.Extensions** を選択して追加（JSON処理用）。

## 2. デザイナー（GUI）での配置
ツールボックスから以下の通り配置してください。

### コントロール構成
- **Panel** (`Dock: Top`): 操作ボタン用コンテナ。
  - **Button** × 3: 名前 `btnSlot1`~`3`、Tagプロパティにそれぞれ `1`, `2`, `3` を入力。
  - **Button**: 名前 `btnAdd` (行追加)
  - **Button**: 名前 `btnSave` (設定保存)
  - **Button**: 名前 `btnRunAll` (すべて実行)
- **DataGridView**: 名前 `dgv` (`Dock: Fill`)
  - `AllowUserToAddRows`: `False`
  - `RowHeadersVisible`: `False`

### DataGridViewの列設定 (Edit Columns)
以下の順で列を追加し、`DataPropertyName` をモデルのプロパティ名と一致させてください。

| 名前 (Name) | ヘッダー (HeaderText) | 型 (Type) | DataPropertyName |
| :--- | :--- | :--- | :--- |
| colSource | 転送元パス | TextBox | SourcePath |
| colDelete | 元削除 | CheckBox | DeleteAfter |
| colDest | 転送先パス | TextBox | DestPath |
| colEntire | フォルダごと | CheckBox | EntireFolder |
| colTs | 日付付与 | CheckBox | UseTimestamp |
| colStatus | 状態 | TextBox | Status |
| colRunBtn | 実行 | Button | (なし) |
| colDelBtn | 削除 | Button | (なし) |

## 3. ソースコード

### TransferItem.cs (データモデル)
```csharp
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