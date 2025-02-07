using Business.Dtos;
using Business.Factories;
using Business.Models.Responses;
using Data.Interfaces;

namespace Business.Services;

public class StatusService(IStatusRepository statusRepository)
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
}
