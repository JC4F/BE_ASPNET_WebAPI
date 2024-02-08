using AutoMapper;
using BusinessObject.Commons;
using BusinessObject.Dtos;
using BusinessObject.Models;
using DataAccess.Interfaces;
using ImageService;
using ImageService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IBlogRepository blogRepository;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public BlogController(IUserRepository userRepository, IBlogRepository blogRepository, ICloudinaryService cloudinaryService, IConfiguration configuration, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.blogRepository = blogRepository;
            this.cloudinaryService = cloudinaryService; 
            this._configuration = configuration;
            this._mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<ActionResult<APIResponse<IEnumerable<BlogDto>>>> GetAll()
        {
            var currentUser = HttpContext.User;
            var role = currentUser?.FindFirst(ClaimTypes.Role)?.Value ?? null;
            var userId = currentUser?.FindFirst("UserID")?.Value ?? null;
            var blogs = await blogRepository.GetAllBlog(role, userId);
            return new APIResponse<IEnumerable<BlogDto>>
            {
                Data = _mapper.Map<IEnumerable<BlogDto>>(blogs),
                Message = "Get all blog success",
                IsSuccess = true
            };
        }

        [Authorize(Roles = ROLE.ADMIN)]
        [HttpGet("all-pending")]
        public async Task<ActionResult<APIResponse<IEnumerable<BlogDto>>>> GetAllPendng()
        {
            var blogs = await blogRepository.GetAllPendingBlog();
            return new APIResponse<IEnumerable<BlogDto>>
            {
                Data = _mapper.Map<IEnumerable<BlogDto>>(blogs),
                Message = "Get all blog success",
                IsSuccess = true
            };
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<APIResponse<BlogDto>>> GetDetail([FromRoute] Guid id)
        {
            var currentUser = HttpContext.User;
            var role = currentUser?.FindFirst(ClaimTypes.Role)?.Value ?? null;
            var userId = currentUser?.FindFirst("UserID")?.Value ?? null;
            var blog = await blogRepository.FindBlogByID(id, role, userId);
            return new APIResponse<BlogDto>
            {
                Data = _mapper.Map<BlogDto>(blog),
                Message = "Get detail blog success",
                IsSuccess = true
            };
        }

        [HttpPost("upload-image")]
        public async Task<ActionResult<APIResponse<string>>> UploadImage([FromForm]ImageUploadDto data)
        {
            var result = await cloudinaryService.UploadPicture(data);
            return new APIResponse<string>
            {
                Data = result,
                Message = "Upload to cloudinary success",
                IsSuccess = true
            };
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<APIResponse<Object>>> Create(CreateBlogDto data)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserID").Value;

            var result = await blogRepository.CreateBlog(data, new Guid(userId));
            if(result==0)
                return new APIResponse<Object>
                {
                    Data = result,
                    Message = "Create blog fail",
                    IsSuccess = false
                };

            return new APIResponse<Object>
            {
                Data = result,
                Message = "Create blog success",
                IsSuccess = true
            };
        }

        [Authorize]
        [HttpPut("update/{id:guid}")]
        public async Task<ActionResult<APIResponse<Object>>> Update([FromRoute] Guid id, UpdateBlogDto data)
        {
            var currentUser = HttpContext.User;
            string role = currentUser.FindFirst(ClaimTypes.Role).Value;
            string userId = currentUser.FindFirst("UserID").Value;

            try
            {
                var result = await blogRepository.UpdateBlog(id, new Guid(userId), role, data);
                if (result == 0)
                    return new APIResponse<Object>
                    {
                        Data = result,
                        Message = "Update blog fail",
                        IsSuccess = false
                    };

                return new APIResponse<Object>
                {
                    Data = result,
                    Message = "Update blog success",
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new APIResponse<Object>
                {
                    Message = e.Message,
                    IsSuccess = true
                });
            }
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<APIResponse<Object>>> Delete([FromRoute] Guid id)
        {
            var currentUser = HttpContext.User;
            string role = currentUser.FindFirst(ClaimTypes.Role).Value;
            string userId = currentUser.FindFirst("UserID").Value;

            try
            {
                var result = await blogRepository.DeleteBlog(id, new Guid(userId), role);
                if (result == 0)
                    return new APIResponse<Object>
                    {
                        Data = result,
                        Message = "Delete blog fail",
                        IsSuccess = false
                    };

                return new APIResponse<Object>
                {
                    Data = result,
                    Message = "Delete blog success",
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new APIResponse<Object>
                {
                    Message = e.Message,
                    IsSuccess = true
                });
            }
        }
    }
}
