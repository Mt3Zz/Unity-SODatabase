namespace SODatabase.Service
{
    using IntObject = DataObject.IntObject;

    public sealed class IntObjectController : DataObject.IObjectController<IntObject>
    {
        public int InputValue { get; set; } = default;

        public bool UpdateObject(IntObject obj)
        {
            if (obj == null) return false;

            obj.Value = InputValue;
            return true;
        }
    }
}
