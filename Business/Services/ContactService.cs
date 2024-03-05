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

namespace Business.Services
{
    public class ContactService : IContactService
    {
        private readonly AppDbContext context;
        public ContactService(AppDbContext _context)
        {
            context = _context;
        }
        public Contact? GetContact(int id)
        {
            return context.Contact.Find(id);
        }

        public async Task<Contact?> GetContactAsync(int id)
        {
            return await context.Contact.FindAsync(id);
        }

        public List<Contact> GetContacts()
        {
            return context.Contact.ToList();
        }

        public async Task<List<Contact>> GetContactsAsync()
        {
            return await context.Contact.ToListAsync();
        }

        public async Task<ResultSet> SaveContactAsync(Contact contact)
        {
            ResultSet result = new ResultSet();
            try
            {
                DbState state = DbState.Update;
                Contact? data = await context.Contact.FirstOrDefaultAsync(x => x.Id == contact.Id);
                if (data == null)
                {
                    data = new Contact();
                    state = DbState.Insert;
                }
                data.FullName = contact.FullName;
                data.Mail = contact.Mail;
                data.Subject = contact.Subject;
                data.Message = contact.Message;
                data.IsAnswered = contact.IsAnswered;

                if (state == DbState.Insert)
                {
                    data.RegisterDate = DateTime.Now;
                    await context.AddAsync(data);
                }

                int count = await context.SaveChangesAsync();
                if (count > 0)
                {
                    result.Id = data.Id;
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
