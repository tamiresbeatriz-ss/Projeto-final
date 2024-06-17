using FatecSisMed.Web.Models;
using FatecSisMed.Web.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FatecSisMed.Web.Services.Entities;

public class EspecialidadeService : IEspecialidadeService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly JsonSerializerOptions _options;

    public EspecialidadeService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
    }

    private const string apiEndpoint = "/api/especialidade/";

    public async Task<EspecialidadeViewModel> CreateEspecialidade(EspecialidadeViewModel especialidade, string token)
    {
        var client = _clientFactory.CreateClient("MedicoAPI");
        PutTokenInHeaderAuthorization(token, client);

        StringContent content = new StringContent(JsonSerializer.Serialize(especialidade), Encoding.UTF8, "application/json");

        using (var response = await client.PostAsync(apiEndpoint, content))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<EspecialidadeViewModel>(apiResponse, _options);
            }
            return null;
        }
    }

    public async Task<bool> DeleteEspecialidadeById(int id, string token)
    {
        var client = _clientFactory.CreateClient("MedicoAPI");
        PutTokenInHeaderAuthorization(token, client);

        using (var response = await client.DeleteAsync(apiEndpoint + id))
        {
            return response.IsSuccessStatusCode;
        }
    }

    public async Task<EspecialidadeViewModel> FindEspecialidadeById(int id, string token)
    {
        var client = _clientFactory.CreateClient("MedicoAPI");
        PutTokenInHeaderAuthorization(token, client);
        using (var response = await client.GetAsync(apiEndpoint + id))
        {
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<EspecialidadeViewModel>(apiResponse, _options);
            }
            return null;
        }
    }

    public async Task<IEnumerable<EspecialidadeViewModel>> GetAllEspecialidades(string token)
    {
        var client = _clientFactory.CreateClient("MedicoAPI");
        PutTokenInHeaderAuthorization(token, client);

        var response = await client.GetAsync(apiEndpoint);

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<EspecialidadeViewModel>>(apiResponse, _options);
        }
        return null;
    }

    public async Task<EspecialidadeViewModel> UpdateEspecialidade(EspecialidadeViewModel especialidadeViewModel, string token)
    {
        var client = _clientFactory.CreateClient("MedicoAPI");
        PutTokenInHeaderAuthorization(token, client);

        EspecialidadeViewModel especialidade = new EspecialidadeViewModel();

        using (var response = await client.PutAsJsonAsync(apiEndpoint, especialidadeViewModel))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<EspecialidadeViewModel>(apiResponse, _options);
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
