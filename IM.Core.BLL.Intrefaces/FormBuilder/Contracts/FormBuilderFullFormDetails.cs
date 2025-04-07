using System.Collections.Generic;

namespace InfraManager.BLL.FormBuilder.Contracts
{
    public class FormBuilderFullFormDetails
    {
        public FormBuilderFormDetails Form { get; set; }
        public List<Elements> Elements { get; set; }
    }

    public class Elements
    {
        public FormBuilderFormTabDetails Tab { get; set; }

        public List<FormBuilderFormTabFieldDetails> TabElements { get; set; }
    }
}
