using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace InfraManager.Core.Html
{
    public sealed class HtmlImageFileContext: IDisposable
    {
        #region Fields
        private readonly List<string> _filePaths;
        private readonly string _html;
        private bool _isDisposed;
        #endregion

        #region Properties
        #region IsDisposed
        public bool IsDisposed { get { return _isDisposed; } }
        #endregion

        #region Html
        public string Html { get { return _html; } }
        #endregion
        #endregion

        #region Constructors
        public HtmlImageFileContext(string html, IDictionary<Tuple<string, string>, byte[]> images, string tempFolderPath)
        {
            if (html == null)
                throw new ArgumentNullException("html");
            if (images == null)
                throw new NullReferenceException("images");
            if (tempFolderPath == null)
                throw new NullReferenceException("tempFolderPath");
            if (Path.GetInvalidPathChars().Any(ic => tempFolderPath.Contains(ic)))
                throw new ArgumentException("tempFolderPath");
            if (!Directory.Exists(tempFolderPath))
                throw new ArgumentException("tempFolderPath");
            //
            _filePaths = new List<string>();
            _html = html;
            _isDisposed = false;
            //
            foreach (var imageKey in images.Keys)
            {
                var filePath = string.Format(@"{0}\{1}.{2}", tempFolderPath.TrimEnd('\\'), Path.GetRandomFileName(), imageKey.Item2);
                _filePaths.Add(filePath);
                using (var fileStream = File.Create(filePath))
                {
                    fileStream.Write(images[imageKey], 0, images[imageKey].Length);
                    fileStream.Flush();
                }
                _html = Regex.Replace(_html, imageKey.Item1, filePath);
            }
        }
        #endregion

        #region static method Create
        /// <summary>
        /// Not implemented yet!
        /// </summary>
        /// <param name="html"></param>
        /// <param name="images"></param>
        /// <returns></returns>
        public static HtmlImageFileContext Create(string html, IDictionary<Tuple<string, string>, byte[]> images)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region method Dispose
        public void Dispose()
        {
            if (_isDisposed)
                return;
            //
            foreach (var filePath in _filePaths)
                if (File.Exists(filePath))
                    File.Delete(filePath);
            _isDisposed = true;
        }
        #endregion
    }
}
