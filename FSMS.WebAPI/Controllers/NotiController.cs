using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSMS.WebAPI.Controllers
{
    public class NotiController : ControllerBase
    {
        [HttpPost("subscribe")]
        [AllowAnonymous]
        public async Task<IActionResult> SubscribeTopic(IReadOnlyList<string> registrationToken, string topic)

        {
            var response = await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(
                registrationToken, topic);
            Console.WriteLine($"{response.SuccessCount} tokens were subscribed successfully");
            return Ok(response.SuccessCount);
        }


        [HttpPost("unsubscribe")]
        [AllowAnonymous]
        public async Task<IActionResult> UnSubscribeTopic(IReadOnlyList<string> registrationToken, string topic)

        {
            var response = await FirebaseMessaging.DefaultInstance.UnsubscribeFromTopicAsync(
                registrationToken, topic);
            Console.WriteLine($"{response.SuccessCount} tokens were unsubscribed successfully");
            return Ok(response.SuccessCount);
        }
    }
}
