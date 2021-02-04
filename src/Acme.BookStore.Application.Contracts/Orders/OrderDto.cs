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
        public User User { get; set; }
        public Boolean IsApproved { get; set; }
    }

    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }
}