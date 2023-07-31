using PDFProvider.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFProvider.Api.Models
{
    /// <summary>
    /// 套表請求內容
    /// </summary>
    public class FormFillerRq
    {
        /// <summary>
        /// 欄位清單
        /// </summary>
        public List<FormField> items { get; set; }

        /// <summary>
        /// 樣版檔名
        /// </summary>
        public string filename { get; set; }

        /// <summary>
        /// PDF 檔案開啟密碼
        /// </summary>
        public string pin { get; set; }
    }
}