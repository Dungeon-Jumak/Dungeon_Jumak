using UnityEngine;

namespace UnistrokeGestureRecognition.Example {
    public class ExampleRecorderController : MonoBehaviour {
        [SerializeField] private PathDrawerBase _actualPathDrawer;
        [SerializeField] private PathDrawerBase _recordedPathDrawer;

        private IGestureRecorder _recorder;
        private Camera _camera;
        private int _actualPathPoints = 0;

        private void Awake() {
            _recorder = new GestureRecorder(100, .01f);
            _camera = Camera.main;
        }

        private void Update() {
            Clear();
            RecordNewPoint();
        }

        void OnDestroy() {
            _recorder.Dispose();
        }

        private void RecordNewPoint()
        {

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);


                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    Vector2 touchPosition = touch.position;
                    if (touchPosition.x >= 100 && touchPosition.x <= 980 && touchPosition.y >= 100 && touchPosition.y <= 800)
                    {
                        Vector2 point = _camera.ScreenToWorldPoint(touchPosition);
                        UpdatePath(point);
                    }
                }
            }

            else if (Input.GetMouseButton(0))
            {
                var screenPosition = Input.mousePosition;
                if (screenPosition.x >= 100 && screenPosition.x <= 980 && screenPosition.y >= 100 && screenPosition.y <= 800)
                {
                    Vector2 point = _camera.ScreenToWorldPoint(screenPosition);
                    UpdatePath(point);
                }
            }
        }

        private void UpdatePath(Vector2 point)
        {
            _actualPathPoints++;
            _actualPathDrawer.AddPoint(point);
            _recorder.RecordPoint(point);

            _recordedPathDrawer.Clear();

            foreach (var p in _recorder.Path)
            {
                _recordedPathDrawer.AddPoint(p);
            }

            Debug.Log("Actual path points count:" + _actualPathPoints);
            Debug.Log("Recorded path point count:" + _recorder.Length);
        }


        private void Clear()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);


                if (touch.phase == TouchPhase.Began)
                {
                    ResetPaths();
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                ResetPaths();
            }
        }

        private void ResetPaths()
        {
            _recorder.Reset();
            _actualPathDrawer.Clear();
            _recordedPathDrawer.Clear();
            _actualPathPoints = 0;
        }

    }
}