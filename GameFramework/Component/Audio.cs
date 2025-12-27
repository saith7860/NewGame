using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Presentation;
using GameFramework.Core;
using GameFramework.Interfaces;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GameFramework.Component
{
    public class Audio:IAudio
    {
        //sound data
        private Dictionary<string,AudioTrack> sounds= new Dictionary<string, AudioTrack>();
        //playing devices
        private Dictionary<string, WaveOutEvent> outputs = new();
        // File readers
        private Dictionary<string, AudioFileReader> readers = new();
    
        public void AddSound(AudioTrack sound)
        {
            if(!sounds.ContainsKey(sound.Name))
            {
                sounds.Add(sound.Name, sound);
            }
        }
        public void PlaySound(string name)
        {
            if (outputs.ContainsKey(name))
            {
                Stop(name);
            }
            if (!sounds.ContainsKey(name))
                return;
            if (sounds.ContainsKey(name))
            {
              var sound = sounds[name];
              string fullPath = System.IO.Path.Combine(
              AppDomain.CurrentDomain.BaseDirectory,
              sound.FilePath
          );

                var reader = new AudioFileReader(fullPath);
                reader.Volume = sound.Volume;
                readers[name] = reader;
                var output = new WaveOutEvent();
                output.Init(reader);
                output.PlaybackStopped += (s, e) =>
                {
                    if (!sound.Loop)
                    {
                        output.Dispose();
                        reader.Dispose();
                        outputs.Remove(name);
                        readers.Remove(name);
                    }
                    else
                    {
                        reader.Position = 0;
                        output.Play();
                    }
                };
                outputs[name] = output;
                output.Play();
            }
            else
            {
                throw new Exception($"Sound {name} not found.");
            }
        }
        public void Stop(string name)
        {
            if(outputs.ContainsKey(name))
            {
                outputs[name].Stop();
            }
        }
        public void StopAll()
        {
            foreach(var output in outputs.Values)
            {
                output.Stop();
            }
        }
        public void SetVolume(string name, float volume)
        {
            if(readers.ContainsKey(name))
            {
                readers[name].Volume = volume;
            }
        }

    }
}
