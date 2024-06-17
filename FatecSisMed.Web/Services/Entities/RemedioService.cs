using FatecSisMed.Web.Models;
using FatecSisMed.Web.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FatecSisMed.Web.Services.Entities
{
    public class RemédioService : IRemedioService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly JsonSerializerOptions _options;

        public RemédioService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        }

        private const string apiEndpoint = "/api/remedio/";

        public async Task<RemedioViewModel> CreateRemedio(RemedioViewModel remédio, string token)
        {
            var client = _clientFactory.CreateClient("MedicoAPI");
            PutTokenInHeaderAuthorization(token, client);
            StringContent content = new StringContent(JsonSerializer.Serialize(remédio), Encoding.UTF8, "application/json");

            using (var response = await client.PostAsync(apiEndpoint, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<RemedioViewModel>(apiResponse, _options);
                }
                return null;
            }
        }

        public async Task<bool> DeleteRemedioById(int id, string token)
        {
            var client = _clientFactory.CreateClient("MedicoAPI");

            PutTokenInHeaderAuthorization(token, client);

            using (var response = await client.DeleteAsync(apiEndpoint + id))
            {
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<RemedioViewModel> FindRemedioById(int id, string token)
        {
            var client = _clientFactory.CreateClient("MedicoAPI");
            PutTokenInHeaderAuthorization(token, client);
            using (var response = await client.GetAsync(apiEndpoint + id))
            {
                if (response.IsSuccessStatusCode && response.Content is not null)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<RemedioViewModel>(apiResponse, _options);
                }
                return null;
            }
        }

        public async Task<IEnumerable<RemedioViewModel>> GetAllRemedios(string token)
        {
            var client = _clientFactory.CreateClient("MedicoAPI");
            PutTokenInHeaderAuthorization(token, client);

            var response = await client.GetAsync(apiEndpoint);

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<IEnumerable<RemedioViewModel>>(apiResponse, _options);
            }
            return null;
        }

        public async Task<RemedioViewModel> UpdateRemedio(RemedioViewModel remédioViewModel, string token)
        {
            var client = _clientFactory.CreateClient("MedicoAPI");
            PutTokenInHeaderAuthorization(token, client);

            RemedioViewModel remédio = new RemedioViewModel();

            using (var response = await client.PutAsJsonAsync(apiEndpoint, remédioViewModel))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<RemedioViewModel>(apiResponse, _options);
                }
                return null;
            }

        }

        private static void PutTokenInHeaderAuthorization(string token, HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
