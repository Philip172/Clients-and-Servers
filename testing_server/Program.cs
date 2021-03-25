using Class_Library_connect;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketTcpServer
{
    class Program
    {
        //Деньги Игроков, Данные для соединения сервера и Игроков хранятся в библиотеке классов "Class_Library_connect"

        static int i=0;//счётчик

        static void Main(string[] args)
        {
            // Получаем адреса для запуска сокета из библиотеки классов, 
            // созданной для хранения данных для соединения
            IPEndPoint ipPoint = new IPEndPoint(Connect.IPAddress, Connect.port);

            // создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                listenSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    // создаём сокет чтобы получить сообщение
                    Socket listened_socket = listenSocket.Accept();

                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    do
                    {
                        //получаем данные
                        bytes = listened_socket.Receive(data);

                        //декодирование 
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while
                    (
                    //пока этот сокет активен
                    listened_socket.Available > 0
                    );



                    string builder_str = builder.ToString();



                    //получаем сообщение: [веремя_сейчас]: [полученные_данные]
                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder_str);



                    //Формируем сообщение




                    // Стандартное сообщение:
                    string message = "Вы равны";

                    // Нестандартное сообщение:

                    //проверка на Игроков
                    bool next = false;
                    for (i = 0; i < Players_data.players.Length; i++)
                    {
                        if (builder_str.Contains(Players_data.players[i]))
                        {
                            next = true;
                            break;
                        }
                    }

                    if (next)
                    {
                        //идентификатор Игрока1
                        if (builder_str.Contains("_money1"))
                        {
                            try
                            {
                                string can = builder_str;
                                can = can.Replace("_money1", "");
                                Players_data.moneys[0] = Convert.ToInt32(can);



                                //Формирование сообщения для Игрока1
                                int place = Players_data.moneys.Length;
                                for (int i = 0; i < Players_data.moneys.Length; i++)
                                {
                                    if (i == 0)
                                        continue;

                                    if (Players_data.moneys[0] >= Players_data.moneys[i]) place--;
                                }
                                message = "Ваше место:" + place;
                            }
                            catch { }
                        }

                        //идентификатор Игрока2
                        if (builder_str.Contains("_money2"))
                        {
                            try
                            {
                                string can = builder_str;
                                can = can.Replace("_money2", "");
                                Players_data.moneys[1] = Convert.ToInt32(can);



                                //Формирование сообщения для Игрока1
                                int place = Players_data.moneys.Length;
                                for (int i = 0; i < Players_data.moneys.Length; i++)
                                {
                                    if (i == 1)
                                        continue;

                                    if (Players_data.moneys[1] >= Players_data.moneys[i]) place--;
                                }
                                message = "Ваше место:" + place;
                            }
                            catch { }
                        }

                        //идентификатор Игрока3
                        if (builder_str.Contains("_money3"))
                        {
                            try
                            {
                                string can = builder_str;
                                can = can.Replace("_money3", "");
                                Players_data.moneys[2] = Convert.ToInt32(can);



                                //Формирование сообщения для Игрока1
                                int place = Players_data.moneys.Length;
                                for (int i = 0; i < Players_data.moneys.Length; i++)
                                {
                                    if (i == 2)
                                        continue;

                                    if (Players_data.moneys[2] >= Players_data.moneys[i]) place--;
                                }
                                message = "Ваше место:" + place;
                            }
                            catch { }
                        }


                        //отправляем сообщение
                        data = Encoding.Unicode.GetBytes(message);
                        listened_socket.Send(data);


                        // закрываем сокет
                        listened_socket.Shutdown(SocketShutdown.Both);
                        listened_socket.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}