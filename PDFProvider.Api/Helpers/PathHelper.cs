using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PDFProvider.Api.Helpers
{
    public class PathHelper
    {
        public static string GetExistTemplateFileName(string folderPath, string fileName)
        {
            string result = string.Empty;
            foreach (string fullname in Directory.EnumerateFiles(folderPath))
            {
                if (Path.GetFileName(fullname).ToLower() == fileName.ToLower())
                {
                    result = fullname;
                    break;
                }
            }
            return result;
        }
    }
}