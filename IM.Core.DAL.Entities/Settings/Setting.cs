namespace InfraManager.DAL.Settings
{
    public class Setting
    {
        public SystemSettings Id { get; init; }
        public byte[] Value { get; set; }
        public byte[] RowVersion { get; set; }

        public Setting() { }
        public Setting(SystemSettings id, byte[] value)
        {
            Id = id;
            Value = value;
        }
    }
}
