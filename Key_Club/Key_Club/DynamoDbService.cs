using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key_Club
{
    public static class DynamoDbService
    {
        public static async Task getClubList()
        {
            AWSConfigs.RegionEndpoint = RegionEndpoint.USEast1;
            AWSConfigs.AWSProfileName = "demo"; //the name I originally setup

            var client = new AmazonDynamoDBClient("AKIAJDAMLVGBHVE7PHLQ", "pCJrAm1bwV8iekZWkP31rMxvPUThnFBdkFe5TALz", RegionEndpoint.USEast1);
            DynamoDBContext context = new DynamoDBContext(client);

            var request = new ScanRequest
            {
                TableName = "Clubs",
            };

            var results = await client.ScanAsync(request);

            foreach (Dictionary<string, AttributeValue> d in results.Items)
            {
                ClubInfo.clubs.Add(d["club"].S);
            }
        }

        public static async Task getUserInfo(string club, string name)
        {
            AWSConfigs.RegionEndpoint = RegionEndpoint.USEast1;
            AWSConfigs.AWSProfileName = "demo"; //the name I originally setup

            var client = new AmazonDynamoDBClient("AKIAJDAMLVGBHVE7PHLQ", "pCJrAm1bwV8iekZWkP31rMxvPUThnFBdkFe5TALz", RegionEndpoint.USEast1);
            DynamoDBContext context = new DynamoDBContext(client);

            var request = new QueryRequest
            {
                TableName = "Users",
                KeyConditionExpression = "club = :club and userName = :Name",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":club", new AttributeValue { S = club }},
                    {":Name", new AttributeValue { S = name }}
                }
            };

            var results = await client.QueryAsync(request);

            if (results.Count == 0)
                throw new Exception("No such user exists");

            foreach (Dictionary<string, AttributeValue> d in results.Items)
            {
                //SAVE TO DEVICE
                //Handled at platform level

                UserInfo.name = d["userName"].S;
                UserInfo.club = d["club"].S;
                UserInfo.role = d["role"].S;
            }
        }

		public static async void uploadAnnouncement(Announcement a)
        {
            AWSConfigs.RegionEndpoint = RegionEndpoint.USEast1;
            AWSConfigs.AWSProfileName = "demo"; //the name I originally setup

            var client = new AmazonDynamoDBClient("AKIAJDAMLVGBHVE7PHLQ", "pCJrAm1bwV8iekZWkP31rMxvPUThnFBdkFe5TALz", RegionEndpoint.USEast1);
            DynamoDBContext context = new DynamoDBContext(client);

            Table table = Table.LoadTable(client, "Announcements");
            var announcement = new Document();
            announcement["club"] = UserInfo.club;
            announcement["title"] = a.title;
            announcement["date"] = a.date.ToString();
            announcement["description"] = a.description;
            announcement["imgString"] = a.imgString;
            
            var put = await table.PutItemAsync(announcement);
        }

        public static async Task getAnnouncements()
        {
            AWSConfigs.RegionEndpoint = RegionEndpoint.USEast1;
            AWSConfigs.AWSProfileName = "demo"; //the name I originally setup

            var client = new AmazonDynamoDBClient("AKIAJDAMLVGBHVE7PHLQ", "pCJrAm1bwV8iekZWkP31rMxvPUThnFBdkFe5TALz", RegionEndpoint.USEast1);
            DynamoDBContext context = new DynamoDBContext(client);

            var request = new QueryRequest
            {
                TableName = "Announcements",
                KeyConditionExpression = "club = :club",
                ExpressionAttributeValues =
                {
                    {":club", new AttributeValue { S = UserInfo.club } }
                }
            };

            QueryResponse results = await client.QueryAsync(request);

            ClubInfo.announcements.Clear();
            foreach (Dictionary<string, AttributeValue> d in results.Items)
            {
                Announcement an = new Announcement();
                an.title = d["title"].S;
                an.date = DateTime.Parse(d["date"].S);
                an.description = d["description"].S;
                an.imgString = d["imgString"].S;

                ClubInfo.announcements.Add(an);
            }

            ClubInfo.announcements.Sort((a, b) => b.date.CompareTo(a.date));
        }

        public static async void uploadEvent(ServiceEvent e)
        {
            AWSConfigs.RegionEndpoint = RegionEndpoint.USEast1;
            AWSConfigs.AWSProfileName = "demo"; //the name I originally setup

            var client = new AmazonDynamoDBClient("AKIAJDAMLVGBHVE7PHLQ", "pCJrAm1bwV8iekZWkP31rMxvPUThnFBdkFe5TALz", RegionEndpoint.USEast1);
            DynamoDBContext context = new DynamoDBContext(client);

            Table table = Table.LoadTable(client, "Events");
            var sEvent = new Document();
            sEvent["club"] = UserInfo.club;
            sEvent["title"] = e.title;
            sEvent["datetimes"] = new DateTime(e.date.Year, e.date.Month, e.date.Day, e.timeStart.Hour, e.timeStart.Minute, e.timeStart.Second).ToString() + " - " + new DateTime(e.date.Year, e.date.Month, e.date.Day, e.timeEnd.Hour, e.timeEnd.Minute, e.timeEnd.Second).ToString();
            sEvent["description"] = e.description;
            sEvent["location"] = e.location;
            sEvent["link"] = e.signUpLink;

            var put = await table.PutItemAsync(sEvent);
        }

        public static async Task getEvents()
        {
            AWSConfigs.RegionEndpoint = RegionEndpoint.USEast1;
            AWSConfigs.AWSProfileName = "demo"; //the name I originally setup

            var client = new AmazonDynamoDBClient("AKIAJDAMLVGBHVE7PHLQ", "pCJrAm1bwV8iekZWkP31rMxvPUThnFBdkFe5TALz", RegionEndpoint.USEast1);
            DynamoDBContext context = new DynamoDBContext(client);

            var request = new QueryRequest
            {
                TableName = "Events",
                KeyConditionExpression = "club = :club",
                ExpressionAttributeValues =
                {
                    {":club", new AttributeValue { S = UserInfo.club } }
                }
            };

            QueryResponse results = await client.QueryAsync(request);

            ClubInfo.events.Clear();
            foreach (Dictionary<string, AttributeValue> d in results.Items)
            {
                ServiceEvent e = new ServiceEvent();
                e.title = d["title"].S;
                e.description = d["description"].S;
                e.location = d["location"].S;
                e.signUpLink = d["link"].S;

                string datetimes = d["datetimes"].S;
                string[] dates = datetimes.Split('-');

                e.timeStart = DateTime.Parse(dates[0]);
                e.timeEnd = DateTime.Parse(dates[1]);
                e.date = e.timeStart.Date;
                

                ClubInfo.events.Add(e);
            }

            //ClubInfo.events.OrderBy(((a, b) => a.date.CompareTo(b.date));
        }
    }
}
