# PDFProvider.Api

本專案提供一個簡易的 PDF 套表 API，傳入 PDF 表單欄位/值的對應表，以及 PDF 檔名，回傳 PDF JSON Byte Array。

## 安裝

本專案為 Microsoft ASP.NET WEB API 專案(.Net Framework)， 開發環境如下：

* Microsoft Visul Studio 2022
* Microsoft .Net Framework 4.8
* iTextSharp 5.0.0

安裝方式：

* 發佈專案: 建置專案，並產生執行時期的檔案。
* 安裝 IIS
* 發佈至 IIS: 將發佈好的專案執行以 IIS 管理員進行設定即可。

## 使用範例

client 端呼叫範例如下：

```cs
HttpClient client = new HttpClient();
client.BaseAddress = new Uri("https://localhost:44389/");
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/json"));
var model = new FormFillerRq
{
    filename = "template.pdf",
    items =
        new List<Field> {
            new Field { Name = "Field_1", Value = "Value 1" },
            new Field { Name = "Field_2", Value = "Value 2" },
            new Field { Name = "Field_3", Value = "Value 3" },
            new Field { Name = "Field_4", Value = "Value 4" },
            new Field { Name = "Field_5", Value = "Value 5" }
        }
};

HttpResponseMessage response = await client.PostAsJsonAsync(
    "api/Pdf/FormFiller", model);
response.EnsureSuccessStatusCode();
var respText = await response.Content.ReadAsStringAsync();
var result = JsonConvert.DeserializeObject<FormFillerRs>(respText);


Console.WriteLine($"Sccess: {result.Success}");
if (result.Success)
{
    string path = Directory.GetCurrentDirectory();
    System.IO.File.WriteAllBytes(Path.Combine(path, "test.pdf"), result.PdfFile);
}
```

## 貢獻

我們歡迎您參與貢獻！請參考 貢獻指南 了解如何參與。

如果您在使用過程中遇到問題或有任何建議，請在 問題追蹤器 上提交問題。

## 授權

這個專案採用 MIT 授權，詳情請參考 [LICENSE](https://licenses.nuget.org/MIT) 文件。

## 版本歷史

1.0.0 (2023-07-31)：初版釋出，包含  PDF 套表 API。

## 相關連結

* 專案網站
* 文件
* 問題追蹤器
