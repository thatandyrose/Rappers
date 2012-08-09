using System;

namespace Rappers.HipHop
{
    public interface ILog
    {
        void Info(string message);
        void Error(Exception ex);
    }

    public class MockLog : ILog
    {
        public void Info(string message)
        {
            //nothing
        }

        public void Error(Exception ex)
        {
            //nothing
        }
    }
}
