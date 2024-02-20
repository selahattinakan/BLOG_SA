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
    public class ContactService
    {
        public Contact? GetContact(int id)
        {
            using (var context = new AppDbContext())
            {
                return context.Contact.FirstOrDefault(x => x.Id == id);
            }
        }

        public async Task<Contact?> GetContactAsync(int id)
        {
            using (var context = new AppDbContext())
            {
                return await context.Contact.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public List<Contact> GetContacts()
        {
            using (var context = new AppDbContext())
            {
                return context.Contact.ToList();
            }
        }

        public async Task<List<Contact>> GetContactsAsync()
        {
            using (var context = new AppDbContext())
            {
                return await context.Contact.ToListAsync();
            }
        }
    }
}
