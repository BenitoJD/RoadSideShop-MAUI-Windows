﻿using RoadSideShop.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadSideShop.Data
{
    public  class DatabaseService : IAsyncDisposable
    {
        private readonly SQLiteAsyncConnection _connection;
        public DatabaseService()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "RoadSideShopDb.db3");
            _connection =  new SQLiteAsyncConnection(dbPath,
                SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache);
        }
        public async Task InitializeDatabaseAsync()
        {
            await _connection.CreateTableAsync<MenuCategory>();
            await _connection.CreateTableAsync<MenuItem>();
            await _connection.CreateTableAsync<MenuItemCategoryMapping>();
            await _connection.CreateTableAsync<Order>();
            await _connection.CreateTableAsync<OrderItem>();

            await SeedDataAsync();

        }
        public async Task SeedDataAsync()
        {
            var firstCategory = await _connection.Table<MenuCategory>().FirstOrDefaultAsync();
            if (firstCategory != null)
            {
                return;
            }
            var catergories = SeedData.GetMenuCategories();
            var menuItems = SeedData.GetMenuItems();
            var menuItemCategoryMappings = SeedData.GetMenuItemCategoryMappings();

            await _connection.InsertAllAsync(catergories);
            await _connection.InsertAllAsync(menuItems);
            await _connection.InsertAllAsync(menuItemCategoryMappings);
        }
        public async Task<MenuCategory[]> GetMenuCategoriesAsync()
        {
            return await _connection.Table<MenuCategory>().ToArrayAsync();
        }

        public async Task<MenuItem[]> GetMenuItemsByCategoryAsync(int categoryId)
        {
            var query = @"SELECT menu.* FROM MenuItem AS menu
                        JOIN MenuItemCategoryMapping AS mapping
                        ON menu.Id = mapping.MenuItemId
                        WHERE mapping.MenuCategoryId = ?";
            var menuItem = await _connection.QueryAsync<MenuItem>(query, categoryId);
            return [.. menuItem];
        }
        public async Task<string?> PlaceOrderAsync(OrderModel orderModel)
        {
            var order = new Order
            {
                OrderDate = orderModel.OrderDate,
                TotalAmount = orderModel.TotalAmount,
                TotalItemsCount = orderModel.Items.Length,
                PaymentMode = orderModel.PaymentMode,
                
            };

            if (await _connection.InsertAsync(order) > 0)
            {
                foreach (var item in orderModel.Items)
                {

                    item.OrderId = order.Id;

                }
                if (await _connection.InsertAllAsync(orderModel.Items) == 0)
                {
                    await _connection.DeleteAsync(order);
                    return "Error in inserting order items";
                }
            }
            else
            {
                return "Error in inserting order";
            }
            orderModel.Id = order.Id;

            return null;
        }
        public async ValueTask DisposeAsync()
        {
            if (_connection != null)
            {
               await _connection.CloseAsync();
            }
        }
        public async Task<Order[]> GetOrdersAsync()
        {
            return await _connection.Table<Order>().ToArrayAsync();
        }

        public async Task<OrderItem[]> GetOrderItemsAsync(long orderId)
        {
            return await _connection.Table<OrderItem>().Where(x => x.OrderId == orderId).ToArrayAsync();
        }
    }
}
