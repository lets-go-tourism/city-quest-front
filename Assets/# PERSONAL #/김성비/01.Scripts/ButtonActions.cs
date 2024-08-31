using UnityEngine;
using UnityEngine.UI;

public class ButtonActions : MonoBehaviour
{
    Button btn;
    public Transform bs_Tour;

    private void Start()
    {
        btn = GetComponent<Button>();
    }

    public void DoAction(int no)
    {
        // 탐험/미탐험 지역 팝업창 끄기
        if (no == 0)
        {
            Props_UI.instance.PropsUISetting(false, 0);

            // 프랍을 터치할 수 있도록!!
            for (int i = 0; i < Props_UI.instance.props.Length; i++)
                Props_UI.instance.props[i].GetComponent<BoxCollider>().enabled = true;
        }

        // 사진 찍으러 가기
        else if (no == 1)
        {
            //Props_UI.instance.PropsUISetting(false, 0);
            Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);
            Props_UI.instance.props[0].gameObject.SetActive(false);
            Props_UI.instance.CanvasCamera.enabled = true;
        }

        // 사진 찍고 돌아가기
        else if(no == 2)
        {
            //Props_UI.instance.PropsUISetting(true, 0);
            Props_UI.instance.props[0].gameObject.SetActive(true);
            Props_UI.instance.CanvasCamera.enabled = false;
        }
    }

    // 태그 스프라이트 및 내용 바꾸기
    public void ChangeBottomSheet(int num)
    {
        // 장소 팝업 세팅
        if (num == 0)
        {
            Props_UI.instance.tags[0].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[0];
            Props_UI.instance.tags[1].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[1];
            bs_Tour.gameObject.SetActive(false);
            //SortingBottomSheet.instance.SortingPlace();
        }

        // 관광정보 팝업 세팅
        if (num == 1)
        {
            Props_UI.instance.tags[1].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[0];
            Props_UI.instance.tags[0].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[1];
            bs_Tour.gameObject.SetActive(true);
            //SortingBottomSheet.instance.SortingTour();
        }
    }
}
