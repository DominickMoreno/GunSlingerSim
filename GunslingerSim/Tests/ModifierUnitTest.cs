using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Common.Modifier;
using GunslingerSim.Common.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Tests
{
    public class ModifierUnitTest : BaseUnitTest
    {
        #region Setup

        private Rng rng;

        private List<RollType> noRoll;
        private List<RollType> rolld4;
        private List<RollType> rolld6;
        private List<RollType> rolld8;
        private List<RollType> rolld10;
        private List<RollType> rolld12;
        private List<RollType> rolld20;

        protected override void OneTimeSetup()
        {
            rng = new Rng();

            noRoll = new List<RollType>() { RollType.None };
            rolld4 = new List<RollType>() { RollType.d4 };
            rolld6 = new List<RollType>() { RollType.d6 };
            rolld8 = new List<RollType>() { RollType.d8 };
            rolld10 = new List<RollType>() { RollType.d10 };
            rolld12 = new List<RollType>() { RollType.d12 };
            rolld20 = new List<RollType>() { RollType.d20 };
        }

        public ModifierUnitTest()
        {
            AddTest(nameof(Test_Constructor_NullRngForRoll), Test_Constructor_NullRngForRoll);
            AddTest(nameof(Test_Constructor_InvalidRollEnumForRoll), Test_Constructor_InvalidRollEnumForRoll);
            AddTest(nameof(Test_Constructor_SunnyDay), Test_Constructor_SunnyDay);

            AddTest(nameof(Test_Get_BaseModNoRoll), Test_Get_BaseModNoRoll);
            AddTest(nameof(Test_Get_HasRollOneRoll), Test_Get_HasRollOneRoll);
            AddTest(nameof(Test_Get_HasMultipleRolls), Test_Get_HasMultipleRolls);

            AddTest(nameof(Test_GetCrit), Test_GetCrit);

            AddTest(nameof(Test_Add_NullArg), Test_Add_NullArg);
            AddTest(nameof(Test_Add_SingleAddNoValues), Test_Add_SingleAddNoValues);
            AddTest(nameof(Test_Add_SingleAddHasRollNoMod), Test_Add_SingleAddHasRollNoMod);
            AddTest(nameof(Test_Add_SingleAddNoRollHasMod), Test_Add_SingleAddNoRollHasMod);
            AddTest(nameof(Test_Add_SingleAddHasRollAndMod), Test_Add_SingleAddHasRollAndMod);
            AddTest(nameof(Test_Add_SingleAddMultlDice), Test_Add_SingleAddMultlDice);
            AddTest(nameof(Test_Add_ChainAdd), Test_Add_ChainAdd);
        }

        #endregion

        #region Constructor

        private void Test_Constructor_NullRngForRoll()
        {
            Assert.Throws<ArgumentNullException>(() => new Modifier(null, rolld10, 0));
        }

        private void Test_Constructor_InvalidRollEnumForRoll()
        {
            List<RollType> invalidRolls = new List<RollType>() { (RollType)444 };
            Assert.Throws<ArgumentOutOfRangeException>(() => new Modifier(rng, invalidRolls, 0));
        }

        private void Test_Constructor_SunnyDay()
        {
            Assert.DoesNotThrow(() => new Modifier());

            Assert.DoesNotThrow(() => new Modifier(1));
            Assert.DoesNotThrow(() => new Modifier(1));

            Assert.DoesNotThrow(() => new Modifier(rng, noRoll, 2));
            Assert.DoesNotThrow(() => new Modifier(rng, rolld4, 2));
            Assert.DoesNotThrow(() => new Modifier(rng, rolld6, 2));
            Assert.DoesNotThrow(() => new Modifier(rng, rolld8, 2));
            Assert.DoesNotThrow(() => new Modifier(rng, rolld10, 2));
            Assert.DoesNotThrow(() => new Modifier(rng, rolld12, 2));
            Assert.DoesNotThrow(() => new Modifier(rng, rolld20, 2));
        }

        #endregion Constructor

        #region Get

        private void Test_Get_BaseModNoRoll()
        {
            var mod0x = new Modifier();
            var mod0y = new Modifier(0);
            var mod1 = new Modifier(1);
            var mod2 = new Modifier(2);
            var mod11 = new Modifier(11);

            Assert.AreEqual(0, mod0x.Get());
            Assert.AreEqual(0, mod0y.Get());
            Assert.AreEqual(1, mod1.Get());
            Assert.AreEqual(2, mod2.Get());
            Assert.AreEqual(11, mod11.Get());
        }

        private void Test_Get_HasRollOneRoll()
        {
            var mod20 = new Modifier(always20, rolld20, 0);
            var mod22 = new Modifier(always20, rolld20, 2);
            var mod10 = new Modifier(always10, rolld12, 0);
            var mod11 = new Modifier(always10, rolld12, 1);
            var mod1 = new Modifier(always1, rolld8, 0);
            var mod4 = new Modifier(always1, rolld8, 3);

            Assert.AreEqual(20, mod20.Get());
            Assert.AreEqual(22, mod22.Get());
            Assert.AreEqual(10, mod10.Get());
            Assert.AreEqual(11, mod11.Get());
            Assert.AreEqual(1, mod1.Get());
            Assert.AreEqual(4, mod4.Get());
        }

        private void Test_Get_HasMultipleRolls()
        {
            var mod40 = new Modifier(always20, new List<RollType>() { RollType.d20, RollType.d20 }, 0);
            var mod62 = new Modifier(always20, new List<RollType>() { RollType.d20, RollType.d20, RollType.d20 }, 2);

            Assert.AreEqual(40, mod40.Get());
            Assert.AreEqual(62, mod62.Get());
        }

        #endregion Get

        #region Get Crit

        private void Test_GetCrit()
        {
            var mod0x = new Modifier();
            var mod0y = new Modifier(0);
            var mod1 = new Modifier(1);
            var mod3x = new Modifier(always1, rolld4, 1);
            var mod42x = new Modifier(always20, rolld20, 2);
            var mod3y = new Modifier(always1, rolld20, 1);
            var mod42y = new Modifier(always10, new List<RollType>() { RollType.d20, RollType.d20 }, 2);
            var mod123 = new Modifier(always20, new List<RollType>() { RollType.d20, RollType.d20, RollType.d20 }, 3);

            Assert.AreEqual(0, mod0x.GetCrit());
            Assert.AreEqual(0, mod0y.GetCrit());
            Assert.AreEqual(1, mod1.GetCrit());
            Assert.AreEqual(3, mod3x.GetCrit());
            Assert.AreEqual(42, mod42x.GetCrit());
            Assert.AreEqual(3, mod3y.GetCrit());
            Assert.AreEqual(42, mod42y.GetCrit());
            Assert.AreEqual(123, mod123.GetCrit());
        }

        #endregion Get Crit

        #region Add

        private void Test_Add_NullArg()
        {
            IModifier mod = new Modifier();
            Assert.Throws<ArgumentNullException>(() => mod.Add(null));
        }

        private void Test_Add_SingleAddNoValues()
        {
            var testMod = new Modifier();
            var mod0 = new Modifier();

            Assert.DoesNotThrow(() => testMod.Add(mod0));

            Assert.AreEqual(0, testMod.Get());
            Assert.AreEqual(0, testMod.GetCrit());
        }

        private void Test_Add_SingleAddHasRollNoMod()
        {
            var testMod = new Modifier(always10, rolld20, 0);
            var mod1 = new Modifier(always10, rolld12, 0);

            IModifier res = null;
            Assert.DoesNotThrow(() => res = testMod.Add(mod1));

            Assert.IsNotNull(res);
            Assert.AreEqual(20, res.Get());
            Assert.AreEqual(40, res.GetCrit());
        }

        private void Test_Add_SingleAddNoRollHasMod()
        {
            var testMod = new Modifier(4);
            var mod3 = new Modifier(3);

            IModifier res = null;
            Assert.DoesNotThrow(() => res = testMod.Add(mod3));

            Assert.IsNotNull(res);
            Assert.AreEqual(7, res.Get());
            Assert.AreEqual(7, res.GetCrit());
        }

        private void Test_Add_SingleAddHasRollAndMod()
        {
            var testMod = new Modifier(always10, rolld12, 2);
            var mod13 = new Modifier(always10, rolld20, 3);

            IModifier res = null;
            Assert.DoesNotThrow(() => res = testMod.Add(mod13));

            Assert.IsNotNull(res);
            Assert.AreEqual(25, res.Get());
            Assert.AreEqual(45, res.GetCrit());
        }

        private void Test_Add_SingleAddMultlDice()
        {
            var testMod = new Modifier(always10, new List<RollType>() { RollType.d20, RollType.d20, RollType.d20 }, 1);
            var mod42 = new Modifier(always10, new List<RollType>() { RollType.d12, RollType.d12, RollType.d12, RollType.d12 }, 2);

            IModifier res = null;
            Assert.DoesNotThrow(() => res = testMod.Add(mod42));

            Assert.IsNotNull(res);
            var y = res.GetCrit();
            Assert.AreEqual(73, res.Get());
            Assert.AreEqual(143, res.GetCrit());
        }

        private void Test_Add_ChainAdd()
        {
            var a = new Modifier(); //0, 0
            var b = new Modifier(2);    //2, 2
            var c = new Modifier(always1, rolld4, 1);  //2, 3
            var d = new Modifier(always1, new List<RollType>() { RollType.d12, RollType.d12 }, 3); //5, 7
            var e = new Modifier(always1, new List<RollType>() { RollType.d20, RollType.d20, RollType.d20 }, 2); //5, 8

            IModifier res = null;
            Assert.DoesNotThrow(() => res = a.Add(b));
            Assert.DoesNotThrow(() => res = res.Add(c));
            Assert.DoesNotThrow(() => res = res.Add(d));
            Assert.DoesNotThrow(() => res = res.Add(e));

            Assert.IsNotNull(res);
            Assert.AreEqual(14, res.Get()); //0+2+2+5+5 = 14
            Assert.AreEqual(20, res.GetCrit()); //0+2+3+7+8 = 20
        }

        #endregion Add
    }
}
