namespace InfraManager.BLL.CreepingLines;

public class CreepingLineData
{
    public string Name { get; init; }
    public bool Visible { get; init; }
    public byte[] RowVersion { get; set; }
}