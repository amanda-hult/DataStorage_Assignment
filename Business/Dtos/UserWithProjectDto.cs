using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Business.Models;

namespace Business.Dtos;

public class UserWithProjectDto
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public List<BasicProjectModel> Projects { get; set; } = new List<BasicProjectModel>();
}
