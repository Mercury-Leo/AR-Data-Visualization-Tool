using System;
using Core.Factory;
using Presentation.Components.Label.Scripts;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Presentation.Factories {
    public class LabelFactoryProvider : MonoBehaviour {
        [SerializeField] AssetReference _reference;

        FactoryBase<LabelInitializer> _factory;

        public Action<LabelInitializer> OnLabelCreated { get; set; }

        void Awake() {
            _factory = new LabelFactory(_reference.AssetGUID);
        }

        void OnEnable() {
            _factory.OnCreationDone += CreationDone;
        }

        void OnDisable() {
            _factory.OnCreationDone -= CreationDone;
        }

        void CreationDone(LabelInitializer label) {
            if (label is null) {
                Debug.LogError("Failed to get Label prefab.");
                return;
            }

            label.Title = "New";
            OnLabelCreated?.Invoke(label);
        }

        public void RequestLabel() {
            _factory.RequestCreation();
        }
    }
}