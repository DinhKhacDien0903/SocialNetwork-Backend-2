using Microsoft.AspNetCore.Identity;
using SocialNetwork.DTOs.Request;

namespace SocialNetwork.DataAccess.Repositories
{
    public class UserRepository :BaseRepository<UserEntity>, IUserRepository
    {
        public readonly SocialNetworkdDataContext _context;

        private readonly UserManager<UserEntity> _userManager;
        public UserRepository(SocialNetworkdDataContext context, UserManager<UserEntity> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<UserEntity?> GetByUserNameAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<UserEntity?> GetLoginAsync(LoginRequest loginRequest)
        {
            var user =  await _userManager.FindByEmailAsync(loginRequest.Email);
            if(user != null && await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                return user;
            }
            return null;
        }


        public async Task<UserEntity> GetUserInfor(string userId)
        {
            var userIfor = await _userManager.FindByIdAsync(userId);

            if(userIfor == null)
            {
                throw new ArgumentNullException(nameof(userId), "User not found");
            }

            return userIfor;
        }

        public async Task UpdateStatusActiveUser(string userId, bool isActive)
        {
            var user = await _userManager.FindByIdAsync(userId);
            
            if(user == null)
            {
                throw new ArgumentNullException(nameof(userId), "User not found");
            }

            //user.IsActive = isActive;

            //user.LastLogin = DateTime.Now;

            await _userManager.UpdateAsync(user);
        }
    }
}
