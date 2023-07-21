# stardew-data-tracking
A mod and Power BI solution for performing analytics on Stardew Valley game save data. The purpose of this project is not to be a sustainable mod for all use-cases. Instead, this project is meant to learn Power BI or other analytics tools. 

Use it to track relevant, interesting data throughout the first 1-2 years of gameplay to address questions like the below:

- What is the revenue of the farm? How is this evolving over time?
- What are the different revenue sources? Different cost sources?
- What is the net margin (profit) of the farm?
- Where does the farmer spend their time throughout the day(s), typically (and how does this correlate with daily/weekly/monthly revenue)?
- How does the farmer's energy deplete throughout the day(s), typically? Their health?
- What kinds of targets can you infer from this data for each year/season?

## Data
There are three different files produced by the mod. All files are added to the respective save directory by default in _%AppData%/StardewValley/Saves/_.

1. **Transactions:** This file is a list of all the inventory transactions, including all items that enter and exit the inventory. It documents the item name, quantity, quality and transaction types. It includes transaction types for the following:
    - **Bought/Sold:** Buying or selling items at a shop
    - **Deposit/Withdrawal:** Depositing or withdrawing items in a chest
    - **Outbound Shipping:** Depositing items in the _default_ shipping container
    - **Other:** All other ways that items can move in the inventory (picking up, using, throwing away, gifting items, etc.)

2. **Intraday Stats:** This file provides a record of various statistics every 10 minutes in-game time. You can use this file to infer the sleep by taking the maximum time for each in-game day.

3. **Interday Stats:** This file provides a record of various statistics at the end of every in-game day.

# Instructions 
If you want to use or modify this mod, the below instructions can help you.

## For developers / contributors
Below are instructions for you to contribute to the project.

## Step 1. Install Stardew Valley
Install from Steam, GOG or another vendor.

## Step 2. Install SMAPI
See [SMAPI](https://smapi.io/) for instructions. Ensure that you install it properly depending on your client (i.e. Steam, GOG), especially if you want to get achievements.

## Step 3. Fork and clone the Repo
- Fork the Repo in GitHub to your own account
- Clone the Repo to your local machine
- In your SCM tool of choice (i.e. VS Code) ensure that you have the necessary pre-requisites, like the extensions to handle .Net projects and C# code

## Step 4. Make a branch for your changes
- Create a new branch from the 'main' branch of your fork
- Switch to this branch in your local IDE (i.e. VS Code)

## Step 5. Make and commit changes, then open a PR of your branch to origin/main
- Make changes to the code and files (ensure you add comments to .cs files)
- Commit your changes in batches with descriptive commit messages
- When ready, create a pr from your-github-account/stardew-data-tracking/your-development-branch to origin/stardew-data-tracking/main
- In the PR, you can request my review; I'm not a GitHub expert so bear with me

# For players

## Step 1. Install and run Stardew Valley
Install from Steam, GOG or another vendor.

## Step 2. Install SMAPI
See [SMAPI](https://smapi.io/) for instructions. Ensure that you install it properly depending on your client (i.e. Steam, GOG), especially if you want to get achievements.

## Step 3. Download the mod files
Download the files (including the compiled project .zip) from the GitHub repo.

## Step 4. Extract the .zip to the 'Mods' folder
Extract the .zip file to the Mods folder.