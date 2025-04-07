namespace InfraManager.UI.Web.Models
{
    public sealed class ConfigLoginModel
    {
        public ConfigLoginModel()
        { }

        #region properties
        public bool Invalid { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        #endregion
    }
}
