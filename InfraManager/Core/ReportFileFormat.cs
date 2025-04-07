using InfraManager.Core;

namespace InfraManager.Core
{
    public enum ReportFileFormat : byte
    {
        [FriendlyName("Pdf, Portable Document Format - Файл Adobe Acrobat")]
        Pdf = 0,

        [FriendlyName("txt, Простой текстовый файл")]
        Text = 1,

        [FriendlyName("Jpeg, Графический растровый формат")]
        Image = 2,

        [FriendlyName("Html, HyperText Markup Language - Специальный файл текстового типа")]
        Html = 3,

        [FriendlyName("Mht, MIME HTML - Заархивированная интернет-страница")]
        Mht = 4,

        [FriendlyName("Csv, Comma-Separated Values - Текстовый файл")]
        Csv = 5,

        [FriendlyName("Docx, Файл документа Word 2007 с поддержкой XML")]
        Docx = 6,

        [FriendlyName("Rtf, Rich Text Format - Расширенный формат текстового файла")]
        Rtf = 7,

        [FriendlyName("Xls, Рабочая книга Microsoft Excel")]
        Xls = 8,

        [FriendlyName("Xlsx, Рабочая книга Excel 2007 с поддержкой XML")]
        Xlsx = 9
    }
}
