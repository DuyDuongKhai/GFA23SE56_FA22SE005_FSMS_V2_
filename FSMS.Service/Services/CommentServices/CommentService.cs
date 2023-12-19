using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.CommentRepositories;
using FSMS.Entity.Repositories.PostRepositories;
using FSMS.Entity.Repositories.UserRepositories;
using FSMS.Service.Enums;
using FSMS.Service.ViewModels.Comments;

namespace FSMS.Service.Services.CommentServices
{
    public class CommentService : ICommentService
    {
        private IUserRepository _userRepository;
        private IPostRepository _postRepository;
        private ICommentRepository _commentRepository;
        private IMapper _mapper;
        public CommentService(IUserRepository userRepository, IMapper mapper, IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }
        public async Task CreateCommentAsync(CreateComment createComment)
        {
            try
            {

                User existedUser = (await _userRepository.GetByIDAsync(createComment.UserId));
                if (existedUser == null)
                {
                    throw new Exception("User Id does not exist in the system.");
                }

                Post existedPost = (await _postRepository.GetByIDAsync(createComment.PostId));
                if (existedPost == null)
                {
                    throw new Exception("Post Id does not exist in the system.");
                }


                int lastId = (await _commentRepository.GetAsync()).Max(x => x.CommentId);
                Comment comment = new Comment()
                {

                    CommentContent = createComment.CommentContent,
                    PostId = createComment.PostId,
                    UserId = createComment.UserId,
                    Status = StatusEnums.Active.ToString(),
                    CreatedDate = DateTime.Now,
                    CommentId = lastId + 1,
                    ParentId = createComment.ParentId
                };

                await _commentRepository.InsertAsync(comment);
                await _commentRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task DeleteCommentAsync(int key)
        {
            try
            {
                Comment existedComment = await _commentRepository.GetByIDAsync(key);

                if (existedComment == null)
                {
                    throw new Exception("Comment ID does not exist in the system.");
                }
                if (existedComment.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("Comment is not active.");
                }

                existedComment.Status = StatusEnums.InActive.ToString();

                await _commentRepository.UpdateAsync(existedComment);
                await _commentRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<GetComment> GetAsync(int key)
        {
            try
            {
                Comment comment = await _commentRepository.GetByIDAsync(key);

                if (comment == null)
                {
                    throw new Exception("Comment ID does not exist in the system.");
                }

                if (comment.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("Comment is not active.");
                }

                List<GetComment> comments = _mapper.Map<List<GetComment>>(
                  await _commentRepository.GetAsync(includeProperties: "User,Post")
              );

                GetComment result = _mapper.Map<GetComment>(comment);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<GetComment>> GetAllAsync(bool activeOnly = false, int postId = 0)
        {
            try
            {
                IEnumerable<Comment> allComments = await _commentRepository.GetAsync(includeProperties: "User,Post");

                if (activeOnly)
                {
                    allComments = allComments.Where(comment => comment.Status == StatusEnums.Active.ToString());
                }

                if (postId != 0)
                {
                    allComments = allComments.Where(comment => comment.PostId == postId);
                }

                List<GetComment> comments = _mapper.Map<List<GetComment>>(allComments);

                return comments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




        public async Task UpdateCommentAsync(int key, UpdateComment updateComment)
        {
            try
            {
                Comment existedComment = await _commentRepository.GetByIDAsync(key);

                if (existedComment == null)
                {
                    throw new Exception("Comment ID does not exist in the system.");
                }

                if (!string.IsNullOrEmpty(updateComment.CommentContent))
                {
                    existedComment.CommentContent = updateComment.CommentContent;
                }

                if (!string.IsNullOrEmpty(updateComment.Status))
                {
                    if (updateComment.Status != "Active" && updateComment.Status != "InActive")
                    {
                        throw new Exception("Status must be 'Active' or 'InActive'.");
                    }
                    existedComment.Status = updateComment.Status;
                }

                existedComment.UpdateDate = DateTime.Now;


                await _commentRepository.UpdateAsync(existedComment);
                await _commentRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}


