using Microsoft.AspNetCore.Identity;
using SocialNetwork.Domain.IRepositories;
using SocialNetwork.Domain;
using SocialNetwork.DTOs.Request;
using SocialNetwork.DTOs.ViewModels;

namespace SocialNetwork.DataAccess.Repositories
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        public readonly SocialNetworkdDataContext _context;

        private readonly UserManager<UserEntity> _userManager;
        public UserRepository(
            SocialNetworkdDataContext context,
            UserManager<UserEntity> userManager) : base(context)
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
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                return user;
            }
            return null;
        }


        public async Task<UserEntity> GetUserInfor(string userId)
        {
            var userIfor = await _userManager.FindByIdAsync(userId);

            if (userIfor == null)
            {
                throw new ArgumentNullException(nameof(userId), "User not found");
            }

            return userIfor;
        }

        public async Task<IEnumerable<UserEntity>> SearchUserAsync(SearchQuery query, string userId)
        {
            var me = await _userManager.FindByIdAsync(userId);
            var user = await _context.Users
             .Where(x => 
             x.Id!= me.Id.ToString()&&
             (x.FirstName.ToLower().Contains(query.keyWord.ToLower())
                      || x.LastName.ToLower().Contains(query.keyWord.ToLower())
                      || (x.FirstName.ToLower() + " " + x.LastName.ToLower()).Contains(query.keyWord.ToLower())
                      || (x.FirstName.ToLower() + x.LastName.ToLower()).Contains(query.keyWord.ToLower())
                      )
                      )
             .Skip(query.SkipNo)
             .Take(query.TakeNo)
             //.Select(u => new UserSearchViewModel
             //{
             //    FirstName = u.FirstName,
             //    LastName = u.LastName,
             //    AvatarUrl = u.AvatarUrl
             //})
             .ToListAsync();

            //if (user == null || !user.Any())
            //{
            //    return 
            //}
            var userSearch= user.Select(x=> new UserEntity
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                AvatarUrl = x.AvatarUrl,
                Id = x.Id,

            });
            return userSearch;
        }

    public async Task UpdateStatusActiveUser(string userId, bool isActive)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(userId), "User not found");
            }

            //user.IsActive = isActive;

            //user.LastLogin = DateTime.Now;

            await _userManager.UpdateAsync(user);
        }
    }
}
