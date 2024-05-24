using Constants;
using DB_EFCore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_EFCore.Repositories.Interfaces
{
    public interface IContactRepository
    {
        public Contact? GetContact(int id);
        public Task<Contact?> GetContactAsync(int id);
        public List<Contact> GetContacts();
        public Task<List<Contact>> GetContactsAsync();
        public Task<ResultSet> SaveContactAsync(Contact contact);
        public Task<int> GetContactsCount();
    }
}
