using Telegram.Bot;
using MySql.Data.MySqlClient;
using Telegram.Bot.Types;

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
            MySqlConnection conn = new MySqlConnection(connStr);

            var message = update.Message;
            switch (message?.Text?.ToLower())
            {
                case "/start":
                    {
                    conn.Open();
                        string checkuser = "SELECT user_id FROM Users WHERE user_id = @Id";
                        using (MySqlCommand check = new MySqlCommand(checkuser, conn))
                        {
                            check.Parameters.Add(new MySqlParameter("Id", message.Chat.Id));
                            using (MySqlDataReader reader = check.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    await client.SendTextMessageAsync(message.Chat.Id, "Извините, но вы уже существуете в базе данных!" +
                                        "\nВы можете поменять имя в меню");
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
            }
            conn.Close();
        }

        private static async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            Console.WriteLine(exception.Message);
            await client.SendPhotoAsync(exception.Message, InputFile.FromUri("https://raw.githubusercontent.com/GeonAndKotN/BotInTg/master/BotInTg/Photo/HahaErrorMan.png"), caption: "Упс, кажется возникла ошибка, сообщите в службу поддержки о баге!", cancellationToken: token);
        }
    }    
}