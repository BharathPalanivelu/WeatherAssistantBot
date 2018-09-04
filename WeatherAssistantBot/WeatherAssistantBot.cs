using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts;


namespace WeatherAssistantBot
{
    public class WeatherAssistantBot : IBot
    {
        private readonly DialogSet dialogSet;

        private async Task GetNameTask(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            await dialogContext.Prompt(Constants.DialogSteps.NameStep.ToString(), "Hi User.May I please know your name?" );
        }

        private async Task GetPlaceTask(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            var state = dialogContext.Context.GetConversationState<BotState>();
            state.Name = ((TextResult)result).Value;
            await dialogContext.Prompt(Constants.DialogSteps.PlaceStep.ToString(), $"Hi {state.Name}, I am Weather Assistant Bot. \n I can get weather reports for you.\n Please enter a name of place.");
        }

        private async Task GetServiceChoiceTask(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            var state = dialogContext.Context.GetConversationState<BotState>();
            state.Place = ((TextResult)result).Value;
            await dialogContext.Prompt(Constants.DialogSteps.GetServiceChoice.ToString(), $"Please Enter 1 for Today's weather or 2 for next 10 days forecast.");
        }
        

        private async Task ReplyBackTask(DialogContext dialogContext, object result, SkipStepFunction next)
        {
           
            var state = dialogContext.Context.GetConversationState<BotState>();
            int serviceChoice = 0;

            int.TryParse(((TextResult)result).Value, out serviceChoice);
            if (serviceChoice == 1 || serviceChoice == 2)
            {
                state.ServiceChoice = serviceChoice;
                WeatherHelper weatherHelper = new WeatherHelper();
                string weatherReport = await weatherHelper.GetWeatherReport(state.Place, state.ServiceChoice);
                await dialogContext.Context.SendActivity($"Dear {state.Name}, \n {weatherReport}");

            }
            else
            {
                await dialogContext.Context.SendActivity($"Hi {state.Name}, \n You have entered {serviceChoice} which is incorrect. \n Thanks for using WeatherAssitant");
            }
            
            await dialogContext.End();
        }

        public WeatherAssistantBot()
        {
            dialogSet = new DialogSet();
            dialogSet.Add(Constants.DialogSteps.NameStep.ToString(), new Microsoft.Bot.Builder.Dialogs.TextPrompt());
            dialogSet.Add(Constants.DialogSteps.PlaceStep.ToString(), new Microsoft.Bot.Builder.Dialogs.TextPrompt());
            dialogSet.Add(Constants.DialogSteps.GetServiceChoice.ToString(), new Microsoft.Bot.Builder.Dialogs.TextPrompt());
            dialogSet.Add(Constants.DialogSteps.MainDialog.ToString(), new WaterfallStep[] { GetNameTask, GetPlaceTask, GetServiceChoiceTask, ReplyBackTask });

        }
        
        public async Task OnTurn(ITurnContext turnContext)
        {
            var state = turnContext.GetConversationState<BotState>();

            var dialogContext = dialogSet.CreateContext(turnContext, state);

            if (dialogContext.Context.Activity.Type == ActivityTypes.Message)
            {
                await dialogContext.Continue();
                if (!dialogContext.Context.Responded)
                {
                    await dialogContext.Begin(Constants.DialogSteps.MainDialog.ToString());
                }
            }
            
        }
    }
}
