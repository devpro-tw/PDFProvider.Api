using iTextSharp.text;
using iTextSharp.text.pdf;
using PDFProvider.Api.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PDFProvider.Api.Services
{
    public class PdfConverter
    {
        // 紀錄ErrorLog及訊息
        private string _errorMessage = string.Empty;

        // 定義字型
        private BaseFont font = null;
        private BaseFont fontBold = null;

        public string ErrorDescription
        {
            get { return this._errorMessage; }
        }

        static PdfConverter()
        {
            try
            {
                // 抓取新的文字格式 in .dll中
                BaseFont.AddToResourceSearch(Path.Combine(ConfigHelper.PdfLibPath, "iTextAsian.dll"));
                BaseFont.AddToResourceSearch(Path.Combine(ConfigHelper.PdfLibPath, "iTextAsianCmaps.dll"));
            }
            catch (Exception e)
            {
            }
        }

        public byte[] ConvertWithPassword(string iFileName, Dictionary<string, string> parameters, string password)
        {
            byte[] oFile = Convert2015(iFileName, parameters);
            if (oFile != null)
            {
                try
                {
                    // Encrypt
                    if (!string.IsNullOrEmpty(password))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            PdfReader reader = new PdfReader(oFile);
                            PdfEncryptor.Encrypt(reader, ms, true, password, password, PdfWriter.ALLOW_SCREENREADERS);
                            oFile = ms.ToArray();
                        }
                    }
                }
                catch (Exception ex)
                {
                    this._errorMessage = this._errorMessage + ex.Message;
                    return null;
                }
            }
            return oFile;
        }

        public byte[] Convert2015(string iFileName, Dictionary<string, string> parameters)
        {
            try
            {
                string vTag = string.Empty;

                font = BaseFont.CreateFont(@"MHei-Medium", "UniCNS-UCS2-H", BaseFont.EMBEDDED);
                iTextSharp.text.Font tempFont = FontFactory.GetFont(FontFactory.TIMES, 9f, iTextSharp.text.Font.BOLD);
                fontBold = tempFont.BaseFont;
                vTag = "V";

                byte[] oFile = null;
                using (MemoryStream stream = new MemoryStream())
                {
                    // 建立宣告Pdf Reader & Stamper物件
                    PdfReader reader = new PdfReader(iFileName);
                    PdfStamper stamper = new PdfStamper(reader, stream);
                    var flds = stamper.AcroFields.Fields;

                    // 造訪每個 KeyValue
                    foreach (KeyValuePair<string, string> pair in parameters)
                    {
                        string[] subKeys = pair.Key.Split("|".ToCharArray(), 2);
                        if (subKeys.Length == 2)
                        {
                            string key = subKeys[0];
                            string type = subKeys[1].ToLower().Trim();
                            switch (type)
                            {
                                case "radiobutton":
                                    stamper.AcroFields.SetFieldProperty(key + pair.Value, "textfont", fontBold, null);
                                    stamper.AcroFields.SetField(key + pair.Value, vTag);
                                    break;
                                case "checkbox": // 1 : Checked , 0 : Unckecked
                                    if (pair.Value.ToString() == "1")
                                    {
                                        stamper.AcroFields.SetFieldProperty(key, "textfont", fontBold, null);
                                        stamper.AcroFields.SetField(key, vTag);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (flds.ContainsKey(pair.Key)) // 判斷是否進行字串拆解     
                        {
                            stamper.AcroFields.SetFieldProperty(pair.Key, "textfont", font, null);
                            stamper.AcroFields.SetField(pair.Key, pair.Value);
                        }
                        else
                        {   // 進行字串拆解   
                            SplitFieldValue(pair.Key, pair.Value, ref stamper);
                        }

                    }
                    stamper.FormFlattening = true;
                    stamper.Close();
                    reader.Close();
                    oFile = stream.ToArray();
                }
                if (ConfigHelper.PdfCopyToLocal)
                {
                    string tempName = Path.Combine(ConfigHelper.PdfWorkingPath, Guid.NewGuid().ToString("N") + Path.GetExtension(iFileName));
                    File.WriteAllBytes(tempName, oFile);
                }
                return oFile;
            }
            catch (Exception ex)
            {
                this._errorMessage = this._errorMessage + ex.Message;
                return null;
            }
        }

        private void SplitFieldValue(string key, string value, ref PdfStamper stamp)
        {
            if (!string.IsNullOrEmpty(value))
            {
                for (int i = 0; i < value.Length; i++)
                {
                    int j = i + 1;
                    stamp.AcroFields.SetFieldProperty(key + j.ToString(), "textfont", font, null);
                    stamp.AcroFields.SetField(key + j.ToString(), value.Substring(i, 1));
                }
            }
        }
    }
}