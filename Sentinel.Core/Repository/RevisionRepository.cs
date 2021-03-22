using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Repository
{
    public class RevisionRepository : IRevisionRepository
    {
        public int GetLatestCommittedRevisionId()
        {
            throw new System.NotImplementedException();
        }

        public int GetRevisionIdForEditing()
        {
            throw new System.NotImplementedException();
        }
    }
}