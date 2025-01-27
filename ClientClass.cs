using System.Net;
using System.Windows;
using System.Net.Sockets;
using System.Text;
using LastMessenger;

namespace LastMessenger;

public class ClientClass
{
    TcpClient _tcpClient;
    NetworkStream _stream;
    private readonly MainWindow _mainWindow;

    public ClientClass(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
    }

    
    public async Task StartClient(string ip, int port)
    {
        try
        {
            _tcpClient = new TcpClient();
            Console.WriteLine("Attempting to connect...");
            await _tcpClient.ConnectAsync(ip, port);
            Console.WriteLine("Connected to the server!");
            _stream = _tcpClient.GetStream();
            
            
           
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    public async void SendMessage(string message)
    {
        if (_tcpClient != null && _tcpClient.Connected)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            _stream.WriteAsync(buffer, 0, buffer.Length);
        }
    }
    
    public async void ReceiveMessage()
    {   
        
        try
        {
            byte[] buffer = new byte[1024];
            
            while (true)
            {
                int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);

                if (bytesRead == 0)
                {
                    Console.WriteLine("Connection closed by server.");
                    break;
                }

                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                _mainWindow.Dispatcher.Invoke(() =>
                {
                    _mainWindow.MessageList.Add(new MainWindow.MessageModel { Data = receivedMessage });
                });
                
            }
            if (!_tcpClient.Connected)
            {
                Console.WriteLine("Cannot receive message: TCP client is not connected.");
                return; 
            }

        }
        
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    
} 
 