using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using AutoMapper;
using Acme.BookStore.Orders;

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
        }
    }
}