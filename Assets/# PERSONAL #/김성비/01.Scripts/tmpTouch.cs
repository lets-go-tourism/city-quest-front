using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.Android;

public class tmpTouch : MonoBehaviour
{
    /// <summary>
    /// ��ǥ : ���� ���� ������ ��ġ�ϸ� �ش� ������ ������ �޾ƿ� UI�� ǥ���Ѵ�.
    /// 1. ������ ��ġ�ϸ�
    ///     1-1. ���� ����(layer)
    ///          - ���� ����
    ///     1-2. ������ ȹ�� ����
    ///     1-3. ������ �̸�, ���̵�, ���� ������ �Ÿ� ����
    ///     1-4. �߰� ����Ʈ ����
    ///     
    /// 2. 1�� ������ UI�� ǥ���Ѵ�.
    /// </summary>

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

    // ���ҽ�Ʈ ��ġ
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

                moved = 0;
                follow = true;
            }
        }
    }

    private bool began = true;

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

                        // ���������� �޾ƿ�
                        Prop prop = hit.collider.GetComponent<Prop>();

                        // �������� �� propNo �� ������ ����
                        KJY_ConnectionTMP.instance.OnConnectionQuest((int)prop.PropData.propNo);

                        MainView_UI.instance.BackgroundDarkEnable();

                        HttpManager.instance.successDelegate += () => { SettingPropInfo.instance.PropInfoSetting(); };
                        HttpManager.instance.errorDelegate += () => { MainView_UI.instance.BackgroundDarkDisable(); };

                        // propNo �� �ش�Ǵ� �����͸� �޾ƿͼ� �˾�â�� ���� ����

                    }

                    else if (hitLayer == LayerMask.NameToLayer("Tour"))
                    {
                        //DataManager.instance.requestSuccess = false;

                        Props_UI.instance.canvasTour.enabled = true;

                        // ���� ������ �޾ƿ�
                        TourData tourData = hit.collider.GetComponent<TourData>();
                        ServerTourInfo serverTourInfo = tourData.ServerTourInfo;

                        SettingTourInfo.instance.StartCoroutine(nameof(SettingTourInfo.instance.GetTexture), serverTourInfo);

                        // �˾�â�� ���� ����

                    }
                }
            }
        }
    }

    // �ȵ���̵� �ڷΰ���
    void BackTouch()
    {
        if (Application.platform == RuntimePlatform.Android) { }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ����ȭ��
            if (state == State.Main)
            {
                // �� ���� Ȯ�� UI Ȱ��ȭ
#if UNITY_ANDROID
                ShowToast("'�ڷ�' ��ư�� �ѹ� �� �����ø� ����˴ϴ�.");
#endif
                MainView_UI.instance.quitUI.gameObject.SetActive(true);
                StartCoroutine(nameof(ChangeState),State.Quit);
            }
            // �佺Ʈ�޽���
            else if (state == State.Quit)
            {
                Quit();
            }
            // ����â
            else if (state == State.Setting)
            {
                settingUI.SettingCanvasOff();
                StartCoroutine(nameof(ChangeState), State.Main);
            }
            // �˾�â
            else
            {
                // Ž��
                if (PopUpMovement.instance.placeState == PopUpMovement.PlaceState.UP && PopUpMovement.instance.tourState == PopUpMovement.TourState.DOWN)
                {
                    PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveDOWN), true);
                }
                // ����
                else if (PopUpMovement.instance.tourState == PopUpMovement.TourState.UP && PopUpMovement.instance.placeState == PopUpMovement.PlaceState.DOWN)
                {
                    PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveDOWN), false);
                }

                StartCoroutine(nameof(ChangeState), State.Main);
            }
        }
    }

    public IEnumerator ChangeState(State change)
    {
        state = change;

        if (change == State.Quit)
        {
            yield return new WaitForSeconds(2.5f);
            MainView_UI.instance.quitUI.gameObject.SetActive(false);
            state = State.Main;
        }
    }

    public void ShowToast(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        curActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            AndroidJavaObject toast = new AndroidJavaObject("android.widget.Toast", curActivity);
            toast.CallStatic<AndroidJavaObject>("makeText", curActivity, message, 2f).Call("show");
        }));
    }

    // �� ����
    public void Quit()
    {
        MainView_UI.instance.quitUI.gameObject.SetActive(false);
        print("����");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_ANDROID
        Application.Quit();
#endif
    }
}