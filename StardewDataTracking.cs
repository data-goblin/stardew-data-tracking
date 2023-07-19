using System;
using System.IO;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Monsters;

namespace StardewDataTracking
{
    public class StardewDataTracking : Mod
    {
        private string StatsFilePath;
        private const string Delimiter = ",";

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.TimeChanged += OnTimeChanged;
        }

        // Writes the statistics to the CSV when the game first loads.
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            WriteStatisticsToCsv();
        }

        private void OnTimeChanged(object sender, TimeChangedEventArgs e)
        {
            // make sure we're in the game world
            if (!Context.IsWorldReady)
                return;

            // every in-game hour add a new line in the CSV with the stats
            if (e.NewTime % 100 == 0)
            {
                WriteStatisticsToCsv();
            }

        }

        // Write the statistics to the CSV as a new line
        private void WriteStatisticsToCsv()
        {
            StatsFilePath = Path.Combine(Constants.CurrentSavePath, $"{Constants.SaveFolderName}-StardewDataTracking.csv");

            // Headers for the CSV
            string[] headers =
            {
                "PlayerName",
                "FarmName",
                "FarmType",
                "RealDate",
                "GameDay",
                "GameTimeOfDay",
                "TotalMoneyEarned"
                };

            // Check if the file does not exist, create the file and write the header
            if (!File.Exists(StatsFilePath))
            {
                File.WriteAllText(StatsFilePath, string.Join(Delimiter, headers) + Environment.NewLine);
            }

            // Create a string array for the current statistics
            string[] statistics =
            {
                Game1.player.Name, // Player name
                Game1.player.farmName.Value, // Farm name
                Game1.whichFarm.ToString(), // Farm type
                DateTime.Now.ToString(), // Real DateTime
                Game1.Date.TotalDays.ToString(), // Days elapsed
                Game1.timeOfDay.ToString(), // Current time of day
                Game1.player.totalMoneyEarned.ToString() // Total money earned
            };

            // Join the statistics array into a string with the delimiter and append it to the file
            File.AppendAllText(StatsFilePath, string.Join(Delimiter, statistics) + Environment.NewLine);
        }
    }
}
