using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterface
{
    public class FileMetadata
    {
        public string FullPathAndName { get; set; }
        public DateTime Time { get; set; }
        public int Size { get; set; }
        

        public override string ToString()
        {
            return string.Format(@"
Path: {0}
Time: {1},
Size: {2}.", FullPathAndName, Time, Size);
        }
    }
}
