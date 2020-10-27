using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Core.Entities.Base
{
    public abstract class EntityBase<Tid> : IEntityBase<Tid>
    {
        public virtual Tid Id { get; protected set; }

        int? _requestedHashCode;

        public bool IsTransient()
        {
            return Id.Equals(default(Tid));
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is EntityBase<Tid>))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            var itm = (EntityBase<Tid>)obj;

            if (itm.IsTransient() || IsTransient())
                return false;
            else
                return itm == this;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (_requestedHashCode.HasValue)
                    _requestedHashCode = Id.GetHashCode() ^ 31;

                return _requestedHashCode.Value;
            }
            return base.GetHashCode();
        }

        public static bool operator ==(EntityBase<Tid> l, EntityBase<Tid> r)
        {
            if (Equals(l, null))
                return Equals(r, null) ? true : false;
            else
                return l.Equals(r);
        }

        public static bool operator !=(EntityBase<Tid> l, EntityBase<Tid> r)
        {
            return !(l == r);
        }
    }
}
