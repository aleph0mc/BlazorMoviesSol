using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Helpers
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClinet;

        private JsonSerializerOptions defJsonSerOpt =>
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        public HttpService(HttpClient httpClinet)
        {
            _httpClinet = httpClinet;
        }
        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T data)
        {
            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await _httpClinet.PostAsync(url, stringContent);
            return new HttpResponseWrapper<object>(null, response.IsSuccessStatusCode, response);
        }

        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            var responseHTTP = await _httpClinet.GetAsync(url);
            if (responseHTTP.IsSuccessStatusCode)
            {
                var response = await Deserialize<T>(responseHTTP, defJsonSerOpt);
                return new HttpResponseWrapper<T>(response, true, responseHTTP);
            }

            return new HttpResponseWrapper<T>(default, false, responseHTTP);
        }

        private async Task<T> Deserialize<T>(HttpResponseMessage httpResponse, JsonSerializerOptions options)
        {
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseString, options);
        }

        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T data)
        {
            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await _httpClinet.PostAsync(url, stringContent);
            if (response.IsSuccessStatusCode)
            {
                var responseDeserialized = await Deserialize<TResponse>(response, defJsonSerOpt);
                return new HttpResponseWrapper<TResponse>(responseDeserialized, true, response);
            }

            return new HttpResponseWrapper<TResponse>(default, false, response);
        }
    }
}
