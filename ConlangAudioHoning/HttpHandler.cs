/*
* Class for handling the HTTP/HTTPS requests (REST API Requests) for Language Honing
* 
* Copyright (C) 2024 Ronald B. Oakes
*
* This program is free software: you can redistribute it and/or modify it
* under the terms of the GNU General Public License as published by the Free
* Software Foundation, either version 3 of the License, or (at your option)
* any later version.
*
* This program is distributed in the hope that it will be useful, but WITHOUT
* ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or
* FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for
* more details.
*
* You should have received a copy of the GNU General Public License along with
* this program.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace ConlangAudioHoning
{
    /// <summary>
    /// Class for handling all HTTP/HTTPS requests for the ConlangAudioHoning program.
    /// </summary>
    internal sealed class HttpHandler
    {

        /// <summary>
        /// The constructor for HttpHandler is private to ensure that only one instance of this
        /// class exists during any given application execution.
        /// </summary>
        private HttpHandler() => HttpClient = new HttpClient();

        private static readonly object _lock = new();
        private static HttpHandler? _instance = null;

        /// <summary>
        /// Returns the current instance of the HttpHandler class.  If needed
        /// it will instantiate the single allowed instance of this class.
        /// </summary>
        public static HttpHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new HttpHandler();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Returns the HttpClient object contained within the HttpHandler.  This is the
        /// only HttpClient that can be used in the ConlangAudioHoning application.
        /// </summary>
        public HttpClient HttpClient { get; }
    }
}
