namespace InfraManager.DAL;

public interface ILockableForOsi
{
    bool? IsLockedForOsi { get; set; }
}