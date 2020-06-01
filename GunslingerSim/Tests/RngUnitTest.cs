using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Common.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Tests
{
    public class RngUnitTest : BaseUnitTest
    {
        #region Setup

        const int numRepetitions = 50000;

        Rng rng;
        int[] results;

        public RngUnitTest()
        {
            AddTest(nameof(Test_Constructor_InvalidArgs), Test_Constructor_InvalidArgs);
            AddTest(nameof(Test_Constructor_SunnyDay), Test_Constructor_SunnyDay);

            AddTest(nameof(Test_Roll_d4), Test_Roll_d4);
            AddTest(nameof(Test_Roll_d20), Test_Roll_d20);
        }

        protected override void OneTimeSetup()
        {
            rng = new Rng();
        }

        protected override void TearDown()
        {
            results = null;
        }

        private int[] Setup(int size)
        {
            int[] ret = new int[size];
            for (int i = 0; i < size; i++)
            {
                ret[i] = 0;
            }

            return ret;
        }

        private void PrintResults(int[] results, int numResults)
        {
            Console.WriteLine("***");
            for (int i = 0; i < numResults; i++)
            {
                Console.WriteLine($"results[{i}]: {results[i]}.");
            }
            Console.WriteLine("***");
        }

        #endregion Setup

        #region Constructor

        private void Test_Constructor_InvalidArgs()
        {
            Assert.Throws<ArgumentException>(() => new Rng(-1));
            Assert.Throws<ArgumentException>(() => new Rng(-100));
            Assert.Throws<ArgumentException>(() => new Rng(int.MinValue));
            Assert.Throws<ArgumentException>(() => new Rng(21));
            Assert.Throws<ArgumentException>(() => new Rng(10000));
            Assert.Throws<ArgumentException>(() => new Rng(int.MaxValue));
        }

        private void Test_Constructor_SunnyDay()
        {
            Assert.DoesNotThrow(() => new Rng());
            Assert.DoesNotThrow(() => new Rng(0));
            Assert.DoesNotThrow(() => new Rng(1));
            Assert.DoesNotThrow(() => new Rng(19));
            Assert.DoesNotThrow(() => new Rng(20));
        }

        #endregion Constructor

        #region Roll

        private void Test_Roll_d4()
        {
            results = Setup(4);

            for (int i = 0; i < numRepetitions; i++)
            {
                int roll = rng.Roll(RollType.d4);
                Assert.IsTrue(roll > 0);
                Assert.IsTrue(roll < 5);
                results[roll - 1]++;
            }

            PrintResults(results, 4);
        }

        private void Test_Roll_d20()
        {
            results = Setup(20);

            for (int i = 0; i < numRepetitions; i++)
            {
                int roll = rng.Roll(RollType.d20);
                Assert.IsTrue(roll > 0);
                Assert.IsTrue(roll < 21);
                results[roll - 1]++;
            }

            PrintResults(results, 20);
        }

        #endregion
    }
}
