using System.Collections.Generic;
using TMPro;
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
                //    // 할 일 : 카드 정보와 일치하는 장소/관광정보 프랍으로 화면이 이동
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
            else if(began &&  touch.phase == TouchPhase.Moved)
            {
                began = false;
            }
            else if (touch.phase == TouchPhase.Ended && began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                print("터치 시도");

                // 탐험장소 프랍을 터치했을 때
                if (Physics.Raycast(ray, out hit, layerProp))
                {
                    DataManager.instance.requestSuccess = false;

                    // 프랍정보를 받아옴
                    Prop prop = hit.collider.GetComponent<Prop>();

                    // 프랍정보 중 propNo 을 서버에 보냄
                    KJY_ConnectionTMP.instance.OnConnectionQuest((int)prop.PropData.propNo);

                    // propNo 에 해당되는 데이터를 받아와서 팝업창에 정보 세팅
                    SettingPropInfo.instance.StartCoroutine(nameof(SettingPropInfo.instance.PropInfoSetting));
                }

                // 관광정보 프랍을 터치했을 때
                else if (Physics.Raycast(ray, out hit, layerTour))
                {
                    //DataManager.instance.requestSuccess = false;

                    // 관광 정보를 받아옴
                    TourData tourData = hit.collider.GetComponent<TourData>();
                    ServerTourInfo serverTourInfo = tourData.ServerTourInfo;

                    // 암전 키고 << 

                    // 팝업창에 정보 세팅
                    SettingTourInfo.instance.TourInfoSetting();
                }
            }
        }
    }
}