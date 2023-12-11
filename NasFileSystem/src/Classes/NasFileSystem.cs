using System.IO;
using System.Text;

namespace NAS
{
    // NOTE: NAS Storage의 루트 폴더를 관리하기 위한 클래스입니다.
    public class NasFileSystem
    {
        public string rootStorageDirectory { get; private set; }
        public string rootFakeDirectory { get; private set; } = @"FAKE_ROOT\"; // NOTE: 클라이언트에게 NAS 서버의 루트 경로를 숨기기 위해 전송하는 Fake 루트 경로입니다.

        public NasFileSystem(string _rootAbsDirectory)
        {
            rootStorageDirectory = _rootAbsDirectory;

            if (!Directory.Exists(rootStorageDirectory))
                Directory.CreateDirectory(rootStorageDirectory);

            // NOTE:
            // NAS Storage의 루트 폴더를 초기화합니다.
            // 만약, 루트 폴더에 .dmeta 파일과 .fmeta 파일이 없다면 이 때 생성됩니다.
            DirectoryManager manager = DirectoryManager.Get(_rootAbsDirectory, Encoding.UTF8);
        }

        // NOTE: Fake경로를 절대 경로로 변환합니다.
        public string FakeToPath(string _fake)
        {
            int beg = _fake.IndexOf(rootFakeDirectory);

            if (beg != 0)
                return _fake; // NOTE: 잘못된 fake 경로

            string childs = _fake.Substring(rootFakeDirectory.Length, _fake.Length - rootFakeDirectory.Length);
            return rootStorageDirectory + childs;
        }

        // NOTE: 절대 경로를 Fake경로로 변환합니다.
        public string PathToFake(string _path)
        {
            int beg = _path.IndexOf(rootStorageDirectory);

            if (beg != 0)
                return _path; // NOTE: 잘못된 절대 경로

            string childs = _path.Substring(rootStorageDirectory.Length, _path.Length - rootStorageDirectory.Length);
            return rootFakeDirectory + childs;
        }
    }
}
