using System.Collections.Generic;
using Core.Extensions;
using Presentation.Components.Label.Scripts;
using Presentation.Components.Line.Scripts;
using Presentation.Factories;
using Presentation.LayerOrganizer;
using UnityEngine;

namespace Presentation.Visualizer {
    public class DataVisualizer : MonoBehaviour {
        [SerializeField] LayerOrganizerManager _layerOrganizer;

        [SerializeField] Transform _dataParent;

        [SerializeField] LabelFactoryProvider _labelFactory;

        [Header("Prefabs")] [SerializeField] GameObject _horizontalGroup;
        [SerializeField] LineDrawer _linePrefab;

        HashSet<string> _entriesByOrder;
        LabelInitializer _labelGameObject;

        void Start() {
            _labelFactory.OnLabelCreated += LabelCreated;
            _labelFactory.RequestLabel();
        }

        void OnEnable() {
            _layerOrganizer.OnOrderConfirmed += OrderConfirmed;
        }

        void OnDisable() {
            _layerOrganizer.OnOrderConfirmed -= OrderConfirmed;
        }


        void OrderConfirmed(int[] order, string[,] data) {
            if (order.Length <= 0)
                return;
            if (data is null)
                return;

            var orderedEntries = data.GetColumnUniqueEntries(order[0]);

            var layers = new Transform[order.Length];
            for (var i = 0; i < order.Length; i++) 
                layers[i] = Instantiate(_horizontalGroup, _dataParent).transform;
            
            foreach (var entry in orderedEntries)
                GenerateGroup(data, order, 0, entry, null, layers);
        }

        void GenerateGroup(string[,] data, int[] order, int k, string entryData, Transform upperLabel,
            Transform[] layers) {
            if (k >= order.Length)
                return;

            if (string.IsNullOrWhiteSpace(entryData))
                return;

            if (k == 0) {
                var label = CreateLabel(entryData, layers[k]);
                GenerateGroup(data, order, 1, entryData, label.transform, layers);

                return;
            }

            var dataLabels = data.GroupDataByColumnKey(entryData, order, k);

            CreateLabelWithLine(data, order, k, upperLabel, layers, dataLabels);
        }

        void CreateLabelWithLine(string[,] data, int[] order, int k, Transform upperLabel, Transform[] layers,
            HashSet<string> dataLabels) {
            foreach (var value in dataLabels) {
                var line = Instantiate(_linePrefab, layers[k]);
                var label = CreateLabel(value, layers[k]);
                line.SetLineBetweenTwoPoints(upperLabel, label.transform);

                GenerateGroup(data, order, k + 1, value, label.transform, layers);
            }
        }

        Transform CreateLabel(string title, Transform parent) {
            var label = Instantiate(_labelGameObject, parent);
            label.Title = title;
            return label.transform;
        }

        void LabelCreated(LabelInitializer label) {
            _labelGameObject = label;
            _labelFactory.OnLabelCreated -= LabelCreated;
        }
    }
}