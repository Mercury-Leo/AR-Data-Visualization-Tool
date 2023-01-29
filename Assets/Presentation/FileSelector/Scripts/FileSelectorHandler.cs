using System;
using System.IO;
using Core.Extensions;
using Microsoft.MixedReality.Toolkit;
using Presentation.Components.Buttons.Scripts;
using UnityEngine;

namespace Presentation.FileSelector.Scripts {
    public class FileSelectorHandler : MonoBehaviour {
        [SerializeField] StatefulInteractable _actionButton;

        [SerializeField] Transform _files;

        string _dataPath;
        const string DataFolder = "Assets/Data";

        public Action<string> OnFileSelected { get; set; }

        void Awake() {
            _dataPath = DataFolder;
        }

        void Start() {
            PopulateFiles();
        }

        void PopulateFiles() {
            var files = FileExtensions.GetAllFiles(_dataPath);

            if (files is null)
                return;

            foreach (var file in files) {
                if (!FileExtensions.CheckIfValidFileExtension(file, ".csv"))
                    return;

                var action = Instantiate(_actionButton, _files);

                var button = action.GetComponent<FileActionHandler>();
                if (button is null)
                    return;

                button.FilePath = file;
                button.OnFileSelected += FileSelected;
            }
        }

        void FileSelected(string filePath) {
            OnFileSelected?.Invoke(filePath);
        }

        public void SetDataFolderPath(string path) {
            if (string.IsNullOrWhiteSpace(path))
                return;
            if (!Directory.Exists(path))
                return;

            _dataPath = path;
        }
    }
}