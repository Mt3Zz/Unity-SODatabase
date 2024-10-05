namespace SODatabase.Service
{
    using FloatObject = DataObject.FloatObject;

    public sealed class FloatObjectController : DataObject.IObjectController<FloatObject>
    {
        public float InputValue { get; set; } = default;

        public bool UpdateObject(FloatObject obj)
        {
            if (obj == null) return false;

            obj.Value = InputValue;
            return true;
        }
    }
}
