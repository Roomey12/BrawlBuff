using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService.Models
{
    public class Player
    {
        public Club Club { get; set; }

        public int _3vs3Victories { get; set; }

        public bool IsQualifiedFromChampionshipChallenge { get; set; }

        public Icon Icon { get; set; }

        public string Tag { get; set; }

        public string Name { get; set; }

        public int Trophies { get; set; }

        public int ExpLevel { get; set; }

        public int ExpPoints { get; set; }

        public int HighestTrophies { get; set; }

        public int PowerPlayPoints { get; set; }

        public int HighestPowerPlayPoints { get; set; }

        public int SoloVictories { get; set; }

        public int DuoVictories { get; set; }

        public int BestRoboRumbleTime { get; set; }

        public int BestTimeAsBigBrawler { get; set; }

        public List<Brawler> Brawlers { get; set; }

        public string NameColor { get; set; }
    }
}
