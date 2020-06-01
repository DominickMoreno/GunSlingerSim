using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Common.Util;
using GunslingerSim.Objects;
using GunslingerSim.Objects.Factory;
using System;
using System.Collections.Generic;

namespace GunslingerSim.Tests
{
    public abstract class BaseUnitTest
    {
        private Dictionary<string, Action> testsToRun;

        protected Rng always1;
        protected Rng always10;
        protected Rng always20;

        protected GunFactory gunFactory;
        protected IGun mh;
        protected IGun artificerMh;
        protected List<IGun> ohs;
        protected List<Feat> feats;
        protected List<MagicInitiateSpell> buffs;

        public BaseUnitTest()
        {
            testsToRun = new Dictionary<string, Action>();

            always1 = new Rng(1);
            always10 = new Rng(10);
            always20 = new Rng(20);

            gunFactory = new GunFactory();
            mh = gunFactory.Get(GunType.Pepperbox);
            artificerMh = gunFactory.Get(GunType.Pepperbox, WeaponTier.ArtificerReloadProperty);
            ohs = new List<IGun>()
            {
                gunFactory.Get(GunType.PalmPistol),
                gunFactory.Get(GunType.PalmPistol)
            };
            feats = new List<Feat>() { Feat.None };
            buffs = new List<MagicInitiateSpell>() { MagicInitiateSpell.None };
        }

        protected virtual void OneTimeSetup()
        {
            //empty
        }

        protected virtual void Setup()
        {
            //this
        }

        protected virtual void TearDown()
        {
            //this
        }

        public bool Run()
        {
            OneTimeSetup();
            bool allPass = true;
            foreach (string name in testsToRun.Keys)
            {
                Setup();
                try
                {
                    testsToRun[name]();
                }
                catch (Exception e)
                {
                    allPass = false;
                    Console.WriteLine($"------ Failed Test ------");
                    Console.WriteLine($"Test '{name} failed.");
                    Console.WriteLine($"Error: {e.Message}.");
                    Console.WriteLine($"Inner exception: {e.InnerException}.");
                    Console.WriteLine($"Stack trace: {e.StackTrace}.");
                    Console.WriteLine($"-------------------------");
                }

                TearDown();
            }

            return allPass;
        }

        protected void AddTest(string nameOfTest, Action test)
        {
            Assert.IsTrue(!testsToRun.ContainsKey(nameOfTest));
            testsToRun[nameOfTest] = test;
        }
    }
}
