using System;

namespace NAS
{
    public class ServiceResult
    {
        public static ServiceResult Failure => new ServiceResult(10000, "Failure");
        public static ServiceResult Success => new ServiceResult(10001, "Success");
        public static ServiceResult Error => new ServiceResult(10002, "Error");
        public static ServiceResult NetworkError => new ServiceResult(10003, "NetworkError");
        public static ServiceResult InvalidService => new ServiceResult(10004, "InvalidService");
        public static ServiceResult Null => new ServiceResult(-1, "Null");

        public int value { get; private set; }
        public string name { get; private set; }

        public ServiceResult(int _value, string _resultName = null)
        {
            value = _value;
            name = _resultName ?? "";
        }

        public static bool operator ==(ServiceResult _a, ServiceResult _b)
        {
            return _a.value == _b.value;
        }

        public static bool operator !=(ServiceResult _a, ServiceResult _b)
        {
            return _a.value != _b.value;
        }

        public static implicit operator int(ServiceResult _a)
        {
            return _a.value;
        }

        public static explicit operator ServiceResult(int _value)
        {
            return new ServiceResult(_value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else if (obj is ServiceResult)
                return ((ServiceResult)obj).value == this.value;
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
