using Business.Interfaces;
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
            using (context)
            {
                return context.Contact.FirstOrDefault(x => x.Id == id);
            }
        }

        public async Task<Contact?> GetContactAsync(int id)
        {
            using (context)
            {
                return await context.Contact.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public List<Contact> GetContacts()
        {
            using (context)
            {
                return context.Contact.ToList();
            }
        }

        public async Task<List<Contact>> GetContactsAsync()
        {
            using (context)
            {
                return await context.Contact.ToListAsync();
            }
        }
    }
}
