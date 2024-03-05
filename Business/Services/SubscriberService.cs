using Business.Interfaces;
using Constants.Enums;
using Constants;
using DB_EFCore.DataAccessLayer;
using DB_EFCore.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            return context.Subscriber.Find(id);
        }

        public async Task<Subscriber?> GetSubscriberAsync(int id)
        {
            return await context.Subscriber.FindAsync(id);
        }

        public List<Subscriber> GetSubscribers()
        {
            return context.Subscriber.ToList();
        }

        public async Task<List<Subscriber>> GetSubscribersAsync()
        {
            return await context.Subscriber.ToListAsync();
        }

        public async Task<ResultSet> SaveSubscriberAsync(string subMail)
        {
            ResultSet result = new ResultSet();
            try
            {
                Subscriber sub = await context.Subscriber.FirstOrDefaultAsync(x => x.Mail == subMail);
                if (sub != null)
                {
                    result.Result = Result.Fail;
                    result.Message = "Girilen mail ile abonelik mevcut!";
                }
                else
                {
                    sub = new Subscriber();
                    sub.Mail = subMail;
                    sub.RegisterDate = DateTime.Now;
                    await context.AddAsync(sub);
                    int count = await context.SaveChangesAsync();
                    if (count > 0)
                    {
                        result.Id = sub.Id;
                    }
                    else
                    {
                        result.Result = Result.Fail;
                        result.Message = "Db işlemi başarısız";
                    }
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
