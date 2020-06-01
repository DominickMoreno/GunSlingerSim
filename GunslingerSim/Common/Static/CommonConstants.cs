using GunslingerSim.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Common
{
    public static class CommonConstants
    {
        private static Dictionary<int, int> LevelToProficiencyMap = new Dictionary<int, int>()
        {
            { 1, 2 },
            { 2, 2 },
            { 3, 2 },
            { 4, 2 },
            { 5, 3 },
            { 6, 3 },
            { 7, 3 },
            { 8, 3 },
            { 9, 4 },
            { 10, 4 },
            { 11, 4 },
            { 12, 4 },
            { 13, 5 },
            { 14, 5 },
            { 15, 5 },
            { 16, 5 },
            { 17, 6 },
            { 18, 6 },
            { 19, 6 },
            { 20, 6 }
        };

        private static Dictionary<FightingStyle, int> FightingStyleToHitModMap = new Dictionary<FightingStyle, int>()
        {
            {FightingStyle.Archery, 2 },
            {FightingStyle.CloseQuarters, 1 }
        };

        private static Dictionary<int, int> LevelToDexModMap = new Dictionary<int, int>()
        {
            { 1, 3 },
            { 2, 3 },
            { 3, 3 },
            { 4, 4 },
            { 5, 4 },
            { 6, 5 },
            { 7, 5 },
            { 8, 5 },
            { 9, 5 },
            { 10, 5 },
            { 11, 5 },
            { 12, 5 },
            { 13, 5 },
            { 14, 5 },
            { 15, 5 },
            { 16, 5 },
            { 17, 5 },
            { 18, 5 },
            { 19, 5 },
            { 20, 5 }
        };

        private static Dictionary<int, int> FighterLevelToNumAttacksMap = new Dictionary<int, int>()
        {
            { 1, 1 },
            { 2, 1 },
            { 3, 1 },
            { 4, 1 },
            { 5, 2 },
            { 6, 2 },
            { 7, 2 },
            { 8, 2 },
            { 9, 2 },
            { 10, 2 },
            { 11, 3 },
            { 12, 3 },
            { 13, 3 },
            { 14, 3 },
            { 15, 3 },
            { 16, 3 },
            { 17, 3 },
            { 18, 3 },
            { 19, 3 },
            { 20, 4 }
        };

        private static Dictionary<int, int> FighterLevelToCritValue = new Dictionary<int, int>()
        {
            { 1, 20 },
            { 2, 20 },
            { 3, 20 },
            { 4, 20 },
            { 5, 20 },
            { 6, 20 },
            { 7, 20 },
            { 8, 20 },
            { 9, 20 },
            { 10, 20 },
            { 11, 20 },
            { 12, 20 },
            { 13, 20 },
            { 14, 20 },
            { 15, 20 },
            { 16, 20 },
            { 17, 20 },
            { 18, 19 },
            { 19, 19 },
            { 20, 19 }
        };

        private static Dictionary<int, int> ArtificerLevelToWeaponModMap = new Dictionary<int, int>()
        {
            { 0, 0 },
            { 1, 0 },
            { 2, 1 },
        };

        private static Dictionary<Feat, int> FeatToHitModMap = new Dictionary<Feat, int>()
        {
            { Feat.None, 0 },
            { Feat.Sharpshooter, -5 }
        };

        private static Dictionary<Feat, int> FeatToDmgModMap = new Dictionary<Feat, int>()
        {
            { Feat.None, 0 },
            { Feat.Sharpshooter, 10 }
        };

        private static Dictionary<WeaponTier, int> WeaponTierToModMap = new Dictionary<WeaponTier, int>()
        {
            { WeaponTier.None, 0 },
            { WeaponTier.One, 1 },
            { WeaponTier.Two, 2 },
            { WeaponTier.Three, 3 },
            { WeaponTier.ArtificerReloadProperty, 1 },
        };

        public static readonly int MinimumReload = 1;
        public static readonly int MinimumMisfire = 0;
        public static readonly int MaximumMisfire = 20;
        public static readonly int BaseMisfireDc = 8;
        public static readonly int InfiniteAmmo = int.MinValue;

        public static int GetProficiency(int level)
        {
            Assert.IsTrue(LevelToProficiencyMap.ContainsKey(level));
            return LevelToProficiencyMap[level];
        }

        public static int GetFightingStyleHitModifier(FightingStyle fightingStyle)
        {
            Assert.IsTrue(FightingStyleToHitModMap.ContainsKey(fightingStyle));
            return FightingStyleToHitModMap[fightingStyle];
        }

        public static int GetDexMod(int level)
        {
            Assert.IsTrue(LevelToDexModMap.ContainsKey(level));
            return LevelToDexModMap[level];
        }

        public static int GetNumberOfAttacks(int fighterLevel)
        {
            Assert.IsTrue(FighterLevelToNumAttacksMap.ContainsKey(fighterLevel));
            return FighterLevelToNumAttacksMap[fighterLevel];
        }

        public static int GetCritValue(int fighterLevel)
        {
            Assert.IsTrue(FighterLevelToCritValue.ContainsKey(fighterLevel));
            return FighterLevelToCritValue[fighterLevel];
        }

        public static int GetArtificerWeaponMod(int artificerLevel)
        {
            Assert.IsTrue(ArtificerLevelToWeaponModMap.ContainsKey(artificerLevel));
            return ArtificerLevelToWeaponModMap[artificerLevel];
        }

        public static int GetFeatHitMod(Feat feat)
        {
            Assert.IsTrue(FeatToHitModMap.ContainsKey(feat));
            return FeatToHitModMap[feat];
        }

        public static int GetFeatDmgMod(Feat feat)
        {
            Assert.IsTrue(FeatToDmgModMap.ContainsKey(feat));
            return FeatToDmgModMap[feat];
        }

        public static int GetWeaponTierMod(WeaponTier weaponTier)
        {
            Assert.IsTrue(WeaponTierToModMap.ContainsKey(weaponTier));
            return WeaponTierToModMap[weaponTier];
        }
    }
}
