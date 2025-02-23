using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Models.Responses;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class ContactPersonService(IContactPersonRepository contactPersonRepository) : IContactPersonService
{
    private readonly IContactPersonRepository _contactPersonRepository = contactPersonRepository;

    // CREATE
    public async Task<ResultT<ContactPersonModel>> CreateContactPersonAsync(ContactPersonCreateDto dto)
    {
        //check if contact person already exists
        var exists = await _contactPersonRepository.AlreadyExistsAsync(cp => cp.Email == dto.Email);
        if (exists)
            return ResultT<ContactPersonModel>.Conflict("A contact person with the same email already exists.");

        //create new contact person
        var createdEntity = await _contactPersonRepository.CreateAsync(ContactPersonFactory.Create(dto));

        if (createdEntity != null)
        {
            await _contactPersonRepository.SaveAsync();
            return ResultT<ContactPersonModel>.Created(ContactPersonFactory.Create(createdEntity));
        }
        else
        {
            return ResultT<ContactPersonModel>.Error("An error occured while creating the c.");
        }
    }

    // READ
    public async Task<ResultT<ContactPersonEntity>> GetContactPersonEntityByEmailAsync(string email)
    {
        var contactPersonEntity = await _contactPersonRepository.GetAsync(cp => cp.Email == email);

        if (contactPersonEntity == null)
            return ResultT<ContactPersonEntity>.NotFound("Contact person not found.");

        return ResultT<ContactPersonEntity>.Ok(contactPersonEntity);
    }

    public async Task<ResultT<ContactPersonEntity>> GetContactPersonEntityByIdAsync(int id)
    {
        var contactPersonEntity = await _contactPersonRepository.GetAsync(cp => cp.ContactPersonId == id);

        if (contactPersonEntity == null)
            return ResultT<ContactPersonEntity>.NotFound("Contact person not found.");

        return ResultT<ContactPersonEntity>.Ok(contactPersonEntity);
    }
}
