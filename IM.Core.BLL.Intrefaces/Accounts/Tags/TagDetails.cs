namespace InfraManager.BLL.Accounts.Tags
{
    public class TagDetails
    {
        public TagDetails()
        {

        }

        public TagDetails(int ID, string Name)
        {
            this.ID = ID;
            this.Name = Name;
        }

        public int ID { get; set; }
        public string Name { get; set; }
    }
}