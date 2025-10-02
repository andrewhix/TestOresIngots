using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using ItemManager;
using JetBrains.Annotations;
using LocalizationManager;
using ServerSync;
using UnityEngine;

namespace TestOresIngots
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class TestOresIngotsPlugin : BaseUnityPlugin
    {
        internal const string ModName = "TestOresIngots";
        internal const string ModVersion = "1.0.0";
        internal const string Author = "Cthululemon";
        private const string ModGUID = Author + "." + ModName;
        private static string ConfigFileName = ModGUID + ".cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;

        internal static string ConnectionError = "";

        private readonly Harmony _harmony = new(ModGUID);

        public static readonly ManualLogSource TestOresIngotsLogger =
            BepInEx.Logging.Logger.CreateLogSource(ModName);

        private static readonly ConfigSync ConfigSync = new(ModGUID)
            { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

        public enum Toggle
        {
            On = 1,
            Off = 0
        }
        
        public void Awake()
        {
            _serverConfigLocked = config("1 - General", "Lock Configuration", Toggle.On, "If on, the configuration is locked and can be changed by server admins only.");
            _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);
            



            Item UOshadow_ore = new Item("uooresingots", "UOshadow_ore");
            UOshadow_ore.Name.English("Shadow Ore");
            UOshadow_ore.Description.English("A chunk of shadow ore, pulsating with dark energy.");
            UOshadow_ore.Snapshot(); // I don't have an icon for this item in my asset bundle, so I will let the ItemManager generate one automatically
            Item UOshadow_ingot = new Item("uooresingots", "UOshadow_ingot");
            UOshadow_ingot.Name.English("Shadow Ingot");
            UOshadow_ingot.Description.English("An ingot of shadow metal, radiating a faint dark aura.");
            UOshadow_ingot.Snapshot(); // I don't have an icon for this item in my asset bundle, so I will let the ItemManager generate one automatically

            _ = new Conversion(UOshadow_ingot)
            {
                Input = "UOshadow_ore",
                Piece = ConversionPiece.Smelter
            };

            Item UOagapite_ore = new Item("uooresingots", "UOagapite_ore");
            UOagapite_ore.Name.English("Agapite Ore");
            UOagapite_ore.Description.English("A chunk of agapite ore, shimmering with a silvery-blue hue.");
            UOagapite_ore.Snapshot(); // I don't have an icon for this item in my asset bundle, so I will let the ItemManager generate one automatically
            Item UOagapite_ingot = new Item("uooresingots", "UOagapite_ingot");
            UOagapite_ingot.Name.English("Agapite Ingot");
            UOagapite_ingot.Description.English("An ingot of agapite metal, gleaming with a silvery-blue sheen.");
            UOagapite_ingot.Snapshot(); // I don't have an icon for this item in my asset bundle, so I will let the ItemManager generate one automatically
            _ = new Conversion(UOagapite_ingot)
            {
                Input = "UOagapite_ore",
                Piece = ConversionPiece.Smelter
            };

            Item UOgold_ore = new Item("uooresingots", "UOgold_ore");
            UOgold_ore.Name.English("Gold Ore");
            UOgold_ore.Description.English("A chunk of gold ore, glittering with a rich golden color.");
            UOgold_ore.Snapshot(); // I don't have an icon for this item in my asset bundle, so I will let the ItemManager generate one automatically
            Item UOgold_ingot = new Item("uooresingots", "UOgold_ingot");
            UOgold_ingot.Name.English("Gold Ingot");
            UOgold_ingot.Description.English("An ingot of pure gold, shining with a brilliant luster.");
            UOgold_ingot.Snapshot(); // I don't have an icon for this item in my asset bundle, so I will let the ItemManager generate one automatically

            _ = new Conversion(UOgold_ingot)
            {
                Input = "UOgold_ore",
                Piece = ConversionPiece.Smelter
            };

            Item UOverite_ore = new Item("uooresingots", "UOverite_ore");
            UOverite_ore.Name.English("Verite Ore");
            UOverite_ore.Description.English("A chunk of Verite ore, glowing with a mystical light.");
            UOverite_ore.Snapshot(); // I don't have an icon for this item in my asset bundle, so I will let the ItemManager generate one automatically
            Item UOverite_ingot = new Item("uooresingots", "UOverite_ingot");
            UOverite_ingot.Name.English("Verite Ingot");
            UOverite_ingot.Description.English("An ingot of Verite metal, pulsating with a mystical energy.");
            UOverite_ingot.Snapshot(); // I don't have an icon for this item in my asset bundle, so I will let the ItemManager generate one automatically

            _ = new Conversion(UOverite_ingot)
            {
                Input = "UOverite_ore",
                Piece = ConversionPiece.Smelter
            };

            Item UObloodrock_ore = new Item("uooresingots", "UObloodrock_ore");
            UObloodrock_ore.Name.English("Bloodrock Ore");
            UObloodrock_ore.Description.English("A chunk of bloodrock ore, radiating a deep red glow.");
            UObloodrock_ore.Snapshot(); // I don't have an icon for this item in my asset bundle, so I will let the ItemManager generate one automatically
            Item UObloodrock_ingot = new Item("uooresingots", "UObloodrock_ingot");
            UObloodrock_ingot.Name.English("Bloodrock Ingot");
            UObloodrock_ingot.Description.English("An ingot of bloodrock metal, emanating a powerful red aura.");
            UObloodrock_ingot.Snapshot(); // I don't have an icon for this item in my asset bundle, so I will let the ItemManager generate one automatically

            _ = new Conversion(UObloodrock_ingot)
            {
                Input = "UObloodrock_ore",
                Piece = ConversionPiece.Smelter
            };

            Item UOblackrock_ore = new Item("uooresingots", "UOblackrock_ore");
            UOblackrock_ore.Name.English("Blackrock Ore");
            UOblackrock_ore.Description.English("A chunk of blackrock ore, absorbing light around it.");
            UOblackrock_ore.Snapshot(); // I don't have an icon for this item in my asset bundle, so I will let the ItemManager generate one automatically
            Item UOblackrock_ingot = new Item("uooresingots", "UOblackrock_ingot");
            UOblackrock_ingot.Name.English("Blackrock Ingot");
            UOblackrock_ingot.Description.English("An ingot of blackrock metal, exuding a dark and mysterious aura.");
            UOblackrock_ingot.Snapshot(); // I don't have an icon for this item in my asset bundle, so I will let the ItemManager generate one automatically

            _ = new Conversion(UOblackrock_ingot)
            {
                Input = "UOblackrock_ore",
                Piece = ConversionPiece.Smelter
            };

            Item UOsnow_ore = new Item("uooresingots", "UOsnow_ore");
            UOsnow_ore.Name.English("Snow Ore");
            UOsnow_ore.Description.English("A chunk of snow ore, cold to the touch and shimmering with icy crystals.");
            UOsnow_ore.Snapshot(); // I don't have an icon for this item in my asset bundle, so I will let the ItemManager generate one automatically
            Item UOsnow_ingot = new Item("uooresingots", "UOsnow_ingot");
            UOsnow_ingot.Name.English("Snow Ingot");
            UOsnow_ingot.Description.English("An ingot of snow metal, radiating a chilling coldness.");
            UOsnow_ingot.Snapshot(); // I don't have an icon for this item in my asset bundle, so I will let the ItemManager generate one automatically

            _ = new Conversion(UOsnow_ingot)
            {
                Input = "UOsnow_ore",
                Piece = ConversionPiece.Smelter
            };

            Item UOice_ore = new Item("uooresingots", "UOice_ore");
            UOice_ore.Name.English("Ice Ore");
            UOice_ore.Description.English("A chunk of ice ore, frozen solid and sparkling with frost.");
            UOice_ore.Snapshot(); // I don't have an icon for this item in my asset bundle, so I will let the ItemManager generate one automatically
            Item UOice_ingot = new Item("uooresingots", "UOice_ingot");
            UOice_ingot.Name.English("Ice Ingot");
            UOice_ingot.Description.English("An ingot of ice metal, emanating an intense cold.");
            UOice_ingot.Snapshot(); // I don't have an icon for this item in my asset bundle, so I will let the ItemManager generate one automatically

            _ = new Conversion(UOice_ingot)
            {
                Input = "UOice_ore",
                Piece = ConversionPiece.Smelter
            };

            Item UOvalorite_ore = new Item("uooresingots", "UOvalorite_ore");
            UOvalorite_ore.Name.English("Valorite Ore");
            UOvalorite_ore.Description.English("A chunk of valorite ore, shining with a radiant light.");
            UOvalorite_ore.Snapshot(); // I don't have an icon for this item in my asset bundle, so I will let the ItemManager generate one automatically
            Item UOvalorite_ingot = new Item("uooresingots", "UOvalorite_ingot");
            UOvalorite_ingot.Name.English("Valorite Ingot");
            UOvalorite_ingot.Description.English("An ingot of valorite metal, glowing with a radiant energy.");
            UOvalorite_ingot.Snapshot(); // I don't have an icon for this item in my asset bundle, so I will let the ItemManager generate one automatically

            _ = new Conversion(UOvalorite_ingot)
            {
                Input = "UOvalorite_ore",
                Piece = ConversionPiece.Smelter
            };


            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
            SetupWatcher();
        }

        private void OnDestroy()
        {
            Config.Save();
        }

        private void SetupWatcher()
        {
            FileSystemWatcher watcher = new(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        private void ReadConfigValues(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(ConfigFileFullPath)) return;
            try
            {
                TestOresIngotsLogger.LogDebug("ReadConfigValues called");
                Config.Reload();
            }
            catch
            {
                TestOresIngotsLogger.LogError($"There was an issue loading your {ConfigFileName}");
                TestOresIngotsLogger.LogError("Please check your config entries for spelling and format!");
            }
        }


        #region ConfigOptions

        private static ConfigEntry<Toggle> _serverConfigLocked = null!;
        private static ConfigEntry<Toggle> _recipeIsActiveConfig = null!;

        private ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description,
            bool synchronizedSetting = true)
        {
            ConfigDescription extendedDescription =
                new(
                    description.Description +
                    (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"),
                    description.AcceptableValues, description.Tags);
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, extendedDescription);
            //var configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }

        private ConfigEntry<T> config<T>(string group, string name, T value, string description,
            bool synchronizedSetting = true)
        {
            return config(group, name, value, new ConfigDescription(description), synchronizedSetting);
        }

        private class ConfigurationManagerAttributes
        {
            [UsedImplicitly] public int? Order;
            [UsedImplicitly] public bool? Browsable;
            [UsedImplicitly] public string? Category;
            [UsedImplicitly] public Action<ConfigEntryBase>? CustomDrawer;
        }

        #endregion
    }
    
    public static class KeyboardExtensions
    {
        public static bool IsKeyDown(this KeyboardShortcut shortcut)
        {
            return shortcut.MainKey != KeyCode.None && Input.GetKeyDown(shortcut.MainKey) && shortcut.Modifiers.All(Input.GetKey);
        }

        public static bool IsKeyHeld(this KeyboardShortcut shortcut)
        {
            return shortcut.MainKey != KeyCode.None && Input.GetKey(shortcut.MainKey) && shortcut.Modifiers.All(Input.GetKey);
        }
    }
}