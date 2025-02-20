using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models.Responses;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class StatusService(IStatusRepository statusRepository) : IStatusService
{
    private readonly IStatusRepository _statusRepository = statusRepository;

    // READ
    public async Task<ResultT<IEnumerable<StatusDto>>> GetAllStatusesAsync()
    {
        var statuses = (await _statusRepository.GetAllAsync()).Select(StatusFactory.Create).ToList();

        if (statuses.Count == 0)
            return ResultT<IEnumerable<StatusDto>>.NotFound("No statuses found.");

        return ResultT<IEnumerable<StatusDto>>.Ok(statuses);
    }

    public async Task<ResultT<StatusEntity>> GetStatusEntityByIdAsync(int id)
    {
        var statusEntity = await _statusRepository.GetAsync(s => s.StatusId == id);

        if (statusEntity == null)
            return ResultT<StatusEntity>.NotFound("Status not found.");

        return ResultT<StatusEntity>.Ok(statusEntity);
    }
}
