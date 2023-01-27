using System;
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
        LabelInitializer[] _labelGameObjects;

        public Action OnOrderChanged { get; set; }
        public Action<int[], string[,]> OnOrderConfirmed { get; set; }

        void Awake() {
            _fileReader = new CSVReader();
            _dataProcessor = new CSVDataProcessor(_fileReader);
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

            RequestPrefab();
        }

        void RequestPrefab() {
            if (_data is null)
                return;

            var labelAmount = _data.GetLength(1);
            _labelGameObjects = new LabelInitializer[labelAmount];

            for (var i = 0; i < labelAmount; i++) {
                if (_data[0, i] is null)
                    continue;
                _labelFactory.OnLabelCreated += OnLabelCreated;
                _labelFactory.RequestLabel();
            }
        }

        void OnLabelCreated(LabelInitializer label) {
            if (label is null) {
                Debug.LogError("Failed to get Label prefab.");
                return;
            }

            label.Title = _data[0, _labelCount];
            _labelGameObjects[_labelCount] = label;
            PopulateLayers(_labelCount);
            _labelCount++;
            _labelFactory.OnLabelCreated -= OnLabelCreated;
        }

        void PopulateLayers(int index) {
            var label = Instantiate(_labelGameObjects[index], _layerSelector);
            Canvas.ForceUpdateCanvases();
        }

        void ConfirmOrder() {
            OnOrderConfirmed?.Invoke(new[] { 0, 1, 2, 3 }, _data);
        }
    }
}