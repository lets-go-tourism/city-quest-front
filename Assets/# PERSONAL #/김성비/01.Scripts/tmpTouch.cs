using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class tmpTouch : MonoBehaviour
{
    /// <summary>
    /// 목표 : 지도 위의 프랍을 터치하면 해당 프랍의 정보를 받아와 UI로 표현한다.
    /// 1. 프랍을 터치하면
    ///     1-1. 프랍 여부(layer)
    ///          - 사진 정보
    ///     1-2. 프랍을 획득 여부
    ///     1-3. 프랍의 이름, 난이도, 현재 나와의 거리 정보
    ///     1-4. 추가 퀘스트 여부
    ///     
    /// 2. 1의 정보를 UI로 표시한다.
    /// </summary>
    public static tmpTouch instance;

    private void Awake()
    {
        instance = this;

        layerProp = 1 << LayerMask.NameToLayer("Prop");
        layerTour = 1 << LayerMask.NameToLayer("Tour");

        point = new PointerEventData(null);
        //settingUI = GameObject.Find("M_SettingPrefab").GetComponent<SettingCanvasOnOff>();

        follow = false;
    }
    public enum State
    {
        Main,
        Quit,
        Setting,
        Pop,
    }
    public State state;

    int layerProp, layerTour;
    Touch touch;
    public GraphicRaycaster raycaster;
    PointerEventData point;
    SettingCanvasOnOff settingUI;
    bool follow;
    Vector2 originPos;
    Vector2 movedPos;
    float moved;

    void Update()
    {
        RayBottomSheet();

        RayTouch();

        BackTouch();
    }

    private float defaultBottomSheetHeight = 0;
    private float limitBottomSheetLowHeight = 360;
    private float startBottomSheetHeight = 0;

    private void Start()
    {
        defaultBottomSheetHeight = MainView_UI.instance.bottomSheet.anchoredPosition.y;
    }

    private bool realMove = false;

    void RayBottomSheet()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            point.position = touch.position;
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(point, results);
            if (touch.phase == TouchPhase.Began)
            {
                foreach (RaycastResult r in results)
                {
                    if(r.gameObject.CompareTag("BackgroundDark"))
                    {
                        follow = false;
                        break;
                    }

                    else if (r.gameObject.CompareTag("BottomSheet"))// && BottomSheetMovement.instance.state == BottomSheetMovement.State.DOWN)
                    {
                        originPos = touch.position;
                        startBottomSheetHeight = MainView_UI.instance.bottomSheet.anchoredPosition.y;
                        follow = true;
                    }
                }
            }

            else if (touch.phase == TouchPhase.Moved && follow)
            {
                movedPos = touch.position;
                moved = originPos.y - movedPos.y;
                
                if(Mathf.Abs(moved) > 30)
                    realMove = true;

                if(realMove)
                    MainView_UI.instance.bottomSheet.anchoredPosition = new Vector2(0, Mathf.Clamp(startBottomSheetHeight - moved, limitBottomSheetLowHeight, defaultBottomSheetHeight));
            }

            else if (touch.phase == TouchPhase.Ended && follow)
            {
                if(Mathf.Abs(startBottomSheetHeight - MainView_UI.instance.bottomSheet.anchoredPosition.y) > 100)
                {
                    if (BottomSheetMovement.instance.state == BottomSheetMovement.State.UP)
                        BottomSheetMovement.instance.MoveDOWN();
                    else
                        BottomSheetMovement.instance.MoveUP();
                }
                else
                {
                    if (BottomSheetMovement.instance.state == BottomSheetMovement.State.UP)
                        BottomSheetMovement.instance.MoveUP();
                    else
                        BottomSheetMovement.instance.MoveDOWN();
                }

                moved = 0;
                follow = false;
                realMove = false;
            }
        }
    }

    // 바텀시트 터치
    //void RayBottomSheet()
    //{
    //    if (Input.touchCount > 0)
    //    {
    //        touch = Input.GetTouch(0);
    //        point.position = touch.position;
    //        List<RaycastResult> results = new List<RaycastResult>();
    //        raycaster.Raycast(point, results);
    //        if (touch.phase == TouchPhase.Began)
    //        {
    //            foreach (RaycastResult r in results)
    //            {
    //                if (r.gameObject.CompareTag("BottomSheet"))// && BottomSheetMovement.instance.state == BottomSheetMovement.State.DOWN)
    //                {
    //                    if (GetComponent<PopUpMovement>().placeState == PopUpMovement.PlaceState.UP || GetComponent<PopUpMovement>().tourState == PopUpMovement.TourState.UP)
    //                    {
    //                        return;
    //                    }
    //                    else
    //                    {
    //                        originPos = touch.position;
    //                        follow = true;
    //                    }
    //                }
    //            }
    //        }

    //        else if (touch.phase == TouchPhase.Moved && follow)
    //        {
    //            foreach (RaycastResult r in results)
    //            {
    //                if (r.gameObject.CompareTag("BottomSheet"))
    //                {
    //                    if (GetComponent<PopUpMovement>().placeState == PopUpMovement.PlaceState.UP || GetComponent<PopUpMovement>().tourState == PopUpMovement.TourState.UP)
    //                    {
    //                        return;
    //                    }
    //                    else
    //                    {
    //                        //if (touch.position.y >= 948)
    //                        //{
    //                        //    touch.position = new Vector2(0, 948);
    //                        //}
    //                        //else if (touch.position.y <= 360)
    //                        //{
    //                        //    touch.position = new Vector2(0, 360);
    //                        //}

    //                        movedPos = touch.position;

    //                        moved = originPos.y - movedPos.y;


    //                        //print(moved);

    //                        MainView_UI.instance.bottomSheet.anchoredPosition = new Vector2(0, Mathf.Clamp(defaultBottomSheetHeight - moved, limitBottomSheetLowHeight, defaultBottomSheetHeight));

    //                        //if (BottomSheetMovement.instance.state == BottomSheetMovement.State.DOWN && 360 - moved > 360)
    //                        //{
    //                        //    MainView_UI.instance.bottomSheet.anchoredPosition = new Vector2(0, 360 - moved);

    //                        //    if (moved <= -300)
    //                        //    {
    //                        //        BottomSheetMovement.instance.MoveUP();
    //                        //        follow = false;
    //                        //    }
    //                        //}
    //                        //else if (BottomSheetMovement.instance.state == BottomSheetMovement.State.UP && 948 - moved < 948)
    //                        //{
    //                        //    MainView_UI.instance.bottomSheet.anchoredPosition = new Vector2(0, 948 - moved);
    //                        //    if (moved >= 300)
    //                        //    {
    //                        //        BottomSheetMovement.instance.MoveDOWN();
    //                        //        follow = false;
    //                        //    }
    //                        //}
    //                    }
    //                }
    //            }
    //        }

    //        else if (touch.phase == TouchPhase.Ended)
    //        {
    //            if (defaultBottomSheetHeight - moved > (defaultBottomSheetHeight - limitBottomSheetLowHeight) / 2)
    //                BottomSheetMovement.instance.MoveUP();
    //            else if (defaultBottomSheetHeight - moved < (defaultBottomSheetHeight - limitBottomSheetLowHeight) / 2)
    //                BottomSheetMovement.instance.MoveDOWN();

    //            //if (moved >= 0 && moved < 300 && BottomSheetMovement.instance.state == BottomSheetMovement.State.UP)
    //            //{
    //            //    BottomSheetMovement.instance.MoveUP();
    //            //}
    //            //else if (moved <= 0 && moved > -300 && BottomSheetMovement.instance.state == BottomSheetMovement.State.DOWN)
    //            //{
    //            //    BottomSheetMovement.instance.MoveDOWN();
    //            //}

    //            moved = 0;
    //            follow = false;
    //        }
    //    }
    //}

    private bool began = true;

    // 메인화면 프랍 터치
    private void RayTouch()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            // UI 뚫고 GameObject 터치 제한
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (EventSystem.current.IsPointerOverGameObject(i))
                {
                    began = false;
                    return;
                }
            }

            if (touch.phase == TouchPhase.Began)
            {
                began = true;
            }
            else if (began && touch.phase == TouchPhase.Moved)
            {
                if (touch.deltaPosition.magnitude > 5f)
                    began = false;
            }
            else if (touch.phase == TouchPhase.Ended && began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    int hitLayer = hit.collider.gameObject.layer;

                    if (hitLayer == LayerMask.NameToLayer("Prop"))
                    {
                        //DataManager.instance.requestSuccess = false;

                        // 프랍정보를 받아옴
                        Prop prop = hit.collider.GetComponent<Prop>();

                        // 튜토리얼 프랍일경우 바로 연다 

                        QuestData questData = new QuestData();

                        //DataManager.instance.SetQuestInfo();

                        // SettingPropInfo.instance.PropInfoSetting();

                        // 프랍정보 중 propNo 을 서버에 보냄
                        KJY_ConnectionTMP.instance.OnConnectionQuest((int)prop.PropData.propNo);

                        //MainView_UI.instance.BackgroundDarkEnable();

                        HttpManager.instance.successDelegate += () => { SettingPropInfo.instance.PropInfoSetting(); };
                        //HttpManager.instance.errorDelegate += () => { MainView_UI.instance.BackgroundDarkDisable(); };
                    }

                    else if (hitLayer == LayerMask.NameToLayer("Tour"))
                    {
                        //DataManager.instance.requestSuccess = false;

                        Props_UI.instance.canvasTour.enabled = true;

                        // 관광 정보를 받아옴
                        TourData tourData = hit.collider.GetComponent<TourData>();
                        ServerTourInfo serverTourInfo = tourData.ServerTourInfo;

                        SettingTourInfo.instance.StartCoroutine(nameof(SettingTourInfo.instance.GetTexture), serverTourInfo);

                        // 팝업창에 정보 세팅

                    }
                }
            }
        }
    }

    // 안드로이드 뒤로가기
    void BackTouch()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // 메인화면
                if (state == State.Main)
                {
                    // 앱 종료 확인 UI 활성화
                    ToastMessage.ShowToast("'뒤로' 버튼을 한번 더 누르시면 종료됩니다.");
                    StartCoroutine(nameof(ChangeState), State.Quit);
                }
                // 토스트메시지
                else if (state == State.Quit)
                {
                    Quit();
                }
                // 설정창
                else if (state == State.Setting)
                {
                    settingUI.SettingCanvasOff();
                    StartCoroutine(nameof(ChangeState), State.Main);
                }
                // 팝업창
                else if(state == State.Pop)
                {
                    // 탐험
                    if (PopUpMovement.instance.placeState == PopUpMovement.PlaceState.UP)
                    {
                        PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveDOWN), true);
                    }
                    // 관광
                    else if (PopUpMovement.instance.tourState == PopUpMovement.TourState.UP)
                    {
                        PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveDOWN), false);
                    }

                    StartCoroutine(nameof(ChangeState), State.Main);
                }
            }
        }
    }

    public IEnumerator ChangeState(State change)
    {
        state = change;

        if (change == State.Quit)
        {
            yield return new WaitForSeconds(2.5f);
            state = State.Main;
        }
    }

    // 앱 종료
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_ANDROID
        Application.Quit();
#endif
    }
}