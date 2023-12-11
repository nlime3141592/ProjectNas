using System;
using System.Collections.Concurrent;

namespace NAS
{
    // NOTE: 파일 탐색 현황을 기록하는 클래스입니다.
    public class FileBrowseData
    {
        // NOTE: 서버 측 루트 경로의 절대 경로를 클라이언트 측에서 가리기 위한 fake directory입니다.
        public string fakeroot { get; set; }

        // NOTE: 현재 탐색하고 있는 폴더의 fake directory입니다.
        public string fakedir { get; set; }

        // NOTE: 현재 탐색하고 있는 폴더의 하위 폴더/하위 파일 목록입니다.
        public ConcurrentDictionary<int, string> directories { get; private set; } = new ConcurrentDictionary<int, string>();
        public ConcurrentDictionary<int, string> files { get; private set; } = new ConcurrentDictionary<int, string>();

        // NOTE: 현재 다운로드 하고 있는 파일의 목록입니다.
        public ConcurrentDictionary<string, int> downloadingFiles { get; private set; } = new ConcurrentDictionary<string, int>();
    }
}
