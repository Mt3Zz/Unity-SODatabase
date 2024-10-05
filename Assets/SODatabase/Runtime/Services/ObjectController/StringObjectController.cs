namespace SODatabase.Service
{
    using StringObject = DataObject.StringObject;

    public sealed class StringObjectController : DataObject.IObjectController<StringObject>
    {
        public string InputValue { get; set; } = "";

        public bool UpdateObject(StringObject obj)
        {
            if (obj == null) return false;

            obj.Value = InputValue;
            return true;
        }
    }
}
