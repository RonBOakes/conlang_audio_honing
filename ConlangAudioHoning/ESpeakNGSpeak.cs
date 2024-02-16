using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ESpeakWrapper;

namespace ConlangAudioHoning
{
    internal class ESpeakNGSpeak
    {
        private static string ESpeakNGLibPath = @"C:\Program Files\eSpeak NG\libespeak-ng.dll";

        public ESpeakNGSpeak()
        {
            ESpeakWrapper.Client.Initialize(ESpeakNGSpeak.ESpeakNGLibPath);
            ESpeakWrapper.Client.SetPhonemeEvents(true, false);
        }

        public List<ESpeakWrapper.ESpeakVoice> getVoices()
        {
            List<ESpeakWrapper.ESpeakVoice> voices = new List<ESpeakWrapper.ESpeakVoice>();
            List<IntPtr> voicePointers = new List<IntPtr>();

            unsafe
            {
                void** voicePointerArray = (void**)espeak_ListVoices(IntPtr.Zero);
                void* v;
                for(int ix = 0; (v = voicePointerArray[ix]) != null; ix++)
                {
                    voicePointers.Add((IntPtr)v);
                }
            }

            foreach(IntPtr  voicePointer in voicePointers) 
            {
                if (voicePointer != IntPtr.Zero)
                {
#pragma warning disable CS8605
                    ESpeakVoice espeakVoice = (ESpeakVoice)Marshal.PtrToStructure(voicePointer, typeof(ESpeakVoice));
#pragma warning restore CS8605
                    voices.Add(espeakVoice);
                }
            }             

            return voices;
        }

        public void Test()
        {
            // Initialize the eSpeak-ng library via the wrapper
            if(!ESpeakWrapper.Client.SetVoiceByName("English (America)"))
            {
                return;
            }
            if(!ESpeakWrapper.Client.Speak("[[h@'loU]]. This is a test."))
            {
                return;
            }
        }


        // Extra espeakNG Library calls
        [DllImport("libespeak-ng.dll", CharSet = CharSet.Auto)]
        static extern IntPtr espeak_ListVoices(IntPtr voice_spec);

    }
}
