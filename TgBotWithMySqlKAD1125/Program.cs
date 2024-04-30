using Telegram.Bot;
using MySql.Data.MySqlClient;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TgBotWithMySqlKAD1125
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var botclient = new TelegramBotClient("6435067834:AAHD-49wIKSo6ooiN82c0FdMH5_JWogwpwA");
            botclient.StartReceiving(HandleUpdateAsync, HandlePollingErrorAsync);
            Console.WriteLine("Бот был запущен!");

            Console.ReadLine();
        }

        private static async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            // 6435067834:AAHD-49wIKSo6ooiN82c0FdMH5_JWogwpwA - бот для проверки
            // 7140884239:AAFMcWNsUnDo7rFrDQGRlpYovz1C0KewLIQ - основной бот

            string connStr = "server=192.168.200.13;user=student;password=student;database=BotTgKursovaya";
            //string connStr = "server=localhost;user=SuperKAD;database=bottgkursovaya;password=1234;";
            MySqlConnection conn = new MySqlConnection(connStr);


            var message = update.Message;
            conn.Open();

            switch (update.CallbackQuery?.Data)
            {
                
                case "WarSel":
                    { //Как принимать от inline кнопок айди пользователя, или что мне с этим сделать
                        string addclassWarrior = "SELECT user_id, class FROM Users WHERE user_id = @Id AND class = 'неопр'";
                        using (MySqlCommand warrioradding = new MySqlCommand(addclassWarrior, conn))
                        {
                            warrioradding.Parameters.Add(new MySqlParameter("Id", update.CallbackQuery.Message.Chat.Id));
                            using (MySqlDataReader GoWarriorGo = warrioradding.ExecuteReader())
                            {
                                if(GoWarriorGo.Read())
                                {
                                    GoWarriorGo.Close();
                                    string changeToWarrior = "UPDATE `BotTgKursovaya`.`Users` SET `class` = 'Воин' WHERE user_id = @Id;";
                                    using (MySqlCommand ChangeWar = new MySqlCommand(changeToWarrior, conn))
                                    {
                                        ChangeWar.Parameters.Add(new MySqlParameter("Id", update.CallbackQuery.Message.Chat.Id));
                                        using (MySqlDataReader HeheChangeWarrior = ChangeWar.ExecuteReader())
                                            HeheChangeWarrior.Read();
                                    }
                                    
                                    await client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Отныне вы воин!");
                                }
                                else
                                {
                                    await client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Зачем снова нажимать сюда :(");
                                }
                            }
                        }
                    }
                    return;
                case "BerSel":
                    { //Как принимать от inline кнопок айди пользователя, или что мне с этим сделать
                        string addclassBerserk = "SELECT user_id, class FROM Users WHERE user_id = @Id AND class = 'неопр'";
                        using (MySqlCommand berserkadding = new MySqlCommand(addclassBerserk, conn))
                        {
                            berserkadding.Parameters.Add(new MySqlParameter("Id", update.CallbackQuery.Message.Chat.Id));
                            using (MySqlDataReader GoBerserkGo = berserkadding.ExecuteReader())
                            {
                                if (GoBerserkGo.Read())
                                {
                                    GoBerserkGo.Close();
                                    string changeToBerserk = "UPDATE `BotTgKursovaya`.`Users` SET `class` = 'Берсерк' WHERE user_id = @Id;";
                                    using (MySqlCommand ChangeBer = new MySqlCommand(changeToBerserk, conn))
                                    {
                                        ChangeBer.Parameters.Add(new MySqlParameter("Id", update.CallbackQuery.Message.Chat.Id));
                                        using (MySqlDataReader HeheChangeBerserk = ChangeBer.ExecuteReader())
                                            HeheChangeBerserk.Read();
                                    }

                                    await client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Отныне вы берсерк!");
                                }
                                else
                                {
                                    await client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Зачем снова нажимать сюда :(");
                                }
                            }
                        }
                    }
                    return;
                    return;
                case "PalSel":
                    { //Как принимать от inline кнопок айди пользователя, или что мне с этим сделать
                        string addclassWarrior = "SELECT user_id, class FROM Users WHERE user_id = @Id AND class = 'неопр'";
                        using (MySqlCommand warrioradding = new MySqlCommand(addclassWarrior, conn))
                        {
                            warrioradding.Parameters.Add(new MySqlParameter("Id", update.CallbackQuery.Message.Chat.Id));
                            using (MySqlDataReader GoWarriorGo = warrioradding.ExecuteReader())
                            {
                                if (GoWarriorGo.Read())
                                {
                                    GoWarriorGo.Close();
                                    string changeToWarrior = "UPDATE `BotTgKursovaya`.`Users` SET `class` = 'Воин' WHERE user_id = @Id;";
                                    using (MySqlCommand ChangeWar = new MySqlCommand(changeToWarrior, conn))
                                    {
                                        ChangeWar.Parameters.Add(new MySqlParameter("Id", update.CallbackQuery.Message.Chat.Id));
                                        using (MySqlDataReader HeheChangeWarrior = ChangeWar.ExecuteReader())
                                            HeheChangeWarrior.Read();
                                    }

                                    await client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Отныне вы воин!");
                                }
                                else
                                {
                                    await client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Зачем снова нажимать сюда :(");
                                }
                            }
                        }
                    }
                    return;
            }

            //надо пофиксить так, чтобы принимался только текст и ничего более
            switch (message?.Text?.ToLower() /*&& message.Text != null*/)
            {
                case "/start":
                    {
                        string checkuser = "SELECT user_id FROM Users WHERE user_id = @Id";
                        using (MySqlCommand check = new MySqlCommand(checkuser, conn))
                        {
                            check.Parameters.Add(new MySqlParameter("Id", message.Chat.Id));
                            using (MySqlDataReader reader = check.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    ReplyKeyboardMarkup StartMenu = new(new[] { new KeyboardButton[] { "💰магазин💰" }, new KeyboardButton[] { "🧟Спуск🧟" } })
                                    {
                                        ResizeKeyboard = true
                                    };
                                    await client.SendTextMessageAsync(message.Chat.Id, "Извините, но вы уже существуете в базе данных!" +
                                        "\nВы можете поменять имя в меню", replyMarkup: StartMenu);
                                }
                                else
                                {
                                    reader.Close();
                                    string adduser = "INSERT INTO `BotTgKursovaya`.`Users` (`user_id`, `user_name`, `class`,`location`) VALUES (@Id, 'namegood', 'неопр', 'старт');";
                                    using (MySqlCommand addid = new MySqlCommand(adduser, conn))
                                    {
                                        addid.Parameters.Add(new MySqlParameter("Id", message.Chat.Id));
                                        using (MySqlDataReader addsup = addid.ExecuteReader())
                                            //тут меня перебрасывает на таск с ошибкой
                                            addsup.Read();
                                    }
                                    await client.SendTextMessageAsync(message.Chat.Id, "Приветствую!" +
                                    "\nДанный бот является МИНИ РПГ игрой где вам предстоит убивать монстров, исследовать подземелья и прокачивать" +
                                    " собственного персонажа! " +
                                    "\n\nЗа помощью писать в этот чат - https://t.me/+WKfhhZfDpLRhOTky" +
                                    "\n\nДля начала вам надо будет выбрать имя и класс своего персонажа, введите ваше имя");
                                }
                            }
                        }
                        return;
                    }
                default:
                    {
                        string checkname = "SELECT User_name from Users where user_id = @Id and user_name = 'namegood';";
                        using (MySqlCommand checkn = new MySqlCommand(checkname, conn))
                        {
                            checkn.Parameters.Add(new MySqlParameter("Id", message.Chat.Id));
                            using (MySqlDataReader ChangeName = checkn.ExecuteReader())

                            {
                                if (ChangeName.Read())
                                {
                                    string changename = "UPDATE `BotTgKursovaya`.`Users` SET `user_name` = @namems WHERE user_id = @Id;";
                                    using (MySqlCommand namech = new MySqlCommand(changename, conn))
                                    {
                                        ChangeName.Close();
                                        namech.Parameters.Add(new MySqlParameter("namems", update.Message.Text));
                                        namech.Parameters.Add(new MySqlParameter("Id", message.Chat.Id));
                                        using (MySqlDataReader addfirstnamet = namech.ExecuteReader())
                                            addfirstnamet.Read();
                                    }

                                    await client.SendTextMessageAsync(message.Chat.Id, $"Вы поменяли ваше имя на {update.Message.Text}. \nПрекрасное имя!");
                                    InlineKeyboardMarkup ClassCheck = new(new[]
                                     {
                                         new []
                                         {
                                             InlineKeyboardButton.WithCallbackData(text: "🗡️Воин🗡️", callbackData: "WarSel"),
                                         },
                                         new []
                                         {
                                             InlineKeyboardButton.WithCallbackData(text: "\U0001fa93Берсерк\U0001fa93", callbackData: "BerSel"),
                                         },
                                         new []
                                         {
                                             InlineKeyboardButton.WithCallbackData(text: "🛡️Паладин🛡️", callbackData: "PalSel"),
                                         },
                                     });
                                    await client.SendTextMessageAsync(message.Chat.Id, "Отлично, теперь можно приступить к выбору вашего класса!" +
                                        "\n\n🗡️Воин🗡️ = +10% к здоровью и +10% к урону. \n🪓Берсерк🪓 = +20% к урону и -5% к здоровью. \n🛡️Паладин🛡️ = +20% к здоровью и -5 к здоровью", replyMarkup: ClassCheck);
                                }
                                else
                                {
                                    await client.SendTextMessageAsync(message.Chat.Id, "Такой команды не существует :(");
                                }
                                }
                            }
                        }
                        conn.Close();
                        return;
                    //Это для выборки класса
                    /*switch (update.CallbackQuery?.Data)
                    {
                        case "🗡️Воин🗡️":
                            {
                                await client.SendTextMessageAsync(message.Chat.Id, "Вы отныне воин!");
                            }
                            return;
                        case "\U0001fa93Берсерк\U0001fa93":
                            {
                                await client.SendTextMessageAsync(message.Chat.Id, "Вы отныне берсерк!");
                            }
                            return;
                        case "🛡️Паладин🛡️":
                            {
                                await client.SendTextMessageAsync(message.Chat.Id, "Вы отныне паладин!");
                            }
                            return;
                    */

                    }
        }
        private static async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            Console.WriteLine(exception.Message);
            return;
            await client.SendPhotoAsync(exception.Message, InputFile.FromUri("https://raw.githubusercontent.com/GeonAndKotN/BotInTg/master/BotInTg/Photo/HahaErrorMan.png"), caption: "Упс, кажется возникла ошибка, сообщите в службу поддержки о баге!", cancellationToken: token);
        }
    }    
}