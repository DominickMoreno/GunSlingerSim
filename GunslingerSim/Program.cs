using GunslingerSim.Common.Enums;
using GunslingerSim.Common.Util;
using GunslingerSim.Events;
using GunslingerSim.Objects;
using GunslingerSim.Objects.Factory;
using GunslingerSim.Simulator;
using GunslingerSim.Tests;
using System;
using System.Collections.Generic;

namespace GunslingerSim
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            UnitTestSuite testSuite = new UnitTestSuite();
            testSuite.Run();
            */

            GunFactory gunFactory = new GunFactory(); 
            //IGun mh = gunFactory.Get(GunType.Pistol);
            IGun mh = gunFactory.Get(GunType.Pepperbox);
            IGun artificerMh = gunFactory.Get(GunType.Pistol, WeaponTier.ArtificerReloadProperty);
            List<IGun> ohs = new List<IGun>()
                {
                    gunFactory.Get(GunType.PalmPistol),
                    gunFactory.Get(GunType.PalmPistol)
                };
            List<IGun> artificerOhs = new List<IGun>() { gunFactory.Get(GunType.PalmPistol, WeaponTier.ArtificerReloadProperty) };
            //List<Feat> feats = new List<Feat>() { Feat.None };
            List<Feat> feats = new List<Feat>() { Feat.Sharpshooter };
            List<MagicInitiateSpell> buffs = new List<MagicInitiateSpell>() { MagicInitiateSpell.None };
            //List<MagicInitiateSpell> buffs = new List<MagicInitiateSpell>() { MagicInitiateSpell.Bless };
            //List<MagicInitiateSpell> buffs = new List<MagicInitiateSpell>() { MagicInitiateSpell.Hex };

            Rng rng = new Rng();
            //buffs = new List<MagicInitiateSpell>() { MagicInitiateSpell.Bless };
            //buffs = new List<MagicInitiateSpell>() { MagicInitiateSpell.Hex };
            Player player = new Player(rng, 8, 8, FightingStyle.Archery, mh, ohs, feats, buffs);
            //Player player = new Player(rng, 6, 8, FightingStyle.Archery, artificerMh, artificerOhs, feats, buffs);

            int numTurns = 7;
            Enemy enemy = new Enemy(17);

            int numSims = 100000;
            //int numSimsPerThread = numSims / 4;
            int numSimsPerThread = numSims;
            BulkGunSlingerSimulation sim = new BulkGunSlingerSimulation(rng, numTurns, numSims, numSimsPerThread);
            SimulationSummary summary = sim.BulkSimulate(player, enemy);
            int totalTurns = numSims * numTurns;

            double dmgPerTurn = (double)summary.DamageDone / totalTurns;
            double shotsPerTurn = (double)summary.Shots / totalTurns;
            double hitsPerTurn = (double)summary.Hits / totalTurns;
            double critsPerTurn = (double)summary.Crits / totalTurns;
            double costPerSim = ((double)summary.Cost / numSims) / 100;
            double brokenGunsPerTurn = (double)summary.NumberOfBrokenGuns / totalTurns;

            Console.WriteLine("------ Summary ------");
            Console.WriteLine($"Enemy AC: {enemy.ArmorClass}.");
            Console.WriteLine($"Number of Turns per sim: {numTurns}.");
            Console.WriteLine($"Number of total Turns: {totalTurns}.\n");
            Console.WriteLine($"Damage per turn: " + string.Format("{0:0.000}", dmgPerTurn));
            Console.WriteLine($"Shots per turn: " + string.Format("{0:0.000}", shotsPerTurn));
            Console.WriteLine($"Hits per turn: " + string.Format("{0:0.000}", hitsPerTurn));
            Console.WriteLine($"Crits per turn: " + string.Format("{0:0.000}", critsPerTurn));
            Console.WriteLine($"Cost per combat encounter: " + string.Format("{0:0.000}", costPerSim) + "g");
            Console.WriteLine($"Broken Guns per turn: " + string.Format("{0:0.000}", brokenGunsPerTurn));
            //Misfires?
            Console.WriteLine("---------------------");
        }
    }
}
