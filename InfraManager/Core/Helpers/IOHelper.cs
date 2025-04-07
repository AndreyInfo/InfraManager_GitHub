using InfraManager.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace InfraManager.Core.Helpers
{
    public static class IOHelper
    {
        #region method GetFileEncoding
        public static Encoding GetFileEncoding(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            try
            {
                using (var stream = fileInfo.OpenRead())
                    return GetFileEncoding(stream);
            }
            catch (IOException)
            {
                return Encoding.Default;
            }
        }

        public static Encoding GetFileEncoding(byte[] fileContent)
        {
            using (var stream = new MemoryStream(fileContent))
                return GetFileEncoding(stream);
        }

        public static Encoding GetFileEncoding(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            Encoding result = null;
            var position = stream.Position;
            Encoding[] unicodeEncodings = { Encoding.BigEndianUnicode, Encoding.Unicode, Encoding.UTF8 };
            for (int i = 0; result == null && i < unicodeEncodings.Length; i++)
            {
                stream.Position = 0;
                byte[] preamble = unicodeEncodings[i].GetPreamble();
                bool preamblesAreEqual = true;
                for (int j = 0; preamblesAreEqual && j < preamble.Length; j++)
                    preamblesAreEqual = preamble[j] == stream.ReadByte();
                //
                if (preamblesAreEqual)
                    result = unicodeEncodings[i];
            }
            stream.Position = position;
            return result ?? Encoding.Default;
        }
        #endregion

        #region method GetTempFilePath
        public static string GetTempFilePath()
        {
            return string.Concat(Path.GetTempPath(), Path.GetRandomFileName());
        }

        public static string GetTempFilePath(string extension)
        {
            return Path.ChangeExtension(string.Concat(Path.GetTempPath(), Path.GetRandomFileName()), extension);
        }
        #endregion

        #region method ValidateFilePath
        public static void ValidateFilePath(string filePath)
        {
            try
            {
                string path = Path.GetDirectoryName(filePath);
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentException();
            }
            catch (ArgumentException)
            {
                throw new ArgumentValidationException("Путь к файлу не указан, содержит только пробелы или содержит запрещенные символы.");
            }
            catch (PathTooLongException)
            {
                throw new ArgumentValidationException("Длина пути к файлу превышает максимально допустимую.");
            }
            try
            {
                if (string.IsNullOrEmpty(Path.GetFileName(filePath)))
                    throw new ArgumentException();
            }
            catch (ArgumentException)
            {
                throw new ArgumentValidationException("Имя файла не указано или содержит запрещенные символы.");
            }
        }
        #endregion

        #region method ValidateFileName
        public static void ValidateFileName(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(Path.GetFileName(fileName)))
                    throw new ArgumentException();
            }
            catch (ArgumentException)
            {
                throw new ArgumentValidationException("Имя файла содержит запрещенные символы.");
            }
        }
        #endregion

        #region method ValidateDir
        public static void ValidateDir(string dir)
        {
            try
            {
                if (string.IsNullOrEmpty(dir))
                    throw new ArgumentException();
                Path.GetDirectoryName(dir);
            }
            catch (ArgumentException)
            {
                throw new ArgumentValidationException("Путь не указан, содержит только пробелы или содержит запрещенные символы.");
            }
            catch (PathTooLongException)
            {
                throw new ArgumentValidationException("Длина пути превышает максимально допустимую.");
            }
        }
        #endregion

        #region method ParseFileName
        public static void ParseFileName(string fileName, out string name, out string extension)
        {
            if (fileName == null)
                throw new ArgumentNullException("file");
            //
            int dotIndex = fileName.LastIndexOf('.');
            int slashIndex = fileName.LastIndexOf('\\');
            if (dotIndex > 0)
            { //есть точка
                if (dotIndex < slashIndex)
                { //точка - в имени пути. в файле - нет.
                    name = fileName.Substring(slashIndex + 1);
                    extension = string.Empty;
                }
                else if (dotIndex == slashIndex + 1)
                { //имя файла начинается с точки и больше точек не содержит
                    name = fileName.Substring(dotIndex);
                    extension = string.Empty;
                }
                else
                { //нормальный файл.
                    name = fileName.Substring(slashIndex + 1, dotIndex - slashIndex - 1);
                    extension = fileName.Substring(dotIndex + 1);
                }
            }
            else
            { //точек нет.
                name = fileName.Substring(slashIndex + 1);
                extension = string.Empty;
            }
        }
        #endregion

        #region method AccessIsGranted
        public static bool AccessIsGranted(string path, FileSystemRights rights)
        {
            try
            {
                FileInfo fileInfo;
                try
                {
                    fileInfo = new FileInfo(path);//securityEx + unauthorizedEx
                }
                catch (Exception ex)
                {
                    Logging.Logger.Error(ex, "Ошибка получения информации о файле {0}. Проверка доступа невозможна.", path);
                    return false;
                }
                //
                FileSecurity fs;
                try
                {
                    fs = fileInfo.GetAccessControl();//systsemEx + notHeldPrivEx
                }
                catch (Exception ex)
                {
                    Logging.Logger.Error(ex, "Ошибка получения информации о разрешениях к файлу {0}. Проверка доступа невозможна.", path);
                    return false;
                }
                //
                using (WindowsIdentity wi = WindowsIdentity.GetCurrent(false))
                {
                    Type ntType = typeof(NTAccount);
                    bool allow = false;
                    //
                    //формируем группы пользователя
                    List<IdentityReference> groups = new List<IdentityReference>(wi.Groups);
                    foreach (IdentityReference group in wi.Groups)
                        try
                        {
                            IdentityReference ir = group.Translate(ntType);//systemEx
                            groups.Add(ir);
                        }
                        catch (IdentityNotMappedException)
                        {
                            continue;
                        }
                        catch (Exception ex)
                        {
                            Logging.Logger.Warning(ex, "Ошибка распознавания группы пользователя {0}. SID {1} будет игнорирован.", wi.Name, group.ToString());
                            continue;//кривой sid
                        }
                    //
                    //проверяем соответствие разрешений файла к пользователю
                    foreach (FileSystemAccessRule permissions in fs.GetAccessRules(true, true, ntType))
                    {
                        IdentityReference ir;
                        try
                        {
                            ir = permissions.IdentityReference.Translate(ntType);//systemEx
                        }
                        catch (IdentityNotMappedException)
                        {
                            continue;
                        }
                        catch (Exception ex)
                        {
                            Logging.Logger.Warning(ex, "Ошибка распознавания группы в файле {0}. SID {1} будет игнорирован.", path, permissions.IdentityReference.ToString());
                            continue;//кривой sid
                        }
                        //
                        if ((permissions.FileSystemRights & rights) == rights)
                        {//нашли требуемый тип разрешения
                            foreach (IdentityReference ir2 in groups)
                                if (ir2 == ir)
                                {
                                    if (permissions.AccessControlType == AccessControlType.Deny)
                                        return false;//запрет превыше всего!
                                    else if (permissions.AccessControlType == AccessControlType.Allow)
                                        allow = true;//где-то есть разрешение (но нет ли запрета?)
                                }
                        }
                    }
                    //
                    return allow;
                }
            }
            catch (IdentityNotMappedException)
            {
                return false;
            }
            catch (Exception ex)
            {
                Logging.Logger.Error(ex, "Ошибка проверки прав доступа к файлу {0}.", path);
                return false;
            }
        }
        #endregion

        #region method GetUserIsolatedStorage
        public static IsolatedStorageFile GetUserIsolatedStorage()
        {
            return IsolatedStorageFile.GetUserStoreForAssembly();
        }
        #endregion

        #region method GetMD5FileHash
        //нельзя вызвать конструктор, если в локальных политиках безопасности включена опция "Системная криптография: использовать соответствующие стандарту FIPS 140 алгоритмы для шифрования, хэширования и подписывания."
        public static string GetMD5FileHash(string path)
        {
        start:
            try
            {
                using (var md5 = System.Security.Cryptography.MD5.Create())
                using (var stream = File.OpenRead(path))
                {
                    byte[] data = md5.ComputeHash(stream);
                    var retval = Encoding.Default.GetString(data);
                    return retval;
                }
            }
            catch (IOException)
            {
                System.Threading.Thread.Sleep(1000);
                goto start;
            }
        }

        public static string GetSHA512FileHash(string path)
        {
            start:
            try
            {
                using (var sha512 = System.Security.Cryptography.SHA512.Create())
                using (var stream = File.OpenRead(path))
                {
                    byte[] data = sha512.ComputeHash(stream);
                    var retval = Encoding.Default.GetString(data);
                    return retval;
                }
            }
            catch (IOException)
            {
                System.Threading.Thread.Sleep(1000);
                goto start;
            }
        }
        #endregion
    }
}
