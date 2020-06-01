using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Common.Util;
using GunslingerSim.Objects;
using GunslingerSim.Objects.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Tests
{
    public class PlayerStatusUnitTest : BaseUnitTest
    {

        #region Setup

        Enemy enemy;
        Player player;
        PlayerStatus status;

        public PlayerStatusUnitTest()
        {
            AddTest(nameof(Test_Constructor_NullArgs), Test_Constructor_NullArgs);
            AddTest(nameof(Test_Constructor), Test_Constructor);

            AddTest(nameof(Test_CastBuff_InvalidEnum), Test_CastBuff_InvalidEnum);
            AddTest(nameof(Test_CastBuff_None), Test_CastBuff_None);
            AddTest(nameof(Test_CastBuff_Hex), Test_CastBuff_Hex);
            AddTest(nameof(Test_CastBuff_Bless), Test_CastBuff_Bless);
            AddTest(nameof(Test_CastBuff_AlreadyConcentrating), Test_CastBuff_AlreadyConcentrating);

            AddTest(nameof(Test_ActionSurge_HasAction), Test_ActionSurge_HasAction);
            AddTest(nameof(Test_ActionSurge_ActionSurgeUnavailable), Test_ActionSurge_ActionSurgeUnavailable);
            AddTest(nameof(Test_ActionSurge_Success), Test_ActionSurge_Success);

            AddTest(nameof(Test_EndTurn), Test_EndTurn);

            AddTest(nameof(Test_MainHandAttack_NullArg), Test_MainHandAttack_NullArg);
            AddTest(nameof(Test_MainHandAttack_UsedAction), Test_MainHandAttack_UsedAction);
            AddTest(nameof(Test_MainHandAttack_GunCannotAttack), Test_MainHandAttack_GunCannotAttack);
            AddTest(nameof(Test_MainHandAttack_SingleAttack_Always1), Test_MainHandAttack_SingleAttack_Always1);
            AddTest(nameof(Test_MainHandAttack_SingleAttack_Always10), Test_MainHandAttack_SingleAttack_Always10);
            AddTest(nameof(Test_MainHandAttack_SingleAttack_Always20), Test_MainHandAttack_SingleAttack_Always20);
            AddTest(nameof(Test_MainHandAttack_3Atk_Always1), Test_MainHandAttack_3Atk_Always1);
            AddTest(nameof(Test_MainHandAttack_3Atk_Always10), Test_MainHandAttack_3Atk_Always10);
            AddTest(nameof(Test_MainHandAttack_3Atk_Always20), Test_MainHandAttack_3Atk_Always20);
            AddTest(nameof(Test_MainHandAttack_2Atk_Artificer), Test_MainHandAttack_2Atk_Artificer);
            AddTest(nameof(Test_MainHandAttack_3Atk_Reload), Test_MainHandAttack_3Atk_Reload);
            AddTest(nameof(Test_MainHandShoot_Hex), Test_MainHandShoot_Hex);
            AddTest(nameof(Test_MainHandShoot_HexCrit), Test_MainHandShoot_HexCrit);
            AddTest(nameof(Test_MainHandShoot_BlessDoesNotEffectMisfire), Test_MainHandShoot_BlessDoesNotEffectMisfire);
            AddTest(nameof(Test_MainHandShoot_SharpshooterMiss), Test_MainHandShoot_SharpshooterMiss);
            AddTest(nameof(Test_MainHandShoot_SharpshooterHit), Test_MainHandShoot_SharpshooterHit);
            AddTest(nameof(Test_MainHandShoot_SharpshooterCrit), Test_MainHandShoot_SharpshooterCrit);

            AddTest(nameof(Test_OffHandAttack_NullArg), Test_OffHandAttack_NullArg);
            AddTest(nameof(Test_OffHandAttack_BonusActionNotAvailable), Test_OffHandAttack_BonusActionNotAvailable);
            AddTest(nameof(Test_OffHandAttack_CantUseOhAtk), Test_OffHandAttack_CantUseOhAtk);
            AddTest(nameof(Test_OffHandAttack_OhNoAmmo), Test_OffHandAttack_OhNoAmmo);
            AddTest(nameof(Test_OffHandAttack_Miss), Test_OffHandAttack_Miss);
            AddTest(nameof(Test_OffHandAttack_Hit), Test_OffHandAttack_Hit);
            AddTest(nameof(Test_OffHandAttack_Crit), Test_OffHandAttack_Crit);
            AddTest(nameof(Test_OffHandAttack_Hex), Test_OffHandAttack_Hex);
            AddTest(nameof(Test_OffHandAttack_HexCrit), Test_OffHandAttack_HexCrit);
            AddTest(nameof(Test_OffHandAttack_Bless), Test_OffHandAttack_Bless);
            AddTest(nameof(Test_OffHandAttack_ArtificerModifiers), Test_OffHandAttack_ArtificerModifiers);

            AddTest(nameof(Test_FixMainHandMisifre_ActionNotAvail), Test_FixMainHandMisifre_ActionNotAvail);
            AddTest(nameof(Test_FixMainHandFireMisfire_GunNotMisfired), Test_FixMainHandFireMisfire_GunNotMisfired);

            AddTest(nameof(Test_CanSwapOffHand_NoOh), Test_CanSwapOffHand_NoOh);
            AddTest(nameof(Test_CanSwapOffHand_OneOh), Test_CanSwapOffHand_OneOh);
            AddTest(nameof(Test_CanSwapOffHand_CanSwap), Test_CanSwapOffHand_CanSwap);
            AddTest(nameof(Test_CanSwapOffHand_NoShotLoaded), Test_CanSwapOffHand_NoShotLoaded);

            AddTest(nameof(Test_SwapOffHand_NoOh), Test_SwapOffHand_NoOh);
            AddTest(nameof(Test_SwapOffHand_OneOh), Test_SwapOffHand_OneOh);
            AddTest(nameof(Test_SwapOffHand_TwoOhs), Test_SwapOffHand_TwoOhs);
            AddTest(nameof(Test_SwapOffHand_ArtificerOhs), Test_SwapOffHand_ArtificerOhs);
            AddTest(nameof(Test_SwapOffHand_ArtificerOhsMisfire), Test_SwapOffHand_ArtificerOhsMisfire);

            AddTest(nameof(Test_FixMainHandMisfire_Fails), Test_FixMainHandMisfire_Fails);
            AddTest(nameof(Test_FixMainHandMisfire_Succeeds), Test_FixMainHandMisfire_Succeeds);

            AddTest(nameof(Test_ShotsLostToMisfire_1Atk_MissNoMf), Test_ShotsLostToMisfire_1Atk_MissNoMf);
            AddTest(nameof(Test_ShotsLostToMisfire_1Atk_MhMf), Test_ShotsLostToMisfire_1Atk_MhMf);
            AddTest(nameof(Test_ShotsLostToMisfire_1Atk_OhMf), Test_ShotsLostToMisfire_1Atk_OhMf);
            AddTest(nameof(Test_ShotsLostToMisfire_1Atk_BothMf), Test_ShotsLostToMisfire_1Atk_BothMf);
            AddTest(nameof(Test_ShotsLostToMisfire_2Atk_MhHitMf), Test_ShotsLostToMisfire_2Atk_MhHitMf);
            AddTest(nameof(Test_ShotsLostToMisfire_2Atk_MhMfFixHit), Test_ShotsLostToMisfire_2Atk_MhMfFixHit);
            AddTest(nameof(Test_ShotsLostToMisfire_2Atk_MhHitMh), Test_ShotsLostToMisfire_2Atk_MhHitMh);
            AddTest(nameof(Test_ShotsLostToMisfire_2Atk_OhMf), Test_ShotsLostToMisfire_2Atk_OhMf);
            AddTest(nameof(Test_ShotsLostToMisfire_2Atk_AllMf), Test_ShotsLostToMisfire_2Atk_AllMf);
            AddTest(nameof(Test_ShotsLostToMisfire_3Atk_MhMissNoMf), Test_ShotsLostToMisfire_3Atk_MhMissNoMf);
            AddTest(nameof(Test_ShotsLostToMisfire_3Atk_MhHitHitMf), Test_ShotsLostToMisfire_3Atk_MhHitHitMf);
            AddTest(nameof(Test_ShotsLostToMisfire_3Atk_MhHitMfHit), Test_ShotsLostToMisfire_3Atk_MhHitMfHit);
            AddTest(nameof(Test_ShotsLostToMisfire_3Atk_MfMfLost), Test_ShotsLostToMisfire_3Atk_MfMfLost);
            AddTest(nameof(Test_ShotsLostToMisfire_3Atk_OhMf), Test_ShotsLostToMisfire_3Atk_OhMf);
            AddTest(nameof(Test_ShotsLostToMisfire_3Atk_AllMf), Test_ShotsLostToMisfire_3Atk_AllMf);
            AddTest(nameof(Test_ShotsLostToMisfire_FixMh), Test_ShotsLostToMisfire_FixMh);
        }

        protected override void Setup()
        {
            enemy = new Enemy(13);
            player = new Player(always10, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs);
            status = new PlayerStatus(always10, player);
        }

        protected override void TearDown()
        {
            enemy = null;
            player = null;
            status = null;
        }

        #endregion Setup

        #region Constructor

        private void Test_Constructor_NullArgs()
        {
            Assert.Throws<ArgumentNullException>(() => new PlayerStatus(null, player));
            Assert.Throws<ArgumentNullException>(() => new PlayerStatus(always1, null));
        }

        private void Test_Constructor()
        {
            PlayerStatus ret = null;
            Assert.DoesNotThrow(() => ret = new PlayerStatus(always1, player));

            Assert.IsNotNull(ret);
        }

        #endregion Constructor

        #region Cast Buff

        private void Test_CastBuff_InvalidEnum()
        {
            MagicInitiateSpell invalidEnum = (MagicInitiateSpell)3;
            Assert.Throws<ArgumentException>(() => status.CastBuff(invalidEnum));

            invalidEnum = (MagicInitiateSpell)(-1);
            Assert.Throws<ArgumentException>(() => status.CastBuff(invalidEnum));

            invalidEnum = (MagicInitiateSpell)(int.MaxValue);
            Assert.Throws<ArgumentException>(() => status.CastBuff(invalidEnum));

            invalidEnum = (MagicInitiateSpell)(int.MinValue);
            Assert.Throws<ArgumentException>(() => status.CastBuff(invalidEnum));

            Assert.AreEqual(MagicInitiateSpell.None, status.CurrentSpell);
            Assert.IsTrue(status.BonusActionAvailable);
            Assert.IsTrue(status.ActionAvailable);
        }

        private void Test_CastBuff_None()
        {
            Assert.Throws<ArgumentException>(() => status.CastBuff(MagicInitiateSpell.None));
            Assert.AreEqual(MagicInitiateSpell.None, status.CurrentSpell);

            Assert.IsTrue(status.BonusActionAvailable);
            Assert.IsTrue(status.ActionAvailable);
        }

        private void Test_CastBuff_Hex()
        {
            ActionEconomy action = ActionEconomy.FreeAction;
            Assert.DoesNotThrow(() => action = status.CastBuff(MagicInitiateSpell.Hex));

            Assert.IsTrue(!status.BonusActionAvailable);
            Assert.IsTrue(status.ActionAvailable);
            Assert.AreEqual(MagicInitiateSpell.Hex, status.CurrentSpell);
            Assert.AreEqual(ActionEconomy.BonusAction, action);
        }

        private void Test_CastBuff_Bless()
        {
            ActionEconomy action = ActionEconomy.FreeAction;
            Assert.DoesNotThrow(() => action = status.CastBuff(MagicInitiateSpell.Bless));

            Assert.IsTrue(status.BonusActionAvailable);
            Assert.IsTrue(!status.ActionAvailable);
            Assert.AreEqual(MagicInitiateSpell.Bless, status.CurrentSpell);
            Assert.AreEqual(ActionEconomy.Action, action);
        }

        private void Test_CastBuff_AlreadyConcentrating()
        {
            Assert.DoesNotThrow(() => status.CastBuff(MagicInitiateSpell.Bless));

            Assert.Throws<ArgumentException>(() => status.CastBuff(MagicInitiateSpell.Hex));
            Assert.IsTrue(status.BonusActionAvailable);
            Assert.IsTrue(!status.ActionAvailable);
            Assert.AreEqual(MagicInitiateSpell.Bless, status.CurrentSpell);
        }

        #endregion Cast Buff

        #region Action Surge

        private void Test_ActionSurge_HasAction()
        {
            Assert.Throws<ArgumentException>(() => status.ActionSurge());

            Assert.IsTrue(status.ActionSurgeAvailable);
        }

        private void Test_ActionSurge_ActionSurgeUnavailable()
        {
            status.MainHandAttack(enemy);
            status.ActionSurge();

            Assert.Throws<ArgumentException>(() => status.ActionSurge());
            Assert.IsTrue(!status.ActionSurgeAvailable);
        }

        private void Test_ActionSurge_Success()
        {
            status.MainHandAttack(enemy);

            Assert.DoesNotThrow(() => status.ActionSurge());
            Assert.IsTrue(!status.ActionSurgeAvailable);
        }

        #endregion Action Surge

        #region End Turn

        private void Test_EndTurn()
        {
            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            Assert.IsTrue(!status.ActionAvailable);
            Assert.IsTrue(!status.BonusActionAvailable);
            Assert.AreEqual(0, status.NumberOfTurnsPassed);
            Assert.DoesNotThrow(() => status.EndTurn());

            Assert.IsTrue(status.ActionAvailable);
            Assert.IsTrue(status.BonusActionAvailable);
            Assert.AreEqual(1, status.NumberOfTurnsPassed);

            Assert.DoesNotThrow(() => status.MainHandAttack(enemy));
        }

        #endregion End Turn

        #region Main Hand Attack

        private void Test_MainHandAttack_NullArg()
        {
            Assert.Throws<ArgumentNullException>(() => status.MainHandAttack(null));
            Assert.IsTrue(status.ActionAvailable);
        }

        private void Test_MainHandAttack_UsedAction()
        {
            status.MainHandAttack(enemy);

            Assert.Throws<ArgumentException>(() => status.MainHandAttack(enemy));
            Assert.IsTrue(!status.ActionAvailable);
        }

        private void Test_MainHandAttack_GunCannotAttack()
        {
            status = new PlayerStatus(always1, player);
            status.MainHandAttack(enemy);
            status.EndTurn();

            Assert.Throws<ArgumentException>(() => status.MainHandAttack(enemy));   //Gun cannot fire because misfire
            Assert.IsTrue(status.ActionAvailable);
        }

        private void Test_MainHandAttack_SingleAttack_Always1()
        {
            Player newPlayer = new Player(always1, 4, 4, FightingStyle.Archery, mh, ohs, feats, buffs);
            PlayerStatus status = new PlayerStatus(always1, newPlayer);

            bool ret = false;
            Assert.DoesNotThrow(() => ret = status.MainHandAttack(enemy));

            Assert.IsTrue(ret);
            Assert.IsTrue(!status.ActionAvailable);
            Assert.IsTrue(status.BonusActionAvailable);
            Assert.IsTrue(status.OffhandAttackAvailable);
            Assert.IsTrue(!status.HasMisfireReaction);

            Assert.AreEqual(1, status.NumberOfShots);
            Assert.AreEqual(0, status.NumberOfHits);
            Assert.AreEqual(0, enemy.HitsTaken);
            Assert.AreEqual(0, enemy.DamageTaken);

            Assert.AreEqual(GunFiringStatus.Broken, status.MainHand.Status);  //Used reaction for MF (failed)
            Assert.IsTrue(!status.MainHand.CanFire());
            Assert.IsTrue(status.MainHand.HasShotLoaded());
        }

        private void Test_MainHandAttack_SingleAttack_Always10()
        {
            Player newPlayer = new Player(always10, 4, 4, FightingStyle.Archery, mh, ohs, feats, buffs);
            PlayerStatus status = new PlayerStatus(always10, newPlayer);

            bool ret = false;
            Assert.DoesNotThrow(() => ret = status.MainHandAttack(enemy));

            Assert.IsTrue(ret);
            Assert.IsTrue(!status.ActionAvailable);
            Assert.IsTrue(status.BonusActionAvailable);
            Assert.IsTrue(status.OffhandAttackAvailable);
            Assert.IsTrue(status.HasMisfireReaction);

            Assert.AreEqual(1, status.NumberOfShots);
            Assert.AreEqual(1, status.NumberOfHits);
            Assert.AreEqual(1, enemy.HitsTaken);
            Assert.AreEqual(14, enemy.DamageTaken);  //10+4

            Assert.AreEqual(GunFiringStatus.Okay, status.MainHand.Status);
            Assert.IsTrue(status.MainHand.CanFire());
            Assert.IsTrue(status.MainHand.HasShotLoaded());
        }

        private void Test_MainHandAttack_SingleAttack_Always20()
        {
            Player newPlayer = new Player(always20, 4, 4, FightingStyle.Archery, mh, ohs, feats, buffs);
            PlayerStatus status = new PlayerStatus(always20, newPlayer);

            bool ret = false;
            Assert.DoesNotThrow(() => ret = status.MainHandAttack(enemy));

            Assert.IsTrue(ret);
            Assert.IsTrue(!status.ActionAvailable);
            Assert.IsTrue(status.BonusActionAvailable);
            Assert.IsTrue(status.OffhandAttackAvailable);
            Assert.IsTrue(status.HasMisfireReaction);

            Assert.AreEqual(1, status.NumberOfShots);
            Assert.AreEqual(1, status.NumberOfHits);
            Assert.AreEqual(1, enemy.HitsTaken);
            Assert.AreEqual(44, enemy.DamageTaken);  //weird because always 20, but 20*2+4

            Assert.AreEqual(GunFiringStatus.Okay, status.MainHand.Status);
            Assert.IsTrue(status.MainHand.CanFire());
            Assert.IsTrue(status.MainHand.HasShotLoaded());
        }

        private void Test_MainHandAttack_3Atk_Always1()
        {
            Player newPlayer = new Player(always1, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs);
            PlayerStatus status = new PlayerStatus(always1, newPlayer);

            bool ret = false;
            Assert.DoesNotThrow(() => ret = status.MainHandAttack(enemy));

            Assert.IsTrue(ret);
            Assert.IsTrue(!status.ActionAvailable);
            Assert.IsTrue(status.BonusActionAvailable);
            Assert.IsTrue(status.OffhandAttackAvailable);
            Assert.IsTrue(!status.HasMisfireReaction);  //misfire

            Assert.AreEqual(1, status.NumberOfShots);
            Assert.AreEqual(0, status.NumberOfHits);
            Assert.AreEqual(0, enemy.HitsTaken);
            Assert.AreEqual(0, enemy.DamageTaken);

            Assert.AreEqual(GunFiringStatus.Broken, status.MainHand.Status);
            Assert.IsTrue(!status.MainHand.CanFire());
            Assert.IsTrue(status.MainHand.HasShotLoaded());
        }

        private void Test_MainHandAttack_3Atk_Always10()
        {
            Player newPlayer = new Player(always10, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs);
            PlayerStatus status = new PlayerStatus(always10, newPlayer);

            bool ret = false;
            Assert.DoesNotThrow(() => ret = status.MainHandAttack(enemy));

            Assert.IsTrue(ret);
            Assert.IsTrue(!status.ActionAvailable);
            Assert.IsTrue(status.BonusActionAvailable);
            Assert.IsTrue(status.OffhandAttackAvailable);
            Assert.IsTrue(status.HasMisfireReaction);

            Assert.AreEqual(3, status.NumberOfShots);
            Assert.AreEqual(3, status.NumberOfHits);
            Assert.AreEqual(3, enemy.HitsTaken);
            Assert.AreEqual(45, enemy.DamageTaken);  //(10 + 5) * 3

            Assert.AreEqual(GunFiringStatus.Okay, status.MainHand.Status);
            Assert.IsTrue(status.MainHand.CanFire());
            Assert.IsTrue(status.MainHand.HasShotLoaded());
        }

        private void Test_MainHandAttack_3Atk_Always20()
        {
            Player newPlayer = new Player(always20, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs);
            PlayerStatus status = new PlayerStatus(always20, newPlayer);

            bool ret = false;
            Assert.DoesNotThrow(() => ret = status.MainHandAttack(enemy));

            Assert.IsTrue(ret);
            Assert.IsTrue(!status.ActionAvailable);
            Assert.IsTrue(status.BonusActionAvailable);
            Assert.IsTrue(status.OffhandAttackAvailable);
            Assert.IsTrue(status.HasMisfireReaction);

            Assert.AreEqual(3, status.NumberOfShots);
            Assert.AreEqual(3, status.NumberOfHits);
            Assert.AreEqual(3, enemy.HitsTaken);
            Assert.AreEqual(135, enemy.DamageTaken);  //weird bc always20, but ((20 * 2) + 5) * 3

            Assert.AreEqual(GunFiringStatus.Okay, status.MainHand.Status);
            Assert.IsTrue(status.MainHand.CanFire());
            Assert.IsTrue(status.MainHand.HasShotLoaded());
        }

        private void Test_MainHandAttack_3Atk_Reload()
        {
            Player newPlayer = new Player(always10, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs);
            PlayerStatus status = new PlayerStatus(always10, newPlayer);

            bool ret = false;
            Assert.DoesNotThrow(() => ret = status.MainHandAttack(enemy));

            Assert.IsTrue(ret);
            Assert.IsTrue(!status.ActionAvailable);
            Assert.IsTrue(status.BonusActionAvailable);
            Assert.IsTrue(status.OffhandAttackAvailable);
            Assert.IsTrue(status.HasMisfireReaction);

            Assert.AreEqual(3, status.NumberOfShots);
            Assert.AreEqual(3, status.NumberOfHits);
            Assert.AreEqual(3, enemy.HitsTaken);
            Assert.AreEqual(45, enemy.DamageTaken);  //(roll 10 + 5 dex mod) x #attks = (10 + 5) * 3

            Assert.AreEqual(GunFiringStatus.Okay, status.MainHand.Status);
            Assert.IsTrue(status.MainHand.CanFire());
            Assert.IsTrue(status.MainHand.HasShotLoaded());

            //Keep firing, force when it would otherwise reload and make sure it shot when it would have reloaded
            status.EndTurn();
            status.MainHandAttack(enemy);   //Once this is done we've fired 6 times - need to reload.
            status.EndTurn();
            Assert.IsTrue(!status.MainHand.HasShotLoaded());
            status.MainHandAttack(enemy);

            Assert.AreEqual(8, status.NumberOfShots);   //These would be 9 if we didn't have to reload.
            Assert.AreEqual(8, status.NumberOfHits);
            Assert.AreEqual(8, enemy.HitsTaken);
        }

        private void Test_MainHandAttack_2Atk_Artificer()
        {
            Player newPlayer = new Player(always10, 9, 11, FightingStyle.Archery, artificerMh, ohs, feats, buffs);
            PlayerStatus status = new PlayerStatus(always10, newPlayer);

            bool ret = false;
            Assert.DoesNotThrow(() => ret = status.MainHandAttack(enemy));

            Assert.IsTrue(ret);
            Assert.IsTrue(!status.ActionAvailable);
            Assert.IsTrue(status.BonusActionAvailable);
            Assert.IsTrue(status.OffhandAttackAvailable);
            Assert.IsTrue(status.HasMisfireReaction);

            Assert.AreEqual(2, status.NumberOfShots);
            Assert.AreEqual(2, status.NumberOfHits);
            Assert.AreEqual(2, enemy.HitsTaken);
            Assert.AreEqual(32, enemy.DamageTaken);  //(roll 10 + 5 dex mod +1 wep) x #attks = (10 + 5 + 1) * 2

            Assert.AreEqual(GunFiringStatus.Okay, status.MainHand.Status);
            Assert.IsTrue(status.MainHand.CanFire());
            Assert.IsTrue(status.MainHand.HasShotLoaded());

            //Keep firing, force when it would otherwise reload and make sure it shot when it would have reloaded
            status.EndTurn();
            status.MainHandAttack(enemy);
            status.EndTurn();
            status.MainHandAttack(enemy);
            status.EndTurn();
            status.MainHandAttack(enemy);

            Assert.AreEqual(8, status.NumberOfShots);   //These would be 7 if we had reloaded.
            Assert.AreEqual(8, status.NumberOfHits);
            Assert.AreEqual(8, enemy.HitsTaken);
        }

        private void Test_MainHandShoot_Hex()
        {
            Player newPlayer = new Player(always10, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs);
            PlayerStatus status = new PlayerStatus(always10, newPlayer);
            status.CastBuff(MagicInitiateSpell.Hex);

            bool ret = false;
            Assert.DoesNotThrow(() => ret = status.MainHandAttack(enemy));

            Assert.IsTrue(ret);
            Assert.IsTrue(!status.ActionAvailable);
            Assert.IsTrue(!status.BonusActionAvailable);
            Assert.IsTrue(status.OffhandAttackAvailable);
            Assert.IsTrue(status.HasMisfireReaction);

            Assert.AreEqual(3, status.NumberOfShots);
            Assert.AreEqual(3, status.NumberOfHits);
            Assert.AreEqual(3, enemy.HitsTaken);
            Assert.AreEqual(75, enemy.DamageTaken);  //(roll 10 + 5 dex mod + hex) x #attks = (10 + 5 + 10) * 3

            Assert.AreEqual(GunFiringStatus.Okay, status.MainHand.Status);
            Assert.IsTrue(status.MainHand.CanFire());
            Assert.IsTrue(status.MainHand.HasShotLoaded());
        }

        private void Test_MainHandShoot_HexCrit()
        {
            Player newPlayer = new Player(always20, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs);
            PlayerStatus status = new PlayerStatus(always20, newPlayer);
            status.CastBuff(MagicInitiateSpell.Hex);

            bool ret = false;
            Assert.DoesNotThrow(() => ret = status.MainHandAttack(enemy));

            Assert.IsTrue(ret);
            Assert.IsTrue(!status.ActionAvailable);
            Assert.IsTrue(!status.BonusActionAvailable);
            Assert.IsTrue(status.OffhandAttackAvailable);
            Assert.IsTrue(status.HasMisfireReaction);

            Assert.AreEqual(3, status.NumberOfShots);
            Assert.AreEqual(3, status.NumberOfHits);
            Assert.AreEqual(3, enemy.HitsTaken);
            Assert.AreEqual(255, enemy.DamageTaken);  //weird bc always 20: #Atk*(2*(gundie + hex) + dex mod) = 3*(2*(20+20) + 5) = 3*(85) = 255

            Assert.AreEqual(GunFiringStatus.Okay, status.MainHand.Status);
            Assert.IsTrue(status.MainHand.CanFire());
            Assert.IsTrue(status.MainHand.HasShotLoaded());
        }

        private void Test_MainHandShoot_Bless()
        {
            Player newPlayer = new Player(always10, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs);
            PlayerStatus status = new PlayerStatus(always10, newPlayer);

            //weird because we can't mix RNG objs on add, so bless will
            //return 10. Roll 10, at lvl 11, with bless value of 11 will hit a 31.
            //normal: prof + dex mod + arch + bless = 4 + 5 + 2 + bless = 11 + 10 = 21
            Enemy enemy = new Enemy(31);    
            
            status.CastBuff(MagicInitiateSpell.Bless);

            bool ret = false;
            status.ActionSurge();   //bc bless is an action
            Assert.DoesNotThrow(() => ret = status.MainHandAttack(enemy));

            Assert.IsTrue(ret);
            Assert.IsTrue(!status.ActionAvailable);
            Assert.IsTrue(status.BonusActionAvailable);
            Assert.IsTrue(status.OffhandAttackAvailable);
            Assert.IsTrue(status.HasMisfireReaction);

            Assert.AreEqual(3, status.NumberOfShots);
            Assert.AreEqual(3, status.NumberOfHits);
            Assert.AreEqual(3, enemy.HitsTaken);
            Assert.AreEqual(45, enemy.DamageTaken);

            Assert.AreEqual(GunFiringStatus.Okay, status.MainHand.Status);
            Assert.IsTrue(status.MainHand.CanFire());
            Assert.IsTrue(status.MainHand.HasShotLoaded());
        }

        private void Test_MainHandShoot_BlessDoesNotEffectMisfire()
        {
            Rng always2 = new Rng(2);   //note 2 misfires a pepperbox
            Player newPlayer = new Player(always2, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs);
            PlayerStatus status = new PlayerStatus(always2, newPlayer);

            //Want to make sure that even if we would normally hit, a misfire is a guaranteed miss.
            //normal: prof + dex mod + arch + bless = 4 + 5 + 2 + bless = 11 + 1 = 12
            Enemy enemy = new Enemy(10);    
            
            status.CastBuff(MagicInitiateSpell.Bless);

            bool ret = false;
            status.ActionSurge();   //bc bless is an action
            Assert.DoesNotThrow(() => ret = status.MainHandAttack(enemy));

            Assert.IsTrue(ret);
            Assert.IsTrue(!status.ActionAvailable);
            Assert.IsTrue(status.BonusActionAvailable);
            Assert.IsTrue(status.OffhandAttackAvailable);
            Assert.IsTrue(!status.HasMisfireReaction);

            Assert.AreEqual(1, status.NumberOfShots);
            Assert.AreEqual(0, status.NumberOfHits);
            Assert.AreEqual(0, enemy.HitsTaken);
            Assert.AreEqual(0, enemy.DamageTaken);

            Assert.AreEqual(GunFiringStatus.Broken, status.MainHand.Status);
            Assert.IsTrue(!status.MainHand.CanFire());
            Assert.IsTrue(status.MainHand.HasShotLoaded());
        }

        private void Test_MainHandShoot_SharpshooterMiss()
        {
            List<Feat> feats = new List<Feat>() { Feat.Sharpshooter };
            Player newPlayer = new Player(always10, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs);
            PlayerStatus status = new PlayerStatus(always10, newPlayer);
            Enemy enemy = new Enemy(17);  //prof + dex mod + arch + roll + feat=4+5+2+10-5=16

            bool ret = false;
            Assert.DoesNotThrow(() => ret = status.MainHandAttack(enemy));

            Assert.IsTrue(ret);
            Assert.IsTrue(!status.ActionAvailable);
            Assert.IsTrue(status.BonusActionAvailable);
            Assert.IsTrue(status.OffhandAttackAvailable);
            Assert.IsTrue(status.HasMisfireReaction);

            Assert.AreEqual(3, status.NumberOfShots);
            Assert.AreEqual(0, status.NumberOfHits);
            Assert.AreEqual(0, enemy.HitsTaken);
            Assert.AreEqual(0, enemy.DamageTaken);

            Assert.AreEqual(GunFiringStatus.Okay, status.MainHand.Status);
            Assert.IsTrue(status.MainHand.CanFire());
            Assert.IsTrue(status.MainHand.HasShotLoaded());
        }

        private void Test_MainHandShoot_SharpshooterHit()
        {
            List<Feat> feats = new List<Feat>() { Feat.Sharpshooter };
            Player newPlayer = new Player(always10, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs);
            PlayerStatus status = new PlayerStatus(always10, newPlayer);
            Enemy enemy = new Enemy(16);  //prof + dex mod + arch + roll + feat=4+5+2+10-5=16

            bool ret = false;
            Assert.DoesNotThrow(() => ret = status.MainHandAttack(enemy));

            Assert.IsTrue(ret);
            Assert.IsTrue(!status.ActionAvailable);
            Assert.IsTrue(status.BonusActionAvailable);
            Assert.IsTrue(status.OffhandAttackAvailable);
            Assert.IsTrue(status.HasMisfireReaction);

            Assert.AreEqual(3, status.NumberOfShots);
            Assert.AreEqual(3, status.NumberOfHits);
            Assert.AreEqual(3, enemy.HitsTaken);
            Assert.AreEqual(75, enemy.DamageTaken);  //3*(10+5+10)=75

            Assert.AreEqual(GunFiringStatus.Okay, status.MainHand.Status);
            Assert.IsTrue(status.MainHand.CanFire());
            Assert.IsTrue(status.MainHand.HasShotLoaded());
        }

        private void Test_MainHandShoot_SharpshooterCrit()
        {
            List<Feat> feats = new List<Feat>() { Feat.Sharpshooter };
            Player newPlayer = new Player(always20, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs);
            PlayerStatus status = new PlayerStatus(always20, newPlayer);

            bool ret = false;
            Assert.DoesNotThrow(() => ret = status.MainHandAttack(enemy));

            Assert.IsTrue(ret);
            Assert.IsTrue(!status.ActionAvailable);
            Assert.IsTrue(status.BonusActionAvailable);
            Assert.IsTrue(status.OffhandAttackAvailable);
            Assert.IsTrue(status.HasMisfireReaction);

            Assert.AreEqual(3, status.NumberOfShots);
            Assert.AreEqual(3, status.NumberOfHits);
            Assert.AreEqual(3, enemy.HitsTaken);
            Assert.AreEqual(165, enemy.DamageTaken);  //3*(2*20+5+10)=3*55=165

            Assert.AreEqual(GunFiringStatus.Okay, status.MainHand.Status);
            Assert.IsTrue(status.MainHand.CanFire());
            Assert.IsTrue(status.MainHand.HasShotLoaded());
        }

        #endregion Main Hand Attack

        #region Off Hand Attack

        private void Test_OffHandAttack_NullArg()
        {
            status.MainHandAttack(enemy);
            Assert.Throws<ArgumentNullException>(() => status.OffHandAttack(null));

            Assert.IsTrue(status.BonusActionAvailable);
        }

        private void Test_OffHandAttack_BonusActionNotAvailable()
        {
            status.MainHandAttack(enemy);
            status.CastBuff(MagicInitiateSpell.Hex);
            Assert.Throws<ArgumentException>(() => status.OffHandAttack(enemy));
        }

        private void Test_OffHandAttack_CantUseOhAtk()
        {
            Assert.Throws<ArgumentException>(() => status.OffHandAttack(enemy));

            Assert.IsTrue(status.BonusActionAvailable);
        }

        private void Test_OffHandAttack_OhNoAmmo()
        {
            Player newPlayer = new Player(always10, 11, 11, FightingStyle.Archery, mh, new List<IGun>() { gunFactory.Get(GunType.PalmPistol) }, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(always10, newPlayer);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);
            status.EndTurn();
            status.MainHandAttack(enemy);

            Assert.Throws<ArgumentException>(() => status.OffHandAttack(enemy));    //Out of ammo
            Assert.IsTrue(status.BonusActionAvailable);
        }

        private void Test_OffHandAttack_Miss()
        {
            Rng always2 = new Rng(2);
            Player newPlayer = new Player(always2, 11, 11, FightingStyle.Archery, mh, new List<IGun>() { gunFactory.Get(GunType.PalmPistol) }, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(always2, newPlayer);
            Enemy enemy = new Enemy(20);

            status.MainHandAttack(enemy);
            Assert.DoesNotThrow(() => status.OffHandAttack(enemy));

            Assert.IsTrue(!status.BonusActionAvailable);
            Assert.IsTrue(!status.OffhandAttackAvailable);

            Assert.AreEqual(2, status.NumberOfShots);
            Assert.AreEqual(0, status.NumberOfHits);
            Assert.AreEqual(0, enemy.HitsTaken);
            Assert.AreEqual(0, enemy.DamageTaken);

            Assert.AreEqual(GunFiringStatus.Okay, status.CurrentOffHand.Status);
            Assert.IsTrue(status.CurrentOffHand.CanFire());
            Assert.IsTrue(!status.CurrentOffHand.HasShotLoaded());
        }

        private void Test_OffHandAttack_Hit()
        {
            Player newPlayer = new Player(always10, 11, 11, FightingStyle.Archery, mh, new List<IGun>() { gunFactory.Get(GunType.PalmPistol) }, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(always10, newPlayer);

            status.MainHandAttack(enemy);
            Assert.DoesNotThrow(() => status.OffHandAttack(enemy));

            Assert.IsTrue(!status.BonusActionAvailable);
            Assert.IsTrue(!status.OffhandAttackAvailable);

            Assert.AreEqual(4, status.NumberOfShots);
            Assert.AreEqual(4, status.NumberOfHits);
            Assert.AreEqual(4, enemy.HitsTaken);
            Assert.AreEqual(55, enemy.DamageTaken);  //MH: 3*(10 + 5) + OH: 10 = 55

            Assert.AreEqual(GunFiringStatus.Okay, status.CurrentOffHand.Status);
            Assert.IsTrue(status.CurrentOffHand.CanFire());
            Assert.IsTrue(!status.CurrentOffHand.HasShotLoaded());
        }

        private void Test_OffHandAttack_Crit()
        {
            Player newPlayer = new Player(always20, 11, 11, FightingStyle.Archery, mh, new List<IGun>() { gunFactory.Get(GunType.PalmPistol) }, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(always20, newPlayer);

            status.MainHandAttack(enemy);
            Assert.DoesNotThrow(() => status.OffHandAttack(enemy));

            Assert.IsTrue(!status.BonusActionAvailable);
            Assert.IsTrue(!status.OffhandAttackAvailable);

            Assert.AreEqual(4, status.NumberOfShots);
            Assert.AreEqual(4, status.NumberOfHits);
            Assert.AreEqual(4, enemy.HitsTaken);
            Assert.AreEqual(175, enemy.DamageTaken);  //MH: 3*((2*20) + 5) + OH: (2*20) = 135+40=175

            Assert.AreEqual(GunFiringStatus.Okay, status.CurrentOffHand.Status);
            Assert.IsTrue(status.CurrentOffHand.CanFire());
            Assert.IsTrue(!status.CurrentOffHand.HasShotLoaded());
        }

        private void Test_OffHandAttack_Hex()
        {
            Player newPlayer = new Player(always10, 11, 11, FightingStyle.Archery, mh, new List<IGun>() { gunFactory.Get(GunType.PalmPistol) }, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(always10, newPlayer);

            status.CastBuff(MagicInitiateSpell.Hex);
            status.EndTurn();

            status.MainHandAttack(enemy);
            Assert.DoesNotThrow(() => status.OffHandAttack(enemy));

            Assert.IsTrue(!status.BonusActionAvailable);
            Assert.IsTrue(!status.OffhandAttackAvailable);

            Assert.AreEqual(4, status.NumberOfShots);
            Assert.AreEqual(4, status.NumberOfHits);
            Assert.AreEqual(4, enemy.HitsTaken);
            Assert.AreEqual(95, enemy.DamageTaken);  //hex = 10->MH: 3*(10 + 5 + 10) + OH: (10 + 10) = 75+20=95

            Assert.AreEqual(GunFiringStatus.Okay, status.CurrentOffHand.Status);
            Assert.IsTrue(status.CurrentOffHand.CanFire());
            Assert.IsTrue(!status.CurrentOffHand.HasShotLoaded());
        }

        private void Test_OffHandAttack_HexCrit()
        {
            Player newPlayer = new Player(always20, 11, 11, FightingStyle.Archery, mh, new List<IGun>() { gunFactory.Get(GunType.PalmPistol) }, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(always20, newPlayer);

            status.CastBuff(MagicInitiateSpell.Hex);
            status.EndTurn();

            status.MainHandAttack(enemy);
            Assert.DoesNotThrow(() => status.OffHandAttack(enemy));

            Assert.IsTrue(!status.BonusActionAvailable);
            Assert.IsTrue(!status.OffhandAttackAvailable);

            Assert.AreEqual(4, status.NumberOfShots);
            Assert.AreEqual(4, status.NumberOfHits);
            Assert.AreEqual(4, enemy.HitsTaken);
            Assert.AreEqual(335, enemy.DamageTaken);  //hex = 20->MH: 3*(2*20 + 5 + 2*20) + OH: (2*20 + 2*20) = 3*85+80=335

            Assert.AreEqual(GunFiringStatus.Okay, status.CurrentOffHand.Status);
            Assert.IsTrue(status.CurrentOffHand.CanFire());
            Assert.IsTrue(!status.CurrentOffHand.HasShotLoaded());
        }

        private void Test_OffHandAttack_Bless()
        {
            Player newPlayer = new Player(always10, 11, 11, FightingStyle.Archery, mh, new List<IGun>() { gunFactory.Get(GunType.PalmPistol) }, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(always10, newPlayer);
            Enemy enemy = new Enemy(31);    //prof + arc + dex + bless + roll = 4+2+5+10+10=21

            status.CastBuff(MagicInitiateSpell.Bless);
            status.EndTurn();

            status.MainHandAttack(enemy);
            Assert.DoesNotThrow(() => status.OffHandAttack(enemy));

            Assert.IsTrue(!status.BonusActionAvailable);
            Assert.IsTrue(!status.OffhandAttackAvailable);

            Assert.AreEqual(4, status.NumberOfShots);
            Assert.AreEqual(4, status.NumberOfHits);
            Assert.AreEqual(4, enemy.HitsTaken);
            Assert.AreEqual(55, enemy.DamageTaken);  //hex = 10->MH: 3*(10 + 5) + OH: (10) = 45+10

            Assert.AreEqual(GunFiringStatus.Okay, status.CurrentOffHand.Status);
            Assert.IsTrue(status.CurrentOffHand.CanFire());
            Assert.IsTrue(!status.CurrentOffHand.HasShotLoaded());
        }

        private void Test_OffHandAttack_ArtificerModifiers()
        {
            Player newPlayer = new Player(always10, 11, 11, FightingStyle.Archery, mh, new List<IGun>() { gunFactory.Get(GunType.PalmPistol, WeaponTier.ArtificerReloadProperty) }, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(always10, newPlayer);
            Enemy enemy = new Enemy(22);    //prof + arc + dex + roll + artfc mod = 4+2+5+10+1=22

            status.MainHandAttack(enemy);
            Assert.DoesNotThrow(() => status.OffHandAttack(enemy));

            Assert.IsTrue(!status.BonusActionAvailable);
            Assert.IsTrue(!status.OffhandAttackAvailable);

            Assert.AreEqual(4, status.NumberOfShots);
            Assert.AreEqual(1, status.NumberOfHits);
            Assert.AreEqual(1, enemy.HitsTaken);
            Assert.AreEqual(11, enemy.DamageTaken);  //only OH will hit

            Assert.AreEqual(GunFiringStatus.Okay, status.CurrentOffHand.Status);
            Assert.IsTrue(status.CurrentOffHand.CanFire());
            Assert.IsTrue(status.CurrentOffHand.HasShotLoaded());
        }

        #endregion Off Hand Attack

        #region Fix Main Hand Misfire

        private void Test_FixMainHandMisifre_ActionNotAvail()
        {
            status.CastBuff(MagicInitiateSpell.Bless);
            Assert.Throws<ArgumentException>(() => status.FixMainHandMisfire());
        }

        private void Test_FixMainHandFireMisfire_GunNotMisfired()
        {
            Player newPlayer = new Player(always1, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(always1, newPlayer);

            Assert.Throws<ArgumentException>(() => status.FixMainHandMisfire());
            Assert.IsTrue(status.ActionAvailable);

            status.MainHandAttack(enemy);   //will misfire, try to fix, fail -> broke
            status.EndTurn();
            Assert.Throws<ArgumentException>(() => status.FixMainHandMisfire());
        }

        #endregion Fix Main Hand Misfire

        #region Can Swap Off Hand

        private void Test_CanSwapOffHand_NoOh()
        {
            Player newPlayer = new Player(always10, 11, 11, FightingStyle.Archery, mh, new List<IGun>(), feats, buffs);
            PlayerStatus status = new PlayerStatus(always10, newPlayer);

            bool ret = true;
            Assert.DoesNotThrow(() => ret = status.CanSwapOffHand());

            Assert.IsTrue(!ret);
        }

        private void Test_CanSwapOffHand_OneOh()
        {
            List<IGun> singleOh = new List<IGun>() { gunFactory.Get(GunType.PalmPistol) };
            Player newPlayer = new Player(always10, 11, 11, FightingStyle.Archery, mh, singleOh, feats, buffs);
            PlayerStatus status = new PlayerStatus(always10, newPlayer);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            bool ret = true;
            Assert.DoesNotThrow(() => ret = status.CanSwapOffHand());

            Assert.IsTrue(!ret);
        }

        private void Test_CanSwapOffHand_CanSwap()
        {
            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);
            status.EndTurn();

            bool ret = false;
            Assert.DoesNotThrow(() => ret = status.CanSwapOffHand());

            Assert.IsTrue(ret);
        }

        private void Test_CanSwapOffHand_NoShotLoaded()
        {
            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);
            status.EndTurn();
            status.MainHandAttack(enemy);
            status.SwapOffHand();
            status.OffHandAttack(enemy);
            status.EndTurn();

            bool ret = true;
            Assert.DoesNotThrow(() => ret = status.CanSwapOffHand());

            Assert.IsTrue(!ret);
        }

        #endregion Can Swap Off Hand

        #region Swap Off Hand

        private void Test_SwapOffHand_NoOh()
        {
            Player newPlayer = new Player(always10, 11, 11, FightingStyle.Archery, mh, new List<IGun>(), feats, buffs);
            PlayerStatus status = new PlayerStatus(always10, newPlayer);

            Assert.Throws<ArgumentException>(() => status.SwapOffHand());
            Assert.IsTrue(status.CurrentOffHand == null);
        }

        private void Test_SwapOffHand_OneOh()
        {
            List<IGun> singleOh = new List<IGun>() { gunFactory.Get(GunType.PalmPistol) };
            Player newPlayer = new Player(always10, 11, 11, FightingStyle.Archery, mh, singleOh, feats, buffs);
            PlayerStatus status = new PlayerStatus(always10, newPlayer);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            Assert.Throws<ArgumentException>(() => status.SwapOffHand());
            Assert.IsTrue(!status.CurrentOffHand.HasShotLoaded());
        }

        private void Test_SwapOffHand_TwoOhs()
        {
            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);
            bool ret = false;
            Assert.DoesNotThrow(() => ret = status.SwapOffHand());
            Assert.IsTrue(ret);
            status.EndTurn();

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            Assert.Throws<ArgumentException>(() => status.SwapOffHand());
            Assert.IsTrue(!status.CurrentOffHand.HasShotLoaded());
        }

        private void Test_SwapOffHand_ArtificerOhs()
        {
            List<IGun> artificerOhs = new List<IGun>()
            {
                gunFactory.Get(GunType.PalmPistol, WeaponTier.ArtificerReloadProperty),
                gunFactory.Get(GunType.PalmPistol)
            };

            Player newPlayer = new Player(always10, 11, 11, FightingStyle.Archery, mh, artificerOhs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(always10, newPlayer);

            //keep firing, never swap
            bool ret;
            for (int i = 0; i < 10; i++)
            {
                status.MainHandAttack(enemy);
                status.OffHandAttack(enemy);

                ret = true;
                Assert.DoesNotThrow(() => ret = status.CanSwapOffHand());
                Assert.IsTrue(!ret);

                Assert.Throws<ArgumentException>(() => status.SwapOffHand());
                Assert.IsTrue(status.CurrentOffHand.HasShotLoaded());

                status.EndTurn();
            }
        }

        private void Test_SwapOffHand_ArtificerOhsMisfire()
        {
            List<IGun> artificerOhs = new List<IGun>()
            {
                gunFactory.Get(GunType.PalmPistol, WeaponTier.ArtificerReloadProperty),
                gunFactory.Get(GunType.PalmPistol)
            };

            Player newPlayer = new Player(always1, 11, 11, FightingStyle.Archery, mh, artificerOhs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(always1, newPlayer);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            bool ret = false;
            Assert.DoesNotThrow(() => ret = status.CanSwapOffHand());
            Assert.IsTrue(ret);

            Assert.IsTrue(!status.CurrentOffHand.CanFire());
            Assert.DoesNotThrow(() => status.SwapOffHand());
            Assert.IsTrue(status.CurrentOffHand.CanFire());
        }

        #endregion Swap Off Hand

        #region Uses Mock RNG Sequence
        //Misc tests that require a specific sequence to be able to trigger.

        private void Test_FixMainHandMisfire_Fails()
        {
            //fire (misfire) (1) -> fix on reaction (10) -> attack again (misfire) (1) -> end turn -> Fix (fail) (5)
            List<int> misfireSequence = new List<int>() { 1, 10, 1, 5 };
            Rng misfireRng = new MockRng(misfireSequence);

            Player newPlayer = new Player(misfireRng, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(misfireRng, newPlayer);

            status.MainHandAttack(enemy);
            status.EndTurn();

            bool ret = true;
            Assert.DoesNotThrow(() => ret = status.FixMainHandMisfire());
            Assert.IsTrue(!ret);
            Assert.AreEqual(GunFiringStatus.Broken, status.MainHand.Status);
        }

        private void Test_FixMainHandMisfire_Succeeds()
        {
            //fire (misfire) (1) -> fix on reaction (10) -> attack again (misfire) (1) -> end turn -> Fix (succeed) (6)
            List<int> misfireSequence = new List<int>() { 1, 10, 1, 6 };
            Rng misfireRng = new MockRng(misfireSequence);

            Player newPlayer = new Player(misfireRng, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(misfireRng, newPlayer);

            status.MainHandAttack(enemy);
            status.EndTurn();

            bool ret = false;
            Assert.DoesNotThrow(() => ret = status.FixMainHandMisfire());
            Assert.IsTrue(ret);
            Assert.AreEqual(GunFiringStatus.Okay, status.MainHand.Status);
        }

        #endregion Uses Mock RNG Sequence

        #region Shots Lost To Misfire

        private void Test_ShotsLostToMisfire_1Atk_MissNoMf()
        {
            Rng always3 = new Rng(3);
            Player newPlayer = new Player(always3, 4, 4, FightingStyle.Archery, mh, ohs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(always3, newPlayer);
            Enemy enemy = new Enemy(25);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            Assert.AreEqual(0, status.NumberOfShotsLostToMisfire);
        }

        private void Test_ShotsLostToMisfire_1Atk_MhMf()
        {
            //MH: atk (1) -> fix (15); OH: atk (3)
            List<int> misfireSequence = new List<int>() { 1, 15, 3 };
            Rng misfireRng = new MockRng(misfireSequence);
            Player newPlayer = new Player(misfireRng, 4, 4, FightingStyle.Archery, mh, ohs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(misfireRng, newPlayer);
            Enemy enemy = new Enemy(25);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            Assert.AreEqual(1, status.NumberOfShotsLostToMisfire);
        }

        private void Test_ShotsLostToMisfire_1Atk_OhMf()
        {
            //MH: atk (10) dmg (1); OH: atk (1) (MF)
            List<int> misfireSequence = new List<int>() { 10, 1, 1 };
            Rng misfireRng = new MockRng(misfireSequence);
            Player newPlayer = new Player(misfireRng, 4, 4, FightingStyle.Archery, mh, ohs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(misfireRng, newPlayer);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            Assert.AreEqual(1, status.NumberOfShotsLostToMisfire);
        }

        private void Test_ShotsLostToMisfire_1Atk_BothMf()
        {
            //MH: atk (1) (MF), Fix (10); OH: atk (1) (MF)
            List<int> misfireSequence = new List<int>() { 1, 10, 1 };
            Rng misfireRng = new MockRng(misfireSequence);
            Player newPlayer = new Player(misfireRng, 4, 4, FightingStyle.Archery, mh, ohs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(misfireRng, newPlayer);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            Assert.AreEqual(2, status.NumberOfShotsLostToMisfire);
        }

        private void Test_ShotsLostToMisfire_2Atk_NoMf()
        {
            Rng always3 = new Rng(3);
            Player newPlayer = new Player(always3, 10, 10, FightingStyle.Archery, mh, ohs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(always3, newPlayer);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            Assert.AreEqual(0, status.NumberOfShotsLostToMisfire);
        }

        private void Test_ShotsLostToMisfire_2Atk_MhHitMf()
        {
            //MH: atk (10) dmg (1), MF (1), fix (15); OH: hit (10)
            List<int> misfireSequence = new List<int>() { 10, 1, 1, 15, 10 };
            Rng misfireRng = new MockRng(misfireSequence);
            Player newPlayer = new Player(misfireRng, 10, 10, FightingStyle.Archery, mh, ohs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(misfireRng, newPlayer);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            Assert.AreEqual(1, status.NumberOfShotsLostToMisfire);
        }

        //2 attack - MH: MF, Shot lost
        private void Test_ShotsLostToMisfire_2Atk_MhMfFixHit()
        {
            //MH: atk (MF) 1, fix (15), hit (10), dmg (1); OH: hit (10)
            List<int> misfireSequence = new List<int>() { 1, 15, 10, 1, 10 };
            Rng misfireRng = new MockRng(misfireSequence);
            Player newPlayer = new Player(misfireRng, 10, 10, FightingStyle.Archery, mh, ohs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(misfireRng, newPlayer);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            Assert.AreEqual(1, status.NumberOfShotsLostToMisfire);
        }

        private void Test_ShotsLostToMisfire_2Atk_MhHitMh()
        {
            //MH: atk hit (10) dmg (1), atk MF (1), fix (10); OH: hit (10)
            List<int> misfireSequence = new List<int>() { 10, 1, 1, 10 };
            Rng misfireRng = new MockRng(misfireSequence);
            Player newPlayer = new Player(misfireRng, 10, 10, FightingStyle.Archery, mh, ohs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(misfireRng, newPlayer);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            Assert.AreEqual(1, status.NumberOfShotsLostToMisfire);
        }

        private void Test_ShotsLostToMisfire_2Atk_OhMf()
        {
            //MH: atk hit (10) dmg (1), atk hit (10) dmg (1); OH: atk (MF) (1)
            List<int> misfireSequence = new List<int>() { 10, 1, 10, 1, 1 };
            Rng misfireRng = new MockRng(misfireSequence);
            Player newPlayer = new Player(misfireRng, 10, 10, FightingStyle.Archery, mh, ohs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(misfireRng, newPlayer);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            Assert.AreEqual(1, status.NumberOfShotsLostToMisfire);
        }
        
        private void Test_ShotsLostToMisfire_2Atk_AllMf()
        {
            //MH: atk MF (1), fix fail (1), shot lost; OH: atk MF (1)
            Player newPlayer = new Player(always1, 10, 10, FightingStyle.Archery, mh, ohs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(always1, newPlayer);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            Assert.AreEqual(3, status.NumberOfShotsLostToMisfire);
        }

        private void Test_ShotsLostToMisfire_3Atk_MhMissNoMf()
        {
            Rng always3 = new Rng(3);
            Player newPlayer = new Player(always3, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(always3, newPlayer);
            Enemy enemy = new Enemy(25);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            Assert.AreEqual(0, status.NumberOfShotsLostToMisfire);
        }

        private void Test_ShotsLostToMisfire_3Atk_MhHitHitMf()
        {
            //MH: atk hit (10) dmg (1), atk hit (10) dmg (1), atk MF (1), fix (10); OH: hit (10)
            List<int> misfireSequence = new List<int>() { 10, 1, 10, 1, 1, 10, 10 };
            Rng misfireRng = new MockRng(misfireSequence);
            Player newPlayer = new Player(misfireRng, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(misfireRng, newPlayer);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            Assert.AreEqual(1, status.NumberOfShotsLostToMisfire);
        }

        private void Test_ShotsLostToMisfire_3Atk_MhHitMfHit()
        {
            //MH: atk hit (10) dmg (1), atk (MF) (1), fix (10), hit (10) dmg (1): OH hit (10)
            List<int> misfireSequence = new List<int>() { 10, 1, 1, 10, 10, 1, 10 };
            Rng misfireRng = new MockRng(misfireSequence);
            Player newPlayer = new Player(misfireRng, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(misfireRng, newPlayer);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            Assert.AreEqual(1, status.NumberOfShotsLostToMisfire);
        }

        private void Test_ShotsLostToMisfire_3Atk_MfMfLost()
        {
            //MH: atk (MF) (1), Fix (10), atk (MF) (1), shot lost: OH: hit (10)
            List<int> misfireSequence = new List<int>() { 1, 10, 1, 10 };
            Rng misfireRng = new MockRng(misfireSequence);
            Player newPlayer = new Player(misfireRng, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(misfireRng, newPlayer);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            Assert.AreEqual(2, status.NumberOfShotsLostToMisfire);
        }

        private void Test_ShotsLostToMisfire_3Atk_OhMf()
        {
            //MH: atk hit (10) dmg (1), atk hit (10) dmg (1), atk hit (10) dmg (1); OH: atk (MF) (1)
            List<int> misfireSequence = new List<int>() { 10, 1, 10, 1, 10, 1, 1 };
            Rng misfireRng = new MockRng(misfireSequence);
            Player newPlayer = new Player(misfireRng, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(misfireRng, newPlayer);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            Assert.AreEqual(1, status.NumberOfShotsLostToMisfire);
        }

        private void Test_ShotsLostToMisfire_3Atk_AllMf()
        {
            Player newPlayer = new Player(always1, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(always1, newPlayer);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            Assert.AreEqual(4, status.NumberOfShotsLostToMisfire);
        }

        private void Test_ShotsLostToMisfire_FixMh()
        {
            //MH: atk (MF) (1), Fix Success (15), atk (MF) (1); OH: Hit (10); Fix MF (20), no OH; MH: hit (10), dmg (1), MF (1); OH: MF (1)
            List<int> misfireSequence = new List<int>() { 1, 15, 1, 10, 20, 10, 1, 1, 1};
            Rng misfireRng = new MockRng(misfireSequence);
            Player newPlayer = new Player(misfireRng, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs); ;
            PlayerStatus status = new PlayerStatus(misfireRng, newPlayer);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);
            status.EndTurn();
            status.FixMainHandMisfire();
            status.CurrentOffHand.ReloadChamber();
            status.EndTurn();
            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);

            //3 shots MH, 3 shots, 2 MH MF, OH MF = 3 + 3 + 2 + 1 = 9
            Assert.AreEqual(9, status.NumberOfShotsLostToMisfire);
        }

        #endregion Shots Lost To Misfire

    }
}
