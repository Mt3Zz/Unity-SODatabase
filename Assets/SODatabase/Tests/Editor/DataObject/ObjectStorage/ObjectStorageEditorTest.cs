using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NUnit.Framework;

namespace SODatabase.Editor.Tests.DataObject
{
    using SODatabase.DataObject;


    public class ObjectStorageEditorTest
    {
        private IntObject _obj = default;
        private FloatObject _anotherObj = default;

        private ObjectId _id = default;
        private ObjectId _anotherId = default;


        [SetUp]
        public void Setup()
        {
            _id = new ObjectId("Test");
            _anotherId = new ObjectId("Another");

            _obj = ScriptableObject.CreateInstance<IntObject>();
            _anotherObj = ScriptableObject.CreateInstance<FloatObject>();

            _obj.SetId(_id);
            _anotherObj.SetId(_anotherId);
        }


        [Test]
        public void DeleteObjectForEditor_IncrudesObject_SucceedsToDelete()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);


            // Act
            var isDeleted = storage.DeleteObjectForEditor(_obj);


            // Assert
            Assert.That(isDeleted, Is.True);
            Assert.That(storage.Objects,        Has.Count.Zero);
            Assert.That(storage.TrashedObjects, Has.Count.EqualTo(1).And.Contains(_obj));
        }
        [Test]
        public void DeleteObjectForEditor_DoesnotIncrudeObject_FailsToDelete()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);


            // Act
            var isDeleted = storage.DeleteObjectForEditor(_anotherObj);


            // Assert
            Assert.That(isDeleted, Is.False);
            Assert.That(storage.Objects,        Has.Count.EqualTo(1).And.Contains(_obj));
            Assert.That(storage.TrashedObjects, Has.Count.Zero);
        }


        [Test]
        public void RestoreObjectForEditor_IncludesObject_SucceedsToRestore()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.DeleteObjectForEditor(_obj);


            // Act
            var isRestored = storage.RestoreTrashedObjectForEditor(_obj);


            // Assert
            Assert.That(isRestored, Is.True);
            Assert.That(storage.Objects,        Has.Count.EqualTo(1).And.Contains(_obj));
            Assert.That(storage.TrashedObjects, Has.Count.Zero);
        }
        [Test]
        public void RestoreObjectForEditor_DoesnotIncrudeObject_FailsToRestore()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.DeleteObjectForEditor(_obj);


            // Act
            var isRestored = storage.RestoreTrashedObjectForEditor(_anotherObj);


            // Assert
            Assert.That(isRestored, Is.False);
            Assert.That(storage.Objects,        Has.Count.Zero);
            Assert.That(storage.TrashedObjects, Has.Count.EqualTo(1).And.Contains(_obj));
        }


        [Test]
        public void RemoveObjectForEditor_IncludesObject_SucceedsToRemove()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.DeleteObjectForEditor(_obj);


            // Act
            var isRemoved = storage.RemoveTrashedObjectForEditor(_obj);


            // Assert
            Assert.That(isRemoved, Is.True);
            Assert.That(storage.Objects,        Has.Count.Zero);
            Assert.That(storage.TrashedObjects, Has.Count.Zero);
        }
        [Test]
        public void RemoveObjectForEditor_DoesnotIncludeObject_FailsToRemove()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.DeleteObjectForEditor(_obj);


            // Act
            var isRemoved = storage.RestoreTrashedObjectForEditor(_anotherObj);


            // Assert
            Assert.That(isRemoved, Is.False);
            Assert.That(storage.Objects,        Has.Count.Zero);
            Assert.That(storage.TrashedObjects, Has.Count.EqualTo(1).And.Contains(_obj));
        }


        [Test]
        public void InitOrganizerForEditor_TargetsObjects_OrganizedListEqualsObjects()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);


            // Act
            storage.InitOrganizerForEditor();


            // Assert
            Assert.That(storage.OrganizedListForEditor, Has.Member(_obj));
            Assert.That(storage.OrganizedListForEditor, Is.EqualTo(storage.Objects));
        }
        [Test]
        public void InitOrganizerForEditor_TargetsTrashedObjects_OrganizedListEqualsTrashedObjects()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.DeleteObjectForEditor(_obj);


            // Act
            storage.InitOrganizerForEditor(targetsTrashedObjects : true);


            // Assert
            Assert.That(storage.OrganizedListForEditor, Has.Member(_obj));
            Assert.That(storage.OrganizedListForEditor, Is.EqualTo(storage.TrashedObjects));
        }


        [Test]
        public void DistinctForEditor_DuplicatedObjects_DistinctsObjects()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.AppendObject(_obj);

            Assert.That(storage.Objects, Has.Count.EqualTo(2).And.Contains(_obj));


            // Act
            storage.DistinctForEditor();


            // Assert
            Assert.That(storage.Objects, Has.Count.EqualTo(1).And.Contains(_obj));
            Assert.That(storage.OrganizedListForEditor, Is.EqualTo(storage.Objects));
        }
        [Test]
        public void DistinctForEditor_DifferentObjects_DoesnotDistinctObjects()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.AppendObject(_anotherObj);

            Assert.That(storage.Objects, Has.Count.EqualTo(2).And.Contains(_obj));


            // Act
            storage.DistinctForEditor();


            // Assert
            Assert.That(storage.Objects,                Has.Count.EqualTo(2).And.Contains(_obj).And.Contains(_anotherObj));
            Assert.That(storage.OrganizedListForEditor, Is.EqualTo(storage.Objects));
        }
        [Test]
        public void DistinctForEditor_DuplicatedTrashedObjects_DistinctsTrashedObjects()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.DeleteObjectForEditor(_obj);
            storage.AppendObject(_obj);
            storage.DeleteObjectForEditor(_obj);

            Assert.That(storage.TrashedObjects, Has.Count.EqualTo(2).And.Contains(_obj));


            // Act
            storage.DistinctForEditor(targetsTrashedObjects : true);


            // Assert
            Assert.That(storage.TrashedObjects,         Has.Count.EqualTo(1).And.Contains(_obj));
            Assert.That(storage.OrganizedListForEditor, Is.EqualTo(storage.TrashedObjects));
        }
        [Test]
        public void DistinctForEditor_DifferentTrashedObjects_DoesnotDistinctTrashedObjects()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.DeleteObjectForEditor(_obj);
            storage.AppendObject(_anotherObj);
            storage.DeleteObjectForEditor(_anotherObj);


            // Act
            storage.DistinctForEditor(true);


            // Assert
            Assert.That(storage.TrashedObjects,         Has.Count.EqualTo(2).And.Contains(_obj).And.Contains(_anotherObj));
            Assert.That(storage.OrganizedListForEditor, Is.EqualTo(storage.TrashedObjects));
        }


        [Test]
        public void SortByNameForEditor_DifferentNameObjects_SortObjectsByName()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.AppendObject(_anotherObj);

            var expected = new List<BaseObject> { _obj, _anotherObj };
            expected = expected.OrderBy(item => item.Id.Name).ToList();


            // Act
            storage.SortByNameForEditor();


            // Assert
            Assert.That(storage.Objects,                Is.EqualTo(expected));
            Assert.That(storage.OrganizedListForEditor, Is.EqualTo(storage.Objects));
        }
        [Test]
        public void SortByNameForEditor_DifferentNameTrashedObjects_SortTrashedObjectsByName()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.DeleteObjectForEditor(_obj);
            storage.AppendObject(_anotherObj);
            storage.DeleteObjectForEditor(_anotherObj);

            var tmp = new List<BaseObject> { _obj, _anotherObj };
            var expected = tmp.OrderBy(item => item.Id.Name).ToList();


            // Act
            storage.SortByNameForEditor(targetsTrashedObjects: true);


            // Assert
            Assert.That(storage.TrashedObjects,         Is.EqualTo(expected));
            Assert.That(storage.OrganizedListForEditor, Is.EqualTo(storage.TrashedObjects));
        }


        [Test]
        public void SortByTypeForEditor_DifferentTypeObjects_SortObjectsByType()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.AppendObject(_anotherObj);

            var expected = new List<BaseObject> { _obj, _anotherObj };
            expected = expected.OrderBy(item => item.GetType().Name).ToList();


            // Act
            storage.SortByNameForEditor();


            // Assert
            Assert.That(storage.Objects,                Is.EqualTo(expected));
            Assert.That(storage.OrganizedListForEditor, Is.EqualTo(storage.Objects));
        }
        [Test]
        public void SortByTypeForEditor_DifferentTypeTrashedObjects_SortTrashedObjectsByType()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.DeleteObjectForEditor(_obj);
            storage.AppendObject(_anotherObj);
            storage.DeleteObjectForEditor(_anotherObj);

            var tmp = new List<BaseObject> { _obj, _anotherObj };
            var expected = tmp.OrderBy(item => item.GetType().Name).ToList();


            // Act
            storage.SortByNameForEditor(targetsTrashedObjects: true);


            // Assert
            Assert.That(storage.TrashedObjects,         Is.EqualTo(expected));
            Assert.That(storage.OrganizedListForEditor, Is.EqualTo(storage.TrashedObjects));
        }


        [Test]
        public void SortByAssetNameForEditor_DifferentAssetNameObjects_SortObjectsByAssetName()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.AppendObject(_anotherObj);

            var expected = new List<BaseObject> { _obj, _anotherObj };
            expected = expected.OrderBy(item => item.name).ToList();
            //Debug.Log(string.Join(", ", expected));


            // Act
            storage.SortByAssetNameForEditor();


            // Assert
            Assert.That(storage.Objects, Is.EqualTo(expected));
            Assert.That(storage.OrganizedListForEditor, Is.EqualTo(storage.Objects));
        }
        [Test]
        public void SortByAssetNameForEditor_DifferentAssetNameTrashedObjects_SortTrashedObjectsByAssetName()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.DeleteObjectForEditor(_obj);
            storage.AppendObject(_anotherObj);
            storage.DeleteObjectForEditor(_anotherObj);

            var tmp = new List<BaseObject> { _obj, _anotherObj };
            var expected = tmp.OrderBy(item => item.name).ToList();


            // Act
            storage.SortByAssetNameForEditor(targetsTrashedObjects: true);


            // Assert
            Assert.That(storage.TrashedObjects, Is.EqualTo(expected));
            Assert.That(storage.OrganizedListForEditor, Is.EqualTo(storage.TrashedObjects));
        }


        [Test]
        public void InitFilterForEditor_CallsMethod_OrganizedListEqualsToObjects()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.AppendObject(_anotherObj);


            // Act
            storage.InitFilterForEditor();


            // Assert
            Assert.That(storage.OrganizedListForEditor, Is.EqualTo(storage.Objects));
        }
        [Test]
        public void InitFilterForEditor_CallsMethodForTrashedObjects_OrganizedListEqualsToTrashedObjects()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.DeleteObjectForEditor(_obj);
            storage.AppendObject(_anotherObj);
            storage.DeleteObjectForEditor(_anotherObj);


            // Act
            storage.InitFilterForEditor(targetsTrashedObjects: true);


            // Assert
            Assert.That(storage.OrganizedListForEditor, Is.EqualTo(storage.TrashedObjects));
        }


        [Test]
        public void SetFilterForEditor_SetNameFilterToObjects_SucceedsToFilter()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.AppendObject(_anotherObj);


            // Act
            storage.SetFilterForEditor(nameFilter: "Test");


            // Assert
            Assert.That(storage.OrganizedListForEditor, Has.Member(_obj).And.Not.Member(_anotherObj));
        }
        [Test]
        public void SetFilterForEditor_SetTypeFilterToObjects_SucceedsToFilter()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.AppendObject(_anotherObj);


            // Act
            storage.SetFilterForEditor(typeFilter: _anotherObj.GetType());


            // Assert
            Assert.That(storage.OrganizedListForEditor, Has.Member(_anotherObj).And.Not.Member(_obj));
        }
        [Test]
        public void SetFilterForEditor_SetNameAndTypeFilterToObjects_SucceedsToFilter()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.AppendObject(_anotherObj);


            // Act
            storage.SetFilterForEditor(nameFilter: "Test", typeFilter: _anotherObj.GetType());


            // Assert
            Assert.That(storage.OrganizedListForEditor, Has.Count.Zero);
        }
        [Test]
        public void SetFilterForEditor_SetNameFilterToTrashedObjects_SucceedsToFilter()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.DeleteObjectForEditor(_obj);
            storage.AppendObject(_anotherObj);
            storage.DeleteObjectForEditor(_anotherObj);


            // Act
            storage.SetFilterForEditor(nameFilter: "Test", targetsTrashedObjects: true);


            // Assert
            Assert.That(storage.OrganizedListForEditor, Has.Member(_obj).And.Not.Member(_anotherObj));
        }
        [Test]
        public void SetFilterForEditor_SetTypeFilterToTrashedObjects_SucceedsToFilter()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.DeleteObjectForEditor(_obj);
            storage.AppendObject(_anotherObj);
            storage.DeleteObjectForEditor(_anotherObj);


            // Act
            storage.SetFilterForEditor(typeFilter: _anotherObj.GetType(), targetsTrashedObjects: true);


            // Assert
            Assert.That(storage.OrganizedListForEditor, Has.Member(_anotherObj).And.Not.Member(_obj));
        }
        [Test]
        public void SetFilterForEditor_SetNameAndTypeFilterToTrashedObjects_SucceedsToFilter()
        {
            // Arrange
            var storage = ScriptableObject.CreateInstance<ObjectStorage>();
            storage.AppendObject(_obj);
            storage.DeleteObjectForEditor(_obj);
            storage.AppendObject(_anotherObj);
            storage.DeleteObjectForEditor(_anotherObj);


            // Act
            storage.SetFilterForEditor(nameFilter: "Test", typeFilter: _anotherObj.GetType(), targetsTrashedObjects: true);


            // Assert
            Assert.That(storage.OrganizedListForEditor, Has.Count.Zero);
        }
    }
}
