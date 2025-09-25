
using Master;
using System.Net;
using System.Net.Sockets;

Console.WriteLine("Master server");
// Tcp 3 way handshake make that
int port = 7;
TcpListener listener = new TcpListener(IPAddress.Any, port);

listener.Start();


while (true)
{

    // Waiting for connection
    TcpClient socket = listener.AcceptTcpClient();
    Task.Run(() => HandleClient.Handleclient(socket));

}





