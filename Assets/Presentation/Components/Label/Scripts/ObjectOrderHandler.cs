using System;
using Microsoft.MixedReality.Toolkit.SpatialManipulation;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Presentation.Components.Label.Scripts {
    [RequireComponent(typeof(ObjectManipulator))]
    public class ObjectOrderHandler : MonoBehaviour {
        ObjectManipulator _manipulator;
        bool _isObjectSelected;
        int _index;

        public int Index {
            get => _index;
            set {
                if (_index < 0)
                    return;
                _index = value;
            }
        }

        public Action<int, int> OnIndexSwapped { get; set; }

        void Awake() {
            TryGetComponent(out _manipulator);
        }

        void OnEnable() {
            _manipulator.selectEntered.AddListener(OnObjectSelected);
            _manipulator.selectExited.AddListener(OnObjectDeselected);
        }

        void OnDisable() {
            _manipulator.selectEntered.RemoveListener(OnObjectSelected);
            _manipulator.selectExited.RemoveListener(OnObjectDeselected);
        }

        void OnObjectSelected(SelectEnterEventArgs arg0) {
            _isObjectSelected = true;
        }

        void OnObjectDeselected(SelectExitEventArgs arg0) {
            _isObjectSelected = false;
        }

        void OnTriggerEnter(Collider other) {
            if (!_isObjectSelected)
                return;

            var handler = other.GetComponent<ObjectOrderHandler>();

            if (handler is null)
                return;

            (handler.Index, Index) = (Index, handler.Index);
            transform.SetSiblingIndex(Index);
            OnIndexSwapped?.Invoke(Index, handler.Index);
        }
    }
}