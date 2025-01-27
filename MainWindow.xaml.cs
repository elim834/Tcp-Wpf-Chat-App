using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using EntryPopUp;

namespace LastMessenger;
public partial class MainWindow: Window 
{  
    public  ClientClass ClientClass;
    
    public MainWindow()
    {
        InitializeComponent();
        MessageList = new ObservableCollection<MessageModel>();
        SentView.ItemsSource = MessageList;
        SendButton.Click += SendButton_Click;
        ClientClass = new ClientClass(this);
        DataContext = this;
        Popup.IsOpen = true;
    }
    
    private async void ConnectButton_Click(object sender, RoutedEventArgs e)
    {
        string ip = "127.0.0.1";
        int port = 7891;         
        try
        {
            await ClientClass.StartClient(ip, port); 
            Console.WriteLine("Client started!(Main window)");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in ConnectButton_Click: {ex.Message}");
        }
    }

    private void SendButton_Click(object sender, RoutedEventArgs e)
    {
        string messageToSend = MessagesTextBox.Text;
       
        if (!string.IsNullOrEmpty(messageToSend))
        {
            MessagesTextBox.Clear();
            MessageList.Add(new  MessageModel() { Data ="You: " +   messageToSend });
            ClientClass.ReceiveMessage();
            ClientClass.SendMessage(messageToSend);
            
            
        }
    }

    private ObservableCollection<MessageModel> _messageList;

    public ObservableCollection<MessageModel> MessageList
    {
        get => _messageList;
        set => _messageList = value;
    }

    
    
    public class MessageModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private  string _data;

        public string Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged();
            }
        }
        

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }
}