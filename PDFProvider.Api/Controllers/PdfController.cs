using PDFProvider.Api.Helpers;
using PDFProvider.Api.Models;
using PDFProvider.Api.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Http;

namespace PDFProvider.Api.Controllers
{
    public class PdfController : ApiController
    {
        /// <summary>
        /// PDF 套表
        /// </summary>
        [HttpPost]
        public FormFillerRs FormFiller(FormFillerRq model)
        {
            byte[] ret = null;
            if (!string.IsNullOrEmpty(model.filename))
            {
                string logFileName = Path.Combine(ConfigHelper.PdfLogPath, @"pdf" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".json");
                var fields = model.items.ToDictionary(x => x.Name, x => x.Value);

                try
                {
                    if (ConfigHelper.PdfLogEnabled)
                    {
                        SortedDictionary<string, string> PdfFieldsSorted = new SortedDictionary<string, string>(fields);
                        Dictionary<string, string> PdfFields = new Dictionary<string, string>(PdfFieldsSorted);
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Dictionary<string, string>));
                        using (MemoryStream ms = new MemoryStream())
                        {
                            serializer.WriteObject(ms, PdfFields);
                            string text = Encoding.UTF8.GetString(ms.ToArray());
                            File.WriteAllText(logFileName, text + Environment.NewLine);
                        }
                    }
                }
                catch (Exception e)
                {
                    File.WriteAllText(logFileName, e.Message);
                }

                string templateName = PathHelper.GetExistTemplateFileName(ConfigHelper.PdfTemplatePath, model.filename);
                if (!string.IsNullOrEmpty(templateName))
                {
                    FileInfo finfo = new FileInfo(templateName);
                    if (finfo.Exists)
                    {
                        PdfConverter converter = new PdfConverter();
                        ret = converter.ConvertWithPassword(finfo.FullName, fields, model.pin);
                    }
                }
            }

            var result = new FormFillerRs
            {
                Success = ret != null && ret.Length > 0,
                PdfFile = ret
            };

            return result;
        }
    }
}
