using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Productor.Data
{
    public class Entity<TPk> where TPk : struct
    {
        public TPk Id { get; set; }
    }

    public class EntityGuid : Entity<Guid>
    {

    }

    public class EntityGuidBaseField : EntityGuid
    {
        public DateTime CreateTime { get; set; }
    }
}
