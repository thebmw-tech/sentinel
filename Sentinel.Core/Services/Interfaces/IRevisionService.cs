using Sentinel.Models;

namespace Sentinel.Core.Services.Interfaces
{
    public interface IRevisionService
    {
        RevisionDTO CreateRevisionForEditing();

        void RollbackRevision(int revisionId);
        void RollbackIfNotConfirmed(int revisionId);
        void CommitRevision(int revisionId);
        void ConfirmRevision(int revisionId);

        void CleanupOldLocks();

        RevisionDTO GetRevisionById(int revisionId);
        RevisionDTO GetSafe();
        void UpdateRevisionLock(int revisionId);
        void UnlockRevision(int revisionId);
        void CleanupAllLocks();
        RevisionDTO ResumeEditingRevision();
    }
}