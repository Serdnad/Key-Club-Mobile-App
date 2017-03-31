using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key_Club
{
    public static class UserInfo
    {
        static public string name { get; set; }
        static public string club { get; set; }
        static public string role { get; set; } //member or officer
        static public int ANDROID_GRID_HEIGHT { get; set; } //TEMPORARY solution to storing height of grid in recyclerview
		static public DateTime IOS_SELECTED_DATE { get; set; }

        public static void resetInfo()
        {
            name = "";
            club = "";
            role = "";
        }
    }
}
