using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mee6LevelsParser
{
    public struct LeaderboardUser
    {
        public uint rank { get; set; }
        public string level { get; set; }
        public string username { get; set; }
        public string id { get; set; }
        public string discriminator { get; set; }
        public string avatar { get; set; }
        public string message_count { get; set; }
        public string xp { get; set; }
    }
}
