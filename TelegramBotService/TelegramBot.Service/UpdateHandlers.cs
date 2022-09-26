using System.Collections.Concurrent;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using TelegramBot.Service;
using TelegramBot.Service.Entities;
using TelegramBot.Service.Kafka.Common.Entities;
using TelegramBot.Service.Kafka.Common.Publisher;
using TelegramBot.Service.Services;
using TelegramBot.Service.Telegram;

namespace TelegramBotService;

public static class UpdateHandlers
{
    public static IWebApiService webApiService;
    public static IKafkaService kafkaService;
    public static string kafkaTopicRequest;
    public static ILogger<TelegramBotListener> _logger;
    private static ConcurrentDictionary<long, UserInfo> _usersStateChat = new ConcurrentDictionary<long, UserInfo>();

    public static Task PollingErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };
        _logger.LogWarning(ErrorMessage);
        return Task.CompletedTask;
    }

    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var handler = update.Type switch
        {
            UpdateType.Message            => BotOnMessageReceived(botClient, update.Message!),
            UpdateType.EditedMessage      => BotOnMessageReceived(botClient, update.EditedMessage!),
            UpdateType.CallbackQuery      => BotOnCallbackQueryReceived(botClient, update.CallbackQuery!),
            UpdateType.InlineQuery        => BotOnInlineQueryReceived(botClient, update.InlineQuery!),
            UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(botClient, update.ChosenInlineResult!),
            _                             => UnknownUpdateHandlerAsync(botClient, update)
        };

        try
        {
            await handler;
        }
        #pragma warning disable CA1031
        catch (Exception exception)
        #pragma warning restore CA1031
        {
            await PollingErrorHandler(botClient, exception, cancellationToken);
        }
    }

    private static async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
    {
        _logger.LogInformation($"Receive message type: {message.Type}");
        if (message.Text is not { } messageText)
            return;

        var userInfo = _usersStateChat.GetOrAdd(message.Chat.Id, new UserInfo());

        if (message.Text == "/login")
        {
            await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Авторизация. \nВведите логин:");
            userInfo.StateChat = StateChat.LoginEnterLogin;
            return;
        }
        else if (message.Text == "/registration")
        {
            await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Регистрация. \nВведите логин:");
            userInfo.StateChat = StateChat.Registration;
            return;
        }

        switch (userInfo.StateChat)
        {
            case StateChat.Registration:
                userInfo.Login = message.Text;
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Введите пароль:");
                userInfo.StateChat = StateChat.RegistrationEnterPassword;
                break;
            case StateChat.RegistrationEnterPassword:
                userInfo.Password = message.Text;
                userInfo.StateChat = StateChat.RegistrationInfoComplite;
                break;
            case StateChat.Login:
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Авторизация. \nВведите логин:");
                userInfo.StateChat= StateChat.LoginEnterLogin;
                break;
            case StateChat.LoginEnterLogin:
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Введите пароль:");
                userInfo.Login = message.Text;
                userInfo.StateChat = StateChat.LoginEnterPassword;
                break;
            case StateChat.LoginEnterPassword:
                userInfo.Password = message.Text;
                userInfo.StateChat = StateChat.Autorization;
                break;
            case StateChat.Init:
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Авторизуйтесь или зарегистрируйтесь.");
                break;
            case StateChat.Autorization:
                break;
            default:
                break;
        }

        if (userInfo.StateChat == StateChat.Autorization)
        {
            var token = await webApiService.AutorizationAsync(userInfo.Login, userInfo.Password);
            if (token == null)
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Не верный логин или пароль!");
                userInfo.StateChat = StateChat.Init;
                return;
            }
            else
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Авторизация прошла успеша.");
                userInfo.StateChat = StateChat.Active;
                userInfo.Token = token;
            }
        }

        if (userInfo.StateChat == StateChat.RegistrationInfoComplite)
        {
            var result = webApiService.RegistrationUserAsync(message.Chat.Id, userInfo);
            string textMessage = "Не предвиденная ошибка. Попробуйте позже.";
            if (result.Result == AnswerRegistration.Created)
            {
                textMessage = $"Регистрация прошла успеша. Авторизуйтесь.";
                userInfo.StateChat = StateChat.Login;
            } else if (result.Result == AnswerRegistration.Error)
            {
                textMessage = $"Регистрация не успеша. Попробуйте позже.";
                userInfo.StateChat = StateChat.Init;
            } else if (result.Result == AnswerRegistration.Conflict)
            {
                textMessage = $"Такой пользователь уже существует.";
                userInfo.StateChat = StateChat.Init;
            }
            await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: textMessage);
        }

        if (userInfo.StateChat == StateChat.Active)
        {
            var action = SendInlineKeyboard(botClient, message, TelegramKeyboardMessage.SelectPump());
            Message sentMessage = await action;
            _logger.LogInformation($"The message was sent with id: {sentMessage.MessageId}");
        }
    }

    // Send inline keyboard
    // You can process responses in BotOnCallbackQueryReceived handler
    static async Task<Message> SendInlineKeyboard(ITelegramBotClient botClient, Message message, InlineKeyboardData inlineKeyboardData)
    {
        await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

        // Simulate longer running task
        await Task.Delay(500);

        return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                    text: inlineKeyboardData.textMessage,
                                                    replyMarkup: inlineKeyboardData.inlineKeyboard);
    }

    // Process Inline Keyboard callback data
    private static async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
    {
        var data = callbackQuery?.Data?.Split(";").ToArray();
        if (data != null)
        {
            CommandMessageEnum command;
            if (Enum.TryParse(data[0], out command))
            {
                _logger.LogInformation($"Команда - {data[0]}");
                if (command == CommandMessageEnum.SelectPump)
                {
                    await botClient.AnswerCallbackQueryAsync(
                        callbackQueryId: callbackQuery.Id);
                    _logger.LogInformation($"Выбрана станция № {data[1]}");
                    var action = SendInlineKeyboard(botClient, callbackQuery.Message, TelegramKeyboardMessage.SelectMinutes(numberPump: data[1]));
                    Message sentMessage = await action;
                    _logger.LogInformation($"The message was sent with id: {sentMessage.MessageId}");

                }
                else if ((command == CommandMessageEnum.SelectMinutes))
                {
                    var request = new InsertPumpRequest
                    {
                        Token = _usersStateChat.GetValueOrDefault(callbackQuery.Message!.Chat.Id, new UserInfo()).Token,
                        UserId = callbackQuery.Message!.Chat.Id,
                        RequestId = Guid.NewGuid(),
                        PumpId = Int32.Parse(data[1]),
                        Minutes = Int32.Parse(data[2]),
                    };
                    //await webApiService.StartChargeAsync(request);
                    kafkaService.SendMessage(request, kafkaTopicRequest);
                    _logger.LogInformation($"Отправлены данные (UserId={request.UserId},PumpId={request.PumpId},Minutes={request.Minutes}) в Kafka, топик {kafkaTopicRequest}");

                    await botClient.AnswerCallbackQueryAsync(
                        callbackQueryId: callbackQuery.Id);

                    await botClient.SendTextMessageAsync(
                        chatId: callbackQuery.Message!.Chat.Id,
                        text: $"Отправлена команда заправки на станцию № {data[1]} на {data[2]} мин.");

                    _logger.LogInformation($"Отправлена команда заправки на станцию № {data[1]} на {data[2]} мин.");
                }
                else
                {
                    await botClient.AnswerCallbackQueryAsync(
                        callbackQueryId: callbackQuery.Id,
                        text: $"Не известная команда");
                    _logger.LogWarning("Не известная команда");
                }
            }
        }
    }

    private static async Task BotOnInlineQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
    {
        _logger.LogInformation($"Received inline query from: {inlineQuery.From.Id}");

        InlineQueryResult[] results = {
            // displayed result
            new InlineQueryResultArticle(
                id: "1",
                title: "TgBots",
                inputMessageContent: new InputTextMessageContent(
                    "hello"
                )
            )
        };

        await botClient.AnswerInlineQueryAsync(inlineQueryId: inlineQuery.Id,
                                               results: results,
                                               isPersonal: true,
                                               cacheTime: 0);
    }

    private static Task BotOnChosenInlineResultReceived(ITelegramBotClient botClient, ChosenInlineResult chosenInlineResult)
    {
        _logger.LogInformation($"Received inline result: {chosenInlineResult.ResultId}");
        return Task.CompletedTask;
    }

    private static Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
    {
        _logger.LogWarning($"Unknown update type: {update.Type}");
        return Task.CompletedTask;
    }
}
