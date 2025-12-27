using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.Core
{
    public class AudioTrack
    {
        public string Name { get; set; } = null!;
        public string FilePath { get; set; }=null!;
        public float Volume { get; set; }
        public bool Loop { get; set; } 
        public float Duration { get; set; }
        public AudioTrack(string name, string filePath,bool loop)
        {
            Name = name;
            FilePath = filePath;
            Volume = 1f;
            Loop = loop;
            Duration = 1f;
        }
    }
}
