using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tippr.Application.DTOs.User;

namespace Tippr.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserProfileAsync(string userId);
        Task<UserDto> UpdateUserProfileAsync(string userId, string firstName, string lastName, string profilePictureUrl);
        Task ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    }
}
