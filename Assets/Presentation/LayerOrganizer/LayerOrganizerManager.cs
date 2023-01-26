using System;
using System.Collections.Generic;
using Core.DataProcessor;
using Core.Factory;
using Core.FileReader;
using Infrastructure.DataProcessor;
using Infrastructure.FileReader;
using Presentation.Components.Label.Scripts;
using Presentation.Factories;
using Presentation.FileSelector.Scripts;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Presentation.LayerOrganizer {
    public class LayerOrganizerManager : MonoBehaviour {
        [SerializeField] FileSelectorHandler _setup;

        [SerializeField] Transform _layerSelector;

        [SerializeField] AssetReference _reference;

        IFileReader<string[], string> _fileReader;
        IDataProcessor<string, string[,]> _dataProcessor;
        FactoryBase<LabelInitializer> _factory;

        int _labelCount;
        string[,] _data;
        LabelInitializer[] _labelGameObjects;

        HashSet<string>[] _entries = new HashSet<string>[5];

        public Action OnLayerChanged { get; set; }

        void Awake() {
            _fileReader = new CSVReader();
            _dataProcessor = new CSVDataProcessor(_fileReader);
            _factory = new LabelFactory(_reference.AssetGUID);
        }

        void OnEnable() {
            _setup.OnFileSelected += FileSelected;
            _factory.OnCreationDone += CreationDone;
        }

        void OnDisable() {
            _setup.OnFileSelected -= FileSelected;
            _factory.OnCreationDone -= CreationDone;
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
                _factory.RequestCreation();
            }
        }

        void CreationDone(LabelInitializer label) {
            if (label is null) {
                Debug.LogError("Failed to get Label prefab.");
                return;
            }

            label.Title = _data[0, _labelCount];
            _labelGameObjects[_labelCount] = label;
            PopulateLayers(_labelCount);
            _labelCount++;
        }

        void PopulateLayers(int index) {
            var label = Instantiate(_labelGameObjects[index], _layerSelector);
        }
    }
}