using System;
using System.Collections.Generic;

namespace Sentinel.Core.Entities
{
    public class User : BaseEntity<User>
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }


        public ICollection<UserKey> Keys { get; set; }
    }
}