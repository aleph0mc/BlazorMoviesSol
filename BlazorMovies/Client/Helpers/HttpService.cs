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
        //private readonly HttpClient _httpClinet;
        private readonly HttpClientWithToken _httpClientWithToken;
        private readonly HttpClientWithoutToken _httpClientWithoutToken;

        private JsonSerializerOptions defJsonSerOpt =>
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

        //public HttpService(HttpClient httpClinet)
        //{
        //    _httpClient = httpClient;
        //}

        public HttpService(HttpClientWithToken httpClientWithToken, HttpClientWithoutToken httpClientWithoutToken)
        {
            _httpClientWithToken = httpClientWithToken;
            _httpClientWithoutToken = httpClientWithoutToken;
        }

        private HttpClient GetHttpClient(bool includeToken = true)
        {
            if (includeToken)
            {
                return _httpClientWithToken.HttpClient;
            }
            else
            {
                return _httpClientWithoutToken.HttpClient;
            }
        }

        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T data)
        {
            var httpClient = GetHttpClient();
            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, stringContent);
            return new HttpResponseWrapper<object>(null, response.IsSuccessStatusCode, response);
        }

        public async Task<HttpResponseWrapper<object>> Put<T>(string url, T data)
        {
            var httpClient = GetHttpClient();
            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(url, stringContent);
            return new HttpResponseWrapper<object>(null, response.IsSuccessStatusCode, response);
        }

        public async Task<HttpResponseWrapper<T>> Get<T>(string url, bool includeToken = true)
        {
            var httpClient = GetHttpClient(includeToken);
            var responseHTTP = await httpClient.GetAsync(url);
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

        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T data, bool includeToken = true)
        {
            var httpClient = GetHttpClient(includeToken);
            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, stringContent);
            if (response.IsSuccessStatusCode)
            {
                var responseDeserialized = await Deserialize<TResponse>(response, defJsonSerOpt);
                return new HttpResponseWrapper<TResponse>(responseDeserialized, true, response);
            }

            return new HttpResponseWrapper<TResponse>(default, false, response);
        }

        public async Task<HttpResponseWrapper<object>> Delete(string url)
        {
            var httpClient = GetHttpClient();
            var responseHTTP = await httpClient.DeleteAsync(url);

            return new HttpResponseWrapper<object>(null, responseHTTP.IsSuccessStatusCode, responseHTTP);
        }
    }
}
