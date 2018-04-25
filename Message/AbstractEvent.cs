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
            EventUtcTime = DateTime.UtcNow;
        }
        public Guid EventId { get; set; }
        public DateTime EventUtcTime { get; set; }
    }
}
