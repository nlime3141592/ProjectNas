using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAS.Tests
{
    public static class TestLibProject
    {
        public static string GetTestString(string _message)
        {
            return string.Format("[{0}]", _message);
        }
    }
}
