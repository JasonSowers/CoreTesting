using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace _461_core20.Controllers
{
    [BotAuthentication]
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IConfiguration configuration;

        public MessagesController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // POST api/values
        [HttpPost]
        public async Task<OkResult> Post([FromBody] Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                //MicrosoftAppCredentials.TrustServiceUrl(activity.ServiceUrl);

                                var appCredentials = new MicrosoftAppCredentials(configuration);
                                var connector = new ConnectorClient(new Uri(activity.ServiceUrl), appCredentials);

//                await Conversation.SendAsync(activity, () => new RootDialog());
//
//                 return our reply to the user
                                var reply = activity.CreateReply("HelloWorld");

                                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                //HandleSystemMessage(activity);
            }
            return Ok();
        }
    }
}
