using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consumer.MongoDbEntity
{
    public class CommonEventLog<T> : EventLogBase
    {
        public string MessageAssemblyName { get; set; }
        public string MessageClassFullName { get; set; }
        public string Body { get; set; }
        public T DatabaseId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
