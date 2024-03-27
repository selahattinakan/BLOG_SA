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
        private readonly AppDbContext _context;
        public SubscriberService(AppDbContext context)
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

        public List<Subscriber> GetSubscribers()
        {
            return _context.Subscriber.ToList();
        }

        public async Task<List<Subscriber>> GetSubscribersAsync()
        {
            return await _context.Subscriber.ToListAsync();
        }

        public async Task<ResultSet> SaveSubscriberAsync(string subMail)
        {
            ResultSet result = new ResultSet();
            try
            {
                Subscriber sub = await _context.Subscriber.FirstOrDefaultAsync(x => x.Mail == subMail);
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
                    await _context.AddAsync(sub);
                    int count = await _context.SaveChangesAsync();
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
