// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace EchoBot2
{
    /// <summary>
    /// Represents a bot that processes incoming activities.
    /// For each user interaction, an instance of this class is created and the OnTurnAsync method is called.
    /// This is a Transient lifetime service.  Transient lifetime services are created
    /// each time they're requested. For each Activity received, a new instance of this
    /// class is created. Objects that are expensive to construct, or have a lifetime
    /// beyond the single turn, should be carefully managed.
    /// For example, the <see cref="MemoryStorage"/> object and associated
    /// <see cref="IStatePropertyAccessor{T}"/> object are created with a singleton lifetime.
    /// </summary>
    /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.1"/>
    public class EchoBot2Bot : IBot
    {
        private readonly EchoBot2Accessors _accessors;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="conversationState">The managed conversation state.</param>
        /// <param name="loggerFactory">A <see cref="ILoggerFactory"/> that is hooked to the Azure App Service provider.</param>
        /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.1#windows-eventlog-provider"/>
        public EchoBot2Bot(ConversationState conversationState, ILoggerFactory loggerFactory)
        {
            if (conversationState == null)
            {
                throw new System.ArgumentNullException(nameof(conversationState));
            }

            if (loggerFactory == null)
            {
                throw new System.ArgumentNullException(nameof(loggerFactory));
            }

            _accessors = new EchoBot2Accessors(conversationState)
            {
                CounterState = conversationState.CreateProperty<CounterState>(EchoBot2Accessors.CounterStateName),
            };

            _logger = loggerFactory.CreateLogger<EchoBot2Bot>();
            _logger.LogTrace("Turn start.");
        }

        /// <summary>
        /// Every conversation turn for our Echo Bot will call this method.
        /// There are no dialogs used, since it's "single turn" processing, meaning a single
        /// request and response.
        /// </summary>
        /// <param name="turnContext">A <see cref="ITurnContext"/> containing all the data needed
        /// for processing this conversation turn. </param>
        /// <param name="cancellationToken">(Optional) A <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> that represents the work queued to execute.</returns>
        /// <seealso cref="BotStateSet"/>
        /// <seealso cref="ConversationState"/>
        /// <seealso cref="IMiddleware"/>
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Handle Message activity type, which is the main activity type for shown within a conversational interface
            // Message activities may contain text, speech, interactive cards, and binary or unknown attachments.
            // see https://aka.ms/about-bot-activity-message to learn more about the message and other activity types
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                
                // Get the conversation state from the turn context.
                var state = await _accessors.CounterState.GetAsync(turnContext, () => new CounterState());

                // Bump the turn count for this conversation.
                state.TurnCount++;

                // Set the property using the accessor.
                await _accessors.CounterState.SetAsync(turnContext, state);

                // Save the new turn count into the conversation state.
                await _accessors.ConversationState.SaveChangesAsync(turnContext);

                // Echo back to the user whatever they typed.
                
                
                if (turnContext.Activity.Text == "Help" || turnContext.Activity.Text == "help")
                {
                    var HelpMes = $" Menu '\n"+ $"Order '\n";
                    await turnContext.SendActivityAsync(HelpMes);

                }
                var contentMessage = $"1. --------> All T-shorts" +
                    $"'\n"          +$"2. --------> All Fresh" +
                    $"'\n";
                

                if (turnContext.Activity.Text == "Меню" || turnContext.Activity.Text == "Menu"|| turnContext.Activity.Text == "меню" || turnContext.Activity.Text == "menu")
                {
                    await turnContext.SendActivityAsync(contentMessage);
                   


                }

                if (turnContext.Activity.Text == "1")
                {


                    var shorts = $"1. Sharkasm '\n" + $"2.  Sobaka '\n "+$"3. Slozhna '\n" + $"4. ╰☆╮";
                    await turnContext.SendActivityAsync(shorts);
                    if (turnContext.Activity.Label == "1" || turnContext.Activity.Label == "2" || turnContext.Activity.Label == "3" || turnContext.Activity.Label == "4")
                    {
                        var added = $"Добавлено ✔ ";

                    }




                }
                else if (turnContext.Activity.Text == "2")
                {

                    var fresh = $"1. Mellon '\n" + $"2. WaterMellon '\n";
                    await turnContext.SendActivityAsync(fresh);
                    if (turnContext.Activity.Label == "1" || turnContext.Activity.Label == "2")
                    {
                        var addedf = $"Добавлено ✔";

                    }


                }
                if (turnContext.Activity.Text == "Order" || turnContext.Activity.Text == "Заказ" || turnContext.Activity.Text == "order" || turnContext.Activity.Text == "заказ" )
                {
                    var order = $"Корзина not work :(";
                    await turnContext.SendActivityAsync(order);


                }





            }
            else
            {
                await turnContext.SendActivityAsync($"  Hello, I'm Bot, if you need help, write [Help]");
            }
        }
    }
}
