using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GunslingerSim.Common
{
    public static class Assert
    {
        public static void IsTrue(bool val)
        {
            if (!val)
            {
                throw new ArgumentException();
            }
        }

        public static void AreEqual<T>(T obj1, T obj2)
        {
            if (!AreEqualObjs(obj1, obj2))
            {
                throw new ArgumentException();
            }
        }
        public static void AreNotEqual<T>(T obj1, T obj2)
        {
            if (AreEqualObjs(obj1, obj2))
            {
                throw new ArgumentException();
            }
        }

        public static void IsNotNull(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }

        }

        public static void IsNotEmpty<T>(ICollection<T> collection)
        {
            IsNotNull(collection);
            if (!collection.Any())
            {
                throw new ArgumentException();
            }
        }

        public static void HasNoNullEntries<T>(ICollection<T> collection)
        {
            IsNotNull(collection);
            if (collection.Where(x => x == null).Any())
            {
                throw new ArgumentException();
            }
        }

        public static void ValidEnum<T>(T value) where T : Enum
        {
            if (!Enum.IsDefined(typeof(T), value))
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public static void Throws<T>(Action func) where T : Exception
        {
            bool exceptionThrown = false;
            try
            {
                func.Invoke();
            }
            catch (T)
            {
                exceptionThrown = true;
            }

            if (!exceptionThrown)
            {
                throw new ArgumentException();
            }
        }

        public static void DoesNotThrow(Action func)
        {
            Exception actualException = null;

            try
            {
                func.Invoke();
            }
            catch (Exception e)
            {
                actualException = e;
            }

            if (actualException != null)
            {
                throw new ArgumentException(actualException.Message);
            }
        }

        public static void Fail()
        {
            throw new Exception();
        }

        private static bool AreEqualObjs<T>(T obj1, T obj2)
        {
            return (obj1 == null && obj2 == null) ||
                   (obj1?.Equals(obj2) ?? false);
        }
    }
}
