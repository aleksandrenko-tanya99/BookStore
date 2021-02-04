using Acme.BookStore.Books;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.BookStore.Orders
{
    public class Order : AuditedAggregateRoot<Guid>
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public Order(Guid userId, Guid bookId)
        {
            UserId = userId;
            BookId = bookId;
        }
    }
}