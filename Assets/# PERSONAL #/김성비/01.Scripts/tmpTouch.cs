using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Net.NetworkInformation;

public class tmpTouch : MonoBehaviour
{
    public static tmpTouch instance;

    private void Awake()
    {
        instance = this;

        layerProp = 1 << LayerMask.NameToLayer("Prop");
        layerTour = 1 << LayerMask.NameToLayer("Tour");

        point = new PointerEventData(null);
        point2 = new PointerEventData(null);
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
    public GraphicRaycaster raycaster2;
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

        PopUpRay();
    }

    private float defaultBottomSheetHeight = 0;
    private float limitBottomSheetLowHeight = 360;
    private float startBottomSheetHeight = 0;

    private void Start()
    {
        defaultBottomSheetHeight = MainView_UI.instance.bottomSheet.anchoredPosition.y;
    }

    private bool realMove = false;

    private Vector2 lastPos;
    private PointerEventData point2;
    private bool touched = false;

    private void PopUpRay()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                point2.position = touch.position;
                List<RaycastResult> results = new List<RaycastResult>();
                raycaster2.Raycast(point2, results);

                touched = false;
                if (results[0].gameObject.CompareTag("TutorialPopUp"))
                {
                    touched = true;    
                }
            }
            else if (touched && touch.phase == TouchPhase.Ended)
            {
                lastPos = touch.position;

                if (point2.position.x - lastPos.x > Screen.width / 8)
                {
                    TutorialPopUp.instance.OnClickPopUp(1);
                }
                else if (point2.position.x - lastPos.x < -Screen.width / 8)
                {
                    TutorialPopUp.instance.OnClickPopUp(-1);
                }
                else
                {
                    TutorialPopUp.instance.OnClickPopUp(1);
                }

                touched = false;
            }
        }
    }

    void RayBottomSheet()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                point.position = touch.position;
                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(point, results);
                foreach (RaycastResult r in results)
                {
                    if (r.gameObject.CompareTag("BackgroundDark"))
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
                raycaster2.Raycast(point, results);
                foreach (RaycastResult r in results)
                {
                    if (r.gameObject.CompareTag("BackgroundDark"))
                    {
                        follow = false;
                        break;
                    }
                }
            }

            else if (touch.phase == TouchPhase.Moved && follow)
            {
                movedPos = touch.position;
                moved = originPos.y - movedPos.y;

                for (int i = 0; i < BottomSheetMovement.instance.scrollRects.Length; i++)
                {
                    BottomSheetMovement.instance.scrollRects[i].horizontal = false;
                }

                for (int j = 0; j < BottomSheetMovement.instance.btns.Length; j++)
                {
                    BottomSheetMovement.instance.btns[j].enabled = false;
                }

                if (Mathf.Abs(moved) > 30)
                    realMove = true;

                if (realMove)
                    MainView_UI.instance.bottomSheet.anchoredPosition = new Vector2(0, Mathf.Clamp(startBottomSheetHeight - moved, limitBottomSheetLowHeight, defaultBottomSheetHeight));
            }

            else if (touch.phase == TouchPhase.Ended && follow)
            {
                if (Mathf.Abs(startBottomSheetHeight - MainView_UI.instance.bottomSheet.anchoredPosition.y) > 100)
                {
                    if (BottomSheetMovement.instance.state == BottomSheetMovement.State.UP)
                        BottomSheetMovement.instance.MoveDOWN();
                    else
                        BottomSheetMovement.instance.MoveUP();
                }
                else
                {
                    BottomSheetMovement.instance.MoveUP();
                }

                moved = 0;
                follow = false;
                realMove = false;

                for (int i = 0; i < BottomSheetMovement.instance.scrollRects.Length; i++)
                {
                    BottomSheetMovement.instance.scrollRects[i].horizontal = true;
                }

                for (int j = 0; j < BottomSheetMovement.instance.btns.Length; j++)
                {
                    BottomSheetMovement.instance.btns[j].enabled = true;
                }
            }
        }
    }

    // ���ҽ�Ʈ ��ġ
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

    public bool click { get; private set; } = true;
    private Vector2 startPosition;

    // ����ȭ�� ���� ��ġ
    private void RayTouch()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            // UI �հ� GameObject ��ġ ����
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (EventSystem.current.IsPointerOverGameObject(i))
                {
                    click = false;
                    return;
                }
            }

            if (touch.phase == TouchPhase.Began)
            {
                click = true;
                startPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if ((startPosition - touch.position).sqrMagnitude > 20f)
                    click = false;
            }
            else if (touch.phase == TouchPhase.Ended && click)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    int hitLayer = hit.collider.gameObject.layer;
                    if (hitLayer == LayerMask.NameToLayer("Prop"))
                    {
                        //KJY �߰�
                        SettingManager.instance.EffectSound_PopUpTouch();

                        //DataManager.instance.requestSuccess = false;

                        // ���������� �޾ƿ�
                        Prop prop = hit.collider.GetComponent<Prop>();

                        MainView_UI.instance.BackgroundDarkEnable();

                        // Ʃ�丮�� �����ϰ�� �ٷ� ���� 
                        if (prop.PropData.status)
                        {
                            PopUpMovement.instance.adventured = true;
                            PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveUP), true);
                        }

                        else
                        {
                            PopUpMovement.instance.adventured = false;
                            PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveUP), true);
                        }

                        QuestData questData = new QuestData();

                        //DataManager.instance.SetQuestInfo();

                        // SettingPropInfo.instance.PropInfoSetting();

                        // �������� �� propNo �� ������ ����
                        KJY_ConnectionTMP.instance.OnConnectionQuest((int)prop.PropData.propNo);

                        // roro
                        PropsController.Instance.TintProp = prop;

                        HttpManager.instance.successDelegate += () => { SettingPropInfo.instance.PropInfoSetting(); };
                        //HttpManager.instance.errorDelegate += () => { MainView_UI.instance.BackgroundDarkDisable(); };
                    }

                    else if (hitLayer == LayerMask.NameToLayer("Tour"))
                    {
                        //KJY�߰�
                        SettingManager.instance.EffectSound_ButtonTouch();

                        //Props_UI.instance.canvasTour.enabled = true;
                        MainView_UI.instance.BackgroundDarkEnable();

                        TourData tourData = hit.collider.GetComponent<TourData>();
                        ServerTourInfo serverTourInfo = tourData.ServerTourInfo;

                        //Props_UI.instance.canvasTour.enabled = true;
                        PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveUP), false);

                        SettingTourInfo.instance.StartCoroutine(nameof(SettingTourInfo.instance.GetTexture), serverTourInfo);

                        PropsController.Instance.TintTourData = tourData;
                    }
                }
            }
        }
    }

    // 뒤로가기 버튼
    void BackTouch()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // 메인화면
                if (state == State.Main)
                {
                    // 종료 토스트
                    ToastMessage.ShowToast("'뒤로가기'를 한번 더 누르면 종료됩니다.");
                    StartCoroutine(nameof(ChangeState), State.Quit);
                }
                // 종료가능
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
                else if (state == State.Pop)
                {
                    if (PopUpMovement.instance.placeUNCancel || PopUpMovement.instance.placeADcancel)
                    {
                        ButtonActions.Instance.ChangeCancel(true);
                    }
                    else if (PopUpMovement.instance.tourCancel)
                    {
                        ButtonActions.Instance.ChangeCancel(false);
                    }
                    else
                    {
                        // 장소 팝업
                        if (PopUpMovement.instance.placeState == PopUpMovement.PlaceState.UP)
                        {
                            PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveDOWN), true);
                        }
                        // 관광정보 팝업
                        else if (PopUpMovement.instance.tourState == PopUpMovement.TourState.UP)
                        {
                            PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveDOWN), false);
                        }
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

    // �� ����
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