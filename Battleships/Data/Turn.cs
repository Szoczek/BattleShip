using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Battleships
{
    class Turn
    {
        private WrapPanel PlayerBoard;

        public Turn(WrapPanel wrapPanel)
        {
            PlayerBoard = wrapPanel;
        }

        public async void PlayerTurn(Button button)
        {
            if (button.DataContext != null)
            {
                await Dispatcher.CurrentDispatcher.InvokeAsync(() => MediaPlay(button, true));

                if (button.Content == null)
                {
                    (button.DataContext as Ship).FireAt();
                    button.Content = "Hit";
                }

                if ((button.DataContext as Ship).IsSunken())
                {
                    foreach (Button tile in (button.Parent as WrapPanel).Children)
                    {
                        if ((tile.DataContext != null) && (((tile.DataContext as Ship).GetShipId()) ==
                            (button.DataContext as Ship).GetShipId()))
                        {
                            tile.Background = Brushes.Red;
                            tile.Content = "Sunken";
                        }
                    }
                }
                else
                    button.Background = Brushes.Orange;
            }
            else
            {
                button.Tag = "Water";
                button.Background = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("Images/SeaHit.png", UriKind.Relative))
                };
                await Dispatcher.CurrentDispatcher.InvokeAsync(() => MediaPlay(button, false));
            }
        }
        //AI turn alghoritm
        public Button AiAlghoritm()
        {
            Button output = null;
            int neighboringButtons = 4;
            Button[] neigborTiles = new Button[neighboringButtons];
            for (int i = 0, j = 0; i < PlayerBoard.Children.Count; i++)
            {
                if ((PlayerBoard.Children[i] as Button).DataContext != null)
                {
                    if (((((PlayerBoard.Children[i] as Button).DataContext as Ship).GetShipCondition() == Ship.ShipCondition.Damaged) &&
                        ((PlayerBoard.Children[i] as Button).Content != null) && (NeighborsAvaible(i))))
                    {
                        //lower tile
                        if (i - (int)(PlayerBoard.Width / PlayerBoard.ItemWidth) >= 0) // is null?
                        {
                            if (CheckIfNotVisible(PlayerBoard.Children[i - (int)(PlayerBoard.Width / PlayerBoard.ItemWidth)] as Button))
                            {
                                neigborTiles[j] = PlayerBoard.Children[i - (int)(PlayerBoard.Width / PlayerBoard.ItemWidth)] as Button;
                                j++;
                            }
                            else
                            {
                                neigborTiles[j] = null;
                                neighboringButtons--;
                            }
                        }
                        else
                        {
                            neigborTiles[j] = null;
                            neighboringButtons--;
                        }

                        //upper tile 
                        if (i + (int)(PlayerBoard.Width / PlayerBoard.ItemWidth) <= PlayerBoard.Children.Count) // is null?
                        {
                            if (CheckIfNotVisible(PlayerBoard.Children[i + (int)(PlayerBoard.Width / PlayerBoard.ItemWidth)] as Button))
                            {
                                neigborTiles[j] = PlayerBoard.Children[i + (int)(PlayerBoard.Width / PlayerBoard.ItemWidth)] as Button;
                                j++;
                            }
                            else
                            {
                                neigborTiles[j] = null;
                                neighboringButtons--;
                            }
                        }
                        else
                        {
                            neigborTiles[j] = null;
                            neighboringButtons--;
                        }

                        //left tile
                        if (i - 1 >= 0) // is null?
                        {
                            if (CheckIfNotVisible(PlayerBoard.Children[i - 1] as Button))
                            {
                                neigborTiles[j] = PlayerBoard.Children[i - 1] as Button;
                                j++;
                            }
                            else
                            {
                                neigborTiles[j] = null;
                                neighboringButtons--;
                            }
                        }
                        else
                        {
                            neigborTiles[j] = null;
                            neighboringButtons--;
                        }

                        //right tile
                        if (i + 1 <= PlayerBoard.Children.Count) // is null?
                        {
                            if (CheckIfNotVisible(PlayerBoard.Children[i + 1] as Button))
                            {
                                neigborTiles[j] = PlayerBoard.Children[i + 1] as Button;
                                j++;
                                break;
                            }
                            else
                            {
                                neigborTiles[j] = null;
                                neighboringButtons--;
                                break;
                            }
                        }
                        else
                        {
                            neigborTiles[j] = null;
                            neighboringButtons--;
                            break;
                        }
                    }
                }
            }
            if (neigborTiles.All(x => x == null))
                return PlayerBoard.Children[GetRandom(PlayerBoard.Children.Count)] as Button;

            while (output == null)
            {
                output = neigborTiles[GetRandom(neighboringButtons)];
            }
            return output;
        }

        //check if tile should be Avaible to be chosen by AI
        private bool CheckIfNotVisible(Button tile)
        {
            if (tile.DataContext != null)
            {
                if (((tile.DataContext as Ship).GetShipCondition() != Ship.ShipCondition.Sunken) && (tile.Content == null))
                    return true;
                else
                    return false;
            }
            else
            {
                if (tile.Tag.ToString() == "Water")
                    return false;
                else
                    return true;
            }
        }

        //checks if neighbooring tiles arent hit already
        private bool NeighborsAvaible(int id)
        {
            Button[] neighboors = GetNeighborTiles(id);

            bool cnd = true;
            foreach (Button tile in neighboors)
            {
                if (tile != null)
                {
                    if (tile.DataContext != null)
                    {
                        if (((tile.DataContext as Ship).GetShipCondition() != Ship.ShipCondition.Sunken) && (tile.Content == null))
                            return true;
                        else
                            cnd = false;
                    }
                    else
                    {
                        if (tile.Tag.ToString() == "Water")
                            cnd = false;
                        else
                            return true;
                    }
                }
                else
                    cnd = false;
            }
            return cnd;
        }

        //init neighbors of id tile
        private Button[] GetNeighborTiles(int id)
        {
            Button[] neighboors = new Button[4];

            //tile left
            neighboors[0] = id - 1 < 0 ? null : PlayerBoard.Children[id - 1] as Button;
            //tile right
            neighboors[1] = id + 1 > PlayerBoard.Children.Count ? null : PlayerBoard.Children[id + 1] as Button;
            //tile up
            neighboors[2] = id - (int)(PlayerBoard.Width / PlayerBoard.ItemWidth) < 0 ?
                null : PlayerBoard.Children[id - (int)(PlayerBoard.Width / PlayerBoard.ItemWidth)] as Button;
            //tile down
            neighboors[3] = id + (int)(PlayerBoard.Width / PlayerBoard.ItemWidth) > PlayerBoard.Children.Count ?
                null : PlayerBoard.Children[id + (int)(PlayerBoard.Width / PlayerBoard.ItemWidth)] as Button;

            return neighboors;
        }


        //Donald J Trumps tips...
        private void MediaPlay(Button button, bool hit)
        {
            if ((button.Parent as WrapPanel).Name == "AIBoard")
            {
                MediaPlayer mediaPlayer = new MediaPlayer();
                if (hit)
                    mediaPlayer.Open(new Uri("Sounds/Correct.mp3", UriKind.Relative));
                else
                    mediaPlayer.Open(new Uri("Sounds/Wrong.mp3", UriKind.Relative));
                mediaPlayer.Play();
            }
        }

        //random number generator
        private int GetRandom(int max)
        {
            Random random = new Random();
            return random.Next(max);
        }
    }
}
