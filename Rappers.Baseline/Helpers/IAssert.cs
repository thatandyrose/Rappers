using System;

namespace Rappers.Baseline.Helpers
{
    public interface IAssert
    {
        void AssertOrThrow(bool test, Exception toThrow);
        void AssertOrThrow(bool test, string exceptionMessage);
    }
    public class Asserter : IAssert
    {
        public void AssertOrThrow(bool test, Exception toThrow)
        {
            if(!test)
            {
                throw toThrow;
            }
        }

        public void AssertOrThrow(bool test, string exceptionMessage)
        {
            if(!test)
            {
                throw new Exception(exceptionMessage);
            }
        }
    }
}
