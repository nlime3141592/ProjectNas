using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAS.Tests
{
    internal sealed class NasConfigFileD : NasConfigFile
    {
        public NasConfigFileD(string _directory)
        : base(_directory, ".config-d")
        {

        }

        public override void SaveConfigFile()
        {

        }

        protected override void InitializeConfigFile()
        {

        }

        protected override void LoadConfigFile()
        {

        }
    }
}
