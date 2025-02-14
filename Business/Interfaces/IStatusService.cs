using Business.Dtos;
using Business.Models.Responses;
using Data.Entities;

namespace Business.Interfaces;

public interface IStatusService
{
    Task<ResultT<IEnumerable<StatusDto>>> GetAllStatusesAsync();
    Task<ResultT<StatusEntity>> GetStatusEntityByIdAsync(int id);
}
