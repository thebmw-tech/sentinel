namespace Sentinel.Core.Repository.Interfaces
{
    public interface IRevisionRepository
    {
        int GetSafeCurrentRevision();
        int GetCurrentRevision();
        int? GetInProgressRevision();
    }
}