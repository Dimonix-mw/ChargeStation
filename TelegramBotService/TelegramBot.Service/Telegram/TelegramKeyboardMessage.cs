using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Service.Telegram
{
    public static class TelegramKeyboardMessage
    {
        public static InlineKeyboardData SelectPump()
        {
            var commandMessage = CommandMessageEnum.SelectPump.ToString();
            var inlineKeyboardData = new InlineKeyboardData
            {
                inlineKeyboard = new(
                new[]
                {
                    // first row
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("1", commandMessage + ";1"),
                        InlineKeyboardButton.WithCallbackData("2", commandMessage + ";2"),
                    },
                    // second row
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("3", commandMessage + ";3"),
                        InlineKeyboardButton.WithCallbackData("4", commandMessage + ";4"),
                    },
                }),
                textMessage = "Выберите заправочную станцию:"
            };

            return inlineKeyboardData;
        }

        public static InlineKeyboardData SelectMinutes(string numberPump)
        {
            var commandMessage = CommandMessageEnum.SelectMinutes.ToString();
            var inlineKeyboardData = new InlineKeyboardData
            {
                inlineKeyboard = new(
                new[]
                {
                    // first row
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("15", commandMessage + $";{numberPump};15"),
                        InlineKeyboardButton.WithCallbackData("30", commandMessage + $";{numberPump};30"),
                    },
                    // second row
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("45", commandMessage + $";{numberPump};45"),
                        InlineKeyboardButton.WithCallbackData("60", commandMessage + $";{numberPump};60"),
                    },
                }),
                textMessage = "Выберите количество минут заправки:"
            };

            return inlineKeyboardData;
        }
    }
}
