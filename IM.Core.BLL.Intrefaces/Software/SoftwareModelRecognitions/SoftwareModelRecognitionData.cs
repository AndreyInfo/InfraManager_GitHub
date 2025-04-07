using System;

namespace InfraManager.BLL.Software.SoftwareModelRecognitions;

public class SoftwareModelRecognitionData
{
    public Guid SoftwareModelID { get; init; }
    public int VersionRecognitionID { get; init; }
    public int VersionRecognitionLvl { get; init; }
    public int RedactionRecognition { get; init; }
}
