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
    }
}
