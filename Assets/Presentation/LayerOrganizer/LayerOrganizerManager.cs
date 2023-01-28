using System;
using System.Collections.Generic;
using Core.DataProcessor;
using Core.FileReader;
using Infrastructure.DataProcessor;
using Infrastructure.FileReader;
using Microsoft.MixedReality.Toolkit;
using Presentation.Components.Label.Scripts;
using Presentation.Factories;
using Presentation.FileSelector.Scripts;
using UnityEngine;

namespace Presentation.LayerOrganizer {
    public class LayerOrganizerManager : MonoBehaviour {
        [SerializeField] FileSelectorHandler _setup;
        [SerializeField] Transform _layerSelector;
        [SerializeField] LabelFactoryProvider _labelFactory;

        public StatefulInteractable _visualButton;

        IFileReader<string[], string> _fileReader;
        IDataProcessor<string, string[,]> _dataProcessor;

        int _labelCount;
        string[,] _data;
        LabelInitializer _labelInitializerObject;

        public Action OnOrderChanged { get; set; }
        public Action<HashSet<int>, string[,]> OnOrderConfirmed { get; set; }

        void Awake() {
            _fileReader = new CSVReader();
            _dataProcessor = new CSVDataProcessor(_fileReader);
        }

        void Start() {
            _labelFactory.OnLabelCreated += LabelCreated;
            _labelFactory.RequestLabel();
        }

        void OnEnable() {
            _setup.OnFileSelected += FileSelected;
            _visualButton?.OnClicked.AddListener(ConfirmOrder);
        }

        void OnDisable() {
            _setup.OnFileSelected -= FileSelected;
            _visualButton?.OnClicked.RemoveListener(ConfirmOrder);
        }

        void FileSelected(string filePath) {
            _data = _dataProcessor.ProcessData(filePath);

            PopulateLabels();
        }

        void PopulateLabels() {
            if (_data is null)
                return;

            for (var i = 0; i < _data.GetLength(1); i++) {
                if (_data[0, i] is null)
                    continue;
                CreateLabel(_data[0, i]);
            }
            
            Canvas.ForceUpdateCanvases();
            Canvas.ForceUpdateCanvases();
        }

        void LabelCreated(LabelInitializer label) {
            if (label is null) {
                Debug.LogError("Failed to get Label prefab.");
                return;
            }

            _labelInitializerObject = label;
            _labelFactory.OnLabelCreated -= LabelCreated;
        }

        void CreateLabel(string title) {
            var label = Instantiate(_labelInitializerObject, _layerSelector);
            label.Title = title;
        }

        void ConfirmOrder() {
            OnOrderConfirmed?.Invoke(new HashSet<int> { 3, 2, 0, 1 }, _data);
        }
    }
}