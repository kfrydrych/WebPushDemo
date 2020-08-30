using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WebPush;
using WebPushDemo.Shared;

namespace WebPushDemo.Server.Controllers
{
    [Route("notifications")]
    [ApiController]
    public class NotificationsController : Controller
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public NotificationsController(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        [HttpPut("subscribe")]
        public NotificationSubscription Subscribe(NotificationSubscription subscription)
        {
            subscription.UserId = GetCurrentUser();

            _subscriptionRepository.Save(subscription);

            Send(subscription, "Subscription created");

            return subscription;
        }

        [HttpGet("send-notification")]
        public IActionResult SendNotification(string message)
        {
            var userId = GetCurrentUser();

            var subsritpion = _subscriptionRepository.Find(userId);

            if (subsritpion == null)
                return BadRequest("Subscription not found");

            Send(subsritpion, message);

            return Ok();
        }

        [HttpGet("show-subscription")]
        public IActionResult ShowSubscription()
        {
            var subscriptions = _subscriptionRepository.GetAll();

            return Ok(subscriptions);
        }

        [HttpPost("clear-subscriptions")]
        public IActionResult ClearSubscriptions()
        {
            _subscriptionRepository.Clear();

            return Ok();
        }

        private async Task Send(NotificationSubscription subscription, string message)
        {
            await Task.Delay(5000);

            var publicKey = "BLC8GOevpcpjQiLkO7JmVClQjycvTCYWm6Cq_a7wJZlstGTVZvwGFFHMYfXt6Njyvgx_GlXJeo5cSiZ1y4JOx1o";
            var privateKey = "OrubzSz3yWACscZXjFQrrtDwCKg-TGFuWhluQ2wLXDo";

            var pushSubscription = new PushSubscription(subscription.Url, subscription.P256dh, subscription.Auth);
            var vapidDetails = new VapidDetails("mailto:<someone@example.com>", publicKey, privateKey);
            var webPushClient = new WebPushClient();

            var payload = JsonConvert.SerializeObject(new
            {
                message,
                url = "myorders/1",
            });

            await webPushClient.SendNotificationAsync(pushSubscription, payload, vapidDetails);
        }

        private string GetCurrentUser()
        {
            return "krzysztof.frydrych@gmail.com";
        }
    }
}
