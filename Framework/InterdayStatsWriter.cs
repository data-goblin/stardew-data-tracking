using System;
using System.IO;
using StardewValley;

namespace StardewDataTracking.Framework
{
    public class InterdayStatsWriter
    {
        private const string Delimiter = ",";
        private string OutputPath;

        private uint Shipped;
        private uint PreviousShipped;

        private uint GoodFriends;
        private uint PreviousGoodFriends;

        private string FarmingProfession;
        private string MiningProfession;
        private string ForagingProfession;
        private string FishingProfession;
        private string CombatProfession;

        public InterdayStatsWriter(string statsFilePath)
        {
            OutputPath = statsFilePath;
        }

        // Initially create the CSV and add headers
        public void CreateInterdayStatsCSV(string dataPath, string gameDate)
        {
            // Headers for the CSV
            string[] headers =
            {
                "GameDirectory",
                "PlayerName",
                "FarmName",
                "FarmType",
                "RealDate",
                "RealTime",
                "GameDate",
                "InventorySize",
                "TimeWentToBed",
                "ItemsShipped",
                "TimesUnconscious",
                "DeepestMineLevel",
                "FarmingProfession",
                "MiningProfession",
                "ForagingProfession",
                "FishingProfession",
                "CombatProfession",
                "GoodFriends"
                };

            // Check if the file does not exist, create the file and write the header
            if (!File.Exists(OutputPath))
            {
                // Player Misc Stats
                PreviousShipped = Game1.player.stats.itemsShipped;
                PreviousGoodFriends = Game1.player.stats.goodFriends;

                // Write the headers and initial statistics
                File.WriteAllText(OutputPath, string.Join(Delimiter, headers) + Environment.NewLine);
            }
        }

        // Method to write the stats to the CSV file
        public void WriteInterdayStatsToCSV(string dataPath, string gameDate)
        {
            Shipped = PreviousShipped - Game1.player.stats.itemsShipped;
            GoodFriends = PreviousGoodFriends - Game1.player.stats.goodFriends;

            if (Game1.player.professions.Count > 0)
            {
                FarmingProfession = Game1.player.professions[0].ToString();
            }
            if (Game1.player.professions.Count > 1)
            {
                MiningProfession = Game1.player.professions[1].ToString();
            }
            if (Game1.player.professions.Count > 2)
            {
                ForagingProfession = Game1.player.professions[2].ToString();
            }
            if (Game1.player.professions.Count > 3)
            {
                FishingProfession = Game1.player.professions[3].ToString();
            }
            if (Game1.player.professions.Count > 4)
            {
                CombatProfession = Game1.player.professions[4].ToString();
            }

            string[] Stats =
            {
                dataPath,                                         // Path to the Data folder (to uniquely identify the player across saves)
                Game1.player.Name,                                // Player name
                Game1.player.farmName.Value,                      // Farm name
                Game1.whichFarm.ToString(),                       // Farm type
                DateTime.Now.ToString("yyyy-MM-dd"),              // Real Date
                DateTime.Now.ToString("HH:mm:ss"),                // Real Time
                gameDate,                                         // Game Date
                Game1.player.maxItems.ToString(),                 // Inventory Size
                Shipped.ToString(),                               // Items shipped
                Game1.player.stats.TimesUnconscious.ToString(),   // Times unconscious
                Game1.player.deepestMineLevel.ToString(),         // Deepest level in mines
                FarmingProfession,                                // Farming profession
                MiningProfession,                                 // Mining profession
                ForagingProfession,                               // Foraging profession
                FishingProfession,                                // Fishing profession
                CombatProfession,                                 // Combat profession
                GoodFriends.ToString()                            // Good Friends
            };

            PreviousShipped = Game1.player.stats.itemsShipped;

            // Append the change data to the file
            File.AppendAllText(OutputPath, string.Join(Delimiter, Stats) + Environment.NewLine);
        }

        public string FormatGameTime(int time)
        {
            return $"{time / 100:D2}:{time % 100:D2}";
        }
    }
}