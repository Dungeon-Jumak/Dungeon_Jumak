using System;
using System.Collections.Generic;
using UnistrokeGestureRecognition;
using Unity.Jobs;
using UnityEngine;

namespace UnistrokeGestureRecognition.Example {
    public sealed class ExampleRecognizerController : MonoBehaviour {
        // This is a simple example of how to use this package

        // Set of patterns for recognition
        [SerializeField] private List<ExampleGesturePattern> _patterns;
        [SerializeField, Range(0f, 10f)] private float _newPointMinDistance = 1f;

        [SerializeField] private PathDrawerBase _pathDrawer;
        [SerializeField] private NameController _nameController;

        private Camera _camera;
        private IGestureRecorder _gestureRecorder;

        private IGestureRecognizer<ExampleGesturePattern> _recognizer;

        private JobHandle? _recognizeJob;

        private void Awake() {
            // Use this class to record the gesture path.
            // It uses a resampling algorithm to capture long paths to a limited size buffer.
            // The first value specifies the maximum number of points in the path buffer.
            // A higher value gives a more accurate path, but requires more memory.
            // The second value specifies the minimum distance between the previous and new point to record.
            // If the distance is less than required, the new point will not be added to the recorded path.
            // Useful when you want to exclude points from a path when the user keeps the cursor in one place.
            _gestureRecorder = new GestureRecorder(254, _newPointMinDistance);

            // Pass your patterns to the recognizer constructor.
            // You can also choose the number of points to resample.
            // A higher value gives more accurate results but takes longer to process.
            // 128 is the default and gives good results, but you can choose a better value for your pattern set.
            _recognizer = new GestureRecognizer<ExampleGesturePattern>(_patterns, 128);
        }

        private void Start() {
            _pathDrawer.Show();
            _camera = Camera.main;
        }

        private void OnDestroy() {
            // !!! IMPORTANT !!!
            // Do not forget to dispose the recognizer and recorder to prevent a memory leak, 
            // as they use native arrays to store the necessary data.
            _recognizer.Dispose();
            _gestureRecorder.Dispose();   
        }

        private void Update() {
            if (Input.GetKeyUp(KeyCode.Mouse0)) {
                RecognizeRecordedGesture();
            }

            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                Clear();
            }

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);


                if (touch.phase == TouchPhase.Began)
                {
                    Clear();
                }


                if (touch.phase == TouchPhase.Ended)
                {
                    RecognizeRecordedGesture();
                }
            }

            RecordNewPoint();
        }

        private void LateUpdate() {
            if (!_recognizeJob.HasValue) return;

            // Make sure to complete the recognition task
            _recognizeJob.Value.Complete();

            // Get recognition result
            RecognizeResult<ExampleGesturePattern> result = _recognizer.Result;

            // You can set the required precision value for the results.
            // A score is a value in the range of 0 to 1.
            // Where 1 means that the recorded path is exactly the same as the pattern path.
            // 0.7 is a good choice, but you can choose another value.
            if (result.Score >= .7f) {
                // Get the recognized pattern from the result
                ExampleGesturePattern recognizedPattern = result.Pattern;
                _nameController.Set(recognizedPattern.Name);
                
                Debug.Log($"{recognizedPattern.Name}: {result.Score}");
            }

            _recognizeJob = null;
        }

        private void RecognizeRecordedGesture() {
            // Schedule recorded path recognition and get recognition results in the late update
            // The first value is the recorded gesture path.
            // The second value indicates whether the comparison should 
            // be performed for each pattern in parallel or sequentially.
            _recognizeJob = _recognizer.ScheduleRecognition(_gestureRecorder.Path, true);

            // If you want to get the result instantly, 
            // use _recognizer.Recognize(_gestureRecorder.Path) method instead
        }

        private void Clear() {
            _nameController.Clear();
            _pathDrawer.Clear();
            // Clear the gesture recorder buffer for the new path
            _gestureRecorder.Reset();
        }

        private void RecordNewPoint()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPosition = touch.position;

                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 mobileTouchPosition = new Vector2(touch.position.x, touch.position.y);

                    Camera camera = FindObjectOfType<Camera>();

                    float minX = ((Screen.width - 1000) / 2);
                    float maxX = Screen.width - (Screen.width * camera.rect.x);

                    float minY = (((Screen.height / 2) - 850) / 2) + (Screen.height * camera.rect.y);
                    float maxY = 850 + (((Screen.height / 2) - 1200) / 2);

                    //float minX = (camera.rect.width - 1000) / 2;
                    //float maxX = 1080 - minX;

                    //float minY = ((camera.rect.height / 2) - 850) / 2;
                    //float maxY = 850 + minY;


                    if (mobileTouchPosition.x >= minX && mobileTouchPosition.x <= maxX && mobileTouchPosition.y >= minY && mobileTouchPosition.y <= maxY)
                    {
                        Vector2 point = _camera.ScreenToWorldPoint(mobileTouchPosition);
                        _gestureRecorder.RecordPoint(point);
                        // Show gesture path
                        _pathDrawer.AddPoint(point);
                    }
                }
            }
            else if (Input.GetMouseButton(0))
            {
                var screenPosition = Input.mousePosition;

                if (screenPosition.x >= 100 && screenPosition.x <= 980 && screenPosition.y >= 100 && screenPosition.y <= 500)
                {
                    Vector2 point = _camera.ScreenToWorldPoint(screenPosition);
                    _gestureRecorder.RecordPoint(new Vector2(screenPosition.x, screenPosition.y));
                    // Show gesture path
                    _pathDrawer.AddPoint(point);
                }
            }
        }



        private void OnValidate() {
            if (Application.isPlaying && _recognizer != null) {
                _recognizer.Dispose();
                _recognizer = new GestureRecognizer<ExampleGesturePattern>(_patterns);
            }
        }
    }
}
