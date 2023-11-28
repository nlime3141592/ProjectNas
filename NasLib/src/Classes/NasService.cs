using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAS
{
    public abstract class NasService
    {
        public abstract ServiceResult Execute();

        protected void ShowServiceLog(string _message)
        {
            Console.WriteLine("[{0}] {1}", this.GetType().Name, _message);
        }

        protected void ShowServiceLogFormat(string _format, params object[] _args)
        {
            string message = string.Format(_format, _args);
            Console.WriteLine("[{0}] {1}", this.GetType().Name, message);
        }
    }
}
