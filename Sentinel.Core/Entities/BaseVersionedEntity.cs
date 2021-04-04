using System;
using System.Reflection;

namespace Sentinel.Core.Entities
{
    public abstract class BaseVersionedEntity<T> : BaseEntity<T>, IEquatable<T> where T : BaseVersionedEntity<T>
    {
        public int RevisionId { get; set; }
        public bool Enabled { get; set; }

        public virtual bool Equals(T? other)
        {
            if (GetType() != typeof(T)) throw new NotSupportedException();
            if (other == null) return false;

            foreach (var property in GetType().GetProperties())
            {
                if (!property.GetAccessors()[0].IsVirtual && property.Name != "RevisionId")
                {
                    if (property.GetValue(this) != property.GetValue(other))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public T GetCopyForRevision(int revisionId)
        {
            T output = (T)this.MemberwiseClone();
            output.RevisionId = revisionId;
            return output;
        }
    }
}