using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Common.Modifier;
using GunslingerSim.Common.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Objects
{
    public class Gun : IGun
    {
        public ICollection<GunProperty> Properties { get; protected set; }
        public int Reload { get; protected set; }
        public int Misfire { get; protected set; }
        public GunRange Range { get; protected set; }
        public int GunCost { get; protected set; }
        public int AmmoCost { get; protected set; }
        public IModifier HitModifier { get; protected set; }
        public IModifier DamageModifier { get; protected set; }
        public ICollection<RollType> DamageDice { get; protected set; }

        public Gun(ICollection<GunProperty> properties,
                   int reload,
                   int misfire,
                   ICollection<RollType> damageDice,
                   WeaponTier weaponTier,
                   GunRange range,
                   int gunCost, //value in gold
                   int ammoCost)    //value in copper
        {
            ValidateInput(properties, reload, misfire, damageDice,
                          weaponTier, range, gunCost, ammoCost);

            Properties = properties;    //TODO: deep copy?
            DamageModifier = GetDmgMod(weaponTier);
            HitModifier = GetHitMod(weaponTier);
            Reload = GetReloadValue(reload, weaponTier);
            Misfire = misfire;
            DamageDice = damageDice;
            Range = range;
            AmmoCost = ammoCost;
            GunCost = gunCost;
        }

        protected Gun()
        {
            //Empty for child class
        }

        private void ValidateInput(ICollection<GunProperty> properties,
                                   int reload,
                                   int misfire,
                                   ICollection<RollType> damageDice,
                                   WeaponTier weaponTier,
                                   GunRange range,
                                   int gunCost,
                                   int ammoCost)
        {
            Assert.HasNoNullEntries(properties);
            foreach (GunProperty property in properties)
            {
                Assert.ValidEnum(property);
            }

            Assert.IsTrue(reload >= CommonConstants.MinimumReload);
            Assert.IsTrue(misfire >= CommonConstants.MinimumMisfire);
            Assert.IsTrue(misfire <= CommonConstants.MaximumMisfire);

            Assert.HasNoNullEntries(damageDice);
            Assert.IsNotEmpty(damageDice);
            foreach(RollType damageDie in damageDice)
            {
                Assert.ValidEnum(damageDie);
                Assert.IsTrue(damageDie != RollType.None);
            }

            Assert.ValidEnum(weaponTier);
            Assert.IsNotNull(range);

            Assert.IsTrue(gunCost > 0);
            Assert.IsTrue(ammoCost > 0);
         }

        private int GetReloadValue(int reload, WeaponTier weaponTier)
        {
            return weaponTier == WeaponTier.ArtificerReloadProperty
                ? CommonConstants.InfiniteAmmo
                : reload;
        }

        private IModifier GetDmgMod(WeaponTier weaponTier)
        {
            int weaponTierModValue = CommonConstants.GetWeaponTierMod(weaponTier);
            return new Modifier(weaponTierModValue);
        }

        private IModifier GetHitMod(WeaponTier weaponTier)
        {
            int weaponTierModValue = CommonConstants.GetWeaponTierMod(weaponTier);
            return new Modifier(weaponTierModValue);
        }
    }
}
