using BrawlBuff.Domain.Enums;

namespace BrawlBuff.Domain.Extensions;

public static class EventTypeExtensions
{
    public static string GetString(this EventType eventType)
    {
        switch (eventType)
        {
            case EventType.Event3vs3:
                return "event3vs3";
            case EventType.Event1vs1:
                return "event1vs1";
            case EventType.EventSoloPlayers:
                return "eventSoloPlayers";
            case EventType.Event5of2:
                return "event5of2";
            case EventType.Event3Players:
                return "event3Players";
            case EventType.Event5vs1:
                return "event5vs1";
            case EventType.EventSolo:
                return "eventSolo";
            case EventType.Unknown:
                return "eventSolo";
            default:
                return "NO VALUE GIVEN";
        }
    }
}