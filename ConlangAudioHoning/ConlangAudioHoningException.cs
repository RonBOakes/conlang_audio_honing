using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConlangAudioHoning
{
    internal class ConlangAudioHoningException : Exception
    {
        public ConlangAudioHoningException(string message) : base(message) { }
        public ConlangAudioHoningException(string message, Exception innerException) : base(message, innerException) { }
    }
}
