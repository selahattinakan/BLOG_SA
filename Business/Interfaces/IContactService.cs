using DB_EFCore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IContactService
    {
        public Contact? GetContact(int id);
        public Task<Contact?> GetContactAsync(int id);
        public List<Contact> GetContacts();
        public Task<List<Contact>> GetContactsAsync();
    }
}
