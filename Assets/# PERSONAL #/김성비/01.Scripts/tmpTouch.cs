using TMPro;
using UnityEngine;

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



    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 프랍을 터치했을 때
            if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Prop")))
            {
                // 프랍 정보를 가져와서 팝업창 띄우기
                SettingPropInfo.instance.PropInfoSetting(hit.transform);

                // 더 이상 프랍을 터치할 수 없도록!!
                for (int i = 0; i < Props_UI.instance.props.Length; i++)
                    Props_UI.instance.props[i].GetComponent<BoxCollider>().enabled = false;
            }
            else if (Physics.Raycast(ray, out hit, LayerMask.GetMask("UI")))
            {
                print("dfjsldjkfhsdkj");
                if (hit.transform.CompareTag("BSPlace"))
                {
                    Props_UI.instance.tags[0].sprite = hit.transform.GetComponent<SpritesHolder>().sprites[0];
                    Props_UI.instance.tags[1].sprite = hit.transform.GetComponent<SpritesHolder>().sprites[1];
                    print("0000000000000000000000");
                }
                else if (hit.transform.CompareTag("BSTour"))
                {
                    Props_UI.instance.tags[0].sprite = hit.transform.GetComponent<SpritesHolder>().sprites[1];
                    Props_UI.instance.tags[1].sprite = hit.transform.GetComponent<SpritesHolder>().sprites[0];
                    print("1111111111111111111111111");
                }

            }
        }
    }
}