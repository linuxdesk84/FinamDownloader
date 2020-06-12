using System;
using System.IO;
using System.Net;
using System.Text;


//https://github.com/rickyah/ini-parser


namespace FinamDownloader {
    /// <summary>
    /// интерфейс класса, реализующего возможность загрузки html страниц
    /// </summary>
    interface IHttpWebRequestService {
        bool TryGetResponse(string url, out string responseOrError);
    }

    /// <summary>
    /// класс, реализующий возможность загрузки html страниц
    /// </summary>
    class HttpWebRequestService : IHttpWebRequestService {
        public bool TryGetResponse(string url, out string responseOrError) {
            responseOrError = string.Empty;

            try {
                /* to fix: The request was aborted: Could not create SSL/TLS secure channel.
                 http://qaru.site/questions/45913/the-request-was-aborted-could-not-create-ssltls-secure-channel
                 */
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                // Creates an HttpWebRequest with the specified URL.
                var httpWebRequest = (HttpWebRequest) WebRequest.Create(url);

                // Sends the HttpWebRequest and waits for the response.
                var httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
                // Gets the stream associated with the response.
                var stream = httpWebResponse.GetResponseStream();
                if (stream != null) {
                    // Pipes the stream to a higher level stream reader with the required encoding format.
                    var reader = new StreamReader(stream, Encoding.Default);
                    responseOrError = reader.ReadToEnd();
                    // Releases the resources of the Stream.
                    reader.Close();
                }

                // Releases the resources of the response.
                httpWebResponse.Close();
            }
            catch (Exception e) {
                responseOrError = e.Message;
                return false;
            }

            return true;
        }
    }
}