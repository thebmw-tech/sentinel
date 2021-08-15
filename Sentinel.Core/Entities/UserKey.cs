using System;

namespace Sentinel.Core.Entities
{
    public class UserKey : BaseEntity<UserKey>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string KeyType { get; set; }
        public byte[] Key { get; set; }

        public User User { get; set; }
    }
}