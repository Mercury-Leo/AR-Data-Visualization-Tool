using System;
using Microsoft.MixedReality.Toolkit;
using UnityEngine;

namespace Presentation.Components.Menu.Scripts {
    public class StartingScreen : MonoBehaviour {
        public StatefulInteractable _startButton;

        public Action OnStartClicked { get; set; }

        void OnEnable() {
            _startButton.OnClicked.AddListener(StartVisualization);
        }

        void OnDisable() {
            _startButton.OnClicked.RemoveListener(StartVisualization);
        }

        void StartVisualization() {
            OnStartClicked?.Invoke();
        }
    }
}