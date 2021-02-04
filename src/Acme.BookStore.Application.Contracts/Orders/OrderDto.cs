using Acme.BookStore.Books;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Orders
{
    public class OrderDto : AuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public BookDto Book { get; set; }
    }
}