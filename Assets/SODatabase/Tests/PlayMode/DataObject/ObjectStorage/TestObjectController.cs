namespace SODatabase.Tests.PlayMode.DataObject
{
    internal class TestObjectController : SODatabase.DataObject.IObjectController<TestObject>
    {
        public bool UpdateObject(TestObject obj)
        {
            obj.TestValue += "\nAdditional String";
            return true;
        }
    }
}
