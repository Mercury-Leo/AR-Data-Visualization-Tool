using Core.DataProcessor;
using Core.Extensions;
using Core.FileReader;
using Infrastructure.DataProcessor;
using Infrastructure.FileReader;
using UnityEditor;
using UnityEngine;

namespace Mercury.Editor.OrderSelectWindow {
    public class OrderSelectionWindow : EditorWindow {
        public string _folderPath = "Assets/Data";
        bool _showFiles;
        bool _showNames;
        string _selectedFile;
        int _dragIndex = -1;
        Vector2 _dragStart;
        string[] _rowData;
        Rect[] _stringRects;

        IFileReader<string[], string> _fileReader;
        IDataProcessor<string, string[,]> _dataProcessor;

        void Awake() {
            _fileReader = new CSVReader();
            _dataProcessor = new CSVDataProcessor(_fileReader);
        }

        void OnGUI() {
            _folderPath = EditorGUILayout.TextField("Folder Path", _folderPath);

            if (EditorPrefs.HasKey(EditorExtensions.ColumnOrderKey)) {
                if (GUILayout.Button("Clear order")) {
                    EditorExtensions.DeleteKey(EditorExtensions.ColumnOrderKey);
                }
            }

            if (GUILayout.Button("Load Files")) {
                EditorExtensions.DeleteKey(EditorExtensions.ColumnOrderKey);
                _showFiles = true;
            }

            if (_showFiles) {
                if (ShowFiles()) return;
            }

            if (_showNames) {
                ChangeOrder();
            }
        }

        void ChangeOrder() {
            _stringRects = new Rect[_rowData.Length];

            for (var i = 0; i < _rowData.Length; i++) {
                _stringRects[i] = EditorGUILayout.GetControlRect(GUILayout.Width(100), GUILayout.Height(20));
                GUI.Label(_stringRects[i], _rowData[i]);
            }

            if (GUILayout.Button("Set Order")) {
                SaveOrder();
                Close();
            }

            var currentEvent = Event.current;
            if (currentEvent.type is EventType.MouseDown && currentEvent.button == 0) {
                for (var i = 0; i < _rowData.Length; i++) {
                    if (!_stringRects[i].Contains(currentEvent.mousePosition)) continue;

                    _dragIndex = i;
                    _dragStart = currentEvent.mousePosition;
                    break;
                }
            }
            else if (currentEvent.type is EventType.MouseDrag && _dragIndex != -1) {
                var delta = currentEvent.mousePosition - _dragStart;
                _stringRects[_dragIndex].position += delta;
                _dragStart = currentEvent.mousePosition;
            }
            else if (currentEvent.type is EventType.MouseUp && _dragIndex != -1) {
                for (var i = 0; i < _rowData.Length; i++) {
                    if (i == _dragIndex || !_stringRects[i].Contains(currentEvent.mousePosition)) continue;

                    (_rowData[_dragIndex], _rowData[i]) = (_rowData[i], _rowData[_dragIndex]);
                    break;
                }

                _dragIndex = -1;
            }
        }

        bool ShowFiles() {
            var files = FileExtensions.GetAllFiles(_folderPath);

            if (files is null)
                return true;

            foreach (var file in files) {
                if (!FileExtensions.CheckIfValidFileExtension(file, ".csv"))
                    continue;

                if (GUILayout.Button(FileExtensions.GetFileName(file))) {
                    _selectedFile = file;
                    var data = _dataProcessor.ProcessData(_selectedFile);

                    _rowData = data.GetRowData(0);
                    _showNames = true;
                }
            }

            return false;
        }

        void SaveOrder() {
            EditorExtensions.SaveStringArray(_rowData);
        }

        [MenuItem("AR Tool/ Order Selection")]
        public static void ARToolWindow() {
            GetWindow<OrderSelectionWindow>("Order Selection Tool");
        }
    }
}