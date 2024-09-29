using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


namespace SODatabase.Editor
{
    using BaseObject = DataObject.BaseObject;


    [CustomEditor(typeof(BaseObject), true, isFallback = true)]
    public class BaseObjectEditor : UnityEditor.Editor
    {
        [SerializeField]
        private VisualTreeAsset _layout = default;


        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            if (_layout != null)
            {
                _layout.CloneTree(root);
            }


            CreateDefaultInspector(root);
            CreateDatabaseInformation(root);


            return root;
        }


        private void CreateDefaultInspector(VisualElement root)
        {
            var container = root.Q<VisualElement>("default-inspector");
            if (container == null)
            {
                Debug.Log("default-inspector is null");
                return;
            }

            InspectorElement.FillDefaultInspector(container, serializedObject, this);
        }
        private void CreateDatabaseInformation(VisualElement root)
        {
            var container = root.Q<Foldout>("database-information");
            if (container == null)
            {
                Debug.Log("database-information is null");
                return;
            }


            var targetInstance = (BaseObject)target;

            var uuid = $"Uuid : {targetInstance.Uuid.ToString()}";
            var createdAt = $"Created At : {targetInstance.CreatedAt.ToString()}";
            var updatedAt = $"Updated At : {targetInstance.UpdatedAt.ToString()}";
            var isDeleted = $"Is Deleted : {targetInstance.IsDeleted.ToString()}";
            var version = $"Version : {targetInstance.Version.ToString()}";

            container.Add(new Label(uuid));
            container.Add(new Label(createdAt));
            container.Add(new Label(updatedAt));
            container.Add(new Label(isDeleted));
            container.Add(new Label(version));
        }
    }
}
