using System;
using Core.Extensions;
using Presentation.Components.Buttons.Scripts;
using UnityEngine;

namespace Presentation.Visualizer {
    public class VisualizerSetup : MonoBehaviour {
        [SerializeField] GameObject _actionButton;

        [SerializeField] Transform _files;

        string _dataPath;

        public Action<string> OnFileSelected { get; set; }

        void Awake() {
            _dataPath = "Assets/Data";
        }

        void Start() {
            PopulateFiles();
        }

        void PopulateFiles() {
            var files = FileExtensions.GetAllFiles(_dataPath);

            foreach (var file in files) {
                if (!FileExtensions.CheckIfValidFileExtension(file, ".csv"))
                    return;

                var action = Instantiate(_actionButton, _files);

                var button = action.GetComponent<FileButtonHandler>();
                if (button is null)
                    return;

                button.FilePath = file;
                button.OnFileSelected += FileSelected;
            }
        }

        void FileSelected(string filePath) {
            OnFileSelected?.Invoke(filePath);
        }
    }
}