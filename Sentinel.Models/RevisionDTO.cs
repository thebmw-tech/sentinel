using System;

namespace Sentinel.Models
{
    public class RevisionDTO
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CommitDate { get; set; }
        public DateTime? ConfirmDate { get; set; }
    }
}
