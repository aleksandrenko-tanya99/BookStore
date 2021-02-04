﻿using Acme.BookStore.Orders;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Orders
{
    public interface IOrderAppService :
        ICrudAppService<
            OrderDto,
            Guid,
            GetPagedListOrdersDto,
            CreateOrderDto,
            UpdateOrderDto>
    {
        public Task<OrderDto> GetOrderByContentAsync(CreateOrderDto input);
    }
}