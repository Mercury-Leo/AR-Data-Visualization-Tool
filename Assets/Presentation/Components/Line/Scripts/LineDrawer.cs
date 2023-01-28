using UnityEngine;

namespace Presentation.Components.Line.Scripts {
    public class LineDrawer : MonoBehaviour {
        GameObject _line;
        Transform _startPoint;
        Transform _endPoint;
        float _lineLength;
        const float LineWidth = 0.001f;
        bool _lineCreated;

        public void SetLineBetweenTwoPoints(Transform startPoint, Transform endPoint) {
            if (startPoint is null || endPoint is null)
                return;

            _startPoint = startPoint;
            _endPoint = endPoint;
            _line = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            _lineCreated = true;
        }

        void Update() {
            if (!_lineCreated)
                return;
            UpdatePosition();
        }

        void UpdatePosition() {
            _line.transform.position = (_startPoint.position + _endPoint.position) / 2;
            _line.transform.LookAt(_endPoint);
            _line.transform.localScale = new Vector3(LineWidth, LineWidth,
                Vector3.Distance(_startPoint.position, _endPoint.position));
        }
    }
}