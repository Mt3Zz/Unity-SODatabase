using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SODatabase.Editor
{
    using ObjectId = DataObject.ObjectId;


    [CustomPropertyDrawer(typeof(ObjectId))]
    public class ObjectIdDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            root.style.marginBottom = 10;


            var icon = (Texture)EditorGUIUtility.Load("ScriptableObject On Icon");
            var image = new Image();
            image.image = icon;
            root.Add(image);


            var field = new VisualElement();
            field.style.flexGrow = 1;
            field.style.alignSelf = Align.Center;
            root.Add(field);


            var label = new Label("Object Name");
            field.Add(label);


            var nameField = new TextField();
            var nameProp = property.FindPropertyRelative("_name");
            nameField.BindProperty(nameProp);
            field.Add(nameField);


            return root;
        }
    }
}
