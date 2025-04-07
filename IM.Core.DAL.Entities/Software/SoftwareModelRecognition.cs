using System;

namespace InfraManager.DAL.Software;

public class SoftwareModelRecognition
{
    public Guid SoftwareModelID { get; init; }
    public int VersionRecognitionID { get; init; }
    public int VersionRecognitionLvl { get; init; }
    public int RedactionRecognition { get; init; }

    public virtual SoftwareModel SoftwareModel { get; set; }

}
