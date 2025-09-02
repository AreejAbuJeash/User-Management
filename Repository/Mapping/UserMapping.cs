using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User = Repository.Model.User;
namespace Repository.Mapping
{
    public static class UserMapping
    {
        public static User MapToUserModel(this Entities.User entity)
        {
            return new User
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Email = entity.Email,
                PasswordHash = entity.PasswordHash,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
        public static Entities.User MapToUserEntity(this User model)
        {
            return new Entities.User
            {
                UserName = model.UserName,
                Email = model.Email,
                PasswordHash = model.PasswordHash,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };
        }
        public static Entities.User MapToUserUpdateEntity(this User model)
        {
            return new Entities.User
            {
                Id=model.Id,
                UserName = model.UserName,
                Email = model.Email,
                PasswordHash = model.PasswordHash,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsActive = model.IsActive,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}
