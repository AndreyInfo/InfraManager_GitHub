using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InfraManager.Core.Helpers
{
    public static class ImageHelper
    {
        #region method Image2Base64String
        public static string Image2Base64String(SKImage image)
        {
            if (image == null)
                return null;
            //
            using (var memoryStream = new MemoryStream())
            using(var imageData = image.Encode(SKEncodedImageFormat.Bmp, 100))
            {
                imageData.SaveTo(memoryStream);
                byte[] data = memoryStream.GetBuffer();
                Array.Resize<byte>(ref data, (int)memoryStream.Length);
                return Convert.ToBase64String(data);
            }
        }
        #endregion

        #region method Base64String2Image
        public static SKImage Base64String2Image(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return null;
            //
            using (var memoryStream = new MemoryStream(Convert.FromBase64String(base64String)))
                    return SKImage.FromEncodedData(memoryStream);
        }
        #endregion

        public static string ToHtmlColor(this SKColor color)
        {
            return BitConverter.ToString(new byte[] { color.Red, color.Green, color.Blue }).Replace("-", "");            
        }
        public static int ToArgb(this SKColor color)
        {
            return (int)(uint)color;
        }
        public static SKColor ColorFromArgb(this int value)
        {
            return new SKColor((uint)value);
        }
        public static SKColor ColorFromHtml(this string value)
        {
            return new SKColor(byte.Parse(value.Substring(1, 2)), byte.Parse(value.Substring(3, 2)), byte.Parse(value.Substring(5, 2)), byte.Parse(value.Substring(7, 2)));
        }

        public static SKColor ParseFromString(this string value)
        {
            return SKColor.Parse(value);
        }
    }
}
