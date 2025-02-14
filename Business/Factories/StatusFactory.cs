using Business.Dtos;
using Data.Entities;

namespace Business.Factories;

public static class StatusFactory
{
    public static StatusDto Create(StatusEntity entity)
    {
        return new StatusDto
        {
            StatusId = entity.StatusId,
            StatusName = entity.StatusName,
        };
    }

    //public static StatusEntity Create(StatusDto dto)
    //{
    //    return new StatusEntity
    //    {
    //        StatusId = dto.StatusId,
    //        StatusName = dto.StatusName,
    //    };
    //}
}
