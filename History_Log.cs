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

    public class Download_History
    {

        //    id,"guid","current_path","target_path","start_time","received_bytes","total_bytes","state","danger_type","interrupt_reason","hash",
        //"end_time","opened","last_access_time","transient","referrer","site_url","tab_url","tab_referrer_url",
        //"http_method","by_ext_id","by_ext_name","etag","last_modified","mime_type","original_mime_type"

        public string browser { get; set; }

        public string id { get; set; }
        public string guid { get; set; }

        public string current_path { get; set; }

        public string target_path { get; set; }

        public string start_time { get; set; }

        public string received_bytes { get; set; }
        public string total_bytes { get; set; }
        public string state { get; set; }
        public string danger_type { get; set; }
        public string interrupt_reason { get; set; }
        public string hash { get; set; }
        public string end_time { get; set; }
        public string opened { get; set; }
        public string last_access_time { get; set; }
        public string transient { get; set; }
        public string referrer { get; set; }
        public string site_url { get; set; }

        public string tab_url { get; set; }
        public string tab_referrer_url { get; set; }
        public string http_method { get; set; }
        public string by_ext_id { get; set; }
        public string by_ext_name { get; set; }
        public string etag { get; set; }

        public string last_modified { get; set; }

        public string mime_type { get; set; }



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






        public static List<History>History_Grab()
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

















        public static List<Download_History> Downloads_Grab()
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

            var list = new List<Download_History>();

            foreach (var item in ChromiumPaths)
                list.AddRange(Downloads_Recovery(item.Value, item.Key));

            return list;
        }
        public static List<Download_History> Downloads_Recovery(string path, string browser, string table = "downloads")
        {

            //Get all created profiles from browser path
            //  List<string> loginDataFiles =new List<string>();
            // loginDataFiles.Add(defaultPath);

            List<string> loginDataFiles = GetAllProfiles(path);
            List<Download_History> data = new List<Download_History>();

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

                if (!SQLDatabase.ReadTable(table))
                    continue;

                for (int I = 0; I <= SQLDatabase.GetRowCount() - 1; I++)
                {
                    try
                    {
                        /*         public string browser { get; set; }

        public string id { get; set; }
        public string guid { get; set; }

        public string current_path { get; set; }

        public string target_path { get; set; }

        public string start_time { get; set; }

        public string received_bytes { get; set; }
        public string total_bytes { get; set; }
        public string state { get; set; }
        public string danger_type { get; set; }
        public string interrupt_reason { get; set; }
        public string hash { get; set; }
        public string end_time { get; set; }
        public string opened { get; set; }
        public string last_access_time { get; set; }
        public string transient { get; set; }
        public string referrer { get; set; }
        public string site_url { get; set; }

        public string tab_url { get; set; }
        public string tab_referrer_url { get; set; }
        public string http_method { get; set; }
        public string by_ext_id { get; set; }
        public string by_ext_name { get; set; }
        public string etag { get; set; }

        public string last_modified { get; set; }

        public string mime_type { get; set; }*/

                        //Get values with row number and column name
                        string ID = SQLDatabase.GetValue(I, "id");
                        string guid = SQLDatabase.GetValue(I, "guid");
                        string current_path = SQLDatabase.GetValue(I, "current_path");
                        string target_path = SQLDatabase.GetValue(I, "target_path");
                        string start_time = GetDateFromWebkitTime(SQLDatabase.GetValue(I, "start_time")).ToLocalTime().ToString();// SQLDatabase.GetValue(I, "start_time");
                        string received_bytes = SQLDatabase.GetValue(I, "received_bytes");
                        string total_bytes = SQLDatabase.GetValue(I, "total_bytes");
                        string end_time = GetDateFromWebkitTime(SQLDatabase.GetValue(I, "end_time")).ToLocalTime().ToString();// SQLDatabase.GetValue(I, "end_time");
                        string site_url = SQLDatabase.GetValue(I, "site_url");
                        string referrer = SQLDatabase.GetValue(I, "referrer");

                        string by_ext_id = SQLDatabase.GetValue(I, "by_ext_id");

                        string by_ext_name = SQLDatabase.GetValue(I, "by_ext_name");

                        string last_modified = SQLDatabase.GetValue(I, "last_modified");
                        string http_method = SQLDatabase.GetValue(I, "http_method");
                        string tab_url = SQLDatabase.GetValue(I, "tab_url");

                        string tab_referrer_url = SQLDatabase.GetValue(I, "tab_referrer_url");



                        //
                        //
                        if (!string.IsNullOrEmpty(guid) && !string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(start_time))
                            data.Add(new Download_History() { browser = browser, id = ID, guid = guid, current_path = current_path,
                                target_path = target_path, 
                                start_time = start_time,
                                received_bytes = received_bytes  ,
                                total_bytes = total_bytes ,
                                end_time = end_time,
                                site_url = site_url,
                                tab_referrer_url = tab_referrer_url,
                                tab_url = tab_url ,
                                by_ext_name = by_ext_name,
                                by_ext_id  = by_ext_id
                            }); ;
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
