using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Battleships.Data;

namespace Battleships
{
    /// <summary>
    /// Logika interakcji dla klasy ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        private Settings settings = new Settings();
       

        public ConfigWindow()
        {
            InitializeComponent();
            DataContext = settings;
        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow mainWindow = new MainWindow(settings.Height, settings.Width, settings.ShipLimit, settings.GetLocalization());
                mainWindow.Show();
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Some information are invalid or empty!\nTo continue enter the valid informations about the game.");
            }
        }
    }
}
