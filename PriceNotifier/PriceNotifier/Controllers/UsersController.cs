﻿using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using BLL.Services.UserService;
using Domain.Entities;
using PriceNotifier.DTO;

namespace PriceNotifier.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUserService<User> _userService;

        public UsersController(IUserService<User> userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        public async Task<IEnumerable<UserDto>> GetUsers(string sortDataField,  string sortOrder, string filter,  string filterColumn, int currentPage=1, int recordsPerPage=25)

        {
            var users = await _userService.Get(sortDataField, sortOrder,filter, filterColumn,currentPage,recordsPerPage);
            return Mapper.Map<IEnumerable<UserDto>>(users);
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
                throw new HttpResponseException(HttpStatusCode.BadRequest);
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