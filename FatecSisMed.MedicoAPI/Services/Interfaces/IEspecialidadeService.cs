using FatecSisMed.MedicoAPI.DTO.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FatecSisMed.MedicoAPI.Services.Interfaces
{
    public interface IEspecialidadeService
    {
        Task<IEnumerable<EspecialidadeDTO>> GetAll();
        Task<EspecialidadeDTO> GetById(int id);
        Task<IEnumerable<EspecialidadeDTO>> GetEspecialidadeMedicos();
        Task Create(EspecialidadeDTO especialidadeDTO);
        Task Update(EspecialidadeDTO especialidadeDTO);
        Task Remove(int id);
    }
}
