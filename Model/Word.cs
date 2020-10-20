using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synonymizer {
    class Word {
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }

        public Word(int start, int end) {
            StartIndex = start;
            EndIndex = end;
        }
    }
}
