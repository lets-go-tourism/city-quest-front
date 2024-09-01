using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class MapCameraController : MonoBehaviour
{
    public static MapCameraController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    #region "Input data" 

    [Header("움직이는 카메라")]
    [SerializeField] private Camera _cameraToMove;

    [Space(40f)]

    [Header("가로 값(X) 최소")]
    [SerializeField] float _limitXMin;

    [Header("가로 값(X) 최대")]
    [SerializeField] private float _limitXMax;

    [Header("세로 값(Y) 최소")]
    [SerializeField] private float _limitZMin;

    [Header("Y세로 값(Y) 최대")]
    [SerializeField] private float _limitZMax;

    [Space(40f)]

    [Header("최소 확대 사이즈")]
    [SerializeField] private float _orthoMin = 2f;

    [Header("최대 확대 사이즈")]
    [SerializeField] private float _orthoMax = 12f;


    [Space(40f)]
    [Header("Interpolation step for camera drag")]
    [SerializeField] private float _interpolationStep;
    #endregion

    #region "Private members"

    private Vector3 initPos;
    private Vector2 zoomTarget;

    private bool _lastFramePinch = false;

    private float initDist = 42f; // var for calculation [used in Pinching()]
    private float initOrtho = 6;  // var for calculation [used in Pinching()]

    private bool _initTouch = false; // if init touch is on UI element

    private Vector2 _panVelocity;  //delta position of the touch [camera position derivative]
    #endregion

    public bool rallBack = false;
    [SerializeField] private float rallbackSpeed = 1;

    private void Update()
    {
        if (CheckIfUiHasBeenTouched())
            return;

        // If there are no touches 
        if (Input.touchCount < 1)
        {
            _initTouch = true;
        }

        if (_initTouch == false)
        {
            Panning();
            //Pinching();
        }
        else
        {
            PanningInertia();
            //MinOrthoAchievedAnimation();
            RallBackCameraToMyGPS();
        }
    }


    /// <summary>
    /// Checks if one of the touches have started on a UI element 
    /// </summary>
    private bool CheckIfUiHasBeenTouched()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            bool check = false;

            for (int i = 0; i < Input.touchCount; i++)
            {
                if (EventSystem.current.IsPointerOverGameObject(i)) // implementation for the old input system!!
                {
                    check = true;
                    break;
                }
            }

            // UI 터치가 없으면 _initTouch를 false
            if (check == false)
            {
                _initTouch = false;
            }

            return check;
        }

        return false;
    }


    /// <summary>
    /// Panning that is used to move the camera [ignores UI elements]
    /// </summary>
    private void Panning()
    {
        if (rallBack)
            rallBack = false;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            time = 0;
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

            _panVelocity = touchDeltaPosition;

            PanningFunction(touchDeltaPosition);
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
        {
            time += Time.deltaTime;

            if (time > 0.1f)
            {
                _panVelocity = Vector2.zero;
                time = 0;
            }
        }
    }

    private float time;


    /// <summary>
    /// Pinching that is used for zooming with 2 or more fingers
    /// </summary>
    private void Pinching()
    {
        if (Input.touchCount > 1)
        {
            _panVelocity = Vector2.zero;

            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            if (!_lastFramePinch)
            {
                zoomTarget = _cameraToMove.ScreenToWorldPoint(((Vector3)(touchZero.position + touchOne.position) + new Vector3(0, 0, _cameraToMove.transform.position.y)) / 2);
                initPos = _cameraToMove.transform.position;
                initDist = Vector2.Distance(touchZero.position, touchOne.position);
                initOrtho = _cameraToMove.transform.position.y;
            }

            if (touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved)
            {
                float prevDist = Vector2.Distance(touchZero.position - touchZero.deltaPosition, touchOne.position - touchOne.deltaPosition);
                float dist = Vector2.Distance(touchZero.position, touchOne.position);

                PanningFunction((touchZero.deltaPosition + touchOne.deltaPosition) / 40);

                float y = Mathf.Clamp(_cameraToMove.transform.position.y * (prevDist / dist), _orthoMax, _orthoMin);
                _cameraToMove.transform.position = new Vector3(_cameraToMove.transform.position.x, y, _cameraToMove.transform.position.z);

                float t;
                float x = _cameraToMove.orthographicSize;

                if (initOrtho != _orthoMin)
                {
                    float a = -(1 / ((initOrtho - _orthoMin)));
                    float b = 1 + (_orthoMin / ((initOrtho - _orthoMin)));
                    t = Mathf.Clamp(a * x + b, 0f, 1f);

                    _cameraToMove.transform.position = Vector3.Lerp(initPos, new Vector3(zoomTarget.x, _cameraToMove.transform.position.y, zoomTarget.y), t);

                    LimitCameraMovement();
                }
            }

            _lastFramePinch = true;
            Vector3 prevTarg = ((touchZero.position - touchZero.deltaPosition) + (touchOne.position - touchOne.deltaPosition)) / 2;
            Vector3 targ = (touchZero.position + touchOne.position) / 2;

            zoomTarget = _cameraToMove.ScreenToWorldPoint(_cameraToMove.WorldToScreenPoint(zoomTarget) - (targ - prevTarg));
            initPos = _cameraToMove.ScreenToWorldPoint(_cameraToMove.WorldToScreenPoint(initPos) - (targ - prevTarg));
        }
        else
        {
            _lastFramePinch = false;
        }
    }


    /// <summary>
    ///  The method for panning the camera with one input deltaPosition
    ///  Has a little bit of lag from transform.Translate;
    /// </summary>
    /// <param name="touchDeltaPosition"> the delta position for movement </param>
    private void PanningFunction(Vector2 touchDeltaPosition)
    {
        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, transform.position.y + 1);
        Vector3 screenTouch = screenCenter + new Vector3(touchDeltaPosition.x, touchDeltaPosition.y, 0f);

        Vector3 worldCenterPosition = _cameraToMove.ScreenToWorldPoint(screenCenter);
        Vector3 worldTouchPosition = _cameraToMove.ScreenToWorldPoint(screenTouch);

        //print("worldCenterPosition : " + worldCenterPosition + "worldTouchPosition : " + worldTouchPosition);

        Vector3 worldDeltaPosition = worldTouchPosition - worldCenterPosition;

        transform.Translate(-worldDeltaPosition, Space.World);

        LimitCameraMovement();
    }


    /// <summary>
    /// Inertia of the camera when panning finishes 
    /// </summary>
    private void PanningInertia()
    {
        if (_panVelocity.magnitude < 0.02f)
        {
            _panVelocity = Vector2.zero;
        }

        if (_panVelocity != Vector2.zero)
        {
            _panVelocity = Vector2.Lerp(_panVelocity, Vector2.zero, _interpolationStep * Time.deltaTime);
            PanningFunction(_panVelocity);
        }
    }


    /// <summary>
    /// Camera feedback when achieving minimum ortho
    /// </summary>
    private void MinOrthoAchievedAnimation()
    {
        if (_cameraToMove.orthographicSize < _orthoMin + 0.6f)
        {
            _cameraToMove.orthographicSize = Mathf.Lerp(_cameraToMove.orthographicSize, _orthoMin + 0.6f, 0.06f);
            _cameraToMove.orthographicSize = Mathf.Round(_cameraToMove.orthographicSize * 1000.0f) * 0.001f;
            LimitCameraMovement();
        }
    }


    // 카메라 범위를 제한하는 함수
    private void LimitCameraMovement()
    {
        float xCord = Mathf.Clamp(_cameraToMove.transform.position.x, _limitXMin + (_cameraToMove.orthographicSize * _cameraToMove.aspect), _limitXMax - (_cameraToMove.orthographicSize * _cameraToMove.aspect));
        float zCord = Mathf.Clamp(_cameraToMove.transform.position.z, _limitZMin + (_cameraToMove.orthographicSize * _cameraToMove.aspect), _limitZMax - (_cameraToMove.orthographicSize * _cameraToMove.aspect));

        transform.position = new Vector3(xCord, transform.position.y, zCord);
    }

    public void RallBackCameraToMyGPS()
    {
        if (rallBack == false)
            return;

        _panVelocity = Vector2.zero;
        transform.position = Vector3.Lerp(transform.position, GPS.Instance.GetCurrentGPSPos() + new Vector3(0, _cameraToMove.transform.position.y, 0), Time.deltaTime * rallbackSpeed);
    }

    /// <summary> 
    /// Draw camera boundaries on editor
    /// </summary>
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(_limitXMin, 0, _limitZMin), new Vector3(_limitXMin, 0, _limitZMax));
        Gizmos.DrawLine(new Vector3(_limitXMin, 0, _limitZMax), new Vector3(_limitXMax, 0, _limitZMax));
        Gizmos.DrawLine(new Vector3(_limitXMax, 0, _limitZMax), new Vector3(_limitXMax, 0, _limitZMin));
        Gizmos.DrawLine(new Vector3(_limitXMax, 0, _limitZMin), new Vector3(_limitXMin, 0, _limitZMin));
    }
#endif
}