using FatecSisMed.Web.Models;

namespace FatecSisMed.Web.Services.Interfaces;

public interface IEspecialidadeService
{
    Task<IEnumerable<EspecialidadeViewModel>> GetAllEspecialidades(string token);
    Task<EspecialidadeViewModel> FindEspecialidadeById(int id, string token);
    Task<EspecialidadeViewModel> CreateEspecialidade(EspecialidadeViewModel especialidade , string token);
    Task<EspecialidadeViewModel> UpdateEspecialidade(EspecialidadeViewModel especialidade , string token);
    Task<bool> DeleteEspecialidadeById(int id, string token);
}
