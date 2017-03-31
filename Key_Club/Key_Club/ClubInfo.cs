using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key_Club
{
    public static class ClubInfo
    {
        public static HashSet<string> clubs = new HashSet<string>();
        public static List<Announcement> announcements = new List<Announcement>();
        public static HashSet<ServiceEvent> events = new HashSet<ServiceEvent>();
    }
}
