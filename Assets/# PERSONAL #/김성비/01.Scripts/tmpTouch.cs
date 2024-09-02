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

    int layerProp, layerBottomSheet;
    Touch touch;
    public GraphicRaycaster raycaster;
    PointerEventData point;
    private void Start()
    {
        layerProp = 1 << LayerMask.NameToLayer("Prop");
        //layerProp = 1 << LayerMask.NameToLayer("UI");

        point = new PointerEventData(null);
    }


    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            point.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(point, results);

            foreach(RaycastResult r in results) 
            {
                if (r.gameObject.CompareTag("BottomSheet") && BottomSheetMovement.instance.state == BottomSheetMovement.State.DOWN)
                {
                    BottomSheetMovement.instance.MoveUP();
                }
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

    private void RayTouch()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (EventSystem.current.IsPointerOverGameObject(i))
                    {
                        return;
                    }
                }

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // 프랍을 터치했을 때
                if (Physics.Raycast(ray, out hit, layerProp))
                {
                    print(hit.collider.name);
                    // 프랍 정보를 가져와서 팝업창 띄우기
                    // 할 일 : 그 프랍 중에 프랍 정보 가지고 있는 스크립트 가져오기
                    Prop prop = hit.collider.GetComponent<Prop>();

                    // 그 스크립트 중에 Adventure No 를 서버에 쏘기
                    KJY_ConnectionTMP.instance.OnConnectionQuest((int)prop.PropData.propNo);
                    SettingPropInfo.instance.PropInfoSetting();

                    // 아래 코드 삭제하기
                    //SettingPropInfo.instance.PropInfoSetting(hit.transform.GetComponent<Prop>());

                    // 더 이상 프랍을 터치할 수 없도록!!
                    for (int i = 0; i < Props_UI.instance.props.Length; i++)
                        Props_UI.instance.props[i].GetComponent<BoxCollider>().enabled = false;
                }
            }
        }
    }
    List<QuestData> questList = new List<QuestData>();
    void SettingList()
    {
        foreach(var quest in questList)
        {
            List<string> list = new List<string>()
            {
                quest.locationName,
                quest.addr,
                quest.kakaoMapUrl,
                quest.imageUrl,
                quest.propNo.ToString(),
                quest.status.ToString(),
                quest.difficulty,
                quest.questDesc,
                quest.distance.ToString(),
                quest.date.ToString("MM월 dd일"),
                quest.questImage,
            };
        }

        
    }
}