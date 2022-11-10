using BrawlBuff.Domain.Entities;

namespace BrawlBuff.Application.Common.Comparers;

public class EventComparer : IEqualityComparer<Event>
{
    // mb rewrite this method, FE for case when db Mode == null and incoming is not
    public bool Equals(Event x, Event y)
    {
        if (Object.ReferenceEquals(x, y)) return true;

        if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
            return false;

        return x.BrawlEventId == y.BrawlEventId;
    }

    public int GetHashCode(Event myEvent)
    {
        if (Object.ReferenceEquals(myEvent, null)) return 0;

        return myEvent.BrawlEventId.GetHashCode();
    }
}