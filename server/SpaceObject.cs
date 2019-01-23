using UnityEngine;
using DarkRift;

namespace Game
{ 
    public enum Command { MoveTo, WarpTo, Atack, SetTarget, LandTo, Equipment, Open, TakeOff };
    public enum TypeSO { asteroid, ship, station, waypoint, container };

    public class SpaceObject: IDarkRiftSerializable
    {
        public int Id { get; set; }
        public string VisibleName { get; set; }
        public TypeSO Type { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; } = new Quaternion(0f, 0f, 0f, 1f);
        public float Speed { get; set; }
        public string Prefab { get; set; }
        //public SO_ship ship { get; set; }


        public SpaceObject() { }

        public SpaceObject(SpaceObject value)
        {
            Id = value.Id;
            VisibleName = value.VisibleName;
            Type = value.Type;
            Position = value.Position;
            Rotation = value.Rotation;
            Speed = value.Speed;
            Prefab = value.Prefab;
        }

        public void Deserialize(DeserializeEvent e)
        {
            throw new System.NotImplementedException();
        }

        public void Serialize(SerializeEvent e)
        {
            throw new System.NotImplementedException();
        }
    }
    
}