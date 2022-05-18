using BrawlBuff.Domain.Common;
using BrawlBuff.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Domain.Entities
{
    public class Event : AuditableEntity
    {
        public int Id { get; set; }
        public int BrawlEventId { get; set; }
        public string? Mode { get; set; }
        public string? Map { get; set; }
        public string? ImageUrl { get; set; }

        public static EventType GetEventType(string eventMode)
        {
            // if (eventMode == null)

            var event3vs3 = new List<string>()
            {
                "bounty", "gemGrab", "brawlBall", "heist", "siege", "hotZone", "knockout",
                "trophyThieves", "holdTheTrophy", "volleyBrawl", "basketBrawl"
            };

            var event1vs1 = new List<string>()
            {
                "duels"
            };

            var eventSoloPlayers = new List<string>()
            {
                "soloShowdown", "takedown", "loneStar"
            };

            var event5of2 = new List<string>()
            {
                "duoShowdown"
            };

            var event3players = new List<string>()
            {
                "superCityRampage", "roboRumble", "bossFight"
            };

            var event5vs1 = new List<string>()
            {
                "bigGame"
            };

            var eventSolo = new List<string>()
            {
                "training"
            };

            var selectedEvent = event3vs3.FirstOrDefault(x => x.Equals(eventMode, StringComparison.OrdinalIgnoreCase)) ??
                event1vs1.FirstOrDefault(x => x.Equals(eventMode, StringComparison.OrdinalIgnoreCase)) ??
                eventSoloPlayers.FirstOrDefault(x => x.Equals(eventMode, StringComparison.OrdinalIgnoreCase)) ??
                event5of2.FirstOrDefault(x => x.Equals(eventMode, StringComparison.OrdinalIgnoreCase)) ??
                event3players.FirstOrDefault(x => x.Equals(eventMode, StringComparison.OrdinalIgnoreCase)) ??
                event5vs1.FirstOrDefault(x => x.Equals(eventMode, StringComparison.OrdinalIgnoreCase)) ??
                eventSolo.FirstOrDefault(x => x.Equals(eventMode, StringComparison.OrdinalIgnoreCase));

            //if (selectedEvent == null) // do smth

            if (event3vs3.Contains(selectedEvent)) return EventType.Event3vs3;
            if (event1vs1.Contains(selectedEvent)) return EventType.Event1vs1;
            if (eventSoloPlayers.Contains(selectedEvent)) return EventType.EventSoloPlayers;
            if (event5of2.Contains(selectedEvent)) return EventType.Event5of2;
            if (event3players.Contains(selectedEvent)) return EventType.Event3Players;
            if (event5vs1.Contains(selectedEvent)) return EventType.Event5vs1;
            if (eventSolo.Contains(selectedEvent)) return EventType.EventSolo;
            return EventType.Unknown;
        }
    }
}
