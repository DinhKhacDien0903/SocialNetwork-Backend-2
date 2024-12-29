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

        //public async Task<IEnumerable<UserSearchViewModel>> SearchUserAsync(SearchQuery query, string userId)
        //{
        //    var me = await _userManager.FindByIdAsync(userId);
        //    var user = await _context.Users
        //     .Where(x =>
        //     x.Id != me.Id.ToString() &&
        //     (x.FirstName.ToLower().Contains(query.keyWord.ToLower())
        //    || x.LastName.ToLower().Contains(query.keyWord.ToLower())
        //    || (x.FirstName.ToLower() + " " + x.LastName.ToLower()).Contains(query.keyWord.ToLower())
        //    || (x.FirstName.ToLower() + x.LastName.ToLower()).Contains(query.keyWord.ToLower())))
        //     .Skip(query.SkipNo)
        //     .Take(query.TakeNo)
        //     .ToListAsync();

        //    var listId = user.Select(x => x.Id).ToList();
        //    var relationShip = await _context.Relationships.Where(
        //        x => x.UserID == me.Id && listId.Contains(x.FriendID) || x.FriendID == me.Id == listId.Contains(x.UserID)).ToListAsync();

        //    var results = user.Select(x =>
        //    {
        //        var isFriend = relationShip.Any
        //        (a => a.UserID == me.Id && a.FriendID == x.Id ||
        //        a.FriendID == me.Id && a.UserID == x.Id
        //        );
        //        return new UserSearchViewModel
        //        {
        //            Id = x.Id,
        //            LastName = x.LastName,
        //            FirstName = x.FirstName,
        //            AvatarUrl = x.AvatarUrl,
        //            isRelationShip = isFriend,
        //        };
        //    });



        //    return results;
        //}








        public async Task<IEnumerable<UserSearchViewModel>> SearchUserAsync(SearchQuery query, string userId)
        {
            var me = await _userManager.FindByIdAsync(userId);
            var user = await _context.Users.Skip(query.SkipNo).Take(query.TakeNo).ToListAsync();
            if (query.keyWord != null)
            {
                user = user.Where(x =>
                            x.Id != me.Id.ToString() && (x.FirstName.ToLower().Contains(query.keyWord.ToLower())
                            || x.LastName.ToLower().Contains(query.keyWord.ToLower())
                            || (x.FirstName.ToLower() + " " + x.LastName.ToLower()).Contains(query.keyWord.ToLower())
                            || (x.FirstName.ToLower() + x.LastName.ToLower()).Contains(query.keyWord.ToLower()))).ToList();
            }
            var listId = user.Select(x => x.Id).ToList();
            var relationShip = await _context.Relationships.Where(
                x => x.UserID == me.Id && listId.Contains(x.FriendID) || x.FriendID == me.Id == listId.Contains(x.UserID)).ToListAsync();

            var results = user.Select(x =>
            {
                var isFriend = relationShip.Any
                (a => a.UserID == me.Id && a.FriendID == x.Id ||
                a.FriendID == me.Id && a.UserID == x.Id
                );
                return new UserSearchViewModel
                {
                    Id = x.Id,
                    LastName = x.LastName,
                    FirstName = x.FirstName,
                    AvatarUrl = x.AvatarUrl,
                    isRelationShip = isFriend,
                };
            });



            return results;
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

            if (user == null)
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
