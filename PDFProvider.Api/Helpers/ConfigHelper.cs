using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PDFProvider.Api.Helpers
{
    /// <summary>
    /// Config Helper
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// PDF:Library Path
        /// </summary>
        public static string PdfLibPath
        {
            get
            {
                return ConfigurationManager.AppSettings["PDF:LibPath"];
            }
        }

        /// <summary>
        /// PDF:Log Enabled
        /// </summary>
        public static bool PdfLogEnabled
        {
            get
            {
                var value = ConfigurationManager.AppSettings["PDF:LogEnabled"];
                bool logEnabled;
                if (!bool.TryParse(value, out logEnabled))
                {
                    logEnabled = false;
                }

                return logEnabled;
            }
        }

        /// <summary>
        /// PDF:Log Path
        /// </summary>
        public static string PdfLogPath
        {
            get
            {
                return ConfigurationManager.AppSettings["PDF:LogPath"];
            }
        }

        /// <summary>
        /// PDF:Template Path
        /// </summary>
        public static string PdfTemplatePath
        {
            get
            {
                return ConfigurationManager.AppSettings["PDF:TemplatePath"];
            }
        }

        /// <summary>
        /// PDF:Working Path
        /// </summary>
        public static string PdfWorkingPath
        {
            get
            {
                return ConfigurationManager.AppSettings["PDF:WorkingPath"];
            }
        }

        /// <summary>
        /// PDF:Copy To Local
        /// </summary>
        public static bool PdfCopyToLocal
        {
            get
            {
                var value = ConfigurationManager.AppSettings["PDF:CopyToLocal"];
                bool copyToLocal;
                if (!bool.TryParse(value, out copyToLocal))
                {
                    copyToLocal = false;
                }

                return copyToLocal;
            }
        }
    }
}