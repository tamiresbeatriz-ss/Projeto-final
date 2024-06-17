using FatecSisMed.Web.Models;
using FatecSisMed.Web.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FatecSisMed.Web.Services.Entities;

public class MedicoService : IMedicoService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly JsonSerializerOptions _options;

    public MedicoService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
    }

    private const string apiEndpoint = "/api/medico/";

    public async Task<MedicoViewModel> CreateMedico(MedicoViewModel medico, string token)
    {
        var client = _clientFactory.CreateClient("MedicoAPI");
        PutTokenInHeaderAuthorization(token, client);
        StringContent content = new StringContent(JsonSerializer.Serialize(medico), Encoding.UTF8, "application/json");

        using (var response = await client.PostAsync(apiEndpoint, content))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<MedicoViewModel>(apiResponse, _options);
            }
            return null;
        }
    }

    public async Task<bool> DeleteMedicoById(int id, string token)
    {
        var client = _clientFactory.CreateClient("MedicoAPI");

        PutTokenInHeaderAuthorization(token, client);

        using (var response = await client.DeleteAsync(apiEndpoint + id))
        {
            return response.IsSuccessStatusCode;
        }
    }

    public async Task<MedicoViewModel> FindMedicoById(int id, string token)
    {
        var client = _clientFactory.CreateClient("MedicoAPI");
        PutTokenInHeaderAuthorization(token, client);
        using (var response = await client.GetAsync(apiEndpoint + id))
        {
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<MedicoViewModel>(apiResponse, _options);
            }
            return null;
        }
    }

    public async Task<IEnumerable<MedicoViewModel>> GetAllMedicos(string token)
    {
        var client = _clientFactory.CreateClient("MedicoAPI");
        PutTokenInHeaderAuthorization(token, client);

        var response = await client.GetAsync(apiEndpoint);

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<MedicoViewModel>>(apiResponse, _options);
        }
        return null;
    }

    public async Task<MedicoViewModel> UpdateMedico(MedicoViewModel medicoViewModel, string token)
    {
        var client = _clientFactory.CreateClient("MedicoAPI");
        PutTokenInHeaderAuthorization(token, client);

        MedicoViewModel medico = new MedicoViewModel();

        using (var response = await client.PutAsJsonAsync(apiEndpoint, medicoViewModel))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<MedicoViewModel>(apiResponse, _options);
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
