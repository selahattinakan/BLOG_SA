using Business.Interfaces;
using Constants;
using Constants.Enums;
using DB_EFCore.Entity;
using DB_EFCore.Repositories.Interfaces;

namespace Business.Services
{
    public class SubscriberService : ISubscriberService
    {
        private readonly ISubscriberRepository _repository;

        public SubscriberService(ISubscriberRepository repository)
        {
            _repository = repository;
        }

        public Subscriber? GetSubscriber(int id)
        {
            return _repository.GetSubscriber(id);
        }

        public async Task<Subscriber?> GetSubscriberAsync(int id)
        {
            return await _repository.GetSubscriberAsync(id);
        }

        public List<Subscriber> GetSubscribers()
        {
            return _repository.GetSubscribers();
        }

        public async Task<List<Subscriber>> GetSubscribersAsync()
        {
            return await _repository.GetSubscribersAsync();
        }

        public async Task<ResultSet> SaveSubscriberAsync(string subMail)
        {
            ResultSet result = new ResultSet();
            try
            {
                Subscriber? sub = await _repository.GetSubscriberAsync(subMail);
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
                    result = await _repository.SaveSubscriberAsync(sub);
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
