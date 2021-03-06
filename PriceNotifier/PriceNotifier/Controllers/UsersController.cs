﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.OData;
using System.Web.OData.Extensions;
using System.Web.OData.Query;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BLL.Services.ProductService;
using BLL.Services.UserService;
using Domain.Entities;
using PriceNotifier.AuthFilter;
using PriceNotifier.DTO;
using PriceNotifier.Infrostructure;

namespace PriceNotifier.Controllers
{
    [TokenAuthorize("Admin")]
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public UsersController(IUserService userService, IProductService productService)
        {
            _userService = userService;
            _productService = productService;
        }

        // GET: api/Users
        public PageResult<UserDtoWithCount> GetUsers(ODataQueryOptions<UserDtoWithCount> options)
        {
            var allUsers = _userService.Get();
            var users = allUsers.ProjectTo<UserDtoWithCount>();
            ODataQuerySettings settings = new ODataQuerySettings()
            {
                PageSize = 100
            };
            var results = options.ApplyTo(users,settings);

            return new PageResult<UserDtoWithCount>(
                results as IEnumerable<UserDtoWithCount>,
                Request.ODataProperties().NextLink,
                Request.ODataProperties().TotalCount);
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

        // DELETE: api/Users/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = await _userService.GetById(id);
            var up = user.UserProducts.Where(a => a.UserId == user.Id).Select(c => c.ProductId).ToList();

            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            foreach (var productId in up)
            {
                Product product = await _productService.GetById(productId);
                if (product.UserProducts.Count(c => c.ProductId == productId) == 1)
                {
                    await _productService.Delete(product);
                }
            }

            await _userService.Delete(user);
            return Ok();
        }
    }
}