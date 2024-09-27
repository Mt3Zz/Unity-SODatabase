namespace SODatabase.Tests.PlayMode.DataObject
{
    internal class TestObjectController : SODatabase.DataObject.IObjectController<TestObject>
    {
        public bool Update(TestObject obj)
        {
            obj.TestValue += "\nAdditional String";
            return true;
        }
    }
}
