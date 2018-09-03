using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    public class Ship
    {
        
        private int ShipId { get; set; }
        private ShipTypes Type { get; set; }
        private ShipCondition Condition { get; set; }
        private int Health { get; set; }

        public Ship()
        {

        }

        public Ship(ShipTypes type, int id)
        {
            ShipId = id;
            Type = type;
            Condition = ShipCondition.Intact;
            switch (Type)
            {
                case ShipTypes.Destroyer:
                    Health = 2;
                    break;
                case ShipTypes.Submarine:
                    Health = 3;
                    break;
                case ShipTypes.Cruiser:
                    Health = 4;
                    break;
                case ShipTypes.Battleship:
                    Health = 5;
                    break;
                case ShipTypes.Carrier:
                    Health = 5;
                    break;
            }
        }

        public int GetShipId()
        {
            return ShipId;
        }
        public ShipTypes GetShipType()
        {
            return Type;
        }
        public ShipCondition GetShipCondition()
        {
            return Condition;
        }
        public int GetShipHealth()
        {
            return Health;
        }
        public void FireAt()
        {
            Health--;
            if (Health > 0)
                Condition = ShipCondition.Damaged;
            else
                Condition = ShipCondition.Sunken;
        }
        public bool IsSunken()
        {
            return Health < 1 ? true : false;
        }

        //ships types
        public enum ShipTypes
        {
            Destroyer,
            Submarine,
            Cruiser,
            Battleship,
            Carrier
        };
        public enum ShipCondition
        {
            Intact,
            Damaged,
            Sunken
        }
    }
}
