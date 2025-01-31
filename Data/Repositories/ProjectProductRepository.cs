using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class ProjectProductRepository(DataContext context) : BaseRepository<ProjectProductEntity>(context), IProjectProductRepository
{
}
