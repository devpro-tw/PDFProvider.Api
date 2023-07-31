using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFProvider.Api.Models
{
    /// <summary>
    /// PDF 套表 - 回傳結果
    /// </summary>
    public class FormFillerRs
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success = false;

        /// <summary>
        /// PDF 檔案
        /// </summary>
        public byte[] PdfFile = null;
    }
}