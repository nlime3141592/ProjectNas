using System.IO;

namespace NAS.Tests
{
    internal abstract class NasConfigFile
    {
        public string absConfigFilePath { get; private set; }

        protected NasConfigFile(string _directory, string _configFileName)
        {
            absConfigFilePath = _directory + _configFileName;

            if (File.Exists(absConfigFilePath))
                LoadConfigFile();
            else
                InitializeConfigFile();
        }

        public abstract void SaveConfigFile();
        protected abstract void LoadConfigFile();
        protected abstract void InitializeConfigFile();
    }
}
