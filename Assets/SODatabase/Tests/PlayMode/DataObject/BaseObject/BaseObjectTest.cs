using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


namespace SODatabase.Tests.PlayMode.DataObject
{
    public class BaseObjectTest
    {
        [Test]
        public void Delete_CallsMethod_IsDeletedIsTrue()
        {
            // Arrange
            var obj = ScriptableObject.CreateInstance<TestObject>();
            Assert.That(obj.IsDeleted, Is.False);


            // Act
            obj.Delete();


            // Assert
            Assert.That(obj.IsDeleted, Is.True);
        }
        [UnityTest]
        public IEnumerator Delete_CallsMethod_UpdatesRecordInfo()
        {
            // Arrange
            var obj = ScriptableObject.CreateInstance<TestObject>();
            var initialUpdatedAt = obj.UpdatedAt;
            var initialVersion = obj.Version;


            // Act
            yield return null;
            obj.Delete();


            // Assert
            Assert.That(obj.IsDeleted, Is.True);
            Assert.That(obj.UpdatedAt, Is.GreaterThan(initialUpdatedAt));
            Assert.That(obj.Version, Is.GreaterThan(initialVersion));
        }


        [Test]
        public void Restore_CallsMethod_IsDeletedIsFalse()
        {
            // Arrange
            var obj = ScriptableObject.CreateInstance<TestObject>();
            obj.Delete(); // Set it to deleted
            Assert.That(obj.IsDeleted, Is.True);


            // Act
            obj.Restore();


            // Assert
            Assert.That(obj.IsDeleted, Is.False);
        }
        [UnityTest]
        public IEnumerator Restore_CallsMethod_UpdatesRecordInfo()
        {
            // Arrange
            var obj = ScriptableObject.CreateInstance<TestObject>();
            obj.Delete(); // Set it to deleted
            var initialUpdatedAt = obj.UpdatedAt;
            var initialVersion = obj.Version;


            // Act
            yield return null;
            obj.Restore();


            // Assert
            Assert.That(obj.IsDeleted, Is.False);
            Assert.That(obj.UpdatedAt, Is.GreaterThan(initialUpdatedAt));
            Assert.That(obj.Version, Is.GreaterThan(initialVersion));
        }


        [Test]
        public void Save_CallsMethod_CalledSaverSaveMethod()
        {
            // Arrange
            var obj = ScriptableObject.CreateInstance<TestObject>();
            var mockSaver = new MockSaver();
            Assert.That(mockSaver.IsSaved, Is.False);


            // Act
            obj.Save(mockSaver);


            // Assert
            Assert.That(mockSaver.IsSaved, Is.True);
        }

        [Test]
        public void Load_CallsMethod_CalledSaverLoadMethod()
        {
            // Arrange
            var obj = ScriptableObject.CreateInstance<TestObject>();
            var mockSaver = new MockSaver();
            Assert.That(mockSaver.IsLoaded, Is.False);


            // Act
            obj.Load(mockSaver);


            // Assert
            Assert.That(mockSaver.IsLoaded, Is.True);
        }


        [Test]
        public void Equals_DifferentObjects_AreNotEqual()
        {
            // Arrange
            var obj1 = ScriptableObject.CreateInstance<TestObject>();
            var obj2 = ScriptableObject.CreateInstance<TestObject>();


            // Act
            var result = obj1.Equals(obj2);


            // Assert
            Assert.That(result, Is.False);
        }
        [Test]
        public void Equals_SameObjects_AreEqual()
        {
            // Arrange
            var obj = ScriptableObject.CreateInstance<TestObject>();


            // Act
            var result = obj.Equals(obj);


            // Assert
            Assert.That(result, Is.True);
        }
    }
}
