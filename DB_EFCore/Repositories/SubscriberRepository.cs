using Constants;
using Constants.Enums;
using DB_EFCore.DataAccessLayer;
using DB_EFCore.Entity;
using DB_EFCore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DB_EFCore.Repositories
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly AppDbContext _context;
        public SubscriberRepository(AppDbContext context)
        {
            _context = context;
        }
        public Subscriber? GetSubscriber(int id)
        {
            return _context.Subscriber.Find(id);
        }

        public async Task<Subscriber?> GetSubscriberAsync(int id)
        {
            return await _context.Subscriber.FindAsync(id);
        }

        public async Task<Subscriber?> GetSubscriberAsync(string mail)
        {
            return await _context.Subscriber.FirstOrDefaultAsync(x => x.Mail == mail);
        }

        public List<Subscriber> GetSubscribers()
        {
            return _context.Subscriber.ToList();
        }

        public async Task<List<Subscriber>> GetSubscribersAsync()
        {
            return await _context.Subscriber.ToListAsync();
        }

        public async Task<ResultSet> SaveSubscriberAsync(Subscriber subscriber)
        {
            ResultSet result = new ResultSet();
            try
            {
                await _context.AddAsync(subscriber);
                int count = await _context.SaveChangesAsync();
                if (count > 0)
                {
                    result.Id = subscriber.Id;
                }
                else
                {
                    result.Result = Result.Fail;
                    result.Message = "Db işlemi başarısız";
                }
            }
            catch (Exception ex)
            {
                result.Result = Result.Fail;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
