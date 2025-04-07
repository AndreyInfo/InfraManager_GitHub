namespace InfraManager.BLL.ServiceDesk.Solutions;

public class SolutionData
{
    public string Name { get; init; }
    public string Description { get; init; }
    public string HTMLDescription 
    {
        get => $"<body>\r\n\t<p style=\"text-align:left;text-indent:0pt;margin:0pt 0pt 0pt 0pt;\"><span style=\"color:#000000;background-color:transparent;font-family:'Times New Roman';font-size:12pt;font-weight:normal;font-style:normal;\">{Name}</span></p></body>\r\n";
    }

    public byte[] RowVersion { get; init; }
}