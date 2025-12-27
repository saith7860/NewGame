using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.Core
{
    public class SpriteAnimationTrack
    {
        public List<Image> Frames;
        public float FrameTime;     // ek frame kitni der chale
        public float ElapsedTime;//kitna chal chuka ha
        public int CurrentFrame;//konsa frame current ha
        public bool Loop;

        public SpriteAnimationTrack(List<Image> frames, float frameTime, bool loop)
        {
            Frames = frames;
            FrameTime = frameTime;
            Loop = loop;

            ElapsedTime = 0f;
            CurrentFrame = 0;
        }
    }
}