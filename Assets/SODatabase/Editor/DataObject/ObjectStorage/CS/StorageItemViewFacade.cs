using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace SODatabase.Editor
{
    using BaseObject = DataObject.BaseObject;

    internal class StorageItemViewFacade
    {
        internal Action<BaseObject> OnTrashButtonClicked { get; set; } = (obj) => { };
        internal Action<BaseObject> OnRestoreButtonClicked { get; set; } = (obj) => { };
        internal Action<BaseObject> OnDeleteButtonClicked { get; set; } = (obj) => { };


        private StorageItemView _view;


        internal StorageItemViewFacade()
        {
            _view = new();
        }


        public void InitItem(VisualElement element)
        {
            CacheVisualElements(element);
            SetupVisualElements();
            PopulateVisualElements();
        }
        public void UpdateItem(BaseObject obj, bool isTrashedItem)
        {
            UpdateVisualElements(obj, isTrashedItem);
        }


        private VisualElement detailsSection;
        private VisualElement detailsSection__linkSection;
        private VisualElement detailsSection__titleSection;
        private VisualElement detailsSection__main;
        private VisualElement detailsSection__controllerSection;

        private ObjectField   detailsSection__link;
        private Button        detailsSection__trashButton;
        private Button        detailsSection__restoreButton;
        private Button        detailsSection__deleteButton;

        private void CacheVisualElements(VisualElement root)
        {
            T FindOrCreate<T>(VisualElement container, string name)
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

            detailsSection = FindOrCreate<VisualElement>(root, "details-section");

            detailsSection__linkSection       = FindOrCreate<VisualElement>(detailsSection, "details-section__link-section");
            detailsSection__titleSection      = FindOrCreate<VisualElement>(detailsSection, "details-section__title-section");
            detailsSection__main              = FindOrCreate<VisualElement>(detailsSection, "details-section__main");
            detailsSection__controllerSection = FindOrCreate<VisualElement>(detailsSection, "details-section__controller-section");

            detailsSection__link              = FindOrCreate<ObjectField>(detailsSection__linkSection, "details-section__link");
            detailsSection__trashButton       = FindOrCreate<Button>(detailsSection__controllerSection, "details-section__trash-button");
            detailsSection__restoreButton     = FindOrCreate<Button>(detailsSection__controllerSection, "details-section__restore-button");
            detailsSection__deleteButton      = FindOrCreate<Button>(detailsSection__controllerSection, "details-section__delete-button");
        }
        private void SetupVisualElements()
        {
            // Setup
            _view.SetupLink(detailsSection__link);

            // Associate
            _view.AssociateLinkWithTitleSection(
                detailsSection__link,
                detailsSection__titleSection
            );
            _view.AssociateLinkWithMain(
                detailsSection__link,
                detailsSection__main
            );
            _view.AssociateLinkWithButton(
                detailsSection__link,
                detailsSection__trashButton
            );
            _view.AssociateLinkWithButton(
                detailsSection__link,
                detailsSection__deleteButton
            );
            _view.AssociateLinkWithButton(
                detailsSection__link,
                detailsSection__restoreButton
            );
        }
        private void PopulateVisualElements()
        {
            // Populate
            _view.PopulateButtonWithAction(
                detailsSection__trashButton,
                OnTrashButtonClicked
            );
            _view.PopulateButtonWithAction(
                detailsSection__restoreButton,
                OnRestoreButtonClicked
            );
            _view.PopulateButtonWithAction(
                detailsSection__deleteButton,
                OnDeleteButtonClicked
            );
        }
        private void UpdateVisualElements(BaseObject obj, bool isTrashedItem)
        {
            // Populate
            _view.PopulateLink(
                detailsSection__link,
                obj
            );

            _view.DisplayNormalModeButton(detailsSection__trashButton, isTrashedItem);
            _view.DisplayTrashBoxModeButton(detailsSection__restoreButton, isTrashedItem);
            _view.DisplayTrashBoxModeButton(detailsSection__deleteButton, isTrashedItem);
        }
    }
}
