﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Sentinel.Core.Entities
{
    public class Revision : BaseEntity<Revision>
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CommitDate { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public DateTime? Locked { get; set; }

        public bool HasChanges { get; set; }
        public bool Deleted { get; set; }
    }
}