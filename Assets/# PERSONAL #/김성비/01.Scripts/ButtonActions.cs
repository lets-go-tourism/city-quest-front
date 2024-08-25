using UnityEngine;
using UnityEngine.UI;

public class ButtonActions : MonoBehaviour
{
    Button btn;

    private void Start()
    {
        btn = GetComponent<Button>();
    }

    public void DoAction(int no)
    {
        // 탐험/미탐험 지역 팝업창 끄기
        if (no == 0)
        {
            Props_UI.instance.PropsUISetting(false);
            Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);

            // 프랍을 터치할 수 있도록!!
            for (int i = 0; i < Props_UI.instance.props.Length; i++)
                Props_UI.instance.props[i].GetComponent<BoxCollider>().enabled = true;
        }

        // 사진 찍으러 가기
        else if (no == 1)
        {
            Props_UI.instance.PropsUISetting(false);
            Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);
            Props_UI.instance.props[0].gameObject.SetActive(false);
            Props_UI.instance.CanvasCamera.enabled = true;
        }

        // 사진 찍고 돌아가기
        else if(no == 2)
        {
            Props_UI.instance.PropsUISetting(true);
            Props_UI.instance.props[0].gameObject.SetActive(true);
            Props_UI.instance.CanvasCamera.enabled = false;
        }
    }

    public void ChangeBottomSheet(bool open)
    {
        // true  : 관광탭
        // false : 장소탭
        Props_UI.instance.tour.gameObject.SetActive(open);

        // 관광탭일 때 할 일 : 관광정보 팝업 세팅
        if (open)
        {
            // 관광탭일 때 할 일 : 관광정보 팝업 세팅
        }

        // 장소탭일 때 할 일 : 장소 팝업 세팅
        if (!open)
        {
            // 장소탭일 때 할 일 : 장소 팝업 세팅
        }
    }
}
