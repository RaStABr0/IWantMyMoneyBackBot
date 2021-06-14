using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IWantMyMoneyBackBot.Data;
using IWantMyMoneyBackBot.Data.Repositories;
using IWantMyMoneyBackBot.Telegram;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace IWantMyMoneyBackBot
{
    static class Program
    {
        private static TelegramBotClient _bot;

        private static DebtsRepository _debtsRepository;

        private static UsersRepository _usersRepository;

        private static TelegramDataRepository _telegramDataRepository;
        
        
        private static async Task Main(string[] args)
        {
            Init();
            Subscribe();
            
            Console.WriteLine(_bot.BotId);
            
            var cts = new CancellationTokenSource();


            _bot.StartReceiving(new DefaultUpdateHandler(OnUpdated, OnError), cts.Token);

            Console.ReadLine();
            
            _bot.StopReceiving();
        }

        private static async Task OnUpdated(ITelegramBotClient client, Update update, CancellationToken ct)
        {
            var message = update.Message;
            
            if (message.Type == MessageType.Text)
            {
                var value = message.Text;

                switch (value)
                {
                    case Command.START:
                        var from = message.From;
                        
                        var newUser = new User
                        {
                            TelegramId = from.Id,
                            Name = from.Username ?? $"{from.FirstName} {from.LastName}"
                        };

                        await _usersRepository.Add(newUser);
                        
                        break;
                    
                    case Command.REMOVE_USER:
                        _usersRepository.RemoveUser(message.From.Id);

                        break;
                }
            }
        }
        
        private static async Task OnError(ITelegramBotClient client, Exception update, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        
        private static void Init()
        {
            var ctx = new Context();

            _debtsRepository = new DebtsRepository(ctx);
            _usersRepository = new UsersRepository(ctx);
            _telegramDataRepository = new TelegramDataRepository(ctx);

            var telegramData = GetTelegramData();

            _bot = new TelegramBotClient(telegramData.BotId);
        }

        private static TelegramData GetTelegramData()
        {
            var telegramData = _telegramDataRepository.GetData();

            if (telegramData != null) return telegramData;
            
            telegramData = new TelegramData();
                
            Console.Write("Введите id бота: ");
            telegramData.BotId = Console.ReadLine();
                
            _telegramDataRepository.Add(telegramData);

            return telegramData;
        }

        private static void Subscribe()
        {
            _usersRepository.UserAdded += async newUser => await OnUserAdded(newUser);
            _usersRepository.UserRemoved += OnUserRemoved;
        }

        private static async Task OnUserAdded(User newUser)
        {
            var allUsers = await _usersRepository.GetAll();

            var newDebts = new List<Debt>();
            
            foreach (var user in allUsers)
            {
                newDebts.Add(new Debt
                {
                    DebtorId = newUser.TelegramId,
                    LenderId = user.TelegramId
                });

                newDebts.Add(new Debt
                {
                    DebtorId = user.TelegramId,
                    LenderId = newUser.TelegramId
                });
            }

            await _debtsRepository.AddRange(newDebts);
        }

        private static void OnUserRemoved(User removedUser)
        {
            var (debts, lends) = _debtsRepository.GetDebtsAndLends(removedUser);
            
            _debtsRepository.RemoveRange(debts);
            _debtsRepository.RemoveRange(lends);
        }
    }
}
