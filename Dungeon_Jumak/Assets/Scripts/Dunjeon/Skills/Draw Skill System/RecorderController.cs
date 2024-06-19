//Unity
using UnityEngine;

//NameSpace
namespace UnistrokeGestureRecognition.Example {
    public class RecorderController : MonoBehaviour {

        //Actual Path Drawer : Line Drawer
        [SerializeField] private PathDrawerBase _actualPathDrawer;

        //Recorded Path Drawer : Dot Drawer
        [SerializeField] private PathDrawerBase _recordedPathDrawer;

        //Gesture Recorder
        private IGestureRecorder _recorder;

        //Camera : Main
        private Camera _camera;

        //Actial Path Points
        private int _actualPathPoints = 0;

        private void Awake()
        {
            //Initialize
            //+ Gesture Recorder ( Length , Distance)
            _recorder = new GestureRecorder(100, .01f);
            _camera = Camera.main;
        }

        private void Update()
        {
            //Clear per frame
            Clear();

            //Record New point per frame
            RecordNewPoint();
        }

        //OnDestroy : Dispose !
        void OnDestroy()
        {
            _recorder.Dispose();
        }

        //Check Record New Point
        private void RecordNewPoint()
        {
            //If get mouse down
            if (Input.GetMouseButton(0))
            {
                //Get mouse position
                var screenPosition = Input.mousePosition;

                //Get Vector2
                Vector2 point = _camera.ScreenToWorldPoint(screenPosition);

                //Update Path
                UpdatePath(point);
            }
        }

        //Update Path
        private void UpdatePath(Vector2 point)
        {
            //Increase actual path points
            _actualPathPoints++;

            //Add Point
            _actualPathDrawer.AddPoint(point);

            //Record Point
            _recorder.RecordPoint(point);

            //Record Drawer Clear
            _recordedPathDrawer.Clear();

            //Add Record Points
            foreach (var p in _recorder.Path)
            {
                _recordedPathDrawer.AddPoint(p);
            }

        }

        //Clear
        private void Clear()
        {
            //Clear when mouse button up
            if (Input.GetMouseButtonUp(0))
            {
                ResetPaths();
            }
        }

        //Reset Paths
        private void ResetPaths()
        {
            //Recorder Reset
            _recorder.Reset();

            //Actual Drawer Clear
            _actualPathDrawer.Clear();

            //Record Drawer Clear
            _recordedPathDrawer.Clear();

            //Init Path Points
            _actualPathPoints = 0;
        }
    }
}