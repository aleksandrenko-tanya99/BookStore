using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using Acme.BookStore.Permissions;
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

        public OrderAppService(
            IRepository<Order, Guid> orderRepository,
            IRepository<Author, Guid> authorRepository,
            IRepository<Book, Guid> bookRepository) : base(orderRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            GetPolicyName = BookStorePermissions.Orders.Default;
            GetListPolicyName = BookStorePermissions.Orders.Default;
            CreatePolicyName = BookStorePermissions.Orders.Create;
            UpdatePolicyName = BookStorePermissions.Orders.Edit;
            DeletePolicyName = BookStorePermissions.Orders.Create;
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

            var query = from order in Repository
                        join book in _bookRepository on order.BookId equals book.Id
                        join author in _authorRepository on book.AuthorId equals author.Id
                        where order.UserId == input.UserId
                        select new { order, book, author };

            var result = await AsyncExecuter.ToListAsync(query);

            var totalCount = result.Count();

            var orderDtos = result.Select(o => {
                var order = new OrderDto();
                order.Id = o.order.Id;
                order.UserId = o.order.UserId;
                order.Book = ObjectMapper.Map<Book, BookDto>(o.book);
                order.Book.AuthorName = o.author.Name;
                return order;
            }).ToList();

            return new PagedResultDto<OrderDto>(
                totalCount,
                orderDtos
            );
        }
    }
}