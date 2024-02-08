using AutoMapper;
using BusinessObject.Commons;
using BusinessObject.Dtos;
using BusinessObject.Models;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class BlogRatingController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IBlogRepository blogRepository;
        private readonly IBlogRatingRepository blogRatingRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public BlogRatingController(IUserRepository userRepository, IBlogRepository blogRepository, IBlogRatingRepository blogRatingRepository, IConfiguration configuration, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.blogRatingRepository = blogRatingRepository;
            this.blogRepository = blogRepository;
            this._configuration = configuration;
            this._mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<ActionResult<APIResponse<IEnumerable<BlogRatingDto>>>> GetAll()
        {
            var blogRatings = await blogRatingRepository.GetAllBlogRating();
            return new APIResponse<IEnumerable<BlogRatingDto>>
            {
                Data = _mapper.Map<IEnumerable<BlogRatingDto>>(blogRatings),
                Message = "Get all blogRating success",
                IsSuccess = true
            };
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<APIResponse<BlogRatingDto>>> GetDetail([FromRoute] Guid id)
        {
            var blogRating = await blogRatingRepository.FindBlogRatingByID(id);
            return new APIResponse<BlogRatingDto>
            {
                Data = _mapper.Map<BlogRatingDto>(blogRating),
                Message = "Get detail blogRating success",
                IsSuccess = true
            };
        }

        [Authorize]
        [HttpPost("rating")]
        public async Task<ActionResult<APIResponse<Object>>> Rating(CreateBlogRatingDto data)
        {
            var currentUser = HttpContext.User;
            string userId = currentUser.FindFirst("UserID").Value;

            var blog = await blogRepository.FindBlogByID(data.BlogID, null, null);
            if (blog == null)
            {
                return new NotFoundObjectResult(new APIResponse<Object>
                {
                    Message = "Blog not found",
                    IsSuccess = false
                });
            }

            try
            {
                var result = await blogRatingRepository.RatingBlog(data, new Guid(userId));

                if (result == 0)
                    return new BadRequestObjectResult(new APIResponse<Object>
                    {
                        Message = "Rating blog fail",
                        IsSuccess = false
                    });

                return new APIResponse<Object>
                {
                    Data = result,
                    Message = "Rating Blog success",
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new APIResponse<Object>
                {
                    Message = e.Message,
                    IsSuccess = false
                });
            }
            
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<APIResponse<Object>>> Create(CreateBlogRatingDto data)
        {
            var currentUser = HttpContext.User;
            string userId = currentUser.FindFirst("UserID").Value;

            var blog = await blogRepository.FindBlogByID(data.BlogID, null, null);
            if (blog == null)
            {
                return new NotFoundObjectResult(new APIResponse<Object>
                {
                    Message = "Blog not found",
                    IsSuccess = false
                });
            }

            var result = await blogRatingRepository.CreateBlogRating(data, new Guid(userId));

            if(result==0)
                return new APIResponse<Object>
                {
                    Data = result,
                    Message = "Create BlogRating fail",
                    IsSuccess = false
                };

            return new APIResponse<Object>
            {
                Data = result,
                Message = "Create BlogRating success",
                IsSuccess = true
            };
        }

        [Authorize]
        [HttpPut("update/{id:guid}")]
        public async Task<ActionResult<APIResponse<Object>>> Update([FromRoute] Guid id, UpdateBlogRatingDto data)
        {
            var currentUser = HttpContext.User;
            string userId = currentUser.FindFirst("UserID").Value;
            var blogRating = await blogRatingRepository.FindBlogRatingByID(id);
            if (blogRating == null)
            {
                return new NotFoundObjectResult(new APIResponse<Object>
                {
                    Message = "Blog rating not found",
                    IsSuccess = false
                });
            }

            var result = await blogRatingRepository.UpdateBlogRating(id, new Guid(userId), data);
            if (result == 0)
                return new APIResponse<Object>
                {
                    Data = result,
                    Message = "Update BlogRating fail",
                    IsSuccess = false
                };

            return new APIResponse<Object>
            {
                Data = result,
                Message = "Update BlogRating success",
                IsSuccess = true
            };
        }
    }
}
