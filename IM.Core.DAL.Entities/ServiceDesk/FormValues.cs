using InfraManager.DAL.FormBuilder;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.ServiceDesk
{

    public class FormValues
    {
        public long ID { get; set; }

        public Guid FormBuilderFormID { get; set; }

        public virtual Form Form { get; }

        public virtual ICollection<Values> Values { get; set; } = new HashSet<Values>();

        public byte[] RowVersion { get; set; }
    }
}
