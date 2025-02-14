using Business.Dtos;
using Business.Models.Responses;
using Business.Models;
using Data.Entities;

namespace Business.Interfaces;

public interface IContactPersonService
{
    Task<ResultT<ContactPersonModel>> CreateContactPersonAsync(ContactPersonCreateDto dto);
    Task<ResultT<ContactPersonEntity>> GetContactPersonEntityByEmailAsync(string email);
}
