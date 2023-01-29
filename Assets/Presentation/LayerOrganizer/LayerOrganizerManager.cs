using System;
using Core;
using Core.DataProcessor;
using Core.Extensions;
using Core.FileReader;
using Infrastructure.DataProcessor;
using Infrastructure.FileReader;
using Microsoft.MixedReality.Toolkit;
using Presentation.Components.Label.Scripts;
using Presentation.FileSelector.Scripts;
using UnityEngine;

namespace Presentation.LayerOrganizer {
    public class LayerOrganizerManager : MonoBehaviour {
        [SerializeField] FileSelectorHandler _setup;
        [SerializeField] Transform _layerSelector;

        [SerializeField] ObjectOrderHandler _objectOrderPrefab;

        public StatefulInteractable _visualButton;

        IFileReader<string[], string> _fileReader;
        IDataProcessor<string, string[,]> _dataProcessor;

        int _labelCount;
        string[,] _data;
        int[] _order;

        public Action<int[], string[,]> OnOrderConfirmed { get; set; }

        void Awake() {
            _fileReader = new CSVReader();
            _dataProcessor = new CSVDataProcessor(_fileReader);
        }

        void OnEnable() {
            _setup.OnFileSelected += FileSelected;
            _visualButton.OnClicked.AddListener(ConfirmOrder);
        }

        void OnDisable() {
            _setup.OnFileSelected -= FileSelected;
            _visualButton.OnClicked.RemoveListener(ConfirmOrder);
        }

        void FileSelected(string filePath) {
            _data = _dataProcessor.ProcessData(filePath);

            PopulateLabels();
        }

        void PopulateLabels() {
            if (_data is null)
                return;

            var firstRow = GetOrderData();

            for (var index = 0; index < firstRow.Length; index++) 
                CreateOrderLabel(firstRow[index], index);
        }

        string[] GetOrderData() {
            var firstRow = _data.GetRowData(0);

            _order = new int[firstRow.Length];

#if UNITY_EDITOR
            var editorOrder = EditorExtensions.GetStringArray(EditorExtensions.ColumnOrderKey);
            if (editorOrder is null) {
                SetOrder(firstRow);
                return firstRow;
            }

            SetOrder(firstRow, editorOrder);
            return editorOrder;

#endif
            SetOrder(firstRow);
            return firstRow;
        }

        void SetOrder(string[] original, string[] moved = null) {
            if (moved is null) {
                for (var index = 0; index < original.Length; index++)
                    _order[index] = index;
                return;
            }

            if (moved.Length != original.Length)
                return;

            for (var i = 0; i < original.Length; i++) {
                var index = Array.IndexOf(moved, original[i]);
                if (index != -1)
                    _order[i] = index;
            }
        }

        void CreateOrderLabel(string title, int index) {
            var objectOrder = Instantiate(_objectOrderPrefab, _layerSelector);
            var label = objectOrder.GetComponent<LabelInitializer>();
            if (label is not null)
                label.Title = title;
            objectOrder.Index = index;
            objectOrder.OnIndexSwapped += IndexSwapped;
        }

        void IndexSwapped(int originalIndex, int movedIndex) {
            if (_order is null)
                return;

            (_order[originalIndex], _order[movedIndex]) = (_order[movedIndex], _order[originalIndex]);
        }

        void ConfirmOrder() {
            if (_order is null)
                return;
            OnOrderConfirmed?.Invoke(_order, _data);
        }
    }
}