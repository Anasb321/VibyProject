using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VibyApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Home_Click(object sender, RoutedEventArgs e)
    {
        // Already on home page
    }

    private void Profile_Click(object sender, RoutedEventArgs e)
    {
        ProfileWindow profile = new ProfileWindow();
        profile.Show();
        this.Close();
    }
}