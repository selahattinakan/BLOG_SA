using Constants;
using DB_EFCore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ISubscriberService
    {
        public Subscriber? GetSubscriber(int id);
        public Task<Subscriber?> GetSubscriberAsync(int id);
        public List<Subscriber> GetSubscribers();
        public Task<List<Subscriber>> GetSubscribersAsync();
        public Task<ResultSet> SaveSubscriberAsync(string subMail);
    }
}
