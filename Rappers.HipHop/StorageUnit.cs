using System;

namespace Rappers.HipHop
{
    public class StorageUnit
    {
        public static StorageUnit FromBytes(long bytes)
        {
            return new StorageUnit(bytes);
        }

        public long Bytes { get; private set;}

        public StorageUnit(long bytes)
        {
            Bytes = bytes;
        }
        
        public decimal Kilobytes
        {
            get { return Math.Round(Decimal.Divide(Bytes, 1024), 2); }
        }
        public decimal Megabytes
        {
            get { return Math.Round(Decimal.Divide(Kilobytes, 1024), 2); }
        }
        public decimal Gigabytes
        {
            get { return Math.Round(Decimal.Divide(Megabytes, 1024), 2); }
        }

        public string Friendly()
        {
            if(!IsFraction(Gigabytes))
            {
                return string.Format("{0} gb",Gigabytes);
            }
            if (!IsFraction(Megabytes))
            {
                return string.Format("{0} mb", Megabytes);
            }
            if (!IsFraction(Kilobytes))
            {
                return string.Format("{0} kb", Kilobytes);
            }
            return string.Format("{0} bytes", Bytes);
        }

        private bool IsFraction(decimal value)
        {
            return Math.Floor(value) < 1;
        }
    }
}
