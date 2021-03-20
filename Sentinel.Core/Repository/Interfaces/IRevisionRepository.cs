namespace Sentinel.Core.Repository.Interfaces
{
    public interface IRevisionRepository
    {
        int GetRevisionIdForEditing();
        int GetLatestCommittedRevisionId();
    }
}