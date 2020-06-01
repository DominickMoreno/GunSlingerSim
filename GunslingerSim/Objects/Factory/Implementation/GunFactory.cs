using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Common.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Objects.Factory
{
    public class GunFactory : IGunFactory
    {
        private static readonly ICollection<GunProperty> EmptyProperties = new List<GunProperty>() { GunProperty.None };

        public GunFactory()
        {
            //Empty
        }

        public IGun Get(GunType type, WeaponTier weaponTier)
        {
            Assert.ValidEnum(type);
            Assert.ValidEnum(weaponTier);

            return Build(type, weaponTier);
        }

        public IGun Get(GunType type)
        {
            return Get(type, WeaponTier.None);
        }

        private IGun Build(GunType type, WeaponTier weaponTier)
        {
            Assert.ValidEnum(type);

            IGun ret = null;
            switch (type)
            {
                case GunType.PalmPistol:
                {
                    ret = BuildPalmPistol(weaponTier);
                    break;
                }
                case GunType.Pistol:
                {
                    ret = BuildPistol(weaponTier);
                    break;
                }
                case GunType.Musket:
                {
                    ret = BuildMusket(weaponTier);
                    break;
                }
                case GunType.Pepperbox:
                {
                    ret = BuildPepperbox(weaponTier);
                    break;
                }
                case GunType.Blunderbuss:
                {
                    ret = BuildBlunderbuss(weaponTier);
                    break;
                }
                case GunType.BadNews:
                {
                    ret = BuildBadNews(weaponTier);
                    break;
                }
                case GunType.HandMortar:
                {
                    ret = BuildHandMortar(weaponTier);
                    break;
                }
                default:
                {
                    throw new ArgumentException();
                }
            }

            return ret;
        }

        private IGun BuildPalmPistol(WeaponTier weaponTier)
        {
            return new Gun(new List<GunProperty>() { GunProperty.Light },
                           1,
                           1,
                           new List<RollType>() { RollType.d8 },
                           weaponTier,
                           new GunRange(40, 160),
                           50,
                           10);
        }

        private IGun BuildPistol(WeaponTier weaponTier)
        {
            return new Gun(EmptyProperties,
                           4,
                           1,
                           new List<RollType>() { RollType.d10 },
                           weaponTier,
                           new GunRange(60, 240),
                           150,
                           20);
        }

        private IGun BuildMusket(WeaponTier weaponTier)
        {
            return new Gun(new List<GunProperty>() { GunProperty.TwoHand },
                           1,
                           2,
                           new List<RollType>() { RollType.d12 },
                           weaponTier,
                           new GunRange(120, 480),
                           300,
                           25);
        }

        private IGun BuildPepperbox(WeaponTier weaponTier)
        {
            return new Gun(EmptyProperties,
                           6,
                           2,
                           new List<RollType>() { RollType.d10 },
                           weaponTier,
                           new GunRange(80, 320),
                           250,
                           20);
        }

        private IGun BuildBlunderbuss(WeaponTier weaponTier)
        {
            return new Gun(EmptyProperties,
                           1,
                           2,
                           new List<RollType>() { RollType.d8,
                                                  RollType.d8 },
                           weaponTier,
                           new GunRange(15, 60),
                           300,
                           100);
        }

        private IGun BuildBadNews(WeaponTier weaponTier)
        {
            return new Gun(new List<GunProperty>() { GunProperty.TwoHand },
                           1,
                           3,
                           new List<RollType>() { RollType.d12 },
                           weaponTier,
                           new GunRange(200, 800),
                           1000,    //TODO: this is 'crafted'/custom
                           200);
        }

        private IGun BuildHandMortar(WeaponTier weaponTier)
        {
            return new Gun(new List<GunProperty>() { GunProperty.Explosive },
                           1,
                           3,
                           new List<RollType>() { RollType.d8 },
                           weaponTier,
                           new GunRange(30, 60),
                           1000,    //TODO: this is 'crafted'/custom
                           1000);
        }
    }
}
