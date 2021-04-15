using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mee6LevelsParser
{
    public class Ldrbrd
    {
        public bool admin { get; set; }
        public string banner_url { get; set; }
        public guild guild { get; set; }
        public bool is_member { get; set; }
        public string page { get; set; }
        public string player { get; set; }
        public LeaderboardUser[] players { get; set; }
    }
    public class guild
    {
        public bool allow_join { get; set; }
        public string icon { get; set; }
        public string id { get; set; }
        public bool invite_leaderboard { get; set; }
        public string leaderboard_url { get; set; }
        public string name { get; set; }
        public bool premium { get; set; }
    }
}
