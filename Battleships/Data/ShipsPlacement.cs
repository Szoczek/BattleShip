using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Resources;
using System.Windows;
using static Battleships.Ship;

namespace Battleships
{
    class ShipsPlacement
    {
        // orientation directions
        public enum Orientation
        {
            West,
            East,
            North,
            South
        };
        public Orientation RandomOrientation()
        {
            Random rnd = new Random();
            int direction = rnd.Next(1, 4);

            return direction == 1 ? Orientation.South : direction == 2 ? Orientation.West :
                direction == 3 ? Orientation.East : Orientation.North;
        }

        //check if tile isnt taken by another ship
        public bool TileNotTaken(WrapPanel board, int id, int shipSize, Orientation orientation)
        {
            bool isFree = true;
            switch (orientation)
            {
                case Orientation.West:
                    for (int i = id, j = 0; j < shipSize; i--, j++) 
                    {
                        if ((board.Children[i] as Button).DataContext != null)
                            isFree = false;
                    }
                    return isFree;

                case Orientation.East:
                    for (int i = id, j = 0; j < shipSize; i++, j++)
                    {
                        if ((board.Children[i] as Button).DataContext != null)
                            isFree = false;
                    }
                    return isFree;

                case Orientation.North:
                    for (int i = id, j = 0; j < shipSize; i -= (int)(board.Width / board.ItemWidth), j++)
                    {
                        if ((board.Children[i] as Button).DataContext != null)
                            isFree = false;
                    }
                    return isFree;

                case Orientation.South:
                    for (int i = id, j = 0; j < shipSize; i += (int)(board.Width / board.ItemWidth), j++) 
                    {
                        if ((board.Children[i] as Button).DataContext != null)
                            isFree = false;
                    }
                    return isFree;

                default:
                    return isFree;
            }
        }
        
        //checks if tile is within WrapPanels border
        public bool TileWithinBorder(WrapPanel board, int id, int size, Orientation orientation)
        {
            switch (orientation)
            {
                case Orientation.West:
                    if (id - size >= 0)
                    {
                        for (int i = id; i >= id - size; i--)
                        {
                            if ((i % (int)(board.Width / board.ItemWidth) == 0))
                                return false;
                        }
                        return true;
                    }
                    else
                        return false;

                case Orientation.East:
                    if (id + size <= board.Children.Count)
                    {
                        for (int i = id; i <= id + size; i++)
                        {
                            if ((i % (int)(board.Width / board.ItemWidth) == 0))
                                return false;
                        }
                        return true;
                    }
                    else
                        return false;

                case Orientation.North:
                    if (id - size * (int)(board.Width / board.ItemWidth) >= 0)
                        return true;
                    else
                         return false;

                case Orientation.South:
                    if (id + size * (int)(board.Width / board.ItemWidth) <= board.Height)
                        return true;
                    else
                        return false;
                default:
                    return false;
            }
        }

        //randomize AI ships placement
        public WrapPanel AutoShipsPlacement(WrapPanel board, Ship[] ships)
        {
            Random rndm = new Random();

            foreach (var ship in ships)
            {
                Orientation orientation = RandomOrientation();
                rndmRowCol:
                int id = rndm.Next(0, board.Children.Count);

                if ((TileWithinBorder(board, id, ship.GetShipHealth(), orientation)) &&
                    (TileNotTaken(board, id, ship.GetShipHealth(), orientation)))
                {
                    switch (orientation)
                    {
                        case Orientation.West:
                            for (int i = id, j = 0; j < ship.GetShipHealth(); i--, j++) 
                            {
                                (board.Children[i] as Button).DataContext = ship;
                                if ((board as WrapPanel).Name == "AIBoard")
                                {
                                    (board.Children[i] as Button).Background = Brushes.Transparent;
                                }
                                else
                                {
                                    (board.Children[i] as Button).Background = Brushes.BlueViolet;
                                    (board.Children[i] as Button).BorderBrush = Brushes.Black;
                                }
                            }
                            break;

                        case Orientation.East:
                            for (int i = id, j = 0; j < ship.GetShipHealth(); i++, j++) 
                            {
                                (board.Children[i] as Button).DataContext = ship;
                                if ((board as WrapPanel).Name == "AIBoard")
                                {
                                    (board.Children[i] as Button).Background = Brushes.Transparent;
                                }
                                else
                                {
                                    (board.Children[i] as Button).Background = Brushes.BlueViolet;
                                    (board.Children[i] as Button).BorderBrush = Brushes.Black;
                                }
                            }
                            break;

                        case Orientation.North:
                            for (int i = id, j = 0; j < ship.GetShipHealth(); i -= (int)(board.Width / board.ItemWidth), j++) 
                            {
                                (board.Children[i] as Button).DataContext = ship;
                                if ((board as WrapPanel).Name == "AIBoard")
                                {
                                    (board.Children[i] as Button).Background = Brushes.Transparent;
                                }
                                else
                                {
                                    (board.Children[i] as Button).Background = Brushes.BlueViolet;
                                    (board.Children[i] as Button).BorderBrush = Brushes.Black;
                                }
                            }
                            break;

                        case Orientation.South:
                            for (int i = id, j = 0; j < ship.GetShipHealth(); i += (int)(board.Width / board.ItemWidth), j++) 
                            {
                                (board.Children[i] as Button).DataContext = ship;
                                if ((board as WrapPanel).Name == "AIBoard")
                                {
                                    (board.Children[i] as Button).Background = Brushes.Transparent;
                                }
                                else
                                {
                                    (board.Children[i] as Button).Background = Brushes.BlueViolet;
                                    (board.Children[i] as Button).BorderBrush = Brushes.Black;
                                }
                            }
                            break;
                    }
                }
                else
                    goto rndmRowCol;
            }
            return board;
        }
        //creates ships
        public Ship[] InitShips(int limit)
        {
            Ship[] ships = new Ship[limit];

            int destroyerLimit = (int)Math.Round(limit * 0.40, MidpointRounding.ToEven); //destroyers limit
            int subsLimit = (int)Math.Round(limit * 0.20, MidpointRounding.ToEven); // sumbarines limit
            int CruiserLimit = (int)Math.Round(limit * 0.20, MidpointRounding.ToEven); // cruisers limit
            int battleshipLimit = (int)Math.Round(limit * 0.10, MidpointRounding.ToEven); // battleships limit
            int carrierLimit = (int)Math.Round(limit * 0.10, MidpointRounding.ToEven); // cariers limit

            for (int i = 0; i < limit; i++)
            {
                if (destroyerLimit > 0)
                {
                    ships[i] = new Ship(ShipTypes.Destroyer,limit - i);
                    destroyerLimit--;
                }
                else if (subsLimit > 0)
                {
                    ships[i] = new Ship(ShipTypes.Submarine, limit - i);
                    subsLimit--;
                }
                else if (CruiserLimit > 0)
                {
                    ships[i] = new Ship(ShipTypes.Cruiser, limit - i);
                    CruiserLimit--;
                }
                else if (battleshipLimit > 0)
                {
                    ships[i] = new Ship(ShipTypes.Battleship, limit - i);
                    battleshipLimit--;
                }
                else if (carrierLimit > 0)
                {
                    ships[i] = new Ship(ShipTypes.Carrier, limit - i);
                    carrierLimit--;
                }
            }
            return ships;
        }
    }
}