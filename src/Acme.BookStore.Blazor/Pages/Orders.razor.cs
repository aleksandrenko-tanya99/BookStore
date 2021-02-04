using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Orders;
using Acme.BookStore.Permissions;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Blazor.Pages
{
    public partial class Orders
    {
        private IReadOnlyList<OrderDto> OrderList { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await GetOrdersAsync();
        }
        private async Task GetOrdersAsync()
        {
            var result = await AppService.GetListAsync(
                new GetPagedListOrdersDto
                {
                    UserId = (Guid)CurrentUser.Id,
                    MaxResultCount = PageSize,
                    SkipCount = CurrentPage * PageSize,
                    Sorting = CurrentSorting
                }
            );

            OrderList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

    }
}