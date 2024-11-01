using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SocialNetwork.Services.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository ;
        private readonly IMapper _mapper ;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IBaseRepository<ImagesOfPostEntity> _imageRepository;
        public PostService(IPostRepository postRepository,
            UserManager<UserEntity> userManager
            , IBaseRepository<ImagesOfPostEntity> imageRepository, IMapper mapper)
        {
            _userManager = userManager;
            _postRepository = postRepository; 
            _imageRepository = imageRepository;
            _mapper = mapper;
        }
        public async Task<PostRequest> CreatePostAsync(PostRequest postRequest,string userID)
        {
            if (string.IsNullOrWhiteSpace(postRequest.Content))
            {
                throw new ArgumentException("Content cannot be null or empty.", nameof(postRequest.Content));
            }

            try
            {
                var postEntity = _mapper.Map<PostEntity>(postRequest);
                postEntity.PostID=Guid.NewGuid().ToString();

                postEntity.UserID=userID;
               
                Console.WriteLine($"Creating Post - PostID: {postEntity.PostID}, Content: {postEntity.Content}");

                await _postRepository.AddAsync(postEntity);
                  await _postRepository.SaveChangeAsync();

                if (postRequest.Images != null && postRequest.Images.Count > 0)
                {
                    foreach (var image in postRequest.Images)
                    {
                        if (string.IsNullOrWhiteSpace(image.ImgUrl))
                        {
                            throw new ArgumentException("Image URL cannot be null or empty.");
                        }

                        var imageEntity = _mapper.Map<ImagesOfPostEntity>(image);
                        imageEntity.PostID = postEntity.PostID; 

                        Console.WriteLine($"Adding Image - PostID: {imageEntity.PostID}, ImgUrl: {imageEntity.ImgUrl}");

                        await _imageRepository.AddAsync(imageEntity);
                    }
                    await _imageRepository.SaveChangeAsync();
                }

                return _mapper.Map<PostRequest>(postEntity);
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database Update Error: {dbEx.InnerException?.Message}");
                throw new Exception("An error occurred while creating the post.", dbEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                throw new Exception("An error occurred while creating the post.", ex);
            }
        }







        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var postEntity = await _postRepository.GetByIDAsync(postId);
            if (postEntity == null)
            {
                throw new Exception("Không có bài viết tương ứng với postId");
            }
            postEntity.IsDelete = true;
            _postRepository.Update(postEntity);
            await _postRepository.SaveChangeAsync();
            return true;
        }

        public async Task<IEnumerable<PostViewModel>> GetAllPostsAsync()
        {
            var posts = await _postRepository.GetAllAsync();
            
            return _mapper.Map<IEnumerable<PostViewModel>>(posts);
        }

        public async Task<PostViewModel> GetPostByIdAsync(Guid postId)
        {
            var post = await _postRepository.GetByIDAsync(postId);
            if (post == null)
            {
                return null;
            }
            return _mapper.Map<PostViewModel>(post);
        }


        public async Task<PostViewModel> UpdatePostAsync(PostViewModel post)
        {
            var postEntity = await _postRepository.GetByIDAsync(post.PostID);
            if (postEntity == null)
            {
                throw new Exception("Không tồn tại bài viết");
            }
            _mapper.Map(post, postEntity);
            _postRepository.Update(postEntity);
            await _postRepository.SaveChangeAsync();
            return _mapper.Map<PostViewModel>(postEntity);
        }

        public async Task<IEnumerable<PostViewModel>> GetPostsByUserIdAsync(string userId)
        {
            var posts = await _postRepository.GetAllAsync(); 
            var userPosts = posts.Where(p => p.UserID == userId); 
            return _mapper.Map<IEnumerable<PostViewModel>>(userPosts);
        }
    }
}
