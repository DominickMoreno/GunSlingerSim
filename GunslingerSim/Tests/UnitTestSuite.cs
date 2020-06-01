using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Tests
{
    public class UnitTestSuite
    {
        Dictionary<string, BaseUnitTest> unitTestsToRun;

        public UnitTestSuite()
        {
            unitTestsToRun = new Dictionary<string, BaseUnitTest>();

            ModifierUnitTest modifierUnitTest = new ModifierUnitTest();
            unitTestsToRun[nameof(ModifierUnitTest)] = modifierUnitTest;

            //Don't always run these because RNG + prints
            /*
            RngUnitTest rngUnitTest = new RngUnitTest();
            unitTestsToRun[nameof(RngUnitTest)] = rngUnitTest;
            */

            GunFactoryUnitTest gunFactoryUnitTest = new GunFactoryUnitTest();
            unitTestsToRun[nameof(GunFactoryUnitTest)] = gunFactoryUnitTest;

            GunStatusUnitTest gunStatusUnitTest = new GunStatusUnitTest();
            unitTestsToRun[nameof(GunStatusUnitTest)] = gunStatusUnitTest;

            PlayerStatusUnitTest playerStatusUnitTest = new PlayerStatusUnitTest();
            unitTestsToRun[nameof(PlayerStatusUnitTest)] = playerStatusUnitTest;

            StartTurnEventUnitTest startTurnUnitTest = new StartTurnEventUnitTest();
            unitTestsToRun[nameof(StartTurnEventUnitTest)] = startTurnUnitTest;

            EndTurnEventUnitTest endTurnEventUnitTest = new EndTurnEventUnitTest();
            unitTestsToRun[nameof(EndTurnEventUnitTest)] = endTurnEventUnitTest;

            ActionSurgeEventUnitTest actionSurgeEventUnitTest = new ActionSurgeEventUnitTest();
            unitTestsToRun[nameof(ActionSurgeEventUnitTest)] = actionSurgeEventUnitTest;

            FirstTurnSpellBuffEventUnitTest firstTurnSpellEventUnitTest = new FirstTurnSpellBuffEventUnitTest();
            unitTestsToRun[nameof(FirstTurnSpellBuffEventUnitTest)] = firstTurnSpellEventUnitTest;

            OffHandAttackEventUnitTest oHAttackEventUnitTest = new OffHandAttackEventUnitTest();
            unitTestsToRun[nameof(OffHandAttackEventUnitTest)] = oHAttackEventUnitTest;

            ActionEventUnitTest actionEventUnitTest = new ActionEventUnitTest();
            unitTestsToRun[nameof(ActionEventUnitTest)] = actionEventUnitTest;

            TurnStateMachineUnitTest turnStateMachineUnitTest = new TurnStateMachineUnitTest();
            unitTestsToRun[nameof(TurnStateMachineUnitTest)] = turnStateMachineUnitTest;
        }

        public void Run()
        {
            foreach(string name in unitTestsToRun.Keys)
            {
                Console.WriteLine($"** Executing {name}...");
                bool result = unitTestsToRun[name].Run();
                Console.WriteLine($"** Done executing {name}. All passed? {result}.");
            }
        }
    }
}
