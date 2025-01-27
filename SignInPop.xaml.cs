using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;


namespace EntryPopUp;

public partial class SignInPop : UserControl
{
    public SignInPop()
    {
        InitializeComponent();
        SignInButton.Click += SignInClick;
        this.DataContext = this;
    }

    private List<string> UserList = new List<string>() 
    {
        "name1", "name2", "name3"
    };
    
    private void SignInClick(object sender, RoutedEventArgs e)
    {
        NameCheck();
    }
    
    private bool NameCheck()
    {
        string name = NameTextBox.Text;
        
            if (!UserList.Contains(name))
            {
                MessageTextBlock.Text = "The user name you entered is missing or incorrect.";
                return false;
            }
            else
            {
                MessageTextBlock.Text = $"Access granted. Hi {name}";
                return true;
            }
        
    }
}


    




