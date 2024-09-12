using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    private void Start()
    {
        layerProp = 1 << LayerMask.NameToLayer("Prop");
        layerTour = 1 << LayerMask.NameToLayer("Tour");

        point = new PointerEventData(null);
        settingUI = transform.Find("SettingPrefab").GetComponent<SettingCanvasOnOff>();
    }


    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            point.position = touch.position;
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(point, results);

            foreach (RaycastResult r in results)
            {
                if (r.gameObject.CompareTag("BottomSheet") && BottomSheetMovement.instance.state == BottomSheetMovement.State.DOWN)
                {
                    if (GetComponent<PopUpMovement>().placeState == PopUpMovement.PlaceState.UP || GetComponent<PopUpMovement>().tourState == PopUpMovement.TourState.UP)
                    {
                        return;
                    }
                    else
                    {
                        BottomSheetMovement.instance.MoveUP();
                    }
                }
            }
        }

        RayTouch();

        BackTouch();
    }

    private bool began = true;

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

    void BackTouch()
    {
        if(Application.platform == RuntimePlatform.Android) { }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ����ȭ��
            if (state == State.Main)
            {
                // �� ���� Ȯ�� UI Ȱ��ȭ
                MainView_UI.instance.quitUI.gameObject.SetActive(true);

                state = State.QuitUI;
            }
            // �� ���� Ȯ�� UI 
            else if (state == State.QuitUI)
            {
                // �� ���� Ȯ�� UI ��Ȱ��ȭ
                MainView_UI.instance.quitUI.gameObject.SetActive(false);

                state = State.Main;
            }
            // ����â�� �������� ��
            else if (state == State.Setting)
            {
                settingUI.SettingCanvasOff();
            }
            // �˾�â�� Ȱ��ȭ �Ǿ����� ��
            else
            {
                // Ž��
                if (PopUpMovement.instance.placeState == PopUpMovement.PlaceState.UP && PopUpMovement.instance.tourState == PopUpMovement.TourState.DOWN)
                {
                    PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveDOWN), true);
                    print("Ž�� �˾�â�� ���� ���� ��");
                }
                // ����
                else if (PopUpMovement.instance.tourState == PopUpMovement.TourState.UP && PopUpMovement.instance.placeState == PopUpMovement.PlaceState.DOWN)
                {
                    PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveDOWN), false);
                    print("���� �˾�â�� ���� ���� ��");
                }
            }
        }
    }

    public void Quit(bool quit)
    {
        if (quit)
        {
            MainView_UI.instance.quitUI.gameObject.SetActive(false);
            print("����");
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