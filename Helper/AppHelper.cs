using LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Helper
{
    public static class AppHelper
    {
        public static string GetImagePath(string fileName)
        {
            try
            {
                string appPath = AppDomain.CurrentDomain.BaseDirectory;
                string imagePath = Path.Combine(appPath, "Images", fileName);
                if (File.Exists(imagePath))
                    return imagePath;

                string debugBase = Directory.GetParent(appPath)?.Parent?.Parent?.Parent?.FullName;
                if (!string.IsNullOrEmpty(debugBase))
                {
                    string debugImagePath = Path.Combine(debugBase, "Images", fileName);
                    if (File.Exists(debugImagePath))
                        return debugImagePath;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, $"Error getting image path for {fileName}");
                return string.Empty;
            }
        }

        public static string ConvertIntoHex(string data)
        {
            if (string.IsNullOrEmpty(data))
                return string.Empty;

            var sb = new StringBuilder(data.Length * 4);

            foreach (char ch in data)
            {
                sb.Append(((int)ch).ToString("X4"));
            }

            return sb.ToString();
        }

        public static string ConvertIntoHexBigEndian(string data)
        {
            if (string.IsNullOrEmpty(data))
                return string.Empty;

            byte[] bytes = Encoding.BigEndianUnicode.GetBytes(data);

            var sb = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

    }

}
