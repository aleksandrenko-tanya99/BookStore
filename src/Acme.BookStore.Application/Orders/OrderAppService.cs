using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using Acme.BookStore.Permissions;
using Acme.BookStore.Users;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Orders
{

    [Authorize(BookStorePermissions.Orders.Default)]

    public class OrderAppService :
        CrudAppService<
            Order,
            OrderDto,
            Guid,
            GetPagedListOrdersDto,
             CreateOrderDto>,
        IOrderAppService
    {
        private readonly IRepository<Book, Guid> _bookRepository;
        private readonly IRepository<Author, Guid> _authorRepository;
        private readonly IRepository<AppUser, Guid> _userRepository;

        public OrderAppService(
            IRepository<Order, Guid> orderRepository,
            IRepository<Author, Guid> authorRepository,
            IRepository<AppUser, Guid> userRepository,
            IRepository<Book, Guid> bookRepository) : base(orderRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _userRepository = userRepository;

            GetPolicyName = BookStorePermissions.Orders.Default;
            GetListPolicyName = BookStorePermissions.Orders.Default;
            CreatePolicyName = BookStorePermissions.Orders.Create;
            UpdatePolicyName = BookStorePermissions.Orders.Edit;
            DeletePolicyName = BookStorePermissions.Orders.Create;
        }

        public async Task<OrderDto> GetOrderByContentAsync(CreateOrderDto input)
        {
            await CheckGetPolicyAsync();

            if (!(await Repository.AnyAsync(o => o.UserId == input.UserId && o.BookId == input.BookId))) return null;

            var query = from order in Repository
                        join user in _userRepository on order.UserId equals user.Id
                        join book in _bookRepository on order.BookId equals book.Id
                        join author in _authorRepository on book.AuthorId equals author.Id
                        where order.UserId == input.UserId &&
                        book.Id == input.BookId
                        select new { order, book, author };

            var result = await AsyncExecuter.FirstAsync(query);

            var orderDto = ObjectMapper.Map<Order, OrderDto>(result.order);
            orderDto.Book = ObjectMapper.Map<Book, BookDto>(result.book);
            orderDto.Book.AuthorName = result.author.Name;
            return orderDto;
        }

        public override async Task<OrderDto> CreateAsync(CreateOrderDto input)
        {
            await CheckGetListPolicyAsync();

            var order = new Order(
                input.UserId,
                input.BookId
            );

            await Repository.InsertAsync(order);
            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        public override async Task<PagedResultDto<OrderDto>> GetListAsync(GetPagedListOrdersDto input)
        {
            await CheckGetListPolicyAsync();

            var isUserAdmin = await _userRepository.AnyAsync(x => x.Name == "admin" && x.Id == input.UserId);

            var query = from order in Repository
                        join book in _bookRepository on order.BookId equals book.Id
                        join user in _userRepository on order.UserId equals user.Id
                        join author in _authorRepository on book.AuthorId equals author.Id
                        select new { order, book, author, user };

            var result = await AsyncExecuter.ToListAsync(query);

            var totalCount = result.Count();

            var orderDtos = result.Select(o => {
                var order = ObjectMapper.Map<Order, OrderDto>(o.order);
                order.Book = ObjectMapper.Map<Book, BookDto>(o.book);
                order.User = ObjectMapper.Map<AppUser, User>(o.user);
                order.Book.AuthorName = o.author.Name;
                return order;
            }).ToList();

            if (!isUserAdmin)
            {
                orderDtos = orderDtos.Where(order => order.UserId == input.UserId).ToList();
            }

            return new PagedResultDto<OrderDto>(
                totalCount,
                orderDtos
            );
        }
    }
}