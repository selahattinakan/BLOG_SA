﻿using Business.Interfaces;
using DB_EFCore.DataAccessLayer;
using DB_EFCore.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class SubscriberService : ISubscriberService
    {
        private readonly AppDbContext context;
        public SubscriberService(AppDbContext _context)
        {
            context = _context;
        }
        public Subscriber? GetSubscriber(int id)
        {
            using (context)
            {
                return context.Subscriber.FirstOrDefault(x => x.Id == id);
            }
        }

        public async Task<Subscriber?> GetSubscriberAsync(int id)
        {
            using (context)
            {
                return await context.Subscriber.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public List<Subscriber> GetSubscribers()
        {
            using (context)
            {
                return context.Subscriber.ToList();
            }
        }

        public async Task<List<Subscriber>> GetSubscribersAsync()
        {
            using (context)
            {
                return await context.Subscriber.ToListAsync();
            }
        }
    }
}
