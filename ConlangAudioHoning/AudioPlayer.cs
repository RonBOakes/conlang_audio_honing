using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConlangAudioHoning
{
    /// <summary>
    /// Static class for playing audio using the Windows Media Player
    /// </summary>
    internal static class AudioPlayer
    {
        /// <summary>
        /// Play Windows media from the source URI at the specified volume.
        /// </summary>
        /// <param name="source">URI to the media source</param>
        /// <param name="Volume">Volume from 0.0 to 1.0</param>
        public static void PlayAudio(Uri source, double Volume = 0.75)
        {
            if ((Volume < 0.0) || (Volume > 1.0))
            {
                Volume = 0.75;
            }
            System.Windows.Media.MediaPlayer audioPlayer = new()
            {
                Volume = Volume
            };
            audioPlayer.Open(source);
            audioPlayer.Play();
        }
    }
}
