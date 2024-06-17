using FatecSisMed.Web.Models;

namespace FatecSisMed.Web.Services.Interfaces;

public interface IMedicoService
{
    Task<IEnumerable<MedicoViewModel>> GetAllMedicos(string token);
    Task<MedicoViewModel> FindMedicoById(int id, string token);
    Task<MedicoViewModel> CreateMedico(MedicoViewModel medico, string token);
    Task<MedicoViewModel> UpdateMedico(MedicoViewModel medico, string token);
    Task<bool> DeleteMedicoById(int id, string token);
}
