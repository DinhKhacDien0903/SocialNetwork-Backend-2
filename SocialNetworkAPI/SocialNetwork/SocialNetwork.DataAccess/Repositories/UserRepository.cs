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

        public async Task<int> GetTotalFriendAsync(string userId)
        {
            return await _context.Relationships.Where(x => x.UserID == userId).Select(x => x.FriendID).CountAsync();
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

        public async Task<UserEntity> UpdateUserInforAsync(UserEntity userEntity)
        {
            var query = from u in _context.Users
                        where u.Id == userEntity.Id
                        select u;

            var user = await query.FirstOrDefaultAsync();

            if(user == null)
            {
                throw new ArgumentNullException(nameof(userEntity.Id), "User not found");
            }

            void UpdateField<T>(Action<T> setField, T value, T currentValue)
            {
                if (value != null && !EqualityComparer<T>.Default.Equals(currentValue, value))
                {
                    setField(value);
                }
            }

            UpdateField(value => user.FirstName = value, userEntity.FirstName, user.FirstName);
            UpdateField(value => user.LastName = value, userEntity.LastName, user.LastName);
            UpdateField(value => user.Gender = value, userEntity.Gender, user.Gender);
            UpdateField(value => user.DateOfBirth = value, userEntity.DateOfBirth, user.DateOfBirth);
            UpdateField(value => user.Address = value, userEntity.Address, user.Address);
            UpdateField(value => user.isPrivate = value, userEntity.isPrivate, user.isPrivate);
            UpdateField(value => user.AvatarUrl = value, userEntity.AvatarUrl, user.AvatarUrl);

            await _context.SaveChangesAsync();

            return user;
        }
    }

}
