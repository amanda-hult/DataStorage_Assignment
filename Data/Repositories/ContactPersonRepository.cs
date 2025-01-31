using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class ContactPersonRepository(DataContext context) : BaseRepository<ContactPersonEntity>(context), IContactPersonRepository
{
}
