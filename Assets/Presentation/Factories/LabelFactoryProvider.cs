using System;
using Core.Factory;
using Presentation.Components.Label.Scripts;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Presentation.Factories {
    public class LabelFactoryProvider : MonoBehaviour {
        [SerializeField] AssetReference _labelReference;

        FactoryBase<LabelInitializer> _labelFactory;

        public Action<LabelInitializer> OnLabelCreated { get; set; }

        void Awake() {
            _labelFactory = new LabelFactory(_labelReference.AssetGUID);
        }

        void OnEnable() {
            _labelFactory.OnCreationDone += LabelCreationDone;
        }

        void OnDisable() {
            _labelFactory.OnCreationDone -= LabelCreationDone;
        }

        void LabelCreationDone(LabelInitializer label) {
            if (label is null) {
                Debug.LogError("Failed to get Label prefab.");
                return;
            }

            OnLabelCreated?.Invoke(label);
        }

        public void RequestLabel() {
            _labelFactory.RequestCreation();
        }
    }
}