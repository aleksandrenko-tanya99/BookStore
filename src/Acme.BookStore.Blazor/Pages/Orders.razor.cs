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

        private async Task ApproveOrder(OrderDto order)
        {
            await AppService.UpdateAsync(order.Id, new UpdateOrderDto() { IsApproved = true });
            await GetOrdersAsync();
            StateHasChanged();
        }

        private async Task DeclineOrder(OrderDto order)
        {
            await AppService.DeleteAsync(order.Id);
            await GetOrdersAsync();
            StateHasChanged();
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
        protected override async Task OnDataGridReadAsync(DataGridReadDataEventArgs<OrderDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.Direction != SortDirection.None)
                .Select(c => c.Field + (c.Direction == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page - 1;

            await GetOrdersAsync();

            StateHasChanged();
        }

    }
}