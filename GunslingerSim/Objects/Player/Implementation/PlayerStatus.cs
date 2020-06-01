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
    public class PlayerStatus : IPlayerStatus
    {
        public bool HasMisfireReaction { get; private set; }
        public MagicInitiateSpell CurrentSpell { get; private set; }
        public int NumberOfShots { get; private set; }
        public int NumberOfHits { get; private set; }
        public bool ActionAvailable { get; private set; }
        public bool BonusActionAvailable { get; private set; }
        public bool OffhandAttackAvailable { get; private set; }
        public bool ActionSurgeAvailable { get; private set; }
        public int NumberOfTurnsPassed { get; private set; }
        public int NumberOfShotsLostToMisfire { get; private set; }
        public int NumberOfCrits { get; private set; }

        public IGunStatus MainHand { get; private set; }
        public IList<IGunStatus> OffHands { get; private set; }
        public IGunStatus CurrentOffHand { get; private set; }  //CurrentOH will only be null if there are 0 offhands.
        public IPlayer PlayerBase { get; private set; }

        private IModifier spellHitMod;
        private IModifier spellDmgMod;
        private Rng Rng;
        private bool usedMfReactionThisTurn;

        public PlayerStatus(Rng rng,
                            IPlayer player)
        {
            Assert.IsNotNull(player);
            Assert.IsNotNull(rng);

            HasMisfireReaction = true;
            CurrentSpell = MagicInitiateSpell.None;
            NumberOfShots = 0;
            NumberOfHits = 0;
            NumberOfShotsLostToMisfire = 0;
            NumberOfCrits = 0;
            NumberOfTurnsPassed = 0;
            ActionSurgeAvailable = true;
            ActionAvailable = true;
            BonusActionAvailable = true;
            OffhandAttackAvailable = false;

            Rng = rng;
            PlayerBase = player;
            spellHitMod = null;
            spellDmgMod = null;
            usedMfReactionThisTurn = false;

            MainHand = new GunStatus(Rng, player.MainHand);
            OffHands = player.OffHands.Select(x => (IGunStatus)(new GunStatus(Rng, x))).ToList();
            CurrentOffHand = OffHands.FirstOrDefault();
        }

        public ActionEconomy CastBuff(MagicInitiateSpell spell)
        {
            ValidateCastBuff(spell);

            ActionEconomy action = ActionEconomy.FreeAction;
            switch (spell)
            {
                case MagicInitiateSpell.Bless:
                    {
                        ValidateCastBless();
                        action = CastBless();
                        break;
                    }
                case MagicInitiateSpell.Hex:
                    {
                        ValidateCastHex();
                        action = CastHex();
                        break;
                    }
                default:
                    {
                        throw new ArgumentException();
                    }
            }

            return action;
        }

        public void ActionSurge()
        {
            Assert.IsTrue(!ActionAvailable);
            Assert.IsTrue(ActionSurgeAvailable);

            ActionAvailable = true;
            ActionSurgeAvailable = false;
        }

        public void EndTurn()
        {
            ActionAvailable = true;
            BonusActionAvailable = true;
            OffhandAttackAvailable = false;
            usedMfReactionThisTurn = false;
            NumberOfTurnsPassed++;
        }

        public bool MainHandAttack(IEnemy enemy)    //returns bool: 'if we got a shot off'
        {
            ValidateMainHandAttack(enemy);
            ActionAvailable = false;

            bool shot = false;
            int attackNum = 0;
            while (ContinueUsingAttackActions(attackNum))
            {
                shot |= AttemptMhShoot(enemy);
                attackNum++;
            }

            NumberOfShotsLostToMisfire += DetermineAtksNotShotFromMisfire(MainHand,
                                                                          PlayerBase.NumberOfAttacks,
                                                                          attackNum);
            if (shot)
            {
                OffhandAttackAvailable = true;
            }

            return shot;
        }

        public void OffHandAttack(IEnemy enemy)
        {
            ValidateOffHandAttack(enemy);

            BonusActionAvailable = false;
            AttemptOhShoot(enemy);
        }

        private void ValidateOffHandAttack(IEnemy enemy)
        {
            Assert.IsNotNull(enemy);
            Assert.IsTrue(BonusActionAvailable);
            Assert.IsTrue(OffhandAttackAvailable);
            Assert.IsTrue(CurrentOffHand.CanFire());
            Assert.IsTrue(CurrentOffHand.HasShotLoaded());
        }

        private void ValidateMainHandAttack(IEnemy enemy)
        {
            Assert.IsNotNull(enemy);
            Assert.IsTrue(ActionAvailable);
            Assert.IsTrue(MainHand.CanFire());
        }

        public bool FixMainHandMisfire()    //TODO: remark that we only ever do this for MH, so we make that assumption
        {
            ValidateFixMainHandMisfire();

            ActionAvailable = false;
            NumberOfShotsLostToMisfire += PlayerBase.NumberOfAttacks;
            return MainHand.FixMisfire(PlayerBase.Proficiency);
        }

        public bool CanSwapOffHand()
        {
            return CurrentOffHandCanSwap() &&
                   GetNextOffHand() != null;
        }

        public bool SwapOffHand()
        {
            ValidateSwapOffHand();

            IGunStatus nextOh = GetNextOffHand();
            CurrentOffHand = nextOh;

            return nextOh != null;
        }

        private void ValidateSwapOffHand()
        {
            Assert.IsTrue(CanSwapOffHand());
        }

        private IGunStatus GetNextOffHand()
        {
            return OffHands.Where(x => !x.Equals(CurrentOffHand) &&
                                       x.CanFire() &&
                                       x.HasShotLoaded())
                           .FirstOrDefault();
        }

        private bool CurrentOffHandCanSwap()
        {
            return CurrentOffHand != null &&
                   (!CurrentOffHand.HasShotLoaded() ||
                    !CurrentOffHand.CanFire());
        }

        private bool ContinueUsingAttackActions(int attackNum)    //Note attackNum is 0-indexed; i.e. first attack is 0.
        {
            return MainHand.CanFire() &&
                   attackNum < PlayerBase.NumberOfAttacks;
        }

        private bool AttemptMhShoot(IEnemy enemy)
        {
            bool shot = false;
            if (MainHand.HasShotLoaded())
            {
                MainHandShoot(enemy);
                shot = true;
            }
            else
            {
                MainHand.ReloadChamber();
            }

            return shot;
        }

        private bool AttemptOhShoot(IEnemy enemy)
        {
            bool ret = false;
            if (CurrentOffHand.HasShotLoaded())
            {
                OffHandShoot(enemy);
                ret = true;
            }

            NumberOfShotsLostToMisfire += DetermineAtksNotShotFromMisfire(CurrentOffHand, 1, 1);

            return ret;
        }

        private void MainHandShoot(IEnemy enemy)
        {
            AttackSummary summary = DoMainHandShoot(enemy);
            UpdatePlayerStatus(summary);
            HandleMisfireWithReaction(MainHand);
        }

        private void OffHandShoot(IEnemy enemy)
        {
            OffhandAttackAvailable = false;
            AttackSummary summary = DoOffHandShoot(enemy);
            UpdatePlayerStatus(summary);

            if (UseMisfireReactionForOffHand())
            {
                HandleMisfireWithReaction(CurrentOffHand);
            }
        }

        private AttackSummary DoMainHandShoot(IEnemy enemy) //young would make fun of me if he saw this
        {
            IModifier finalHitMod = (spellHitMod != null)
                ? PlayerBase.HitModifier.Add(spellHitMod)
                : PlayerBase.HitModifier;

            IModifier finalDmgMod = (spellDmgMod != null)
                ? PlayerBase.MainHandDamageModifier.Add(spellDmgMod)
                : PlayerBase.MainHandDamageModifier;

            CombatStats combatStats = new CombatStats(finalHitMod, finalDmgMod, PlayerBase.CritValue);
            return MainHand.Shoot(enemy, combatStats);
        }

        private AttackSummary DoOffHandShoot(IEnemy enemy)
        {
            IModifier finalHitMod = (spellHitMod != null)
                ? PlayerBase.HitModifier.Add(spellHitMod)
                : PlayerBase.HitModifier;

            IModifier finalDmgMod = spellDmgMod ?? new Modifier();  //OH doesn't get dex mod

            CombatStats combatStats = new CombatStats(finalHitMod, finalDmgMod, PlayerBase.CritValue);
            return CurrentOffHand.Shoot(enemy, combatStats);
        }

        private int DetermineAtksNotShotFromMisfire(IGunStatus gun,
                                                    int numExpectedAttacks,
                                                    int attackNum)
        {
            int ret = 0;
            if (usedMfReactionThisTurn || !gun.CanFire())
            {
                if (attackNum == numExpectedAttacks)
                {
                    /* If attackNum is our max, but we can't fire, then it misfired/broke on the last shot */
                    ret = 1;
                }
                else
                {
                    /* we do numAttacks-1 because we always increment at the end of the first loop;
                     * i.e. if we misfire on first attack, numAttacks == 1, but that should count
                     * for a loss. */
                    ret = numExpectedAttacks - (attackNum - 1);
                }
            }

            usedMfReactionThisTurn = false;
            return ret;
        }

        private bool UseMisfireReactionForOffHand()
        {
            return CurrentOffHand.CurrentAmmo == CommonConstants.InfiniteAmmo;
        }

        private void UpdatePlayerStatus(AttackSummary summary)
        {
            NumberOfShots++;
            if (summary.Hit)
            {
                if (summary.Crit)
                {
                    NumberOfCrits++;
                }

                NumberOfHits++;
            }
        }

        private void HandleMisfireWithReaction(IGunStatus gun)
        {
            if (HasMisfireReaction &&
                gun.Status == GunFiringStatus.Misfired)
            {
                HasMisfireReaction = false;
                usedMfReactionThisTurn = true;
                gun.FixMisfire(PlayerBase.Proficiency);
            }
        }

        private void ValidateCastBuff(MagicInitiateSpell spell)
        {
            Assert.ValidEnum(spell);
            Assert.IsTrue(spell != MagicInitiateSpell.None);
            Assert.IsTrue(CurrentSpell == MagicInitiateSpell.None);
        }

        private ActionEconomy CastBless()
        {
            ActionAvailable = false;
            CurrentSpell = MagicInitiateSpell.Bless;
            spellHitMod = new Modifier(Rng, new List<RollType>() { RollType.d4 }, 0);

            return ActionEconomy.Action;
        }

        private ActionEconomy CastHex()
        {
            BonusActionAvailable = false;
            CurrentSpell = MagicInitiateSpell.Hex;
            spellDmgMod = new Modifier(Rng, new List<RollType>() { RollType.d6 }, 0);

            return ActionEconomy.BonusAction;
        }

        private void ValidateCastBless()
        {
            Assert.IsTrue(ActionAvailable);
        }

        private void ValidateCastHex()
        {
            Assert.IsTrue(BonusActionAvailable);
        }

        private void ValidateFixMainHandMisfire()
        {
            Assert.IsTrue(ActionAvailable);
            Assert.AreEqual(GunFiringStatus.Misfired, MainHand.Status);
        }
    }
}
