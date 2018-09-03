using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Battleships.Data;

namespace Battleships
{
    public partial class MainWindow : Window
    {
        //game starting parameters
        private int height, width, shipLimit;
        private static string location;
        private Game game;
        private Board board;
        private ShipsPlacement aiPlacement;
        private ShipsPlacement playerPlacement; 

        public MainWindow(int Height, int Width, int ShipLimit, string Location)
        {
            InitializeComponent();

            //init board parameters
            height = Height;
            width = Width;
            shipLimit = ShipLimit;
            location = Location;

            //init settings
            game = new Game(location);
            board = new Board();
            aiPlacement = new ShipsPlacement();
            playerPlacement = new ShipsPlacement();

            //bind data to xaml
            SetBoards();
            SetWeatherData();
        }

        //restart game
        private void GameResetButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigWindow configWindow = new ConfigWindow();
            configWindow.Show();
            this.Close();
        }

        //player & AI boards update
        void SetBoards()
        {
            Ship[] aiShips = aiPlacement.InitShips(shipLimit);
            AIBoard = board.InitBoard(AIBoard, height, width, game);
            AIBoard = aiPlacement.AutoShipsPlacement(AIBoard, aiShips);
            game.SetAIBoard(AIBoard);

            Ship[] playerShips = playerPlacement.InitShips(shipLimit);
            PlayerBoard = board.InitBoard(PlayerBoard, height, width, game);
            PlayerBoard = playerPlacement.AutoShipsPlacement(PlayerBoard, playerShips);
            game.SetPlayerBoard(PlayerBoard);
        }

        //weather data update
        void SetWeatherData()
        {
            Image imageArrow = WindDirArrowImg;
            WindSpeedLbl.Text = game.WeatherData.windSpeed;
            WindDirectionLbl.Text = game.WeatherData.windDirection;
            RotateTransform rotateTransform = new RotateTransform(game.WeatherData.GetWeatherResponse().Data.Wind.Deg);
            imageArrow.RenderTransform = rotateTransform;
        }
    }
}
