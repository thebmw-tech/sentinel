using System;

namespace Sentinel.Core.Entities
{
    public class User : BaseEntity<User>
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}