using BrawlBuff.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Common.Comparers
{
    public class EventComparer : IEqualityComparer<Event>
    {
        // mb rewrite this method, FE for case when db Mode == null and incoming is not
        public bool Equals(Event x, Event y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.BrawlEventId == y.BrawlEventId && x.Mode == y.Mode && x.Map == y.Map;
        }

        public int GetHashCode(Event myEvent)
        {
            if (Object.ReferenceEquals(myEvent, null)) return 0;

            int hashBrawlEventId = myEvent.BrawlEventId.GetHashCode();
            int hashMode = myEvent.Mode == null ? 0 : myEvent.Mode.GetHashCode();
            int hashMap = myEvent.Map == null ? 0 : myEvent.Map.GetHashCode();

            return hashBrawlEventId ^ hashMode ^ hashMap;
        }
    }
}
