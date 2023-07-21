using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.Menus;
using StardewDataTracking.Framework;

namespace StardewDataTracking
{
    public class StardewDataTracking : Mod
    {
        private SDate CurrentDate;
        private string FormattedDate;

        private TxnWriter txnWriter;
        private string TxnFilePath;

        private IntradayStatsWriter intradaystatsWriter;
        private InterdayStatsWriter interdaystatsWriter;
        private string StatsFilePath;

        private string DataPath = Constants.DataPath;

        private int PreviousMoney;
        private int MoneyChange;

        private string TransactionType;
        private const string Delimiter = ",";

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.GameLoop.TimeChanged += OnTimeChanged;
            helper.Events.Display.MenuChanged += this.OnMenuChanged;
            helper.Events.Player.InventoryChanged += OnInventoryChanged;
            helper.Events.World.ChestInventoryChanged += OnChestInventoryChanged;
        }

        // Initializes the CSV files when the game first loads.
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            // Enter the in-game date
            CurrentDate = SDate.Now();
            FormattedDate = $"{CurrentDate.Year}-{CurrentDate.Season}-{CurrentDate.Day}";

            // Create the "Transactions" csv file in the save file directory when the game loads
            TxnFilePath = Path.Combine(Constants.CurrentSavePath, $"{Constants.SaveFolderName}-Transactions-StardewDataTracking.csv");
            txnWriter = new TxnWriter(TxnFilePath);
            txnWriter.CreateTxnCSV(DataPath, FormattedDate);

            // Create the "Stats" csv file in the save file directory when the game loads
            StatsFilePath = Path.Combine(Constants.CurrentSavePath, $"{Constants.SaveFolderName}-IntradayStats-StardewDataTracking.csv");
            intradaystatsWriter = new IntradayStatsWriter(StatsFilePath);
            intradaystatsWriter.CreateIntradayStatsCSV(DataPath, FormattedDate);

            // Create the "Stats" csv file in the save file directory when the game loads
            StatsFilePath = Path.Combine(Constants.CurrentSavePath, $"{Constants.SaveFolderName}-InterdayStats-StardewDataTracking.csv");
            interdaystatsWriter = new InterdayStatsWriter(StatsFilePath);
            interdaystatsWriter.CreateInterdayStatsCSV(DataPath, FormattedDate);

