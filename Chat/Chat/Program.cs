using ClassLib;
using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                Random rnd = new Random();
                int uId = rnd.Next(1, 10000);

                bus.Subscribe<Message>(uId.ToString(), HandleTextMessage);

                Console.WriteLine("Enter your name: ");
                var name = Console.ReadLine();

                var input = "";
                Console.WriteLine("Enter a message. 'Quit' to quit.");
                while ((input = Console.ReadLine()) != "Quit")
                {
                    bus.Publish(new Message
                    {
                        Name = name,
                        Text = input
                    });
                }
            }
        }

        static void HandleTextMessage(ClassLib.Message message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0}: {1}", message.Name, message.Text);
            Console.ResetColor();
        }
    }
}
