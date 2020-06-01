using GunslingerSim.Common;
using GunslingerSim.Common.Util;
using GunslingerSim.Events;
using GunslingerSim.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GunslingerSim.Simulator
{
    public class GunSlingerSimulation : IGunSlingerSimulation
    {
        private ITurnStateMachine stateMachine;
        private Rng rng;
        private int numTurns;

        public GunSlingerSimulation(ITurnStateMachine stateMachine, 
                                    Rng rng,
                                    int numTurns)
        {
            Assert.IsNotNull(stateMachine);
            Assert.IsNotNull(rng);
            Assert.IsTrue(numTurns > 0);

            this.stateMachine = stateMachine;
            this.rng = rng;
            this.numTurns = numTurns;
        }

        public SimulationSummary Simulate(IPlayer player, IEnemy enemy)
        {
            Assert.IsNotNull(player);
            Assert.IsNotNull(enemy);

            PlayerStatus status = new PlayerStatus(rng, player);
            IEnemy freshEnemy = enemy.Copy();
            CompleteTurns(status, freshEnemy);
            return AccumulateResults(status, freshEnemy);
        }

        private void CompleteTurns(IPlayerStatus status, IEnemy enemy)
        {
            for (int i = 0; i < numTurns; i++)
            {
                stateMachine.TakeTurn(status, enemy);
            }
        }

        private SimulationSummary AccumulateResults(IPlayerStatus status, IEnemy enemy)
        {
            return new SimulationSummary()
            {
                Cost = (ulong)status.MainHand.Cost +
                       (ulong)status.OffHands
                                    .Select(x => x.Cost)
                                    .Sum(),
                Crits = (ulong)status.NumberOfCrits,
                DamageDone = (ulong)enemy.DamageTaken,
                Hits = (ulong)enemy.HitsTaken,
                Shots = (ulong)status.NumberOfShots,
                NumberOfBrokenGuns = GetNumberOfBrokenGuns(status),
                ShotsLostToMisfire = 0  //TODO: Calculate at end of sim? Num max shots - reloads - actual shots
            };
        }

        private ulong GetNumberOfBrokenGuns(IPlayerStatus status)
        {
            ulong total = status.MainHand.CanFire()
                ? (ulong)0 : 1;
            total += (ulong)status.OffHands.Where(x => !x.CanFire()).Count();
            return total;
        }

        //TODO: this
        /*
        private ulong GetShotsLostToMisfire(IPlayerStatus status)
        {
            ulong numExpectedShots = GetNumExpectedShots(status, numTurns);
            return numExpectedShots - (ulong)status.NumberOfShots;
        }

        private ulong GetNumExpectedShots(IPlayerStatus status, int numTurns)
        {
            ulong numExpectedOhShots = GetNumExpectedOhShots(IPlayerStatus status, numTurns);
            ulong numExpectedMhShots = GetNumExpectedOhShots(IPlayerStatus status, numTurns);
        }

        private ulong GetNumExpectedOhS
            */
    }
}
