namespace SODatabase.Service
{
    using BoolObject = DataObject.BoolObject;

    public sealed class BoolObjectController : DataObject.IObjectController<BoolObject>
    {
        public bool InputValue { get; internal set; } = false;

        public bool UpdateObject(BoolObject obj)
        {
            if (obj == null) return false;

            obj.Value = InputValue;
            return true;
        }
    }
}
