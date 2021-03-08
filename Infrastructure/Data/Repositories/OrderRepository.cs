﻿using Core.Entities.OrderModels;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{

    public class OrderRepository : IOrderRepository
    {
        private readonly WebshopDataContext _context;
        public OrderRepository(WebshopDataContext context)
        {
            _context = context;
        }
        public async Task<Order> CreateOrder(Order model)
        {
            _context.Orders.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<Order> DeleteOrder(int id)
        {
            var order = _context.Orders.FindAsync(id).Result;
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> EditOrder(Order model)
        {
            _context.Orders.Update(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<Order> GetOrder(int id,string email)
        {          
            return await _context.Orders.Where(x => x.Id == id && x.BuyerEmail == email)
               .Include(x => x.DeliveryMethod)              
               .Include(x=>x.ShipToAddress)    
               .Include(x=>x.OrderItems)
               .FirstOrDefaultAsync();
            
        }        

        public async Task<IReadOnlyList<Order>> GetOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUser(string email)
        {          
            return await _context.Orders.Where(x=>x.BuyerEmail == email)
               .Include(x => x.DeliveryMethod)
               .Include(x => x.OrderItems)
               .Include(x => x.ShipToAddress)
               .OrderByDescending(x => x.OrderDate)               
               .ToListAsync();
        }
    }
}