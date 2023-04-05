using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Topshelf;

public class FioModule : ModuleBase<SocketCommandContext>
{
    [Command("!fio")]
    public async Task FioAsync(string playerName)
    {
        Console.WriteLine("FioAsync called");
        Console.WriteLine($"Received player name: {playerName}");
        double rating = GetPlayerRatingFromDatabase(playerName);
        if (rating != 0.0)
        {
            Console.WriteLine($"Player Raiting: {rating}");
            Console.WriteLine($"Sending message: {playerName}'s rating is: {rating}");
            await ReplyAsync($"{playerName}'s rating is: {rating}\nCheck on website: http://mythic-rating.ddns.net:8080/player_runs.php?playerName={playerName}");

            var records = new List<dynamic>()
            {
                new { Dungeon = "DOS", F = GetMythicLevelFromDatabase("playerlist_dos_forti", playerName), T = GetMythicLevelFromDatabase("playerlist_dos_tyra", playerName) },
                new { Dungeon = "HOA", F = GetMythicLevelFromDatabase("playerlist_hoa_forti", playerName), T = GetMythicLevelFromDatabase("playerlist_hoa_tyra", playerName) },
                new { Dungeon = "MOTS", F = GetMythicLevelFromDatabase("playerlist_mots_forti", playerName), T = GetMythicLevelFromDatabase("playerlist_mots_tyra", playerName) },
                new { Dungeon = "PF", F = GetMythicLevelFromDatabase("playerlist_pf_forti", playerName), T = GetMythicLevelFromDatabase("playerlist_pf_tyra", playerName) },
                new { Dungeon = "SD", F = GetMythicLevelFromDatabase("playerlist_sd_forti", playerName), T = GetMythicLevelFromDatabase("playerlist_sd_tyra", playerName) },
                new { Dungeon = "SG", F = GetMythicLevelFromDatabase("playerlist_sg_forti", playerName), T = GetMythicLevelFromDatabase("playerlist_sg_tyra", playerName) },
                new { Dungeon = "SOW", F = GetMythicLevelFromDatabase("playerlist_sow_forti", playerName), T = GetMythicLevelFromDatabase("playerlist_sow_tyra", playerName) },
                new { Dungeon = "SOA", F = GetMythicLevelFromDatabase("playerlist_soa_forti", playerName), T = GetMythicLevelFromDatabase("playerlist_soa_tyra", playerName) },
                new { Dungeon = "TOP", F = GetMythicLevelFromDatabase("playerlist_top_forti", playerName), T = GetMythicLevelFromDatabase("playerlist_top_tyra", playerName) },
                new { Dungeon = "NW", F = GetMythicLevelFromDatabase("playerlist_nw_forti", playerName), T = GetMythicLevelFromDatabase("playerlist_nw_tyra", playerName) },
            };

            int maxDungeonLength = records.Max(r => r.Dungeon.Length);
            int maxFLength = records.Max(r => r.F.Length);
            int maxNLength = records.Max(r => r.T.Length);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($" {new string(' ', maxDungeonLength )}  F{new string(' ', maxFLength )}  T");

            foreach (var record in records)
            {
                sb.AppendLine($"{record.Dungeon.PadRight(maxDungeonLength + 2)} {record.F.PadRight(maxFLength + 2)} {record.T.PadRight(maxNLength + 2)}");
            }

            await ReplyAsync($"```{sb.ToString()}```");
        }
        else
        {
            await ReplyAsync($"We couldn't find any data for player {playerName}");
        }

    }



[Command("!dos")]
    public async Task dosasync()
    {
        string connectionString = "server=localhost;user=root;database=firestorm_mythicplus;port=3306;password=******";
        StringBuilder sb = new StringBuilder();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT CASE WHEN table_name = 'playerlist_dos_forti' THEN 'DOS Fortified' WHEN table_name = 'playerlist_dos_tyra' THEN 'DOS Tyrannical' ELSE 'N/A' END as 'Dungeon', MythicLevel, Time, Groups, Completed, Raiting FROM (SELECT 'playerlist_dos_forti' as table_name, MythicLevel, Time, Groups, Completed, Raiting FROM playerlist_dos_forti UNION SELECT 'playerlist_dos_tyra' as table_name, MythicLevel, Time, Groups, Completed, Raiting FROM playerlist_dos_tyra) as combined_table ORDER BY Raiting DESC LIMIT 5", connection);
            var records = new List<dynamic>();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    records.Add(new
                    {
                        Dungeon = reader["Dungeon"].ToString(),
                        MythicLevel = reader["MythicLevel"].ToString(),
                        Time = reader["Time"].ToString(),
                        Groups = reader["Groups"].ToString(),
                        Completed = reader["Completed"].ToString(),
                        Raiting = reader["Raiting"].ToString()
                    });
                }
            }
            int maxDungeonLength = records.Max(r => r.Dungeon.Length);
            int maxMythicLevelLength = records.Max(r => r.MythicLevel.Length);
            int maxTimeLength = records.Max(r => r.Time.Length);
            int maxGroupsLength = records.Max(r => r.Groups.Length);
            int maxCompletedLength = records.Max(r => r.Completed.Length);
            int maxRaitingLength = records.Max(r => r.Raiting.Length);

            foreach (var record in records)
            {
                sb.AppendLine($"{record.Dungeon.PadRight(maxDungeonLength)}    {record.MythicLevel.PadRight(maxMythicLevelLength)}    {record.Time.PadRight(maxTimeLength)}    {record.Groups.PadRight(maxGroupsLength)}    {record.Completed.PadRight(maxCompletedLength)}    {record.Raiting.PadRight(maxRaitingLength)}");
            }
            await ReplyAsync($"```{sb.ToString()}```");
            connection.Close();
        }
    }


    [Command("!hoa")]
    public async Task hoaasync()
    {
        string connectionString = "server=localhost;user=root;database=firestorm_mythicplus;port=3306;password=******";
        StringBuilder sb = new StringBuilder();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT CASE WHEN table_name = 'playerlist_hoa_forti' THEN 'HOA Fortified' WHEN table_name = 'playerlist_hoa_tyra' THEN 'HOA Tyrannical' ELSE 'N/A' END as 'Dungeon', MythicLevel, Time, Groups, Completed, Raiting FROM (SELECT 'playerlist_hoa_forti' as table_name, MythicLevel, Time, Groups, Completed, Raiting FROM playerlist_hoa_forti UNION SELECT 'playerlist_hoa_tyra' as table_name, MythicLevel, Time, Groups, Completed, Raiting FROM playerlist_hoa_tyra) as combined_table ORDER BY Raiting DESC LIMIT 5", connection);
            var records = new List<dynamic>();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    records.Add(new
                    {
                        Dungeon = reader["Dungeon"].ToString(),
                        MythicLevel = reader["MythicLevel"].ToString(),
                        Time = reader["Time"].ToString(),
                        Groups = reader["Groups"].ToString(),
                        Completed = reader["Completed"].ToString(),
                        Raiting = reader["Raiting"].ToString()
                    });
                }
            }
            int maxDungeonLength = records.Max(r => r.Dungeon.Length);
            int maxMythicLevelLength = records.Max(r => r.MythicLevel.Length);
            int maxTimeLength = records.Max(r => r.Time.Length);
            int maxGroupsLength = records.Max(r => r.Groups.Length);
            int maxCompletedLength = records.Max(r => r.Completed.Length);
            int maxRaitingLength = records.Max(r => r.Raiting.Length);

            foreach (var record in records)
            {
                sb.AppendLine($"{record.Dungeon.PadRight(maxDungeonLength)}    {record.MythicLevel.PadRight(maxMythicLevelLength)}    {record.Time.PadRight(maxTimeLength)}    {record.Groups.PadRight(maxGroupsLength)}    {record.Completed.PadRight(maxCompletedLength)}    {record.Raiting.PadRight(maxRaitingLength)}");
            }
            await ReplyAsync($"```{sb.ToString()}```");
            connection.Close();
        }
    }


    [Command("!pf")]
    public async Task pfasync()
    {
        string connectionString = "server=localhost;user=root;database=firestorm_mythicplus;port=3306;password=******";
        StringBuilder sb = new StringBuilder();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT CASE WHEN table_name = 'playerlist_pf_forti' THEN 'PF Fortified' WHEN table_name = 'playerlist_pf_tyra' THEN 'PF Tyrannical' ELSE 'N/A' END as 'Dungeon', MythicLevel, Time, Groups, Completed, Raiting FROM (SELECT 'playerlist_pf_forti' as table_name, MythicLevel, Time, Groups, Completed, Raiting FROM playerlist_pf_forti UNION SELECT 'playerlist_pf_tyra' as table_name, MythicLevel, Time, Groups, Completed, Raiting FROM playerlist_pf_tyra) as combined_table ORDER BY Raiting DESC LIMIT 5", connection);
            var records = new List<dynamic>();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    records.Add(new
                    {
                        Dungeon = reader["Dungeon"].ToString(),
                        MythicLevel = reader["MythicLevel"].ToString(),
                        Time = reader["Time"].ToString(),
                        Groups = reader["Groups"].ToString(),
                        Completed = reader["Completed"].ToString(),
                        Raiting = reader["Raiting"].ToString()
                    });
                }
            }
            int maxDungeonLength = records.Max(r => r.Dungeon.Length);
            int maxMythicLevelLength = records.Max(r => r.MythicLevel.Length);
            int maxTimeLength = records.Max(r => r.Time.Length);
            int maxGroupsLength = records.Max(r => r.Groups.Length);
            int maxCompletedLength = records.Max(r => r.Completed.Length);
            int maxRaitingLength = records.Max(r => r.Raiting.Length);

            foreach (var record in records)
            {
                sb.AppendLine($"{record.Dungeon.PadRight(maxDungeonLength)}    {record.MythicLevel.PadRight(maxMythicLevelLength)}    {record.Time.PadRight(maxTimeLength)}    {record.Groups.PadRight(maxGroupsLength)}    {record.Completed.PadRight(maxCompletedLength)}    {record.Raiting.PadRight(maxRaitingLength)}");
            }
            await ReplyAsync($"```{sb.ToString()}```");
            connection.Close();
        }
    }

    [Command("!mots")]
    public async Task motsasync()
    {
        string connectionString = "server=localhost;user=root;database=firestorm_mythicplus;port=3306;password=******";
        StringBuilder sb = new StringBuilder();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT CASE WHEN table_name = 'playerlist_mots_forti' THEN 'MOTS Fortified' WHEN table_name = 'playerlist_mots_tyra' THEN 'MOTS Tyrannical' ELSE 'N/A' END as 'Dungeon', MythicLevel, Time, Groups, Completed, Raiting FROM (SELECT 'playerlist_mots_forti' as table_name, MythicLevel, Time, Groups, Completed, Raiting FROM playerlist_mots_forti UNION SELECT 'playerlist_mots_tyra' as table_name, MythicLevel, Time, Groups, Completed, Raiting FROM playerlist_mots_tyra) as combined_table ORDER BY Raiting DESC LIMIT 5", connection);
            var records = new List<dynamic>();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    records.Add(new
                    {
                        Dungeon = reader["Dungeon"].ToString(),
                        MythicLevel = reader["MythicLevel"].ToString(),
                        Time = reader["Time"].ToString(),
                        Groups = reader["Groups"].ToString(),
                        Completed = reader["Completed"].ToString(),
                        Raiting = reader["Raiting"].ToString()
                    });
                }
            }
            int maxDungeonLength = records.Max(r => r.Dungeon.Length);
            int maxMythicLevelLength = records.Max(r => r.MythicLevel.Length);
            int maxTimeLength = records.Max(r => r.Time.Length);
            int maxGroupsLength = records.Max(r => r.Groups.Length);
            int maxCompletedLength = records.Max(r => r.Completed.Length);
            int maxRaitingLength = records.Max(r => r.Raiting.Length);

            foreach (var record in records)
            {
                sb.AppendLine($"{record.Dungeon.PadRight(maxDungeonLength)}    {record.MythicLevel.PadRight(maxMythicLevelLength)}    {record.Time.PadRight(maxTimeLength)}    {record.Groups.PadRight(maxGroupsLength)}    {record.Completed.PadRight(maxCompletedLength)}    {record.Raiting.PadRight(maxRaitingLength)}");
            }
            await ReplyAsync($"```{sb.ToString()}```");
            connection.Close();
        }
    }

    [Command("!sd")]
    public async Task sdasync()
    {
        string connectionString = "server=localhost;user=root;database=firestorm_mythicplus;port=3306;password=******";
        StringBuilder sb = new StringBuilder();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT CASE WHEN table_name = 'playerlist_sd_forti' THEN 'SD Fortified' WHEN table_name = 'playerlist_sd_tyra' THEN 'SD Tyrannical' ELSE 'N/A' END as 'Dungeon', MythicLevel, Time, Groups, Completed, Raiting FROM (SELECT 'playerlist_sd_forti' as table_name, MythicLevel, Time, Groups, Completed, Raiting FROM playerlist_sd_forti UNION SELECT 'playerlist_sd_tyra' as table_name, MythicLevel, Time, Groups, Completed, Raiting FROM playerlist_sd_tyra) as combined_table ORDER BY Raiting DESC LIMIT 5", connection);
            var records = new List<dynamic>();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    records.Add(new
                    {
                        Dungeon = reader["Dungeon"].ToString(),
                        MythicLevel = reader["MythicLevel"].ToString(),
                        Time = reader["Time"].ToString(),
                        Groups = reader["Groups"].ToString(),
                        Completed = reader["Completed"].ToString(),
                        Raiting = reader["Raiting"].ToString()
                    });
                }
            }
            int maxDungeonLength = records.Max(r => r.Dungeon.Length);
            int maxMythicLevelLength = records.Max(r => r.MythicLevel.Length);
            int maxTimeLength = records.Max(r => r.Time.Length);
            int maxGroupsLength = records.Max(r => r.Groups.Length);
            int maxCompletedLength = records.Max(r => r.Completed.Length);
            int maxRaitingLength = records.Max(r => r.Raiting.Length);

            foreach (var record in records)
            {
                sb.AppendLine($"{record.Dungeon.PadRight(maxDungeonLength)}    {record.MythicLevel.PadRight(maxMythicLevelLength)}    {record.Time.PadRight(maxTimeLength)}    {record.Groups.PadRight(maxGroupsLength)}    {record.Completed.PadRight(maxCompletedLength)}    {record.Raiting.PadRight(maxRaitingLength)}");
            }
            await ReplyAsync($"```{sb.ToString()}```");
            connection.Close();
        }
    }

    [Command("!sg")]
    public async Task sgasync()
    {
        string connectionString = "server=localhost;user=root;database=firestorm_mythicplus;port=3306;password=******";
        StringBuilder sb = new StringBuilder();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT CASE WHEN table_name = 'playerlist_sg_forti' THEN 'SG Fortified' WHEN table_name = 'playerlist_sg_tyra' THEN 'SG Tyrannical' ELSE 'N/A' END as 'Dungeon', MythicLevel, Time, Groups, Completed, Raiting FROM (SELECT 'playerlist_sg_forti' as table_name, MythicLevel, Time, Groups, Completed, Raiting FROM playerlist_sg_forti UNION SELECT 'playerlist_sg_tyra' as table_name, MythicLevel, Time, Groups, Completed, Raiting FROM playerlist_sg_tyra) as combined_table ORDER BY Raiting DESC LIMIT 5", connection);
            var records = new List<dynamic>();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    records.Add(new
                    {
                        Dungeon = reader["Dungeon"].ToString(),
                        MythicLevel = reader["MythicLevel"].ToString(),
                        Time = reader["Time"].ToString(),
                        Groups = reader["Groups"].ToString(),
                        Completed = reader["Completed"].ToString(),
                        Raiting = reader["Raiting"].ToString()
                    });
                }
            }
            int maxDungeonLength = records.Max(r => r.Dungeon.Length);
            int maxMythicLevelLength = records.Max(r => r.MythicLevel.Length);
            int maxTimeLength = records.Max(r => r.Time.Length);
            int maxGroupsLength = records.Max(r => r.Groups.Length);
            int maxCompletedLength = records.Max(r => r.Completed.Length);
            int maxRaitingLength = records.Max(r => r.Raiting.Length);

            foreach (var record in records)
            {
                sb.AppendLine($"{record.Dungeon.PadRight(maxDungeonLength)}    {record.MythicLevel.PadRight(maxMythicLevelLength)}    {record.Time.PadRight(maxTimeLength)}    {record.Groups.PadRight(maxGroupsLength)}    {record.Completed.PadRight(maxCompletedLength)}    {record.Raiting.PadRight(maxRaitingLength)}");
            }
            await ReplyAsync($"```{sb.ToString()}```");
            connection.Close();
        }
    }


    [Command("!soa")]
    public async Task soaasync()
    {
        string connectionString = "server=localhost;user=root;database=firestorm_mythicplus;port=3306;password=******";
        StringBuilder sb = new StringBuilder();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT CASE WHEN table_name = 'playerlist_soa_forti' THEN 'SOA Fortified' WHEN table_name = 'playerlist_soa_tyra' THEN 'SOA Tyrannical' ELSE 'N/A' END as 'Dungeon', MythicLevel, Time, Groups, Completed, Raiting FROM (SELECT 'playerlist_soa_forti' as table_name, MythicLevel, Time, Groups, Completed, Raiting FROM playerlist_soa_forti UNION SELECT 'playerlist_soa_tyra' as table_name, MythicLevel, Time, Groups, Completed, Raiting FROM playerlist_soa_tyra) as combined_table ORDER BY Raiting DESC LIMIT 5", connection);
            var records = new List<dynamic>();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    records.Add(new
                    {
                        Dungeon = reader["Dungeon"].ToString(),
                        MythicLevel = reader["MythicLevel"].ToString(),
                        Time = reader["Time"].ToString(),
                        Groups = reader["Groups"].ToString(),
                        Completed = reader["Completed"].ToString(),
                        Raiting = reader["Raiting"].ToString()
                    });
                }
            }
            int maxDungeonLength = records.Max(r => r.Dungeon.Length);
            int maxMythicLevelLength = records.Max(r => r.MythicLevel.Length);
            int maxTimeLength = records.Max(r => r.Time.Length);
            int maxGroupsLength = records.Max(r => r.Groups.Length);
            int maxCompletedLength = records.Max(r => r.Completed.Length);
            int maxRaitingLength = records.Max(r => r.Raiting.Length);

            foreach (var record in records)
            {
                sb.AppendLine($"{record.Dungeon.PadRight(maxDungeonLength)}    {record.MythicLevel.PadRight(maxMythicLevelLength)}    {record.Time.PadRight(maxTimeLength)}    {record.Groups.PadRight(maxGroupsLength)}    {record.Completed.PadRight(maxCompletedLength)}    {record.Raiting.PadRight(maxRaitingLength)}");
            }
            await ReplyAsync($"```{sb.ToString()}```");
            connection.Close();
        }
    }



    [Command("!sow")]
    public async Task sowasync()
    {
        string connectionString = "server=localhost;user=root;database=firestorm_mythicplus;port=3306;password=******";
        StringBuilder sb = new StringBuilder();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT CASE WHEN table_name = 'playerlist_sow_forti' THEN 'SOW Fortified' WHEN table_name = 'playerlist_sow_tyra' THEN 'SOW Tyrannical' ELSE 'N/A' END as 'Dungeon', MythicLevel, Time, Groups, Completed, Raiting FROM (SELECT 'playerlist_sow_forti' as table_name, MythicLevel, Time, Groups, Completed, Raiting FROM playerlist_sow_forti UNION SELECT 'playerlist_sow_tyra' as table_name, MythicLevel, Time, Groups, Completed, Raiting FROM playerlist_sow_tyra) as combined_table ORDER BY Raiting DESC LIMIT 5", connection);
            var records = new List<dynamic>();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    records.Add(new
                    {
                        Dungeon = reader["Dungeon"].ToString(),
                        MythicLevel = reader["MythicLevel"].ToString(),
                        Time = reader["Time"].ToString(),
                        Groups = reader["Groups"].ToString(),
                        Completed = reader["Completed"].ToString(),
                        Raiting = reader["Raiting"].ToString()
                    });
                }
            }
            int maxDungeonLength = records.Max(r => r.Dungeon.Length);
            int maxMythicLevelLength = records.Max(r => r.MythicLevel.Length);
            int maxTimeLength = records.Max(r => r.Time.Length);
            int maxGroupsLength = records.Max(r => r.Groups.Length);
            int maxCompletedLength = records.Max(r => r.Completed.Length);
            int maxRaitingLength = records.Max(r => r.Raiting.Length);

            foreach (var record in records)
            {
                sb.AppendLine($"{record.Dungeon.PadRight(maxDungeonLength)}    {record.MythicLevel.PadRight(maxMythicLevelLength)}    {record.Time.PadRight(maxTimeLength)}    {record.Groups.PadRight(maxGroupsLength)}    {record.Completed.PadRight(maxCompletedLength)}    {record.Raiting.PadRight(maxRaitingLength)}");
            }
            await ReplyAsync($"```{sb.ToString()}```");
            connection.Close();
        }
    }

    [Command("!top")]
    public async Task topasync()
    {
        string connectionString = "server=localhost;user=root;database=firestorm_mythicplus;port=3306;password=******";
        StringBuilder sb = new StringBuilder();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT CASE WHEN table_name = 'playerlist_top_forti' THEN 'TOP Fortified' WHEN table_name = 'playerlist_top_tyra' THEN 'TOP Tyrannical' ELSE 'N/A' END as 'Dungeon', MythicLevel, Time, Groups, Completed, Raiting FROM (SELECT 'playerlist_top_forti' as table_name, MythicLevel, Time, Groups, Completed, Raiting FROM playerlist_top_forti UNION SELECT 'playerlist_top_tyra' as table_name, MythicLevel, Time, Groups, Completed, Raiting FROM playerlist_top_tyra) as combined_table ORDER BY Raiting DESC LIMIT 5", connection);
            var records = new List<dynamic>();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    records.Add(new
                    {
                        Dungeon = reader["Dungeon"].ToString(),
                        MythicLevel = reader["MythicLevel"].ToString(),
                        Time = reader["Time"].ToString(),
                        Groups = reader["Groups"].ToString(),
                        Completed = reader["Completed"].ToString(),
                        Raiting = reader["Raiting"].ToString()
                    });
                }
            }
            int maxDungeonLength = records.Max(r => r.Dungeon.Length);
            int maxMythicLevelLength = records.Max(r => r.MythicLevel.Length);
            int maxTimeLength = records.Max(r => r.Time.Length);
            int maxGroupsLength = records.Max(r => r.Groups.Length);
            int maxCompletedLength = records.Max(r => r.Completed.Length);
            int maxRaitingLength = records.Max(r => r.Raiting.Length);

            foreach (var record in records)
            {
                sb.AppendLine($"{record.Dungeon.PadRight(maxDungeonLength)}    {record.MythicLevel.PadRight(maxMythicLevelLength)}    {record.Time.PadRight(maxTimeLength)}    {record.Groups.PadRight(maxGroupsLength)}    {record.Completed.PadRight(maxCompletedLength)}    {record.Raiting.PadRight(maxRaitingLength)}");
            }
            await ReplyAsync($"```{sb.ToString()}```");
            connection.Close();
        }
    }


    [Command("!nw")]
    public async Task nwasync()
    {
        string connectionString = "server=localhost;user=root;database=firestorm_mythicplus;port=3306;password=******";
        StringBuilder sb = new StringBuilder();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT CASE WHEN table_name = 'playerlist_nw_forti' THEN 'NW Fortified' WHEN table_name = 'playerlist_nw_tyra' THEN 'NW Tyrannical' ELSE 'N/A' END as 'Dungeon', MythicLevel, Time, Groups, Completed, Raiting FROM (SELECT 'playerlist_nw_forti' as table_name, MythicLevel, Time, Groups, Completed, Raiting FROM playerlist_nw_forti UNION SELECT 'playerlist_nw_tyra' as table_name, MythicLevel, Time, Groups, Completed, Raiting FROM playerlist_nw_tyra) as combined_table ORDER BY Raiting DESC LIMIT 5", connection);
            var records = new List<dynamic>();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    records.Add(new
                    {
                        Dungeon = reader["Dungeon"].ToString(),
                        MythicLevel = reader["MythicLevel"].ToString(),
                        Time = reader["Time"].ToString(),
                        Groups = reader["Groups"].ToString(),
                        Completed = reader["Completed"].ToString(),
                        Raiting = reader["Raiting"].ToString()
                    });
                }
            }
            int maxDungeonLength = records.Max(r => r.Dungeon.Length);
            int maxMythicLevelLength = records.Max(r => r.MythicLevel.Length);
            int maxTimeLength = records.Max(r => r.Time.Length);
            int maxGroupsLength = records.Max(r => r.Groups.Length);
            int maxCompletedLength = records.Max(r => r.Completed.Length);
            int maxRaitingLength = records.Max(r => r.Raiting.Length);

            foreach (var record in records)
            {
                sb.AppendLine($"{record.Dungeon.PadRight(maxDungeonLength)}    {record.MythicLevel.PadRight(maxMythicLevelLength)}    {record.Time.PadRight(maxTimeLength)}    {record.Groups.PadRight(maxGroupsLength)}    {record.Completed.PadRight(maxCompletedLength)}    {record.Raiting.PadRight(maxRaitingLength)}");
            }
            await ReplyAsync($"```{sb.ToString()}```");
            connection.Close();
        }
    }

   

    private string GetMythicLevelFromDatabase(string tableName, string playerName)
    {
        string connectionString = "server=localhost;user=root;database=firestorm_mythicplus;port=3306;password=******";
        string mythicLevel = "";
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            using (MySqlCommand command = new MySqlCommand($"SELECT MythicLevel FROM {tableName} WHERE PlayerName = @PlayerName", connection))
            {
                command.Parameters.AddWithValue("@PlayerName", playerName);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read() && !reader.IsDBNull(reader.GetOrdinal("MythicLevel")) && reader.GetString(reader.GetOrdinal("MythicLevel")).Trim() != "")
                    {
                        mythicLevel = reader.GetString("MythicLevel");
                    }
                    else
                    {
                        mythicLevel = "N/A";
                    }
                }
            }
        }
        return mythicLevel;
    }

    private double GetPlayerRatingFromDatabase(string playerName)
    {
        string connectionString = "server=localhost;user=root;database=firestorm_mythicplus;port=3306;password=******";

        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        MySqlCommand cmd2 = new MySqlCommand("SELECT ROUND(PlayerRaiting, 2) AS PlayerRaiting FROM raiting_total WHERE PlayerName = '" + playerName + "'", connection);

        object ratingResult = cmd2.ExecuteScalar();
        if (ratingResult == null)
        {
            return 0.0;
        }

        return (double)ratingResult;
        connection.Close();
    }



    [Command("!thepleb")]
    public async Task BotCommand([Remainder] string message)
    {
        try
        {
            var response = await GenerateResponse(message);
            await Context.Channel.SendMessageAsync(response);
        }
        catch (HttpRequestException ex)
        {
            await Context.Channel.SendMessageAsync("An error occurred while sending the request to Thepleb: " + ex.Message);
        }
    }

    public static async Task<string> GenerateResponse(string message)
    {
        try
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer *************************");
            client.DefaultRequestHeaders.Add("OpenAI-Organization-Id", "********************************");
            var data = new { prompt = message };
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            Console.WriteLine("Content: " + content.ReadAsStringAsync().Result);
            content.Headers.ContentEncoding.Add("utf-8");

            Console.WriteLine("Content: " + content);
            var response = await client.PostAsync("https://api.openai.com/v1/engines/davinci/completions?model=text-moderation-playground", content);
            if (response.IsSuccessStatusCode)
            {
                // request was successful, continue processing the response
                var responseText = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(responseText);
                return result.choices[0].text;
            }
            else
            {
                // request was not successful, log or handle the error
                Console.WriteLine("Error: " + response.StatusCode);
                return null;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
            Console.WriteLine(e.StackTrace);
            return null;
        }



    }





    [Command("!best")]
    public async Task bestasync()
    {
       
            string connectionString = "server=localhost;user=root;database=firestorm_mythicplus;port=3306;password=******";
            StringBuilder sb = new StringBuilder();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT PlayerName,PlayerRaiting FROM raiting_total ORDER BY PlayerRaiting DESC LIMIT 5", connection);
                var records = new List<dynamic>();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        records.Add(new
                        {
                            PlayerName = reader["PlayerName"].ToString(),
                            PlayerRaiting = reader["PlayerRaiting"].ToString(),
                        });
                    }
                }
                int maxPlayerNameLength = records.Max(r => r.PlayerName.Length);
                int maxPlayerRaitingLength = records.Max(r => r.PlayerRaiting.Length);

                int counter = 1;
                foreach (var record in records)
                {
                    sb.AppendLine($"{counter}.    {record.PlayerName.PadRight(maxPlayerNameLength)}    {record.PlayerRaiting.PadRight(maxPlayerRaitingLength)}");
                    counter++;
                }
                await ReplyAsync($"```{sb.ToString()}```");
                connection.Close();
            }
        
    }


    [Command("!best")]
    public async Task bestpriestasync(string PlayerClass)
    {

      
        string connectionString = "server=localhost;user=root;database=firestorm_mythicplus;port=3306;password=******";
        StringBuilder sb = new StringBuilder();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT PlayerName,PlayerRaiting FROM raiting_total WHERE PlayerClass = 'color_"+PlayerClass+"' ORDER BY PlayerRaiting DESC LIMIT 5", connection);
            var records = new List<dynamic>();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    records.Add(new
                    {
                        PlayerName = reader["PlayerName"].ToString(),
                        PlayerRaiting = reader["PlayerRaiting"].ToString(),
                    });
                }
            }
            int maxPlayerNameLength = records.Max(r => r.PlayerName.Length);
            int maxPlayerRaitingLength = records.Max(r => r.PlayerRaiting.Length);

            int counter = 1;
            foreach (var record in records)
            {
                sb.AppendLine($"{counter}.    {record.PlayerName.PadRight(maxPlayerNameLength)}    {record.PlayerRaiting.PadRight(maxPlayerRaitingLength)}");
                counter++;
            }
            await ReplyAsync($"```{sb.ToString()}```");
            connection.Close();
        }

    }

    [Command("!stats")]
    public async Task statsasync()
    {

        string connectionString = "server=localhost;user=root;database=firestorm_mythicplus;port=3306;password=******";

        string[] prefixes = new string[] { "Deathknight:", "Demonhunter:", "Druid:", "Hunter:", "Mage:", "Monk:", "Priest:", "Paladin:", "Rogue:", "Shaman:", "Warrior:", "Warlock:", "Total players: " };
        int maxLength = prefixes.Max(s => s.Length);

        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        MySqlCommand cmd2 = new MySqlCommand("SELECT COUNT(*) FROM raiting_total WHERE PlayerClass = 'color_deathknight'", connection);
        object dk = cmd2.ExecuteScalar();
        string deathknight = prefixes[0].PadRight(maxLength) + dk.ToString();

        MySqlCommand cmd3 = new MySqlCommand("SELECT COUNT(*) FROM raiting_total WHERE PlayerClass = 'color_demonhunter'", connection);
        object dh = cmd3.ExecuteScalar();
        string demonhunter = prefixes[1].PadRight(maxLength) + dh.ToString();

        MySqlCommand cmd4 = new MySqlCommand("SELECT COUNT(*) FROM raiting_total WHERE PlayerClass = 'color_druid'", connection);
        object dr = cmd4.ExecuteScalar();
        string druid = prefixes[2].PadRight(maxLength) + dr.ToString();

        MySqlCommand cmd5 = new MySqlCommand("SELECT COUNT(*) FROM raiting_total WHERE PlayerClass = 'color_hunter'", connection);
        object hn = cmd5.ExecuteScalar();
        string hunter = prefixes[3].PadRight(maxLength) + hn.ToString();

        MySqlCommand cmd6 = new MySqlCommand("SELECT COUNT(*) FROM raiting_total WHERE PlayerClass = 'color_mage'", connection);
        object mg = cmd6.ExecuteScalar();
        string mage = prefixes[4].PadRight(maxLength) + mg.ToString();

        MySqlCommand cmd7 = new MySqlCommand("SELECT COUNT(*) FROM raiting_total WHERE PlayerClass = 'color_monk'", connection);
        object mk = cmd7.ExecuteScalar();
        string monk = prefixes[5].PadRight(maxLength) + mk.ToString();

        MySqlCommand cmd8 = new MySqlCommand("SELECT COUNT(*) FROM raiting_total WHERE PlayerClass = 'color_priest'", connection);
        object pr = cmd8.ExecuteScalar();
        string priest = prefixes[6].PadRight(maxLength) + pr.ToString();

        MySqlCommand cmd9 = new MySqlCommand("SELECT COUNT(*) FROM raiting_total WHERE PlayerClass = 'color_paladin'", connection);
        object pa = cmd9.ExecuteScalar();
        string paladin = prefixes[7].PadRight(maxLength) + pa.ToString();

        MySqlCommand cmd10 = new MySqlCommand("SELECT COUNT(*) FROM raiting_total WHERE PlayerClass = 'color_rogue'", connection);
        object ro = cmd10.ExecuteScalar();
        string rogue = prefixes[8].PadRight(maxLength) + ro.ToString();

        MySqlCommand cmd11 = new MySqlCommand("SELECT COUNT(*) FROM raiting_total WHERE PlayerClass = 'color_shaman'", connection);
        object sh = cmd11.ExecuteScalar();
        string shaman = prefixes[9].PadRight(maxLength) + sh.ToString();

        MySqlCommand cmd12 = new MySqlCommand("SELECT COUNT(*) FROM raiting_total WHERE PlayerClass = 'color_warrior'", connection);
        object wr = cmd12.ExecuteScalar();
        string warrior = prefixes[10].PadRight(maxLength) + wr.ToString();

        MySqlCommand cmd13 = new MySqlCommand("SELECT COUNT(*) FROM raiting_total WHERE PlayerClass = 'color_warlock'", connection);
        object wl = cmd13.ExecuteScalar();
        string warlock = prefixes[11].PadRight(maxLength) + wl.ToString();

        MySqlCommand cmd14 = new MySqlCommand("SELECT COUNT(*) FROM raiting_total", connection);
        object to = cmd14.ExecuteScalar();
        string total = prefixes[12].PadRight(maxLength) + to.ToString();

        await ReplyAsync($"```{total}\n\n{deathknight}\n{demonhunter}\n{druid}\n{hunter}\n{mage}\n{monk}\n{priest}\n{paladin}\n{rogue}\n{shaman}\n{warrior}\n{warlock}```");

        connection.Close();


    }

  

    public class MyBot
    {
        private DiscordSocketClient _client;
        private CommandService _commands;

        public async Task StartAsync()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages | GatewayIntents.MessageContent
            });
            _commands = new CommandService();

            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;
            _client.MessageReceived += MessageReceivedAsync;

            string token = "TOKEN_HERE";

            await _client.LoginAsync(TokenType.Bot, token);
            await _commands.AddModulesAsync(typeof(FioModule).Assembly, null);
            await _client.StartAsync();
        }

        public async Task StopAsync()
        {
            await _client.StopAsync();
            await _client.LogoutAsync();
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private Task ReadyAsync()
        {
            Console.WriteLine($"{_client.CurrentUser} is connected!");

            foreach (var guild in _client.Guilds)
            {
                Console.WriteLine(guild.Name);
            }

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(SocketMessage message)
        {
            Console.WriteLine("MessageReceivedAsync called");

            if (message.Author.Id == _client.CurrentUser.Id)
                return;

            SocketUserMessage msg = message as SocketUserMessage;
            if (msg == null)
            {
                Console.WriteLine("SocketUserMessage is null");
                return;
            }

            int argPos = 0;

            SocketCommandContext context = new SocketCommandContext(_client, msg);

            await _commands.ExecuteAsync(context, argPos, null);
        }
    }

    static void Main()
    {
        HostFactory.Run(x =>
        {
            x.Service<MyBot>(s =>
            {
                s.ConstructUsing(name => new MyBot());
                s.WhenStarted(tc => tc.StartAsync());
                s.WhenStopped(tc => tc.StopAsync());
            });
            x.RunAsLocalSystem();

            x.SetDescription("Firestorm Discord Bot");
            x.SetDisplayName("FirestormRatingBot");
            x.SetServiceName("FirestormRatingBot");
        });
    }
}