using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
                if (r.gameObject.CompareTag("BottomSheet"))
                {
                    BottomSheetMovement.instance.MoveUP();
                }
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

                // ������ ��ġ���� ��
                if (Physics.Raycast(ray, out hit, layerProp))
                {
                    print(hit.collider.name);
                    // ���� ������ �����ͼ� �˾�â ����
                    // �� �� : �� ���� �߿� ���� ���� ������ �ִ� ��ũ��Ʈ ��������
                    // �� ��ũ��Ʈ �߿� Adventure No �� ������ ���
                    // �Ʒ� �ڵ� �����ϱ�
                    SettingPropInfo.instance.PropInfoSetting(hit.transform.GetComponent<Prop>());

                    // �� �̻� ������ ��ġ�� �� ������!!
                    for (int i = 0; i < Props_UI.instance.props.Length; i++)
                        Props_UI.instance.props[i].GetComponent<BoxCollider>().enabled = false;
                }
            }
        }
    }
}