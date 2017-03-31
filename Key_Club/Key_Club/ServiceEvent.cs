using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key_Club
{
    public class ServiceEvent
    {
        public DateTime date { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public DateTime timeStart { get; set; }
        public DateTime timeEnd { get; set; }
        public string signUpLink { get; set; }


        public ServiceEvent()
        {

        }

        public ServiceEvent(DateTime d, string t, string desc)
        {
            date = d;
            title = t;
            description = desc;
        }

        public ServiceEvent(DateTime d, string t, string desc, string location, DateTime start, DateTime end)
        {
            date = d;
            title = t;
            description = desc;
            this.location = location;
            timeStart = start;
            timeEnd = end;
            signUpLink = "NONE";
        }

        public ServiceEvent(DateTime d, string t, string desc, string location, DateTime start, DateTime end, string link)
        {
            date = d;
            title = t;
            description = desc;
            this.location = location;
            timeStart = start;
            timeEnd = end;
            signUpLink = link;
        }
    }
}
