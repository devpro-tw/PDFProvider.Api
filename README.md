# PDFProvider.Api

本專案提供 PDF 套表 API，傳入 PDF 表單欄位/值的對應表，以及 PDF 樣版名稱，將回傳 PDF JSON Byte Array，讓用戶端進行 PDF 預覽或轉存。

本專案使用  [iTextSharp 5.0.0.0](https://sourceforge.net/projects/itextsharp/) 來產生 PDF 檔案。 (本專案並未修改 iTextSharp 原始程式碼)。

iTextSharp 套件的相關連結如下：

* https://sourceforge.net/projects/itextsharp/
* http://itextpdf.com/
* 請參考 https://nuget.info/packages/iTextSharp/5.0.5 的授權資訊


## 安裝

本專案為 Microsoft ASP.NET WEB API 專案(.Net Framework)， 開發環境如下：

* Microsoft Visul Studio 2022
* Microsoft .Net Framework 4.8

安裝方式：

* 發佈專案: 建置專案，並產生執行時期的檔案。
* 安裝 IIS
* 發佈至 IIS: 將發佈好的執行檔案以 IIS 管理員進行設定即可。

## 使用範例

client 端呼叫範例如下：

```cs
HttpClient client = new HttpClient();
client.BaseAddress = new Uri("https://localhost:44389/");
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/json"));
var model = new
{
    TemplateName = "template",
    Items = new[]  {
        new { Name = "Field_1", Value = "Value 1" },
        new { Name = "Field_2", Value = "Value 2" },
        new { Name = "Field_3", Value = "Value 3" },
        new { Name = "Field_4", Value = "Value 4" },
        new { Name = "Field_5", Value = "Value 5" }
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
else
{
    Console.WriteLine($"Error: {result.ErrorMessage}");
}
```

## 貢獻

我們對所有拉取請求保持開放態度。如果您找到錯誤、設計問題或拼寫錯誤，請向我們發送拉取請求。

如果您在使用過程中遇到問題或有任何建議，請在 [問題追蹤器](https://github.com/devpro-tw/PDFProvider.Api/issues) 上提交問題。

## 授權

這是一個在 GNU AGPLv3 許可證下發佈的開源專案。詳細資訊，請參閱許可證或訪問 GNU 官方頁面： http://www.gnu.org/licenses/agpl-3.0.html.

## 版本歷史

1.0.0 (2023-07-31)：初版釋出，包含  PDF 套表 API。

## 相關連結

* [專案網站](https://github.com/devpro-tw/PDFProvider.Api)
* [問題追蹤器](https://github.com/devpro-tw/PDFProvider.Api/issues)
