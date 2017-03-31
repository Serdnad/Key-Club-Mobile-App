using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key_Club
{
    public class Announcement
    {
        public string title { get; set; }
        public string description { get; set; }
        public string imgString { get; set; }
        public DateTime date { get; set; }

        public Announcement()
        {

        }

        public Announcement(string title, string description, DateTime date)
        {
            this.title = title;
            this.description = description;
            imgString = "NONE"; //change to default to path of a logo
            this.date = date;
        }

        public Announcement(string title, string description, string path, DateTime date)
        {
            this.title = title;
            this.description = description;
            imgString = path;
            this.date = date;
        }
    }
}
