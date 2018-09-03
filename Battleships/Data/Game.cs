using Battleships.Data;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Battleships
{
    public class Game
    {
        //game
        private GameStates GameState { get; set; }
        private WrapPanel PlayerBoard { get; set; }
        private WrapPanel AIBoard { get; set; }
        private int TurnCounter { get; set; }
        private Turn Turn { get; set; }
        private bool AutoGame { get; set; } = false;

        //weather
        private string Localization { get; set; }
        public WeatherData WeatherData { get; set; }

        public enum GameStates
        {
            Finished,
            Ongoing
        }

        public void SetPlayerBoard(WrapPanel playerBoard)
        {
            this.PlayerBoard = playerBoard;
            Turn = new Turn(PlayerBoard);
        }

        public void SetAIBoard(WrapPanel aiBoard)
        {
            this.AIBoard = aiBoard;
        }

        public Game(string location)
        {
            Localization = location;
            this.GameState = GameStates.Ongoing;
            WeatherData = new WeatherData(location);
            TurnCounter = 0;
        }

        private int GetRandomButton(int max)
        {
            Random random = new Random();
            return random.Next(max);

        }

        private int ButtonRenderWindHit(WrapPanel board, Button button)
        {
            int id = board.Children.IndexOf(button);
            int row = (int)(board.Width / board.ItemWidth);

            int windSpeed = (int)Math.Round(WeatherData.GetWeatherResponse().Data.Wind.Speed);
            double windDegree = WeatherData.GetWeatherResponse().Data.Wind.Deg;

            switch (WeatherData.WindDirection(windDegree))
            {
                case WeatherData.WindDir.North:
                    if (id - windSpeed * row >= 0)
                    {
                        return id - windSpeed * row;
                    }
                    else
                        return board.Children.Count + id - windSpeed * row;

                case WeatherData.WindDir.NorthEast:
                    if (id + windSpeed - windSpeed * row >= 0)
                    {
                        return id + windSpeed - windSpeed * row;
                    }
                    else
                        return board.Children.Count + id + windSpeed - windSpeed * row;

                case WeatherData.WindDir.East:
                    if (id + windSpeed < board.Children.Count)
                    {
                        if ((id + windSpeed < (id / row + 1) * row))
                            return id + windSpeed;
                        else
                            return id - row + windSpeed;
                    }
                    else
                        return id - row + windSpeed;

                case WeatherData.WindDir.SouthEast:
                    if (id + windSpeed + (windSpeed * row) < board.Children.Count)
                    {
                        return id + windSpeed + (windSpeed * row);
                    }
                    else
                        return row - (board.Children.Count - id) + windSpeed * row - row + windSpeed;

                case WeatherData.WindDir.South:
                    if (id + windSpeed * row < board.Children.Count)
                    {
                        return id + windSpeed * row;
                    }
                    else
                        return row - (board.Children.Count - id) + windSpeed * row - row;

                case WeatherData.WindDir.SouthWest:
                    if (id - windSpeed + (windSpeed * row) < board.Children.Count)
                    {
                        return id - windSpeed + (windSpeed * row);
                    }
                    else
                        return row - (board.Children.Count - id) + windSpeed * row - row - windSpeed;

                case WeatherData.WindDir.West:
                    if (id - windSpeed > 0)
                    {
                        if ((id - windSpeed > (id / row + 1) * row - row))
                            return id - windSpeed;
                        else
                            return id + row - windSpeed;
                    }
                    else
                        return id + row - windSpeed;

                case WeatherData.WindDir.NorthWest:
                    if (id - windSpeed - (windSpeed * row) >= 0)
                    {
                        return id - windSpeed - (windSpeed * row);
                    }
                    else
                        return board.Children.Count + id - windSpeed - windSpeed * row;

                default:
                    return board.Children.IndexOf((button as Button));
            }
        }

        public async void AIBoard_Button_Click(object sender, RoutedEventArgs e)
        {
            Button aiButton = (sender as Button);
            WrapPanel aiPanel = (aiButton.Parent as WrapPanel);

            if (GameState == GameStates.Ongoing)
            {
                //player turn
                TurnCounter++;
                await Dispatcher.CurrentDispatcher.InvokeAsync(() => Turn.PlayerTurn(
                aiPanel.Children[ButtonRenderWindHit(aiPanel, aiButton)] as Button));
                CheckIfPlayerWon(aiButton);

                //ai turn
                Button playerButton = await Dispatcher.CurrentDispatcher.InvokeAsync(() => Turn.AiAlghoritm());
                await Dispatcher.CurrentDispatcher.InvokeAsync(() => Turn.PlayerTurn(playerButton));
                CheckIfAiWon(playerButton);

                await Dispatcher.CurrentDispatcher.InvokeAsync(() => WeatherData.WeatherInfoUpdate());
            }
            else
                MessageBox.Show("The game is finished!");
        }

        public void PlayerBoard_Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You cannot fire at your own ships!");
        }

        private void CheckIfPlayerWon(Button button)
        {
            bool win = true;
            foreach (Button btn in (button.Parent as WrapPanel).Children)
            {
                if (btn.DataContext != null)
                    if ((btn.DataContext as Ship).GetShipCondition() != Ship.ShipCondition.Sunken)
                        win = false;
            }
            if (win)
            {
                MessageBox.Show($"You win! Game was {TurnCounter} turns long.");
                GameState = GameStates.Finished;
            }
        }

        private void CheckIfAiWon(Button button)
        {
            bool win = true;
            foreach (Button btn in (button.Parent as WrapPanel).Children)
            {
                if ((btn.DataContext != null))
                    if ((btn.DataContext as Ship).GetShipCondition() != Ship.ShipCondition.Sunken)
                        win = false;
            }
            if (win)
            {
                MessageBox.Show($"You loose! Game was {TurnCounter} turns long.");
                GameState = GameStates.Finished;
            }
        }
    }
}
