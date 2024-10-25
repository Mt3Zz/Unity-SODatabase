using System.Text;
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
            //ShowPropertyNames();


            var root = new VisualElement();
            if (_layout != null)
            {
                _layout.CloneTree(root);
            }

            CacheVisualElements(root);
            SetupVisualElements();

            return root;
        }


        // VisualElements
        private VisualElement titleSection;
        private VisualElement titleSection__iconSection;
        private VisualElement titleSection__nameSection;
        private Label         titleSection__placeholderTitle;
        private TextField     titleSection__placeholder;

        private VisualElement defaultInspectorSection;

        private Foldout       databaseInformationSection;

        private void CacheVisualElements(VisualElement root)
        {
            T FindElementOrCreate<T>(VisualElement container, string name)
                where T : VisualElement, new()
            {
                T element = container.Q<T>(name);
                if (element == null)
                {
                    var msg = ""
                        + $"{name} is null.\n"
                        + $"A new instance of {typeof(T)} has been created.\n";
                    Debug.Log(msg);
                    element = new T();
                }
                return element;
            }


            titleSection = FindElementOrCreate
                <VisualElement>(
                root,
                "title-section");
            titleSection__iconSection = FindElementOrCreate
                <VisualElement>(
                titleSection,
                "title-section__icon-section");
            titleSection__nameSection = FindElementOrCreate
                <VisualElement>(
                titleSection,
                "title-section__name-section");
            titleSection__placeholderTitle = FindElementOrCreate
                <Label>(
                titleSection__nameSection,
                "title-section__placeholder-title");
            titleSection__placeholder = FindElementOrCreate
                <TextField>(
                titleSection__nameSection,
                "title-section__placeholder");


            defaultInspectorSection = FindElementOrCreate
                <VisualElement>(
                root,
                "default-inspector");


            databaseInformationSection = FindElementOrCreate
                <Foldout>(
                root,
                "database-information");
        }
        private void SetupVisualElements()
        {
            // Setup Title Section
            SetupTitleSectionIconSection(titleSection__iconSection);
            SetupTitleSectionPlaceholder(titleSection__placeholder);

            // Setup Default Inspector Section
            SetupDefaultInspector(defaultInspectorSection);

            // Setup Database Information Section
            SetupDatabaseInformation(databaseInformationSection);
        }
        private void ShowPropertyNames()
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("Property Name List\n");

            var iterator = serializedObject.GetIterator();
            while (iterator.NextVisible(true))
            {
                msg.Append($"- {iterator.propertyPath}\n");
            }
            msg.Append("\n");
            Debug.Log(msg.ToString());
        }


        private void SetupTitleSectionIconSection(VisualElement iconSection)
        {
            var pic = (Texture)EditorGUIUtility.Load("ScriptableObject On Icon");
            var icon = new Image
            {
                image = pic
            };
            
            iconSection.Add(icon);
        }
        private void SetupTitleSectionPlaceholder(TextField placeholder)
        {
            var prop = serializedObject.FindProperty("_id._name");
            placeholder.BindProperty(prop);
        }
        private void SetupDefaultInspector(VisualElement container)
        {
            InspectorElement.FillDefaultInspector(container, serializedObject, this);
        }
        private void SetupDatabaseInformation(Foldout foldout)
        {
            var targetInstance = (BaseObject)target;

            var uuid = $"Uuid : {targetInstance.Uuid.ToString()}";
            var createdAt = $"Created At : {targetInstance.CreatedAt.ToString()}";
            var updatedAt = $"Updated At : {targetInstance.UpdatedAt.ToString()}";
            var isDeleted = $"Is Deleted : {targetInstance.IsDeleted.ToString()}";
            var version = $"Version : {targetInstance.Version.ToString()}";

            foldout.Add(new Label(uuid));
            foldout.Add(new Label(createdAt));
            foldout.Add(new Label(updatedAt));
            foldout.Add(new Label(isDeleted));
            foldout.Add(new Label(version));
        }
    }
}
