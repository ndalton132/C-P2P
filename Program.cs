using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter the IP address to connect to:");
        string ipAddress = Console.ReadLine(); // Specify the desired IP address

        Console.WriteLine("Enter the port number to connect on:");
        int port = int.Parse(Console.ReadLine()); // Specify the desired port number

        // Create a TCP client and connect to the specified IP address and port
        TcpClient client = new TcpClient();
        client.Connect(ipAddress, port);
        Console.WriteLine($"Connected to {ipAddress}:{port}");

        // Start a thread to handle incoming messages
        Thread receiveThread = new Thread(ReceiveMessages);
        receiveThread.Start(client);

        // Send messages
        while (true)
        {
            Console.Write("You: ");
            string message = Console.ReadLine();
            SendMessage(client, message);
        }
    }

    static void ReceiveMessages(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();

        while (true)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Other: " + receivedMessage);
        }
    }

    static void SendMessage(TcpClient client, string message)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        stream.Write(buffer, 0, buffer.Length);
    }
}

