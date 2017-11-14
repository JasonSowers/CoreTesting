using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Autofac;
using CoreTesting_00.Dialogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Extensions.Configuration;
using Microsoft.Bot.Builder.Internals.Fibers;
using Newtonsoft.Json.Linq;

namespace CoreTesting_00.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IConfiguration configuration;

        static MessagesController()
        {
            BotBuilderStuff.UpdateContainer();
        }

        public MessagesController(IConfiguration configuration)
        {
            this.configuration = configuration;

            // TODO: should not be updating container with function parameter
            Conversation.UpdateContainer(builder =>
            {
                var appCredentials = new MicrosoftAppCredentials(configuration);
                builder.RegisterInstance(appCredentials).AsSelf();
            });
        }

            // POST api/values
            [Authorize(Roles = "Bot")]
        [HttpPost]
        public async Task<OkResult> Post([FromBody] Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                //MicrosoftAppCredentials.TrustServiceUrl(activity.ServiceUrl);
                var appCredentials = new MicrosoftAppCredentials(configuration);
                var connector = new ConnectorClient(new Uri(activity.ServiceUrl), appCredentials);

                // return our reply to the user
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
