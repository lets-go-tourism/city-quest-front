using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    public enum State
    {
        Main,
        QuitUI,
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

    private void Start()
    {
        layerProp = 1 << LayerMask.NameToLayer("Prop");
        layerTour = 1 << LayerMask.NameToLayer("Tour");

        point = new PointerEventData(null);
        settingUI = GameObject.Find("SettingPrefab").GetComponent<SettingCanvasOnOff>();

        follow = false;
    }

    void Update()
    {
        RayBottomSheet();

        RayTouch();

        BackTouch();
    }

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
                    if (r.gameObject.CompareTag("BottomSheet"))// && BottomSheetMovement.instance.state == BottomSheetMovement.State.DOWN)
                    {
                        if (GetComponent<PopUpMovement>().placeState == PopUpMovement.PlaceState.UP || GetComponent<PopUpMovement>().tourState == PopUpMovement.TourState.UP)
                        {
                            return;
                        }
                        else
                        {
                            originPos = touch.position;
                            follow = true;
                        }
                    }
                }
            }

            else if (touch.phase == TouchPhase.Moved && follow)
            {
                foreach (RaycastResult r in results)
                {
                    if (r.gameObject.CompareTag("BottomSheet"))
                    {
                        if (GetComponent<PopUpMovement>().placeState == PopUpMovement.PlaceState.UP || GetComponent<PopUpMovement>().tourState == PopUpMovement.TourState.UP)
                        {
                            return;
                        }
                        else
                        {
                            //if (touch.position.y >= 948)
                            //{
                            //    touch.position = new Vector2(0, 948);
                            //}
                            //else if (touch.position.y <= 360)
                            //{
                            //    touch.position = new Vector2(0, 360);
                            //}

                            movedPos = touch.position;

                            moved = originPos.y - movedPos.y;

                            print(moved);

                            if (BottomSheetMovement.instance.state == BottomSheetMovement.State.DOWN && 360 - moved > 360)
                            {
                                MainView_UI.instance.bottomSheet.anchoredPosition = new Vector2(0, 360 - moved);

                                if (moved <= -300)
                                {
                                    BottomSheetMovement.instance.MoveUP();
                                    follow = false;
                                }
                            }
                            else if (BottomSheetMovement.instance.state == BottomSheetMovement.State.UP && 948 - moved < 948)
                            {
                                MainView_UI.instance.bottomSheet.anchoredPosition = new Vector2(0, 948 - moved);
                                if (moved >= 300)
                                {
                                    BottomSheetMovement.instance.MoveDOWN();
                                    follow = false;
                                }
                            }
                        }
                    }
                }
            }

            else if (touch.phase == TouchPhase.Ended)
            {
                if (moved >= 0 && moved < 300 && BottomSheetMovement.instance.state == BottomSheetMovement.State.UP)
                {
                    BottomSheetMovement.instance.MoveUP();
                }
                else if (moved <= 0 && moved > -300 && BottomSheetMovement.instance.state == BottomSheetMovement.State.DOWN)
                {
                    BottomSheetMovement.instance.MoveDOWN();
                }

                follow = true;
            }
        }
    }

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

                        // 프랍정보 중 propNo 을 서버에 보냄
                        KJY_ConnectionTMP.instance.OnConnectionQuest((int)prop.PropData.propNo);

                        MainView_UI.instance.BackgroundDarkEnable();

                        HttpManager.instance.successDelegate += () => { SettingPropInfo.instance.PropInfoSetting(); };
                        HttpManager.instance.errorDelegate += () => { MainView_UI.instance.BackgroundDarkDisable(); };

                        // propNo 에 해당되는 데이터를 받아와서 팝업창에 정보 세팅

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
        if (Application.platform == RuntimePlatform.Android) { }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 메인화면
            if (state == State.Main)
            {
                // 앱 종료 확인 UI 활성화
                MainView_UI.instance.quitUI.gameObject.SetActive(true);

                state = State.QuitUI;
            }
            // 앱 종료 확인 UI 
            else if (state == State.QuitUI)
            {
                // 앱 종료 확인 UI 비활성화
                MainView_UI.instance.quitUI.gameObject.SetActive(false);

                state = State.Main;
            }
            // 설정창이 켜져있을 때
            else if (state == State.Setting)
            {
                settingUI.SettingCanvasOff();
            }
            // 팝업창이 활성화 되어있을 때
            else
            {
                // 탐험
                if (PopUpMovement.instance.placeState == PopUpMovement.PlaceState.UP && PopUpMovement.instance.tourState == PopUpMovement.TourState.DOWN)
                {
                    PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveDOWN), true);
                }
                // 관광
                else if (PopUpMovement.instance.tourState == PopUpMovement.TourState.UP && PopUpMovement.instance.placeState == PopUpMovement.PlaceState.DOWN)
                {
                    PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveDOWN), false);
                }
            }
        }
    }

    // 앱 종료
    public void Quit(bool quit)
    {
        if (quit)
        {
            MainView_UI.instance.quitUI.gameObject.SetActive(false);
            print("꺼짐");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        else
        {
            MainView_UI.instance.quitUI.gameObject.SetActive(false);
            state = State.Main;
        }
    }
}