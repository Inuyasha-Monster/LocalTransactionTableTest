using System;
using System.Collections.Generic;
using System.Text;

namespace Message
{
    public abstract class AbstractEvent
    {
        protected AbstractEvent()
        {
            EventId = Guid.NewGuid();
            EventCreateTime = DateTime.Now;
        }
        public Guid EventId { get; set; }
        public DateTime EventCreateTime { get; set; }
    }
}
