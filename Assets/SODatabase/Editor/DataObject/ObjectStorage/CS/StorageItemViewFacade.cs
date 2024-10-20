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
        internal BaseObject BindingObject { get; set; }
        internal bool IsTrashedItem { get; set; } = false;

        internal Action OnTrashButtonClicked { get; set; } = () => { };
        internal Action OnRestoreButtonClicked { get; set; } = () => { };
        internal Action OnDeleteButtonClicked { get; set; } = () => { };


        private StorageItemView _view;


        public void SetupItem(VisualElement item)
        {
            if (BindingObject == null) return;
            _view = new(IsTrashedItem);

            CacheVisualElements(item);
            SetupVisualElements();
            PopulateVisualElements();
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

        internal void CacheVisualElements(VisualElement root)
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
            _view.SetupDetailsSectionLink(detailsSection__link);
            _view.SetupDetailsSectionTrashButton(detailsSection__trashButton);
            _view.SetupDetailsSectionRestoreButton(detailsSection__restoreButton);
            _view.SetupDetailsSectionDeleteButton(detailsSection__deleteButton);

            // Associate
            _view.AssociateDetailsSection__LinkWithTitleSection(
                detailsSection__link,
                detailsSection__titleSection
            );
            _view.AssociateDetailsSection__LinkWithMain(
                detailsSection__link,
                detailsSection__main
            );
        }
        private void PopulateVisualElements()
        {
            // Populate
            _view.PopulateDetailsSection__Link(
                detailsSection__link, 
                BindingObject
            );
            
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

    }
}
