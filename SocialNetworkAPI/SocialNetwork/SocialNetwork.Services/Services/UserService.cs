using Microsoft.AspNetCore.Identity;

namespace SocialNetwork.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IRelationshipRepository _relationshipRepository;

        private readonly IMapper _mapper;

        private readonly IPasswordHasher<IdentityUser> _passwordHasher;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IPasswordHasher<IdentityUser> passwordHasher,
            IRelationshipRepository relationshipRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _relationshipRepository = relationshipRepository;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userRepository.GetByIDAsync(id);

            if (user == null)
            {
                return false;
            }

            _userRepository.Delete(user);
            await _userRepository.SaveChangeAsync();

            return true;
        }
 
        public async Task<IEnumerable<UserViewModel>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<UserViewModel>>(users);
        }

        public async Task<UserViewModel> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetByIDAsync(id);

            return _mapper.Map<UserViewModel>(user);
        }

        public async Task<UserViewModel> GetUserInforAsync(string userId)
        {
            var userEntity = await _userRepository.GetUserInfor(userId);

            return _mapper.Map<UserViewModel>(userEntity);
        }

        public string HashPassWord(string password)
        {
            var user = new IdentityUser();
            var passwordHashed = _passwordHasher.HashPassword(user, password);

            return passwordHashed;
        }

        public bool VerifyPassword(string hashedPassword, string providePassword)
        {
            var user = new IdentityUser();
            var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, providePassword);

            return result == PasswordVerificationResult.Success;
        }

        public async Task<IEnumerable<FriendViewModel>> GetFriendOnlinesAsync(string userId)
        {
            List<FriendViewModel> result = new List<FriendViewModel>();

            var friends = await _relationshipRepository.GetFriendIdByUserId(userId);

            foreach(var item in friends)
            {
                var user = await _userRepository.GetUserInfor(item);
                if (user.IsActive)
                {
                    result.Add(_mapper.Map<FriendViewModel>(user));
                }
            }

            return result;
        }
    }
}
