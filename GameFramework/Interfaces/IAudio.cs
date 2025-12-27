using GameFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.Interfaces
{
    public interface IAudio
    {
        void AddSound(AudioTrack sound);
        public void PlaySound(string name);
        void Stop(string name);
        void StopAll();
        void SetVolume(string name, float volume);
    }
}
