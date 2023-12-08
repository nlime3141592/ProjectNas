using System;

namespace NAS
{
    public class NasServiceResult
    {
        public static NasServiceResult Failure => new NasServiceResult(10000, "Failure");
        public static NasServiceResult Success => new NasServiceResult(10001, "Success");
        public static NasServiceResult Error => new NasServiceResult(10002, "Error");
        public static NasServiceResult NetworkError => new NasServiceResult(10003, "NetworkError");
        public static NasServiceResult InvalidService => new NasServiceResult(10004, "InvalidService");
        public static NasServiceResult Loopback => new NasServiceResult(10005, "Loopback");
        public static NasServiceResult Null => new NasServiceResult(-1, "Null");

        public int value { get; private set; }
        public string name { get; private set; }

        public NasServiceResult(int _value, string _resultName = null)
        {
            value = _value;
            name = _resultName ?? "";
        }

        public static bool operator ==(NasServiceResult _a, NasServiceResult _b)
        {
            return _a.value == _b.value;
        }

        public static bool operator !=(NasServiceResult _a, NasServiceResult _b)
        {
            return _a.value != _b.value;
        }

        public static implicit operator int(NasServiceResult _a)
        {
            return _a.value;
        }

        public static explicit operator NasServiceResult(int _value)
        {
            return new NasServiceResult(_value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else if (obj is NasServiceResult)
                return ((NasServiceResult)obj).value == this.value;
            else if (obj is Int32)
                return (Int32)obj == this.value;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
