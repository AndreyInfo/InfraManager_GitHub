using System;

namespace InfraManager.BLL.ServiceDesk
{
    public class DocumentDataDetails
    {
        public Guid Id { get; init; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public byte[] Data { get; set; }
    }
}
