using System;

namespace InfraManager.DAL.ServiceDesk
{
    public class KBArticleParameter
    {
        public Guid ID { get; set; }

        public int ReadCount { get; set; }

        public int UseCount { get; set; }

        public double Rating { get; set; }

        public int VoteCount { get; set; }
    }
}
