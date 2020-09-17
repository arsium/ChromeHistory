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
        public string browser { get; set; }
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


        public static List<History> Grab()
        {
            Dictionary<string, string> ChromiumPaths = new Dictionary<string, string>()
            {
                {
                    "Chrome",
                    LocalApplicationData + @"\Google\Chrome\User Data"
                },
                {
                    "Opera",
                    Path.Combine(ApplicationData, @"Opera Software\Opera Stable")
                },
                {
                    "Yandex",
                    Path.Combine(LocalApplicationData, @"Yandex\YandexBrowser\User Data")
                },
                {
                    "360 Browser",
                    LocalApplicationData + @"\360Chrome\Chrome\User Data"
                },
                {
                    "Comodo Dragon",
                    Path.Combine(LocalApplicationData, @"Comodo\Dragon\User Data")
                },
                {
                    "CoolNovo",
                    Path.Combine(LocalApplicationData, @"MapleStudio\ChromePlus\User Data")
                },
                {
                    "SRWare Iron",
                    Path.Combine(LocalApplicationData, @"Chromium\User Data")
                },
                {
                    "Torch Browser",
                    Path.Combine(LocalApplicationData, @"Torch\User Data")
                },
                {
                    "Brave Browser",
                    Path.Combine(LocalApplicationData, @"BraveSoftware\Brave-Browser\User Data")
                },
                {
                    "Iridium Browser",
                    LocalApplicationData + @"\Iridium\User Data"
                },
                {
                    "7Star",
                    Path.Combine(LocalApplicationData, @"7Star\7Star\User Data")
                },
                {
                    "Amigo",
                    Path.Combine(LocalApplicationData, @"Amigo\User Data")
                },
                {
                    "CentBrowser",
                    Path.Combine(LocalApplicationData, @"CentBrowser\User Data")
                },
                {
                    "Chedot",
                    Path.Combine(LocalApplicationData, @"Chedot\User Data")
                },
                {
                    "CocCoc",
                    Path.Combine(LocalApplicationData, @"CocCoc\Browser\User Data")
                },
                {
                    "Elements Browser",
                    Path.Combine(LocalApplicationData, @"Elements Browser\User Data")
                },
                {
                    "Epic Privacy Browser",
                    Path.Combine(LocalApplicationData, @"Epic Privacy Browser\User Data")
                },
                {
                    "Kometa",
                    Path.Combine(LocalApplicationData, @"Kometa\User Data")
                },
                {
                    "Orbitum",
                    Path.Combine(LocalApplicationData, @"Orbitum\User Data")
                },
                {
                    "Sputnik",
                    Path.Combine(LocalApplicationData, @"Sputnik\Sputnik\User Data")
                },
                {
                    "uCozMedia",
                    Path.Combine(LocalApplicationData, @"uCozMedia\Uran\User Data")
                },
                {
                    "Vivaldi",
                    Path.Combine(LocalApplicationData, @"Vivaldi\User Data")
                },
                {
                    "Sleipnir 6",
                    Path.Combine(ApplicationData, @"Fenrir Inc\Sleipnir5\setting\modules\ChromiumViewer")
                },
                {
                    "Citrio",
                    Path.Combine(LocalApplicationData, @"CatalinaGroup\Citrio\User Data")
                },
                {
                    "Coowon",
                    Path.Combine(LocalApplicationData, @"Coowon\Coowon\User Data")
                },
                {
                    "Liebao Browser",
                    Path.Combine(LocalApplicationData, @"liebao\User Data")
                },
                {
                    "QIP Surf",
                    Path.Combine(LocalApplicationData, @"QIP Surf\User Data")
                },
                {
                    "Edge Chromium",
                    Path.Combine(LocalApplicationData, @"Microsoft\Edge\User Data")
                }
            };

            var list = new List<History>();

            foreach (var item in ChromiumPaths)
                list.AddRange(History_Recovery(item.Value, item.Key));

            return list;
        }


        private static List<string> GetAllProfiles(string DirectoryPath)
        {
            List<string> loginDataFiles = new List<string>
            {
                DirectoryPath + @"\Default\History",
                DirectoryPath + @"\History"
            };

            if (Directory.Exists(DirectoryPath))
            {
                foreach (string dir in Directory.GetDirectories(DirectoryPath))
                {
                    if (dir.Contains("Profile"))
                        loginDataFiles.Add(dir + @"\History");
                }
            }

            return loginDataFiles;
        }

        public static List<History> History_Recovery(string path, string browser, string table = "urls")
        {

            //Get all created profiles from browser path
            //  List<string> loginDataFiles =new List<string>();
            // loginDataFiles.Add(defaultPath);

            List<string> loginDataFiles = GetAllProfiles(path);
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
                            data.Add(new History() { browser = browser , ID = ID, URL = host, Title = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(Title)), visit_count = visit_count , typed_count = typed_count , last_visit_Time = last_visit_Time }); ;
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
