using Business.Interfaces;
using Constants;
using Constants.Enums;
using DB_EFCore.Entity;
using DB_EFCore.Repositories.Interfaces;

namespace Business.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _repository;
        public ContactService(IContactRepository repository)
        {
            _repository = repository;
        }
        public Contact? GetContact(int id)
        {
            return _repository.GetContact(id);
        }

        public async Task<Contact?> GetContactAsync(int id)
        {
            return await _repository.GetContactAsync(id);
        }

        public List<Contact> GetContacts()
        {
            return _repository.GetContacts();
        }

        public async Task<List<Contact>> GetContactsAsync()
        {
            return await _repository.GetContactsAsync();
        }

        public async Task<int> GetContactsCount()
        {
            return await _repository.GetContactsCount();
        }

        public async Task<ResultSet> SaveContactAsync(Contact contact)
        {
            ResultSet result = new ResultSet();
            try
            {
                if (await _repository.GetContactsCount() > 1000) //max 1000 iletişim olsun, captcha aşılıp saldırı olursa. ilerde ayarlar içine al
                {
                    throw new Exception("Max kayıt adedi aşıldı");
                }
                DbState state = DbState.Update;
                Contact? data = await _repository.GetContactAsync(contact.Id);
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
                    result = await _repository.SaveContactAsync(data);
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
