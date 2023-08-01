using NLog;
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
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// PDF 套表
        /// </summary>
        [HttpPost]
        public FormFillerRs FormFiller(FormFillerRq model)
        {
            var result = new FormFillerRs();
            byte[] ret = null;
            if (!string.IsNullOrEmpty(model.TemplateName))
            {
                string logFileName = Path.Combine(ConfigHelper.PdfLogPath, @"pdf" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".json");
                var fields = model.Items.ToDictionary(x => x.Name, x => x.Value);

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
                    _logger.Error(e);
                }

                string templateFilename = PathHelper.GetExistTemplateFileName(ConfigHelper.PdfTemplatePath, $"{model.TemplateName}.pdf");
                if (!string.IsNullOrEmpty(templateFilename))
                {
                    FileInfo finfo = new FileInfo(templateFilename);
                    if (finfo.Exists)
                    {
                        PdfConverter converter = new PdfConverter();
                        ret = converter.ConvertWithPassword(finfo.FullName, fields, model.Pin);
                    }
                }
                else
                {
                    result.ErrorMessage = "樣版檔案不存在";
                    _logger.Error(result.ErrorMessage);
                }
            }

            result.Success = ret != null && ret.Length > 0;
            result.PdfFile = ret;

            return result;
        }
    }
}
