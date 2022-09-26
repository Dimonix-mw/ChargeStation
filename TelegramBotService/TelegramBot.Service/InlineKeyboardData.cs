using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Service
{
    public class InlineKeyboardData
    {
        public InlineKeyboardMarkup inlineKeyboard { get; set; }
        public string textMessage { get; set; }
    }
}
