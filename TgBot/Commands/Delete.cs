using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TgBot.Models;

namespace TgBot.Commands
{
    public static class Delete
    {
        public static List<FixtureResponse> favorites = new List<FixtureResponse>();
        private static InlineKeyboardMarkup inlineKeyboard { get; set; }

        public static async Task Execute(MessageEventArgs message)
        {
            Program.client.OnCallbackQuery -= ButtonPressedDelete;
            favorites = await ApiRepository.GetAllFavorite($"{ApiClient.Base}/Fixture/all_favorites?user={message.Message.Chat.Id}");
            if (favorites != null)
            {

                List<InlineKeyboardButton[]> ToDeleteList = new List<InlineKeyboardButton[]>();

               

                for (int i = 1; i <= favorites.Count; i++)
                {
                    InlineKeyboardButton button = new InlineKeyboardButton() { CallbackData ="D"+ i.ToString(), Text = i.ToString() };
                    InlineKeyboardButton[] row = new InlineKeyboardButton[1] { button };
                    ToDeleteList.Add(row);
                }

                InlineKeyboardButton cancel = new InlineKeyboardButton() { CallbackData = "DCancel", Text = "Cancel" };
                InlineKeyboardButton[] row_cancel = new InlineKeyboardButton[1] { cancel };
                ToDeleteList.Add(row_cancel);

                inlineKeyboard = new InlineKeyboardMarkup(ToDeleteList);

                await Program.client.SendTextMessageAsync(message.Message.From.Id, "Select match to delete:", replyMarkup: inlineKeyboard);

                Program.client.OnCallbackQuery += ButtonPressedDelete;

            }
            else
            {
                await Program.client.SendTextMessageAsync(message.Message.From.Id, @"You don't have any favorite matches yet.", parseMode: ParseMode.Html);
                return;
            }          
        }

        public static async void ButtonPressedDelete(object sender, CallbackQueryEventArgs e)
        {
            try
            {               
                await Program.client.EditMessageReplyMarkupAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId, null);

                string buttonText;
                if (e.CallbackQuery.Data.StartsWith("D"))
                {
                    buttonText = e.CallbackQuery.Data.Substring(1);
                    if (buttonText == "Cancel")
                    {
                        Program.client.OnCallbackQuery -= ButtonPressedDelete;
                        return;
                    }
                    var id = favorites[Int32.Parse(buttonText) - 1].Id;
                    await ApiRepository.DeleteFromFavor($"{ApiClient.Base}/Fixture/delete?id={id}", e.CallbackQuery.From.Id);
                    Program.client.OnCallbackQuery -= ButtonPressedDelete;
                    return;
                }
            }
            catch (Exception)
            {
                Program.client.OnCallbackQuery -= ButtonPressedDelete;
                return;
                throw;
            }
           


            
        }
    }
}
