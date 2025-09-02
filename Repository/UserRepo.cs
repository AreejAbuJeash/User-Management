using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using Repository.Model;
using Repository.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using User = Repository.Model.User;
using Repository.Mapping;
namespace Repository;

public class UserRepo : IUserRepo
{
    private readonly UserManagementContext _context;

    public UserRepo(UserManagementContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
       var entity= await _context.Users.FindAsync(id);
        return entity?.MapToUserModel();
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.Select(u =>u.MapToUserModel()).ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user.MapToUserEntity());
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user.MapToUserUpdateEntity());
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
   
}