using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Battleships.Data
{
    class Board
    {
        public WrapPanel InitBoard(WrapPanel board, int height, int width, Game game)
        {
            //wrap panel borders
            board.Height = height; //y
            board.Width = width; //x
            board.ItemHeight = height / 30;
            board.ItemWidth = width / 30;

            //wrap panel background
            board.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("Images/Sea.png", UriKind.Relative))
            };

            //buttons creation
            Button[,] buttons = new Button[(int)board.ItemHeight, (int)board.ItemWidth];
            for (int i = 0; i < (int)board.ItemHeight; i++) //row
            {
                for (int j = 0; j < (int)board.ItemWidth; j++) //col
                {
                    buttons[i, j] = new Button()
                    {
                        Tag = "Unknown",
                        Width = 30,
                        Height = 30,
                        Background = Brushes.Transparent
                    };
                }
            }
            //adding children
            for (int i = 0; i < buttons.GetLength(0); i++)
            {
                for (int j = 0; j < buttons.GetLength(1); j++)
                {
                    //adding click event if AIBoard
                    if (board.Name == "AIBoard")
                        buttons[i, j].Click += new RoutedEventHandler(game.AIBoard_Button_Click);
                    //adding click event if AIBoard
                    else
                        buttons[i, j].Click += new RoutedEventHandler(game.PlayerBoard_Button_Click);

                    board.Children.Add(buttons[i, j]); // adding children to WrapPanel
                }
            }
            return board;
        }
    }
}
