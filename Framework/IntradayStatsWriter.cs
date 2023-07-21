using System;
using System.IO;
using StardewValley;

namespace StardewDataTracking.Framework
{
    public class IntradayStatsWriter
    {
        private const string Delimiter = ",";
        private string OutputPath;

        private uint Steps;
        private uint PreviousSteps;

        private uint Monsters;
        private uint PreviousMonsters;

        private uint Money;
        private uint PreviousMoney;

        private uint Seeds;
        private uint PreviousSeeds;

        private uint Gifts;
        private uint PreviousGifts;

        private uint Crafted;
        private uint PreviousCrafted;

        private uint Shipped;
        private uint PreviousShipped;

        private uint Forage;
        private uint PreviousForage;

        private uint Rocks;
        private uint PreviousRocks;

        private uint Dirt;
        private uint PreviousDirt;

        private uint Fish;
        private uint PreviousFish;

        private uint Fished;
        private uint PreviousFished;

        private uint Stone;
        private uint PreviousStone;

        private uint Coal;
        private uint PreviousCoal;

        private uint Copper;
        private uint PreviousCopper;

        private uint Iron;
        private uint PreviousIron;

        private uint Gold;
        private uint PreviousGold;

        private uint Iridium;
        private uint PreviousIridium;

        private uint Prismatic;
        private uint PreviousPrismatic;

        private uint Diamond;
        private uint PreviousDiamond;

        private uint Coins;
        private uint PreviousCoins;

        private uint Geodes;
        private uint PreviousGeodes;

        private uint Bars;
        private uint PreviousBars;

        private uint Wool;
        private uint PreviousWool;

        private uint RabbitWool;
        private uint PreviousRabbitWool;

        private uint GoatMilk;
        private uint PreviousGoatMilk;

        private uint CowMilk;
        private uint PreviousCowMilk;

        private uint Chicken;
        private uint PreviousChicken;

        private uint Duck;
        private uint PreviousDuck;

        private uint Cheese;
        private uint PreviousCheese;

        private uint GoatCheese;
        private uint PreviousGoatCheese;

        private uint Cooked;
        private uint PreviousCooked;

        private uint Beverages;
        private uint PreviousBeverages;

        private uint Preserves;
        private uint PreviousPreserves;

        private int FarmingExp;
        private int PreviousFarmingExp;

        private int MiningExp;
        private int PreviousMiningExp;

        private int ForagingExp;
        private int PreviousForagingExp;

        private int FishingExp;
        private int PreviousFishingExp;

        private int CombatExp;
        private int PreviousCombatExp;

        public IntradayStatsWriter(string statsFilePath)
        {
            OutputPath = statsFilePath;
        }

        // Initially create the CSV and add headers
        public void CreateIntradayStatsCSV(string dataPath, string gameDate)
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
                "CurrentLocation",
                "CurrentWallet",
                "Health",
                "MaxHealth",
                "Stamina",
                "MaxStamina",
                "Steps",
                "MonstersKilled",
                "MoneyEarned",
                "TotalMoneyEarned",
                "SeedsPlanted",
                "GiftsGiven",
                "ItemsCrafted",
                "ItemsShipped",
                "ItemsForaged",
                "RocksCrushed",
                "DirtHoed",
                "FishCaught",
                "TimesFished",
                "StoneCollected",
                "CoalCollected",
                "CopperCollected",
                "IronCollected",
                "GoldCollected",
                "IridiumCollected",
                "PrismaticShardsCollected",
                "DiamondsCollected",
                "CoinsCollected",
                "GeodesCracked",
                "BarsProduced",
                "SheepWoolProduced",
                "RabbitWoolProduced",
                "GoatMilkProduced",
                "CowMilkProduced",
                "ChickenEggsProduced",
                "DuckEggsProduced",
                "CheeseProduced",
                "GoatCheeseProduced",
                "MealsCooked",
                "BeveragesMade",
                "PreservesMade",
                "FarmingExp",
                "MiningExp",
                "ForagingExp",
                "FishingExp",
                "CombatExp"
                };

