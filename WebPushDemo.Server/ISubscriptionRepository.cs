using System.Collections.Generic;
using System.Linq;
using WebPushDemo.Shared;

namespace WebPushDemo.Server
{
    public interface ISubscriptionRepository
    {
        void Save(NotificationSubscription subscription);
        IEnumerable<NotificationSubscription> GetAll();
        NotificationSubscription Find(string userId);
        void Clear();
    }

    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly List<NotificationSubscription> _subscriptions = new List<NotificationSubscription>();
        public void Save(NotificationSubscription subscription)
        {
            _subscriptions.Add(subscription);
        }

        public IEnumerable<NotificationSubscription> GetAll()
        {
            return _subscriptions;
        }

        public NotificationSubscription Find(string userId)
        {
            return _subscriptions.SingleOrDefault(x => x.UserId == userId);
        }

        public void Clear()
        {
            _subscriptions.Clear();
        }
    }
}
