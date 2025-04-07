using System;

namespace InfraManager.BLL.CrudWeb
{
    [Obsolete("DeleteModel используется для множественного удаления, переходить на единичное(не использовать DeleteModel")]
    public class DeleteModel<T>
    {
        public T ID { get; set; }
        public string Name { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
