using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public List<FormField> Items { get; set; }

        /// <summary>
        /// 樣版檔名
        /// </summary>
        [Required]
        public string TemplateName { get; set; }

        /// <summary>
        /// PDF 檔案開啟密碼
        /// </summary>
        public string Pin { get; set; }
    }
}