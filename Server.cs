using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Server;

public class Server
{   
    private readonly List<TcpClient> _clients = new List<TcpClient>();
    private readonly object _lock = new object();
    
    
        public static async Task Main(String[] args)
        {
            Server server = new Server(); 
            await server.StartServer();  
        }
    
        private async Task HandleConnectionAsync(TcpClient tcpClient)
        {
            try
            {
                lock (_lock)
                {
                    _clients.Add(tcpClient);
                }
                Console.WriteLine($"Client connected: {tcpClient.Client.RemoteEndPoint}");
                NetworkStream networkStream = tcpClient.GetStream();
                
                byte[] welcomeMessage = Encoding.ASCII.GetBytes("Connected to server!");
                await networkStream.WriteAsync(welcomeMessage, 0, welcomeMessage.Length);
                
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string datareceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    
                    await BroadcastMessage(datareceived, tcpClient);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                lock (_lock)
                {
                    _clients.Remove(tcpClient); 
                }
                tcpClient.Close();
                Console.WriteLine("Client disconnected");
            }
            
        }

        private async Task BroadcastMessage(string message, TcpClient sender)
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);

            lock (_lock)
            {
                foreach (var client in _clients)
                {
                    if (client != sender)
                    {
                        try
                        {
                            NetworkStream Stream = client.GetStream();
                            if (Stream.CanWrite)
                            {
                                Stream.WriteAsync(messageBytes, 0, messageBytes.Length);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    
                }
                
            }
        }
        
        public async Task StartServer()
        {
            try
            {  
                var listener = new TcpListener(IPAddress.Loopback, 7891);
                listener.Start(10);
                
                Console.WriteLine("Server started, waiting for a connection...");
    

                 while (true)
                 {
                     var acceptTcpClientAsync = await listener.AcceptTcpClientAsync();
                     Console.WriteLine("Started listening...");
                     _ = HandleConnectionAsync(acceptTcpClientAsync);
                
                 }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            

        }
    
    
}