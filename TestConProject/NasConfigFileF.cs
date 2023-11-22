using System.IO;
using System.Text;

namespace NAS.Tests
{
    internal sealed class NasConfigFileF : NasConfigFile
    {
        public char fileExtension { get; private set; } = 'a';
        public int chunkCount { get; private set; } = 0;

        public NasConfigFileF(string _directory, string _fileName)
        : base(NasFileSystem.GetFileString(_directory, _fileName), ".config-fd")
        {

        }

        public override void SaveConfigFile()
        {

        }

        protected override void InitializeConfigFile()
        {
            FileStream stream = new FileStream(absConfigFilePath, FileMode.CreateNew, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII);

            writer.Write(fileExtension);
            writer.Write(chunkCount);

            writer.Close();
            stream.Close();
        }

        protected override void LoadConfigFile()
        {
            FileStream stream = new FileStream(absConfigFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);

            fileExtension = reader.ReadChar();
            chunkCount = reader.ReadInt32();

            reader.Close();
            stream.Close();
        }
    }
}
