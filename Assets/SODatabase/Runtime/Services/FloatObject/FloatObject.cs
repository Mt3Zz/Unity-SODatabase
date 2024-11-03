using UnityEngine;

namespace SODatabase
{
    public sealed class FloatObject : DataObject.BaseObject
    {
        public float Value
        {
            get => _value;
            set => _value = value;
        }
        [SerializeField]
        private float _value = 0f;


        public void Add(float amount)
        {
            _value += amount;
        }
        public void Substract(float amount)
        {
            _value -= amount;
        }
    }
}
