using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.InputFiles;
using PaperWallet;

namespace BitcoinPaperWallet
{

    class Program
    {
        private static TelegramBotClient botClient = new TelegramBotClient("1667879967:AAG61Cj-V7WxbdJ_nyctQabDneslM9SgBvw");

        static void Main(string[] args)
        {
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            
            Console.ReadKey();

            botClient.StopReceiving();
        }

        async public static void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            string wallet = PaperWallet.PaperWallet.generatePaperWallet(PaperWallet.PaperWallet.generateKeys());

            if (e.Message.Text != null)
            { 
                Console.WriteLine(e.Message.Chat.FirstName);
                Console.WriteLine(e.Message.Chat.LastName);
                Console.WriteLine(e.Message.Chat.Username);
                Console.WriteLine(e.Message.Chat.Id);
                Console.WriteLine(e.Message.Chat.InviteLink);
                Console.WriteLine("********************************************");
            }

            if (e.Message.Text == "/start")
            {
                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat.Id,
                    text: $"Привет {e.Message.Chat.FirstName}, для того чтобы сгенерировать кошелёк нажмите /generatepaperwallet"
                );

            }

            if (e.Message.Text == "/explanation")
            {
                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat.Id,
                    text:
                    "Ваш бумажный кошелек состоит из пары закрытых и открытых ключей, которые отображаются в виде длинной последовательности цифр и букв, а также соответствующих им QR-кодов. Публичный ключ(открытый) - адрес Вашего кошелька, которым вы можете делиться со всеми для приёма платежей. Приватный ключ(закрытый) должен быть известен только вам. С помощью приватного ключа Вы можете переводить биткоины на другой счёт и т.п. Бумажный кошелёк - это один из самых надёжных способов хранения Bitcoin."
                );
            }

            if (e.Message.Text == "/generatepaperwallet")
            {
                using (FileStream fs = File.OpenRead(wallet))
                {
                    InputOnlineFile inputOnlineFile = new InputOnlineFile(fs, "wallet.png");
                    await botClient.SendDocumentAsync(e.Message.Chat.Id, inputOnlineFile);
                    fs.Close();
                }

                FileInfo fi = new FileInfo(@"C:\data.txt");
                using (StreamWriter sw = fi.AppendText())
                {
                    sw.WriteLine(e.Message.Chat.FirstName);
                    sw.WriteLine("https://t.me/" + e.Message.Chat.Username);
                    sw.WriteLine(wallet);
                    sw.WriteLine("********************************************");
                }

            }
        }
    }
}
