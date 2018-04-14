using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Productor.Data
{
    public class MqMessage
    {
        public MqMessage()
        {
            Id = Guid.NewGuid();
            CreateTime = DateTime.Now;
        }
        public Guid Id { get; set; }
        public string MessageAssemblyName { get; set; }
        public string MessageClassFullName { get; set; }
        public string Body { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
