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
    public class SubscriberService
    {
        public Subscriber? GetSubscriber(int id)
        {
            using (var context = new AppDbContext())
            {
                return context.Subscriber.FirstOrDefault(x => x.Id == id);
            }
        }

        public async Task<Subscriber?> GetSubscriberAsync(int id)
        {
            using (var context = new AppDbContext())
            {
                return await context.Subscriber.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public List<Subscriber> GetSubscribers()
        {
            using (var context = new AppDbContext())
            {
                return context.Subscriber.ToList();
            }
        }

        public async Task<List<Subscriber>> GetSubscribersAsync()
        {
            using (var context = new AppDbContext())
            {
                return await context.Subscriber.ToListAsync();
            }
        }
    }
}
