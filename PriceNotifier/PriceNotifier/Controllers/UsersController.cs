using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using BLL;
using BLL.Services.UserService;
using Domain.Entities;
using PriceNotifier.DTO;

namespace PriceNotifier.Controllers
{
    [RoutePrefix("api/books")]
    public class UsersController : ApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
       
        public async Task<PageResult<UserDtoWithCount>> GetUsers(string sortDataField,  string sortOrder, string filter,  string filterColumn, int? currentPage, int? recordsPerPage)
        {
            if (!currentPage.HasValue)
            {
                currentPage = 1;
            }

            if (!recordsPerPage.HasValue)
            {
                recordsPerPage = 25;
            }

            var users = await _userService.Get(sortDataField, sortOrder,filter, filterColumn,currentPage.Value,recordsPerPage.Value);
            return new PageResult<UserDtoWithCount>
            {
                Data = Mapper.Map<IEnumerable<UserDtoWithCount>>(users.Data),
                TotalItems = users.TotalItems
            };
        }

        // GET: api/Users/5
        [ResponseType(typeof(UserDto))]
        public async Task<UserDto> GetUser(int id)
        {
            User user = await _userService.GetById(id);
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return Mapper.Map<User, UserDto>(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(UserDto))]
        public async Task<UserDto> PutUser(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                throw new HttpResponseException(response);
            }

            User user = await _userService.GetById(userDto.Id);

            if (user != null)
            {
                var userUpdated = Mapper.Map(userDto, user);
                await _userService.Update(userUpdated);
                userDto = Mapper.Map(userUpdated, userDto);
                return userDto;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        // POST: api/Users
        [ResponseType(typeof(UserDto))]
        public async Task<UserDto> PostUser(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var userFound = _userService.GetById(userDto.Id);
            if (userFound == null)
            {
                var user = Mapper.Map<UserDto, User>(userDto);
                var userUpdated = await _userService.Create(user);
                userDto = Mapper.Map(userUpdated, userDto);
                return userDto;
            }

            throw new HttpResponseException(HttpStatusCode.Conflict);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = await _userService.GetById(id);
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            await _userService.Delete(user);
            return Ok();
        }
    }
}