using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Common.Modifier;
using GunslingerSim.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GunslingerSim.Objects
{
    public class Player : IPlayer
    {
        public IModifier HitModifier { get { return GetHitMod(); } }
        public IModifier MainHandDamageModifier { get { return GetMhDmgMod(); } }
        public IModifier OffHandDamageModifier { get { return GetOhDmgMod(); } }
        public IGun MainHand { get; protected set; }
        public IList<IGun> OffHands { get; protected set; }
        public int NumberOfAttacks { get { return numberOfAttacks; } }
        public int Proficiency { get; private set; }
        public int CritValue { get; private set; }

        private Rng Rng;
        private int dexMod;
        private int fightingStyleToHitMod;
        private int numberOfAttacks;
        private int featHitMods;
        private int featDmgMods;
        private ICollection<MagicInitiateSpell> buffs;

        public Player(Rng rng,
                      int fighterLevel,
                      int totalLevel,
                      FightingStyle fightingStyle,
                      IGun mainHand,
                      IList<IGun> offHands,
                      ICollection<Feat> feats,
                      ICollection<MagicInitiateSpell> buffs)
        {
            ValidateInput(rng, mainHand, offHands, feats, buffs);

            Rng = rng;
            MainHand = mainHand;
            OffHands = offHands;    //TODO: deep copy?

            featHitMods = GetFeatHitMods(feats);
            featDmgMods = GetFeatDmgMods(feats);

            fightingStyleToHitMod = CommonConstants.GetFightingStyleHitModifier(fightingStyle);
            numberOfAttacks = CommonConstants.GetNumberOfAttacks(fighterLevel);
            CritValue = CommonConstants.GetCritValue(fighterLevel);
            dexMod = CommonConstants.GetDexMod(fighterLevel);

            Proficiency = CommonConstants.GetProficiency(totalLevel);
            this.buffs = buffs;
        }

        public IPlayerStatus GetStatus()
        {
            return new PlayerStatus(Rng, this);
        }

        public bool CanCastBuff(MagicInitiateSpell spell)
        {
            Assert.ValidEnum(spell);
            return buffs.Contains(spell);
        }

        protected Player()
        {
            //Empty for child classes
        }

        private void ValidateInput(Rng rng,
                                   IGun mainHand,
                                   IList<IGun> offHands,
                                   ICollection<Feat> feats,
                                   ICollection<MagicInitiateSpell> buffs)
        {
            Assert.IsNotNull(rng);
            Assert.IsNotNull(mainHand);
            Assert.HasNoNullEntries(offHands);
            Assert.HasNoNullEntries(feats);
            foreach (Feat feat in feats)
            {
                Assert.ValidEnum(feat);
            }

            Assert.HasNoNullEntries(buffs);
            foreach(MagicInitiateSpell buff in buffs)
            {
                Assert.ValidEnum(buff);
            }
        }

        private int GetFeatHitMods(ICollection<Feat> feats)
        {
            return feats.Select(x => CommonConstants.GetFeatHitMod(x))
                        .Sum();
        }

        private int GetFeatDmgMods(ICollection<Feat> feats)
        {
            return feats.Select(x => CommonConstants.GetFeatDmgMod(x))
                        .Sum();
        }

        private IModifier GetHitMod()
        {
            //TODO: artifier hit mod will be a gun property
            int baseModVal = Proficiency +
                             dexMod +
                             fightingStyleToHitMod +
                             featHitMods;

            return new Modifier(baseModVal);
        }

        private IModifier GetMhDmgMod()
        {
            int baseModVal = dexMod +
                             featDmgMods;

            return new Modifier(baseModVal);
        }

        private IModifier GetOhDmgMod()
        {
            return new Modifier(featDmgMods);
        }
    }
}
