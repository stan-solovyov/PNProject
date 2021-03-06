﻿using System.Linq;
using System.Threading.Tasks;
using BLL.Models;
using Domain.Entities;
using Domain.Repository;

namespace BLL.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Product> _productRepository;

        public UserService(IRepository<User> userRepository, IRepository<Product> productRepository)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task<User> Create(User user)
        {
            await _userRepository.Create(user);
            return user;
        }

        public Task Update(User user)
        {
            return _userRepository.Update(user);
        }

        public async Task<User> GetById(int userId)
        {
            User user = await _userRepository.Get(userId);
            return user;
        }

        public Task Delete(User user)
        {
            return _userRepository.Delete(user);
        }

        public IQueryable<UserFromDbWithCount> Get()
        {

            var query = _userRepository.Query();

            var queryFinal = query.GroupJoin(_productRepository.Query(), user => user.Id, product => product.Id,
                (user, product) => new UserFromDbWithCount
                {
                    Id = user.Id,
                    Username = user.Username,
                    SocialNetworkUserId = user.SocialNetworkUserId,
                    SocialNetworkName = user.SocialNetworkName,
                    CountTrackedItems = user.UserProducts.Count(c => c.Checked)
                });

            return queryFinal;
        }
    }
}
