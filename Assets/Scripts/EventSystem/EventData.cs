using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventFrame
{
    public class EventData
    {
        public EventID eid;

        protected EventData(EventID eid)
        {
            this.eid = eid;
        }

        public void Send()
        {
            EventManager.instance.SendEvent(this);
        }

        public static EventData CreateEvent(EventID eventId)
        {
            return new EventData(eventId);
        }
    }
}