            PreviousMoney = Game1.player.Money;
        }

        // Update the in-game date
        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            CurrentDate = SDate.Now();
            FormattedDate = $"{CurrentDate.Year}-{CurrentDate.Season}-{CurrentDate.Day}";

            interdaystatsWriter.WriteInterdayStatsToCSV(DataPath, FormattedDate);
        }

        // To update the "Stats" file
        private void OnTimeChanged(object sender, TimeChangedEventArgs e)
        {
            // make sure we're in the game world
            if (!Context.IsWorldReady)
                return;

            // every in-game 10 min add a new line in the CSV with the stats
            if (e.NewTime % 10 == 0)
            {
                intradaystatsWriter.WriteIntradayStatsToCSV(DataPath, FormattedDate);
                Monitor.Log($"Added intraday statistics for {FormattedDate} {Game1.timeOfDay}", LogLevel.Debug);
            }
        }

        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            // when the player opens a quest log or mailbox menu...
            if (e.NewMenu is QuestLog)
            {
                // store the current money amount
                this.PreviousMoney = Game1.player.Money;
            }
            // when the player closes a quest log or mailbox menu...
            else if (e.OldMenu is QuestLog)
            {
                // calculate the money difference
                int moneyDifference = Game1.player.Money - this.PreviousMoney;
                if (moneyDifference > 0)
                {
                    txnWriter.WriteTxnToCSV(DataPath, FormattedDate, "Completed Quest", "player", 0, 0, moneyDifference, "Completed Quest");
                }
            }

            // when the player opens a quest log or mailbox menu...
            if (e.NewMenu is LetterViewerMenu)
            {
                // store the current money amount
                this.PreviousMoney = Game1.player.Money;
            }
            // when the player closes a quest log or mailbox menu...
            else if (e.OldMenu is LetterViewerMenu)
            {
                // calculate the money difference
                int moneyDifference = Game1.player.Money - this.PreviousMoney;
                if (moneyDifference > 0)
                {
                    txnWriter.WriteTxnToCSV(DataPath, FormattedDate, "Mail", "player", 0, 0, moneyDifference, "Mail");
                }
            }
        }

        // Handle the event that the inventory changes
        private void OnInventoryChanged(object sender, InventoryChangedEventArgs e)
        {
            // Get any possible change in money
            MoneyChange = Game1.player.Money - PreviousMoney;

            // get the tile position of the shipping bin
            Vector2 shippingBinTile = new Vector2(71, 14); // This position is for the standard farm, you might need to adjust for other farm types.

            // calculate the distance between the player and the shipping bin
            float distance = Vector2.Distance(Game1.player.getTileLocation(), shippingBinTile);

            TransactionType = $"{(MoneyChange > 0 ? "Sold" : (MoneyChange < 0 ? "Bought" : null))}";

            // When a new stack is added to the inventory
            foreach (var addedItem in e.Added)
            {
                int quality = 0;
                if (addedItem is StardewValley.Object obj)
                {
                    quality = obj.Quality;
                }
                if (addedItem.Stack != 0)
                {
                    // Writes the inventory addition to the stats CSV
                    txnWriter.WriteTxnToCSV(DataPath, FormattedDate, addedItem.DisplayName, "player", addedItem.Stack, quality, MoneyChange, TransactionType);

                    // Outputs to the SMAPI log the item added, quantity and quality
                    Monitor.Log($"Player added a stack of {addedItem.DisplayName}, Quantity: {addedItem.Stack}, Quality: {quality}", LogLevel.Debug);
                }
            }

            // When a new stack is removed from the inventory
            foreach (var removedItem in e.Removed)
            {
                int quality = 0;
                if (removedItem is StardewValley.Object obj)
                {
                    quality = obj.Quality;
                }
                if (removedItem.Stack != 0)
                {
                    TransactionType = $"{(distance <= 2 ? "Outbound Shipping" : TransactionType)}";

                    // Writes the inventory removal to the stats CSV
                    txnWriter.WriteTxnToCSV(DataPath, FormattedDate, removedItem.DisplayName, "player", -removedItem.Stack, quality, MoneyChange, TransactionType);

                    // Outputs to the SMAPI log the item removed, quantity and quality
                    Monitor.Log($"Player removed a stack of {removedItem.DisplayName} ({TransactionType}), Quantity: {-removedItem.Stack}, Quality: {quality}", LogLevel.Debug);
                }
            }

            // When a the quantity of a stack in the inventory changes
            foreach (var quantityChange in e.QuantityChanged)
            {
                int quality = 0;
                if (quantityChange.Item is StardewValley.Object obj)
                {
                    quality = obj.Quality;
                }
                // Determine whether the quantity increased or decreased
                string changeType = quantityChange.NewSize > quantityChange.OldSize ? "added" : "removed";

                // How much was added or removed
                int quantityDifference = quantityChange.NewSize - quantityChange.OldSize;

                // Only add a transaction when there was a real "change"
                if (quantityDifference != 0)
                {
                    // Writes the inventory removal to the stats CSV
                    txnWriter.WriteTxnToCSV(DataPath, FormattedDate, quantityChange.Item.DisplayName, "player", quantityDifference, quality, MoneyChange, TransactionType);

                    // Outputs to the SMAPI log the item changed, quantity and quality
                    Monitor.Log($"Player {changeType} item: {quantityChange.Item.DisplayName}, Quantity: {quantityDifference}, Quality: {quality}", LogLevel.Debug);
                }
            }
            // Update PreviousMoney to player's current wallet
            PreviousMoney = Game1.player.Money;
        }

        private void OnChestInventoryChanged(object sender, ChestInventoryChangedEventArgs e)
        {
            string ChestID;
            ChestID = $"chest-{e.Location.Name}-{e.Chest.TileLocation.X}-{e.Chest.TileLocation.Y}";

            // When a new stack is added to the inventory
            foreach (var addedItem in e.Added)
            {
                int quality = 0;
                if (addedItem is StardewValley.Object obj)
                {
                    quality = obj.Quality;
                }
                // Writes the inventory addition to the stats CSV
                txnWriter.WriteTxnToCSV(DataPath, FormattedDate, addedItem.DisplayName, ChestID, addedItem.Stack, quality, 0, "Deposit");

                // Outputs to the SMAPI log the item added, quantity and quality
                Monitor.Log($"Player added a stack of: {addedItem.DisplayName} to {ChestID}. Quantity: {addedItem.Stack}, Quality: {quality}", LogLevel.Debug);
            }

            // When a new stack is removed from the inventory
            foreach (var removedItem in e.Removed)
            {
                int quality = 0;
                if (removedItem is StardewValley.Object obj)
                {
                    quality = obj.Quality;
                }
                // Writes the inventory removal to the stats CSV
                txnWriter.WriteTxnToCSV(DataPath, FormattedDate, removedItem.DisplayName, ChestID, -removedItem.Stack, quality, 0, "Withdrawal");

                // Outputs to the SMAPI log the item removed, quantity and quality
                Monitor.Log($"Player removed a stack of: {removedItem.DisplayName} to {ChestID}. Quantity: {-removedItem.Stack}, Quality: {quality}", LogLevel.Debug);
            }

            foreach (var quantityChange in e.QuantityChanged)
            {
                int quality = 0;
                if (quantityChange.Item is StardewValley.Object obj)
                {
                    quality = obj.Quality;
                }

                // Compute the difference between the new size and old size
                int quantityDifference = quantityChange.NewSize - quantityChange.OldSize;

                // Writes the inventory change to the stats CSV.
                txnWriter.WriteTxnToCSV(DataPath, FormattedDate, quantityChange.Item.DisplayName, ChestID, quantityDifference, quality, 0, $"{(quantityDifference > 0 ? "Deposit" : "Withdrawal")}");

                // Outputs to the SMAPI log the item changed, quantity and quality.
                Monitor.Log($"Player {(quantityDifference > 0 ? "Added" : "Removed")} {quantityChange.Item.DisplayName} to {ChestID}. Quantity: {quantityDifference}, Quality: {quality}", LogLevel.Debug);
            }
        }
    }
}