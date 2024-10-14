using System;
using System.Net;
using System.Net.Sockets;

var ip = IPAddress.Parse("192.168.100.115");
var port = 27001;
var endPoint = new IPEndPoint(ip, port);

using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

try
{
    socket.Bind(endPoint);
    socket.Listen();
    Console.WriteLine($"Server:{socket.LocalEndPoint}");

    while (true)
    {
        var client = socket.Accept();
        Console.WriteLine($"Client: {client.RemoteEndPoint}");

        string folder = Path.Combine(socket.RemoteEndPoint.ToString());
        Directory.CreateDirectory(folder);

        Screenshots(socket, folder);
        socket.Close();

    }
    
    

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

void Screenshots(Socket socket,string folder)
{
    try
    {
        var bytes = new byte[1024];
        int readBytes;

        while ((readBytes = socket.Receive(bytes)) > 0)
        {
            string filePath = Path.Combine(folder);
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                stream.Write(bytes, 0, readBytes);
            }
            Console.WriteLine($"Downloaded {filePath}");
        }
    }
    
    catch(Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}