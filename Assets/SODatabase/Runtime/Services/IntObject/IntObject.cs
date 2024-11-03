using UnityEngine;

namespace SODatabase
{
    public sealed class IntObject : DataObject.BaseObject
    {
        public int Value
        {
            get => _value;
            set => _value = value;
        }
        [SerializeField]
        private int _value = 0;


        public void Add(int amount)
        {
            _value += amount;
        }
        public void Substract(int amount)
        {
            _value -= amount;
        }
    }
}
