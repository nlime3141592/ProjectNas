using NAS.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAS
{
    public class NasFileSystem
    {
        public string rootStorageDirectory { get; private set; }
        public string rootFakeDirectory { get; private set; } = @"FAKE_ROOT\";

        public NasFileSystem(string _rootAbsDirectory)
        {
            rootStorageDirectory = _rootAbsDirectory;

            if (!Directory.Exists(rootStorageDirectory))
                Directory.CreateDirectory(rootStorageDirectory);

            DirectoryManager manager = DirectoryManager.Get(_rootAbsDirectory, Encoding.UTF8);

            // TEST: 테스트 코드입니다.
            manager.TryAddFolder("firstFolder");
            manager.TryAddFolder("secondFolder");
            manager.TryAddFolder("thirdFolder");
        }

        public string FakeToPath(string _fake)
        {
            int beg = _fake.IndexOf(rootFakeDirectory);

            if (beg != 0)
                return _fake; // NOTE: 잘못된 fake

            string childs = _fake.Substring(rootFakeDirectory.Length, _fake.Length - rootFakeDirectory.Length);
            return rootStorageDirectory + childs;
        }

        public string PathToFake(string _path)
        {
            int beg = _path.IndexOf(rootStorageDirectory);

            if (beg != 0)
                return _path; // NOTE: 잘못된 경로

            string childs = _path.Substring(rootStorageDirectory.Length, _path.Length - rootStorageDirectory.Length);
            return rootFakeDirectory + childs;
        }
    }
}
