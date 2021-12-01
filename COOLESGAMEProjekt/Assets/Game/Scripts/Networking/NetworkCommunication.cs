using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Networking.Security;

namespace Networking
{
    class NetworkCommunication
    {
        public enum UseCase
        {
            Intern,
            StartMenu,
            Game,
            StartUp
        }

        public delegate void ReceiveDataDel(UseCase useCase, string data);

        private void SendData(UseCase useCase, ClientInfo Client, string data)
        {
            byte[] ToSendBytes = Encoding.UTF8.GetBytes(useCase + ";" + data);
            Client.Socket.Send(ToSendBytes, ToSendBytes.Length);
        }

        private void ReceiveData(ClientInfo Client, ReceiveDataDel DataDel)
        {
            IPEndPoint end = new IPEndPoint(Client.IpAddress, Client.Port);
            byte[] ReveivedBytes = Client.Socket.Receive(ref end);
            string[] data = Encoding.UTF8.GetString(ReveivedBytes).Split(';');
            DataDel((UseCase)Enum.Parse(typeof(UseCase), data[0]), data[1]);
        }
        public static bool SendDataSicher(UdpClient Socket, string data, int MaxTrys = 5, int MaxRespones = 3)
        {
            do
            {
                byte[] ToSendBytes = Encoding.UTF8.GetBytes(data);
                IPEndPoint EndPoint = (IPEndPoint)Socket.Client.RemoteEndPoint;
                Socket.Send(ToSendBytes, ToSendBytes.Length);
                if (MaxRespones > 0)
                {
                    string str = ReceiveDataSicher(Socket, ref EndPoint, MaxTrys, MaxRespones - 1);
                    if (str == Settings.ReseivedMessage) return true;
                }
                else
                {
                    return true;
                }
                MaxTrys--;
            } while (0 < MaxTrys);
            return false;
        }

        public static string ReceiveDataSicher(UdpClient Socket, ref IPEndPoint EndPoint, int MaxTrys = 5, int MaxRespones = 2)
        {
            do
            {
                try
                {
                    byte[] ReveivedBytes = Socket.Receive(ref EndPoint);
                    string data = StringEncrypt.instance.Decrypt(Encoding.UTF8.GetString(ReveivedBytes));
                    if (MaxRespones > 0)
                    {
                        SendDataSicher(Socket, data, MaxTrys, MaxRespones - 1);
                    }
                    return data;
                }
                catch (TimeoutException)
                {
                    MaxTrys--;
                }

            } while (0 < MaxTrys);
            return null;
        }
    }
}