using JetBrains.Annotations;
using System.Collections.Generic;
using TMPro;
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

    int layerProp, layerTour;
    Touch touch;
    public GraphicRaycaster raycaster;
    PointerEventData point;
    private void Start()
    {
        layerProp = 1 << LayerMask.NameToLayer("Prop");
        layerTour = 1 << LayerMask.NameToLayer("Tour");

        point = new PointerEventData(null);
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
                //else if (r.gameObject.CompareTag("Quest"))
                //{
                //    print("dfkjslkfjsdlkfsdl");
                //}
                //else if(r.gameObject.CompareTag("CardPlace") && BottomSheetMovement.instance.state == BottomSheetMovement.State.UP)
                //{
                //    print(r.gameObject.transform.parent);
                //    // �� �� : ī�� ������ ��ġ�ϴ� ���/�������� �������� ȭ���� �̵�
                //}
                //else if(r.gameObject.CompareTag("CardTour") && BottomSheetMovement.instance.state == BottomSheetMovement.State.UP)
                //{
                //    print(r.gameObject.transform.parent);
                //}
            }
        }

        RayTouch();
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

                    else if(hitLayer == LayerMask.NameToLayer("Tour"))
                    {
                        //DataManager.instance.requestSuccess = false;

                        Props_UI.instance.canvasTour.enabled = true;

                        // ���� ������ �޾ƿ�
                        TourData tourData = hit.collider.GetComponent<TourData>();
                        ServerTourInfo serverTourInfo = tourData.ServerTourInfo;

                        //
                        SettingTourInfo.instance.StartCoroutine(nameof(SettingTourInfo.instance.GetTexture), serverTourInfo);

                        // �˾�â�� ���� ����
                        
                    }
                }
            }
        }
    }
}