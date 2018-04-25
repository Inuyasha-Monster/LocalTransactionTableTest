using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consumer.MongoDbEntity
{
    public class CommonEventEntity : ConsumerLogBase<Guid>
    {
        public string MessageAssemblyName { get; set; }
        public string MessageClassFullName { get; set; }
        public string Body { get; set; }
    }
}
