using GunslingerSim.Common;
using GunslingerSim.Common.Util;
using GunslingerSim.Events;
using GunslingerSim.Objects;
using GunslingerSim.Objects.Factory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GunslingerSim.Simulator
{
    public class BulkGunSlingerSimulation : IBulkGunSlingerSimulation
    {
        private Rng rng;
        private int numTurns;
        private int numSims;
        private int numSimsPerThread;
        private ConcurrentBag<List<SimulationSummary>> sims;

        public BulkGunSlingerSimulation(Rng rng,
                                        int numTurns,
                                        int numSims,
                                        int numSimsPerThread)
        {
            Assert.IsNotNull(rng);
            Assert.IsTrue(numSims > 0);
            Assert.IsTrue(numSimsPerThread > 0);
            Assert.IsTrue(numSims % numSimsPerThread == 0);

            this.rng = rng;
            this.numTurns = numTurns;
            this.numSims = numSims;
            this.numSimsPerThread = numSimsPerThread;

            sims = new ConcurrentBag<List<SimulationSummary>>();
        }

        public SimulationSummary BulkSimulate(IPlayer player, IEnemy enemy)
        {
            List<Task> tasks = new List<Task>();
            int numThreads = numSims / numSimsPerThread;
            for (int i = 0; i < numThreads; i++)
            {
                tasks.Add(BuildSimTask(rng.Copy(), player, enemy.Copy()));
            }

            tasks.ForEach(x => x.Wait());

            IEnumerable<SimulationSummary> flattenedSimsList = sims.SelectMany(x => x);
            return Condense(flattenedSimsList);
        }

        private Task BuildSimTask(Rng rng, IPlayer player, IEnemy enemy)
        {
            return Task.Run(() => SimThread(player, enemy));
        }

        private void SimThread(IPlayer player, IEnemy enemy)
        {
            TurnStateFactory factory = new TurnStateFactory();
            TurnStateMachine stateMachine = new TurnStateMachine(factory);
            GunSlingerSimulation simulator = new GunSlingerSimulation(stateMachine, rng, numTurns);

            List<SimulationSummary> summaries = new List<SimulationSummary>(numSimsPerThread);

            for (int i = 0; i < numSimsPerThread; i++)
            {
                summaries.Add(simulator.Simulate(player, enemy));
            }

            sims.Add(summaries);
        }

        private SimulationSummary Condense(IEnumerable<SimulationSummary> summaries)
        {
            return new SimulationSummary()
            {
                Cost = summaries.Select(x => x.Cost).Aggregate((a, b) => a + b),
                Crits = summaries.Select(x => x.Crits).Aggregate((a, b) => a + b),
                DamageDone = summaries.Select(x => x.DamageDone).Aggregate((a, b) => a + b),
                Hits = summaries.Select(x => x.Hits).Aggregate((a, b) => a + b),
                NumberOfBrokenGuns = summaries.Select(x => x.NumberOfBrokenGuns).Aggregate((a, b) => a + b),
                Shots = summaries.Select(x => x.Shots).Aggregate((a, b) => a + b),
                ShotsLostToMisfire = summaries.Select(x => x.ShotsLostToMisfire).Aggregate((a, b) => a + b),
            };
        }

    }
}
