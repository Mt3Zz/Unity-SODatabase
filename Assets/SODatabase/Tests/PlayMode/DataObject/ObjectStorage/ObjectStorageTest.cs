using NUnit.Framework;
using System.Linq;
using UnityEngine;


namespace SODatabase.Tests.PlayMode.DataObject
{
    using ObjectId = SODatabase.DataObject.ObjectId;
    using ObjectStorage = SODatabase.DataObject.ObjectStorage;


    public class ObjectStorageTest
    {
        private TestObject _obj = default;
        private TestObject _obj1 = default;
        private TestObject _obj2 = default;

        private ObjectId _id = default;
        private ObjectId _id1 = default;
        private ObjectId _id2 = default;


        [SetUp]
        public void Setup()
        {
            _id = new ObjectId("Test");
            _id1 = new ObjectId("Test1");
            _id2 = new ObjectId("Test2");

            _obj = ScriptableObject.CreateInstance<TestObject>();
            _obj1 = ScriptableObject.CreateInstance<TestObject>();
            _obj2 = ScriptableObject.CreateInstance<TestObject>();
        }


        [Test]
        public void DuplicatesObjects_SameIds_FindsDuplicatedObject()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();

            _obj1.SetId(_id);
            _obj2.SetId(_id);

            storage.Objects.Add(_obj1);
            storage.Objects.Add(_obj2);


            // Act
            var duplicates = storage.DuplicatedObjects;


            // Assert
            Assert.That(storage.Objects.Contains(_obj1), Is.True);
            Assert.That(storage.Objects.Contains(_obj2), Is.True);
        }
        [Test]
        public void DuplicatesObjects_DifferentIds_DoesNotFindDuplicatedObject()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();

            _obj1.SetId(_id1);
            _obj2.SetId(_id2);

            storage.Objects.Add(_obj1);
            storage.Objects.Add(_obj2);


            // Act
            var duplicates = storage.DuplicatedObjects;


            // Assert
            Assert.That(duplicates.Count, Is.EqualTo(0));
            Assert.That(duplicates.Contains(_obj1), Is.False);
            Assert.That(duplicates.Contains(_obj2), Is.False);
        }


        [Test]
        public void ReadObject_ValidId_GetObject()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();

            _obj.SetId(_id);
            storage.Objects.Add(_obj);


            // Act
            var obj = storage.ReadObject(_id);


            // Assert
            Assert.That(obj, Is.EqualTo(_obj));
        }
        [Test]
        public void ReadObject_InvalidId_GetNullObject()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();

            _obj.SetId(_id1);
            storage.Objects.Add(_obj);


            // Act
            var obj = storage.ReadObject(_id2);


            // Assert
            Assert.That(obj, Is.EqualTo(null));
        }


        [Test]
        public void UpdateObject_ValidId_SetValueToObject()
        {
            // Arrange
            var obj = ScriptableObject.CreateInstance<TestObject>();
            obj.SetId(_id);

            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.Objects.Add(obj);

            var controller = new TestObjectController();

            var expected = "TestObject"
                + "\nAdditional String";


            // Act
            var isUpdated = storage.UpdateObject<TestObject>(_id, controller);


            // Assert
            Assert.That(isUpdated, Is.True);
            Assert.That(obj.TestValue, Is.EqualTo(expected));
        }
        [Test]
        public void UpdateObject_InvalidId_DoesNotChangeObjectValue()
        {
            // Arrange
            var obj = ScriptableObject.CreateInstance<TestObject>();
            obj.SetId(_id1);

            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.Objects.Add(obj);

            var controller = new TestObjectController();

            var expected = "TestObject";


            // Act
            var isUpdated = storage.UpdateObject<TestObject>(_id2, controller);


            // Assert
            Assert.That(isUpdated, Is.False);
            Assert.That(obj.TestValue, Is.EqualTo(expected));
        }
    }
}
