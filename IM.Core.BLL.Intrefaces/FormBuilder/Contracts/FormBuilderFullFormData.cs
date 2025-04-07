using System.Collections.Generic;
using InfraManager.BLL.FormBuilder.Forms;

namespace InfraManager.BLL.FormBuilder.Contracts;

public class FormBuilderFullFormData
{
    public FormBuilderFormData Form { get; set; }
    public List<Elements> Elements { get; set; }
}
