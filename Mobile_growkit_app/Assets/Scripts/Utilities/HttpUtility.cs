//using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Utilities
{
    /// <summary> A service providing eight clients to use for HTTP calls.</summary>
    public class TinyHttpClientPool
    {
        public const string ServiceName = "TinyHttpClientPool";
        /// <summary> The mechanism to keep track of in-use client instances to distribute the free clients when requested.</summary>
        private byte _clientsInUse;

        /// <summary> The pool of eight ready-to-use HttpClient instances.</summary>
        private readonly HttpClient[] _clientPool = new HttpClient[]
        {
            new HttpClient(),
            new HttpClient(),
            new HttpClient(),
            new HttpClient(),
            new HttpClient(),
            new HttpClient(),
            new HttpClient(),
            new HttpClient()
        };

        /// <summary> Gets the index of a free client in the pool and set it to in-use.</summary>
        /// <returns> The index of the client leased to the request.</returns>
        /// <remarks> If all clients are in use, a new attempt will be made in 50 miliseconds</remarks>
        private async Task<byte> GetClientIndexAsync()
        {
            bool requestingClient = true;
            byte clientIndex = 0;

            do
            {
                // checks if all bits have been set to 1, signalling that al clients are in use.
                if (_clientsInUse == 0b1111_1111)
                {
                    // Client pool starved, retrying in 50 miliseconds.
                    await Task.Delay(50);
                    continue;
                }

                // make sure that only one thread is accessing the pool at this moment in time to prevent a client from being assigned to two different requests.
                lock (_clientPool)
                {
                    for (byte i = 0; i < _clientPool.Length; i++)
                    {
                        if ((_clientsInUse & (1 << i)) == 0)
                        {
                            _clientsInUse |= (byte)(1 << i);
                            requestingClient = false;
                            clientIndex = i;
                            break;
                        }
                    }
                }
            } while (requestingClient);

            return (clientIndex);
        }

        /// <summary> Makes a request to the given endpoint with the given method.</summary>
        /// <param name="httpMethod"> The given request method.</param>
        /// <param name="requestUri"> The given Request URI</param>
        /// <param name="content"> The attached content to the request.</param>
        /// <param name="headerValuePairs"> The attached single-value headers.</param>
        /// <param name="headerValueCollectionPairs"> The attached multi-value headers.</param>
        /// <returns> The resulting response message. null if no request could have been made.</returns>
        public async Task<HttpResponseMessage> RequestAsync(
            HttpMethod httpMethod,
            string requestUri, 
            HttpContent content = null,
            Dictionary<string, string> headerValuePairs = null,
            Dictionary<string,IEnumerable<string>> headerValueCollectionPairs = null)
        {
            var clientIndex = await GetClientIndexAsync();
            HttpClient client = _clientPool[clientIndex];
            HttpResponseMessage response;

            //start building the request message
            using (var requestMessage = new HttpRequestMessage(httpMethod, requestUri))
            {
                if (content != null)
                    requestMessage.Content = content;

                if (headerValuePairs != null)
                    foreach (var headerValuePair in headerValuePairs)
                    {
                        requestMessage.Headers.Add(headerValuePair.Key, headerValuePair.Value);
                    }

                if(headerValueCollectionPairs != null)
                    foreach (var headerValueCollectionPair in headerValueCollectionPairs)
                    {
                        requestMessage.Headers.Add(headerValueCollectionPair.Key, headerValueCollectionPair.Value);
                    }

                // Send the request to the adress given in the request.
                try
                {
                    response = await client.SendAsync(requestMessage);
                }
                finally
                {
                    _clientsInUse ^= (byte)(1 << clientIndex);
                }
            }

            return response;
        }
    }
}
