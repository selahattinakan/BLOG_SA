using Constants;
using DB_EFCore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_EFCore.Repositories.Interfaces
{
    public interface ISubscriberRepository
    {
        public Subscriber? GetSubscriber(int id);
        public Task<Subscriber?> GetSubscriberAsync(int id);
        public Task<Subscriber?> GetSubscriberAsync(string mail);
        public List<Subscriber> GetSubscribers();
        public Task<List<Subscriber>> GetSubscribersAsync();
        public Task<ResultSet> SaveSubscriberAsync(Subscriber subscriber);
    }
}