            // Check if the file does not exist, create the file and write the header
            if (!File.Exists(OutputPath))
            {
                // Player Misc Stats
                PreviousSteps = Game1.player.stats.stepsTaken;
                PreviousMoney = Game1.player.totalMoneyEarned;
                PreviousSeeds = Game1.player.stats.seedsSown;
                PreviousGifts = Game1.player.stats.giftsGiven;
                PreviousCrafted = Game1.player.stats.itemsCrafted;
                PreviousShipped = Game1.player.stats.itemsShipped;
                PreviousForage = Game1.player.stats.itemsForaged;

                // Other activities
                PreviousRocks = Game1.player.stats.rocksCrushed;
                PreviousDirt = Game1.player.stats.dirtHoed;
                PreviousFished = Game1.player.stats.timesFished;
                PreviousFish = Game1.player.stats.fishCaught;

                // Mining
                PreviousStone = Game1.player.stats.stoneGathered;
                PreviousCoal = Game1.player.stats.coalFound;
                PreviousCopper = Game1.player.stats.copperFound;
                PreviousIron = Game1.player.stats.ironFound;
                PreviousGold = Game1.player.stats.goldFound;
                PreviousIridium = Game1.player.stats.iridiumFound;
                PreviousPrismatic = Game1.player.stats.prismaticShardsFound;
                PreviousDiamond = Game1.player.stats.diamondsFound;
                PreviousCoins = Game1.player.stats.coinsFound;
                PreviousGeodes = Game1.player.stats.geodesCracked;
                PreviousBars = Game1.player.stats.barsSmelted;

                // Animals                
                PreviousWool = Game1.player.stats.sheepWoolProduced;
                PreviousRabbitWool = Game1.player.stats.rabbitWoolProduced;
                PreviousGoatMilk = Game1.player.stats.goatMilkProduced;
                PreviousCowMilk = Game1.player.stats.cowMilkProduced;
                PreviousChicken = Game1.player.stats.chickenEggsLayed;
                PreviousDuck = Game1.player.stats.duckEggsLayed;
                PreviousCheese = Game1.player.stats.cheeseMade;
                PreviousGoatCheese = Game1.player.stats.goatCheeseMade;
                PreviousCooked = Game1.player.stats.itemsCooked;
                PreviousBeverages = Game1.player.stats.beveragesMade;
                PreviousPreserves = Game1.player.stats.preservesMade;

                // Combat
                PreviousMonsters = Game1.player.stats.monstersKilled;

                // Experience points
                PreviousFarmingExp = Game1.player.experiencePoints[0];
                PreviousMiningExp = Game1.player.experiencePoints[1];
                PreviousForagingExp = Game1.player.experiencePoints[2];
                PreviousFishingExp = Game1.player.experiencePoints[3];
                PreviousCombatExp = Game1.player.experiencePoints[4];

                // Initial statistics for the first time starting the game with the mod
                string[] InitStats =
                {
                    dataPath,                                        // Path to the Data folder (to uniquely identify the player across saves)
                    Game1.player.Name,                               // Player name
                    Game1.player.farmName.Value,                     // Farm name
                    Game1.whichFarm.ToString(),                      // Farm type
                    DateTime.Now.ToString("yyyy-MM-dd"),             // Real Date
                    DateTime.Now.ToString("HH:mm:ss"),               // Real Time
                    gameDate,                                        // Game Date
                    FormatGameTime(Game1.timeOfDay),                 // Game Time
                    Game1.player.currentLocation.Name,               // Player's current location
                    Game1.player.Money.ToString(),                   // Current Money
                    Game1.player.health.ToString(),                  // Current Health level
                    Game1.player.maxHealth.ToString(),               // Current Max Health level
                    Game1.player.stamina.ToString(),                 // Current Energy level
                    Game1.player.maxStamina.ToString(),              // Max Energy level
                    PreviousSteps.ToString(),                        // Steps Taken
                    PreviousMonsters.ToString(),                     // Monsters Killed
                    PreviousMoney.ToString(),                        // Money earned
                    Game1.player.totalMoneyEarned.ToString(),        // Total money earned
                    PreviousSeeds.ToString(),                        // Seeds planted
                    PreviousGifts.ToString(),                        // Gifts given
                    PreviousCrafted.ToString(),                      // Items crafted
                    PreviousShipped.ToString(),                      // Items shipped
                    PreviousForage.ToString(),                       // Items foraged
                    PreviousRocks.ToString(),                        // Rocks crushed
                    PreviousDirt.ToString(),                         // Dirt hoed
                    PreviousFish.ToString(),                         // Fish caught
                    PreviousFished.ToString(),                       // Times fished
                    PreviousStone.ToString(),                        // Stone collected
                    PreviousCoal.ToString(),                         // Coal collected
                    PreviousCopper.ToString(),                       // Copper collected
                    PreviousIron.ToString(),                         // Iron collected
                    PreviousGold.ToString(),                         // Gold collected
                    PreviousIridium.ToString(),                      // Iridium collected
                    PreviousPrismatic.ToString(),                    // Prismatic shards collected
                    PreviousDiamond.ToString(),                      // Diamond collected
                    PreviousCoins.ToString(),                        // Coins collected
                    PreviousGeodes.ToString(),                       // Geodes collected
                    PreviousBars.ToString(),                         // Bars collected
                    PreviousWool.ToString(),                         // Sheep wool collected
                    PreviousRabbitWool.ToString(),                   // Rabbit wool collected
                    PreviousGoatMilk.ToString(),                     // Goat milk produced
                    PreviousCowMilk.ToString(),                      // Cow milk produced
                    PreviousChicken.ToString(),                      // Chicken eggs produced
                    PreviousDuck.ToString(),                         // Duck eggs produced
                    PreviousCheese.ToString(),                       // Cheese produced
                    PreviousGoatCheese.ToString(),                   // Goat cheese produced
                    PreviousCooked.ToString(),                       // Meals produced
                    PreviousBeverages.ToString(),                    // Beverages produced
                    PreviousPreserves.ToString(),                    // Preserves produced
                    PreviousFarmingExp.ToString(),                   // Farming experience
                    PreviousMiningExp.ToString(),                    // Mining experience
                    PreviousForagingExp.ToString(),                  // Foraging experience
                    PreviousFishingExp.ToString(),                   // Fishing experience
                    PreviousCombatExp.ToString(),                    // Combat experience
                };



                // Write the headers and initial statistics
                File.WriteAllText(OutputPath, string.Join(Delimiter, headers) + Environment.NewLine + string.Join(Delimiter, InitStats) + Environment.NewLine);
            }
        }

        // Method to write the stats to the CSV file
        public void WriteIntradayStatsToCSV(string dataPath, string gameDate)
        {
            Steps = PreviousSteps - Game1.player.stats.stepsTaken;

            Monsters = PreviousMonsters - Game1.player.stats.monstersKilled;

            Money = Game1.player.totalMoneyEarned - PreviousMoney;
            Seeds = Game1.player.stats.seedsSown - PreviousSeeds;
            Gifts = Game1.player.stats.giftsGiven - PreviousGifts;
            Crafted = Game1.player.stats.itemsCrafted - PreviousCrafted;
            Shipped = Game1.player.stats.itemsShipped - PreviousShipped;
            Forage = Game1.player.stats.itemsForaged - PreviousForage;

            Rocks = Game1.player.stats.rocksCrushed - PreviousRocks;
            Dirt = Game1.player.stats.dirtHoed - PreviousDirt;
            Fish = Game1.player.stats.fishCaught - PreviousFish;
            Fished = Game1.player.stats.timesFished - PreviousFished;

            Stone = Game1.player.stats.stoneGathered - PreviousStone;
            Coal = Game1.player.stats.coalFound - PreviousCoal;
            Copper = Game1.player.stats.copperFound - PreviousCopper;
            Iron = Game1.player.stats.ironFound - PreviousIron;
            Gold = Game1.player.stats.goldFound - PreviousGold;
            Iridium = Game1.player.stats.iridiumFound - PreviousIridium;
            Prismatic = Game1.player.stats.prismaticShardsFound - PreviousPrismatic;
            Diamond = Game1.player.stats.diamondsFound - PreviousDiamond;
            Coins = Game1.player.stats.coinsFound - PreviousCoins;
            Geodes = Game1.player.stats.geodesCracked - PreviousGeodes;
            Bars = Game1.player.stats.barsSmelted - PreviousBars;

            Wool = Game1.player.stats.sheepWoolProduced - PreviousWool;
            RabbitWool = Game1.player.stats.rabbitWoolProduced - PreviousWool;
            GoatMilk = Game1.player.stats.goatMilkProduced - PreviousGoatMilk;
            CowMilk = Game1.player.stats.cowMilkProduced - PreviousCowMilk;
            Chicken = Game1.player.stats.chickenEggsLayed - PreviousChicken;
            Duck = Game1.player.stats.duckEggsLayed - PreviousDuck;
            Cheese = Game1.player.stats.cheeseMade - PreviousCheese;
            GoatCheese = Game1.player.stats.goatCheeseMade - PreviousGoatCheese;

            Cooked = Game1.player.stats.itemsCooked - PreviousCooked;
            Beverages = Game1.player.stats.beveragesMade - PreviousBeverages;
            Preserves = Game1.player.stats.preservesMade - PreviousPreserves;

            FarmingExp = Game1.player.experiencePoints[0] - PreviousFarmingExp;
            MiningExp = Game1.player.experiencePoints[1] - PreviousMiningExp;
            ForagingExp = Game1.player.experiencePoints[2] - PreviousForagingExp;
            FishingExp = Game1.player.experiencePoints[3] - PreviousFishingExp;
            CombatExp = Game1.player.experiencePoints[4] - PreviousCombatExp;

            string[] Stats =
            {
                dataPath,                                // Path to the Data folder (to uniquely identify the player across saves)
                Game1.player.Name,                       // Player name
                Game1.player.farmName.Value,             // Farm name
                Game1.whichFarm.ToString(),              // Farm type
                DateTime.Now.ToString("yyyy-MM-dd"),     // Real Date
                DateTime.Now.ToString("HH:mm:ss"),       // Real Time
                gameDate,                                // Game Date
                FormatGameTime(Game1.timeOfDay),         // Game Time
                Game1.player.currentLocation.Name,       // Player's current location
                Game1.player.Money.ToString(),           // Current Money
                Game1.player.health.ToString(),          // Current Health level
                Game1.player.maxHealth.ToString(),       // Current Max Health level
                Game1.player.stamina.ToString(),         // Current Energy level
                Game1.player.maxStamina.ToString(),      // Max Energy level
                Steps.ToString(),                        // Steps Taken
                Monsters.ToString(),                     // Monsters Killed
                Money.ToString(),                        // Money earned
                Game1.player.totalMoneyEarned.ToString(),// Total money earned
                Seeds.ToString(),                        // Seeds planted
                Gifts.ToString(),                        // Gifts given
                Crafted.ToString(),                      // Items crafted
                Shipped.ToString(),                      // Items shipped
                Forage.ToString(),                       // Items foraged
                Rocks.ToString(),                        // Rocks crushed
                Dirt.ToString(),                         // Dirt hoed
                Fish.ToString(),                         // Fish caught
                Fished.ToString(),                       // Times fished
                Stone.ToString(),                        // Stone collected
                Coal.ToString(),                         // Coal collected
                Copper.ToString(),                       // Copper collected
                Iron.ToString(),                         // Iron collected
                Gold.ToString(),                         // Gold collected
                Iridium.ToString(),                      // Iridium collected
                Prismatic.ToString(),                    // Prismatic shards collected
                Diamond.ToString(),                      // Diamond collected
                Coins.ToString(),                        // Coins collected
                Geodes.ToString(),                       // Geodes collected
                Bars.ToString(),                         // Bars collected
                Wool.ToString(),                         // Sheep wool collected
                RabbitWool.ToString(),                   // Rabbit wool collected
                GoatMilk.ToString(),                     // Goat milk produced
                CowMilk.ToString(),                      // Cow milk produced
                Chicken.ToString(),                      // Chicken eggs produced
                Duck.ToString(),                         // Duck eggs produced
                Cheese.ToString(),                       // Cheese produced
                GoatCheese.ToString(),                   // Goat cheese produced
                Cooked.ToString(),                       // Meals produced
                Beverages.ToString(),                    // Beverages produced
                Preserves.ToString(),                    // Preserves produced
                FarmingExp.ToString(),                   // Farming experience
                MiningExp.ToString(),                    // Mining experience
                ForagingExp.ToString(),                  // Foraging experience
                FishingExp.ToString(),                   // Fishing experience
                CombatExp.ToString(),                    // Combat experience
            };
            PreviousSteps = Game1.player.stats.stepsTaken;
            PreviousMonsters = Game1.player.stats.monstersKilled;
            PreviousMoney = Game1.player.totalMoneyEarned;
            PreviousSeeds = Game1.player.stats.seedsSown;
            PreviousGifts = Game1.player.stats.giftsGiven;
            PreviousCrafted = Game1.player.stats.itemsCrafted;
            PreviousShipped = Game1.player.stats.itemsShipped;
            PreviousForage = Game1.player.stats.itemsForaged;
            PreviousRocks = Game1.player.stats.rocksCrushed;
            PreviousDirt = Game1.player.stats.dirtHoed;
            PreviousFish = Game1.player.stats.fishCaught;
            PreviousFished = Game1.player.stats.timesFished;
            PreviousStone = Game1.player.stats.stoneGathered;
            PreviousCoal = Game1.player.stats.coalFound;
            PreviousCopper = Game1.player.stats.copperFound;
            PreviousIron = Game1.player.stats.ironFound;
            PreviousGold = Game1.player.stats.goldFound;
            PreviousIridium = Game1.player.stats.iridiumFound;
            PreviousPrismatic = Game1.player.stats.prismaticShardsFound;
            PreviousDiamond = Game1.player.stats.diamondsFound;
            PreviousCoins = Game1.player.stats.coinsFound;
            PreviousGeodes = Game1.player.stats.geodesCracked;
            PreviousBars = Game1.player.stats.barsSmelted;
            PreviousWool = Game1.player.stats.sheepWoolProduced;
            PreviousWool = Game1.player.stats.rabbitWoolProduced;
            PreviousGoatMilk = Game1.player.stats.goatMilkProduced;
            PreviousCowMilk = Game1.player.stats.cowMilkProduced;
            PreviousChicken = Game1.player.stats.chickenEggsLayed;
            PreviousDuck = Game1.player.stats.duckEggsLayed;
            PreviousCheese = Game1.player.stats.cheeseMade;
            PreviousGoatCheese = Game1.player.stats.goatCheeseMade;
            PreviousCooked = Game1.player.stats.itemsCooked;
            PreviousBeverages = Game1.player.stats.beveragesMade;
            PreviousPreserves = Game1.player.stats.preservesMade;
            PreviousFarmingExp = Game1.player.experiencePoints[0];
            PreviousMiningExp = Game1.player.experiencePoints[1];
            PreviousForagingExp = Game1.player.experiencePoints[2];
            PreviousFishingExp = Game1.player.experiencePoints[3];
            PreviousCombatExp = Game1.player.experiencePoints[4];

            // Append the change data to the file
            File.AppendAllText(OutputPath, string.Join(Delimiter, Stats) + Environment.NewLine);
        }

        public string FormatGameTime(int time)
        {
            return $"{time / 100:D2}:{time % 100:D2}";
        }
    }
}