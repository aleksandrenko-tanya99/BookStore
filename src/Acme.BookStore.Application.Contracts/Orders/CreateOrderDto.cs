using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Acme.BookStore.Orders
{
    public class CreateOrderDto
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid BookId { get; set; }
    }
}