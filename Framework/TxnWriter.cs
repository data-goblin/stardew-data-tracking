using System;
using System.IO;
using StardewValley;

namespace StardewDataTracking.Framework
{
    public class TxnWriter
    {
        private const string Delimiter = ",";
        private string OutputPath;

        public TxnWriter(string statsFilePath)
        {
            OutputPath = statsFilePath;
        }

        // Initially create the CSV and add headers
        public void CreateTxnCSV(string dataPath, string gameDate)
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
                "GameTime",
                "MoneyInWallet",
                "MoneyInWalletChange",
                "TotalMoneyEarned",
                "ItemName",
                "InventoryType",
                "ItemTxnQuantity",
                "ItemQuality",
                "TransactionType"
                };

            // Check if the file does not exist, create the file and write the header
            if (!File.Exists(OutputPath))
            {
                // Initial statistics for the first time starting the game with the mod
                string[] InitStats =
                {
                    dataPath, // Path to the Data folder (to uniquely identify the player across saves)
                    Game1.player.Name, // Player name
                    Game1.player.farmName.Value, // Farm name
                    Game1.whichFarm.ToString(), // Farm type
                    DateTime.Now.ToString("yyyy-MM-dd"), // Real Date
                    DateTime.Now.ToString("HH:mm:ss"), // Real Time
                    gameDate, // Game Date
                    FormatGameTime(Game1.timeOfDay), // Current time of day
                    Game1.player.Money.ToString(), // Current money in wallet
                    "0", // Change in money
                    Game1.player.totalMoneyEarned.ToString(), // Total money earned
                    "GameStart", // Name of the item added/removed
                    "player", // Player or chest inventory
                    "0", // Quantity of the item added/removed
                    "0", // Quality of the item added/removed
                    "Initial Load" // The type of transaction
                };

                // Write the headers and initial statistics
                File.WriteAllText(OutputPath, string.Join(Delimiter, headers) + Environment.NewLine + string.Join(Delimiter, InitStats) + Environment.NewLine);
            }
        }

        // Method to write the stats to the CSV file
        public void WriteTxnToCSV(string dataPath, string gameDate, string itemName, string inventoryType, int quantity, int quality, int money, string transactionType)
        {
            string[] Stats =
            {
                dataPath, // Path to the Data folder (to uniquely identify the player across saves)
                Game1.player.Name, // Player name
                Game1.player.farmName.Value, // Farm name
                Game1.whichFarm.ToString(), // Farm type
                DateTime.Now.ToString("yyyy-MM-dd"), // Real Date
                DateTime.Now.ToString("HH:mm:ss"), // Real Time
                gameDate, // Game Date
                FormatGameTime(Game1.timeOfDay), // Game Time
                Game1.player.Money.ToString(), // Current money in wallet
                money.ToString(), // Change in money
                Game1.player.totalMoneyEarned.ToString(), // Total money earned
                itemName, // Name of the item added/removed
                inventoryType, // Player or chest inventory
                quantity.ToString(), // Quantity of the item added/removed
                quality.ToString(), // Quality of the item added/removed
                transactionType // The type of transaction (bought, sold, gifted, used...)
            };

            // Append the change data to the file
            File.AppendAllText(OutputPath, string.Join(Delimiter, Stats) + Environment.NewLine);
        }

        public string FormatGameTime(int time)
        {
            return $"{time / 100:D2}:{time % 100:D2}";
        }
    }
}