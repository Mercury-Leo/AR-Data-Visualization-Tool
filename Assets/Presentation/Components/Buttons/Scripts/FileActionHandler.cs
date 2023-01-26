using System;
using Core.Extensions;
using Microsoft.MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;

namespace Presentation.Components.Buttons.Scripts {
    [RequireComponent(typeof(PressableButton))]
    public class FileActionHandler : MonoBehaviour {
        [SerializeField] TMP_Text _textComponent;
        
        PressableButton _pressableButton;
        string _filePath;

        public string FilePath {
            get => _filePath;
            set {
                if (string.IsNullOrWhiteSpace(_filePath))
                    return;
                _textComponent.text = FileExtensions.GetFileName(value);
                _filePath = value;
            }
        }

        public Action<string> OnFileSelected { get; set; }

        void Awake() {
            TryGetComponent(out _pressableButton);
        }

        void OnEnable() {
            _pressableButton.OnClicked.AddListener(ButtonClicked);
        }

        void OnDisable() {
            _pressableButton.OnClicked.RemoveListener(ButtonClicked);
        }

        void ButtonClicked() {
            OnFileSelected?.Invoke(FilePath);
        }
    }
}