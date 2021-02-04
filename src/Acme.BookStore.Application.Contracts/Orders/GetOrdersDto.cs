using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Acme.BookStore.Orders
{
    public class GetOrdersDto
    {
        [Required]
        public Guid UserId { get; set; }
    }
}