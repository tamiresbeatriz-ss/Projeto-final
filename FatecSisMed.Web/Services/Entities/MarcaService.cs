using FatecSisMed.Web.Models;
using FatecSisMed.Web.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FatecSisMed.Web.Services.Entities
{
    public class MarcaService : IMarcaService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly JsonSerializerOptions _options;

        public MarcaService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        }

        private const string apiEndpoint = "/api/marca/";

        public async Task<MarcaViewModel> CreateMarca(MarcaViewModel marca, string token)
        {
            var client = _clientFactory.CreateClient("MedicoAPI");
            PutTokenInHeaderAuthorization(token, client);

            StringContent content = new StringContent(JsonSerializer.Serialize(marca), Encoding.UTF8, "application/json");

            using (var response = await client.PostAsync(apiEndpoint, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<MarcaViewModel>(apiResponse, _options);
                }
                return null;
            }
        }

        public async Task<bool> DeleteMarcaById(int id, string token)
        {
            var client = _clientFactory.CreateClient("MedicoAPI");
            PutTokenInHeaderAuthorization(token, client);

            using (var response = await client.DeleteAsync(apiEndpoint + id))
            {
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<MarcaViewModel> FindMarcaById(int id, string token)
        {
            var client = _clientFactory.CreateClient("MedicoAPI");
            PutTokenInHeaderAuthorization(token, client);
            using (var response = await client.GetAsync(apiEndpoint + id))
            {
                if (response.IsSuccessStatusCode && response.Content is not null)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<MarcaViewModel>(apiResponse, _options);
                }
                return null;
            }
        }

        public async Task<IEnumerable<MarcaViewModel>> GetAllMarcas(string token)
        {
            var client = _clientFactory.CreateClient("MedicoAPI");
            PutTokenInHeaderAuthorization(token, client);

            var response = await client.GetAsync(apiEndpoint);

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<IEnumerable<MarcaViewModel>>(apiResponse, _options);
            }
            return null;
        }

        public async Task<MarcaViewModel> UpdateMarca(MarcaViewModel marcaViewModel, string token)
        {
            var client = _clientFactory.CreateClient("MedicoAPI");
            PutTokenInHeaderAuthorization(token, client);

            using (var response = await client.PutAsJsonAsync(apiEndpoint, marcaViewModel))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<MarcaViewModel>(apiResponse, _options);
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
