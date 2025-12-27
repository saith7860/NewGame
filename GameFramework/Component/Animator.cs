using GameFramework.Core;
using GameFramework.Interfaces;
using GameFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.Component
{
    public class Animator:IAnimator
    {
        public GameObject Owner { get; set; } = null!;
        private SpriteAnimationTrack? currentTrack;
      public void PlayAnimation(SpriteAnimationTrack track)
        {
            currentTrack = track;
        }

      public void Update(float deltaTime)
        {
            if (currentTrack == null)
                return;
            currentTrack.ElapsedTime += deltaTime;
            if (currentTrack.ElapsedTime >= currentTrack.FrameTime)
            {
                currentTrack.CurrentFrame++;
                currentTrack.ElapsedTime = 0f;
                if (currentTrack.CurrentFrame >= currentTrack.Frames.Count)
                {
                    if (currentTrack.Loop)
                    {
                        currentTrack.CurrentFrame = 0;
                    }
                    else
                    {
                        currentTrack.CurrentFrame = currentTrack.Frames.Count - 1; // Hold on last frame
                    }
                }
                // Update the owner's sprite to the current frame
                Owner.Sprite = currentTrack.Frames[currentTrack.CurrentFrame];
            }
        }
        public void Destroy()
        {
            currentTrack = null;
        }

    }
}
