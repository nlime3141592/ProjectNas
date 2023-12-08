using System;
using System.Collections.Concurrent;

namespace NAS
{
    public class FileBrowseData
    {
        public string fakeroot { get; set; }
        public string fakedir { get; set; }

        public ConcurrentDictionary<int, string> directories { get; private set; } = new ConcurrentDictionary<int, string>();
        public ConcurrentDictionary<int, string> files { get; private set; } = new ConcurrentDictionary<int, string>();
    }
}
