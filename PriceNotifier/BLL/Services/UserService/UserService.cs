using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task Update(User user)
        {
            await _userRepository.Update(user);
        }

        public async Task<User> GetById(int userId)
        {
            User user = await _userRepository.Get(userId);
            return user;
        }

        public async Task Delete(User user)
        {
            await _userRepository.Delete(user);
        }

        public async Task<PageResult<UserFromDbWithCount>> Get(string sortDataField, string sortOrder, string filter, string filterField, int currentPage, int recordsPerPage)
        {
            
            var begin = (currentPage - 1) * recordsPerPage;
            var query = _userRepository.Query();

            var queryFinal = query.GroupJoin(_productRepository.Query(), user => user.Id, product => product.UserId,
                (user, product) => new UserFromDbWithCount
                {
                    Id=user.Id,
                    Username = user.Username,
                    SocialNetworkUserId = user.SocialNetworkUserId,
                    SocialNetworkName = user.SocialNetworkName,
                    CountTrackedItems= product.Count()
                });

            switch (filterField)
            {
                case "Id":
                    queryFinal = queryFinal.OrderBy(x => x.Id).Where(x => x.Id.ToString().Contains(filter));
                    break;
                case "Username":
                    queryFinal = queryFinal.OrderBy(x => x.Id).Where(x => x.Username.Contains(filter));
                    break;
            }

            switch (sortOrder)
            {
                case "asc":
                    if (sortDataField == "Id")
                    {
                        queryFinal = queryFinal.OrderBy(x => x.Id);
                    }

                    if (sortDataField == "Username")
                    {
                        queryFinal = queryFinal.OrderBy(x => x.Username);
                    }

                    if (sortDataField == "CountTrackedItems")
                    {
                        queryFinal = queryFinal.OrderBy(x => x.CountTrackedItems);
                    }

                    break;

                case "desc":
                    if (sortDataField == "Id")
                    {
                        queryFinal = queryFinal.OrderByDescending(x => x.Id);
                    }

                    if (sortDataField == "Username")
                    {
                        queryFinal = queryFinal.OrderByDescending(x => x.Username);
                    }

                    if (sortDataField == "CountTrackedItems")
                    {
                        queryFinal = queryFinal.OrderByDescending(x => x.CountTrackedItems);
                    }

                    break;
                default:
                    queryFinal = queryFinal.OrderBy(x => x.Id);
                    break;
            }

            //if (filterField == "Id" || filterField == "Username")
            //{
            //    var m = await queryFinal.Skip(begin).Take(recordsPerPage).ToListAsync();
            //    //var s = Mapper.Map<IEnumerable<UserFromDbWithCount>>(m);
            //    return new PageResult<UserFromDbWithCount> { Data = m, TotalItems = totalItems };
            //}

            //if (sortOrder == "asc" || sortOrder == "desc")
            //{
            //    var m = await queryFinal.Skip(begin).Take(recordsPerPage).ToListAsync();
            //    //var s = Mapper.Map<IEnumerable<UserFromDbWithCount>>(m);
            //    return new PageResult<UserFromDbWithCount> { Data = m, TotalItems = totalItems };
            //}

            var totalItems = queryFinal.Count();
            var a = await queryFinal.Skip(begin).Take(recordsPerPage).ToListAsync();

            return new PageResult<UserFromDbWithCount>
            {
                Data = a,
                TotalItems = totalItems
            };
        }
    }
}
