using Constants;
using Constants.Enums;
using DB_EFCore.DataAccessLayer;
using DB_EFCore.Entity;
using DB_EFCore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DB_EFCore.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly AppDbContext _context;
        public ContactRepository(AppDbContext context)
        {
            _context = context;
        }
        public Contact? GetContact(int id)
        {
            return _context.Contact.Find(id);
        }

        public async Task<Contact?> GetContactAsync(int id)
        {
            return await _context.Contact.FindAsync(id);
        }

        public List<Contact> GetContacts()
        {
            return _context.Contact.ToList();
        }

        public async Task<List<Contact>> GetContactsAsync()
        {
            return await _context.Contact.ToListAsync();
        }

        public async Task<int> GetContactsCount()
        {
            return (await _context.Contact.ToListAsync()).Count;
        }

        public async Task<ResultSet> SaveContactAsync(Contact contact)
        {
            ResultSet result = new ResultSet();
            try
            {
                await _context.AddAsync(contact);
                int count = await _context.SaveChangesAsync();
                if (count > 0)
                {
                    result.Id = contact.Id;
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
