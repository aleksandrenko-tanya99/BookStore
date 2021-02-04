using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using AutoMapper;
using Acme.BookStore.Orders;
using Acme.BookStore.Users;

namespace Acme.BookStore
{
    public class BookStoreApplicationAutoMapperProfile : Profile
    {
        public BookStoreApplicationAutoMapperProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<CreateUpdateBookDto, Book>();

            CreateMap<Author, AuthorDto>();

            CreateMap<Author, AuthorLookupDto>();

            CreateMap<Order, OrderDto>();
            CreateMap<CreateOrderDto, OrderDto>();

            CreateMap<AppUser, User>();
        }
    }
}