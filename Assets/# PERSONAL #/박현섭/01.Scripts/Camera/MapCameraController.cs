using DG.Tweening;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
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
    [SerializeField] private Camera _cam;

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

    [Header("최소 확대 높이")]
    [SerializeField] private float _heightMin = 2f;

    [Header("최대 확대 높이")]
    [SerializeField] private float _heightMax = 12f;


    [Space(40f)]
    [Header("Interpolation step for camera drag")]
    [SerializeField] private float _interpolationStep;
    #endregion

    #region "Private members"

    private Vector3 initPos;
    private Vector3 zoomTarget;

    private bool _lastFramePinch = false;

    private float initDist = 42f; // var for calculation [used in Pinching()]
    private float initHeight = 6;  // var for calculation [used in Pinching()]

    private bool _initTouch = false; // if init touch is on UI element

    private Vector2 _panVelocity;  //delta position of the touch [camera position derivative]
    #endregion

    // 터치 이동이 아닌 이동 변수들
    private Vector3 m_TargetPosition;
    private bool m_Moving = false;
    [SerializeField] private float m_MoveSpeed = 1;

    // 사운드 관련
    private AudioSource m_AudioSource;
    private bool m_Audio = false;

    // 화면의 중앙이 어디인지
    private float value = 0.7f;
    public bool isBottom = false;

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (coolTime > 0)
            coolTime -= Time.deltaTime;
        else
            coolTime = 0;


        m_Audio = false;
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
            Pinching();
        }
        else
        {
            PanningInertia();
            //MinOrthoAchievedAnimation();
            CameraMoveToTarget();
        }

        //if (m_Audio && m_AudioSource.isPlaying == false)
        //    m_AudioSource.Play();
        //else
        //    m_AudioSource.Stop();
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
        // 카메라가 이동중일 경우 취소
        if (m_Moving)
            m_Moving = false;

        if (Input.touchCount > 0 && Input.touchCount < 2 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            m_Audio = true;
            time = 0;
            BottomSheetMovement.instance.MoveDOWN();

            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

            _panVelocity = touchDeltaPosition;

            PanningFunction(touchDeltaPosition);
        }
        else if (Input.touchCount > 0 && Input.touchCount < 2 && Input.GetTouch(0).phase == TouchPhase.Stationary)
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
                zoomTarget = _cam.ScreenToWorldPoint(((Vector3)(touchZero.position + touchOne.position)) / 2 + new Vector3(0, 0, _cam.transform.position.y));
                initPos = _cam.transform.position - new Vector3(0, _cam.transform.position.y, 0);
                initDist = Vector2.Distance(touchZero.position, touchOne.position);
                initHeight = _cam.transform.position.y;
            }

            if (touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved)
            {
                float prevDist = Vector2.Distance(touchZero.position - touchZero.deltaPosition, touchOne.position - touchOne.deltaPosition);
                float dist = Vector2.Distance(touchZero.position, touchOne.position);

                PanningFunction((touchZero.deltaPosition + touchOne.deltaPosition) / 40);

                Vector2 center = (touchOne.position + touchZero.position) / 2;
                Vector3 currentPinchPosition = _cam.ScreenToWorldPoint((Vector3)center + new Vector3(0, 0, _cam.transform.position.y));

                float y = Mathf.Clamp(_cam.transform.position.y * (prevDist / dist), _heightMax, _heightMin);
                _cam.transform.position = new Vector3(_cam.transform.position.x, y, _cam.transform.position.z);

                Vector3 newPinchPosition = _cam.ScreenToWorldPoint((Vector3)center + new Vector3(0, 0, _cam.transform.position.y));

                _cam.transform.position -= newPinchPosition - currentPinchPosition;
                //_cameraToMove.transform.position = new Vector3(_cameraToMove.transform.position.x, y, _cameraToMove.transform.position.z);

                float t;
                float x = _cam.transform.position.y;

                //if (initHeight != _heightMin)
                //{
                //    float a = -(1 / ((initHeight - _heightMin)));
                //    float b = 1 + (_heightMin / ((initHeight - _heightMin)));
                //    t = Mathf.Clamp(a * x + b, 0f, 1f);
                //    t = 1;
                //    _cameraToMove.transform.position = Vector3.Lerp(new Vector3(initPos.x, _cameraToMove.transform.position.y, initPos.z), new Vector3(zoomTarget.x, _cameraToMove.transform.position.y, zoomTarget.z), t);

                //    

                //_cameraToMove.transform.position = Vector3.Lerp(new Vector3(initPos.x, _cameraToMove.transform.position.y, initPos.z), new Vector3(zoomTarget.x, _cameraToMove.transform.position.y, zoomTarget.z), t);
                LimitCameraMovement();
            }

            _lastFramePinch = true;
            Vector3 prevTarg = ((touchZero.position - touchZero.deltaPosition) + (touchOne.position - touchOne.deltaPosition)) / 2;
            Vector3 targ = (touchZero.position + touchOne.position) / 2;


            //print("1 : " + initPos + "와 " + zoomTarget);
            zoomTarget = _cam.ScreenToWorldPoint(_cam.WorldToScreenPoint(zoomTarget) - (targ - prevTarg));
            initPos = _cam.ScreenToWorldPoint(_cam.WorldToScreenPoint(initPos) - (targ - prevTarg));
            //print("2 : " + initPos + "와 " + zoomTarget);
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

        Vector3 worldCenterPosition = _cam.ScreenToWorldPoint(screenCenter);
        Vector3 worldTouchPosition = _cam.ScreenToWorldPoint(screenTouch);

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
        if (_cam.orthographicSize < _heightMin + 0.6f)
        {
            _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, _heightMin + 0.6f, 0.06f);
            _cam.orthographicSize = Mathf.Round(_cam.orthographicSize * 1000.0f) * 0.001f;
            LimitCameraMovement();
        }
    }


    private float coolTime = 0;

    // 카메라 범위를 제한하는 함수
    private void LimitCameraMovement()
    {
        float xCord = Mathf.Clamp(_cam.transform.position.x, _limitXMin + (_cam.orthographicSize * _cam.aspect), _limitXMax - (_cam.orthographicSize * _cam.aspect));
        float zCord = Mathf.Clamp(_cam.transform.position.z, _limitZMin + (_cam.orthographicSize * _cam.aspect), _limitZMax - (_cam.orthographicSize * _cam.aspect));

        if (coolTime == 0 && (transform.position.x != xCord || transform.position.z != zCord))
        {
            coolTime = 10;
            ToastMessage.ShowToast("현재 행궁동 바깥 지역은 서비스 하고 있지 않아요");
        }

        transform.position = new Vector3(xCord, transform.position.y, zCord);
    }

    public void CameraMoveToTarget()
    {
        if (m_Moving == false)
            return;

        _panVelocity = Vector2.zero;

        Vector3 targetPos = m_TargetPosition - (isBottom ? _cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height * 0.7f, _cam.transform.position.y)) - _cam.transform.position : Vector3.zero);
        if (isBottom)
        {
            targetPos -= new Vector3(0, targetPos.y - _cam.transform.position.y, 0);
        }

        transform.position = 
            // 러프 해서
            Vector3.Lerp(transform.position,

            // 타겟 포지션으로
            targetPos,

            // 시간을 보간해서
            Time.deltaTime * m_MoveSpeed);
        LimitCameraMovement();
    }

    public void StartCameraMoveToTarget(Vector3 targetPos)
    {
        m_Moving = true;
        m_TargetPosition = new Vector3(targetPos.x, _cam.transform.position.y, targetPos.z);
    }

    public void StopCameraMove()
    {
        m_Moving = false;
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