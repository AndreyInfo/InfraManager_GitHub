namespace InfraManager.WebApi.Contracts.Models.Documents
{
    public class DocumentFileModel
    {
        /// <summary>
        /// Содержимое файла
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// Тип файла
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// Имя файла
        /// </summary>
        public string FileName { get; set; }
    }
}
