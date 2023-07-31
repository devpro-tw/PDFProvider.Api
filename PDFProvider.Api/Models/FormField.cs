using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFProvider.Api.Models
{
    /// <summary>
    /// PDF 套表欄位名稱/值
    /// </summary>
    public class FormField
    {
        /// <summary>
        /// 欄位名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 欄位值
        /// </summary>
        public string Value { get; set; }
    }
}