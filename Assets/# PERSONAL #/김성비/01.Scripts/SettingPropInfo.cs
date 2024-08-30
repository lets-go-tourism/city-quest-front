using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class SettingPropInfo : MonoBehaviour
{
    public static SettingPropInfo instance;

    private void Awake()
    {
        instance = this;
    }

    public void PropInfoSetting(Transform trs)
    {
        // UNREVEAL
        if (trs.GetComponent<tmpPropReveal>().state == tmpPropReveal.State.UNREVEAL)
        {
            SettingNO(trs);
        }

        // ADVENTURING , REVEAL
        else
        {
            SettingYES(trs);
        }
    }

   public InfoHolder holderN;
   public InfoHolder holderY;

    void SettingNO(Transform tr)
    {
        // 데이터 세팅
        Props_UI.instance.propModeling.GetComponent<MeshRenderer>().material = tr.GetComponent<MeshRenderer>().material;
        holderN.infos[1].GetComponent<TextMeshProUGUI>().text = tr.name;
        holderN.infos[2].GetComponent<TextMeshProUGUI>().text = "99.99" + "km";
        //holder.infos[3].GetComponent<Image>().sprite = ;
        holderN.infos[4].GetComponent<TextMeshProUGUI>().text = "경기 00시 00구 00로 00";
        holderN.GoURL("ADRESS");
        //holder.infos[5].GetComponent<Image>().sprite = ;
        holderN.infos[6].GetComponent<TextMeshProUGUI>().text = "어떤 퀘스트일까요";

        // UI 활성화
        Props_UI.instance.PropsUISetting(true, 2);
    }

    void SettingYES(Transform tr)
    {
        // 데이터 세팅
        Props_UI.instance.propModeling.GetComponent<MeshRenderer>().material = tr.GetComponent<MeshRenderer>().material;
        holderY.infos[1].GetComponent<TextMeshProUGUI>().text = tr.name;
        holderY.infos[2].GetComponent<TextMeshProUGUI>().text = "0월 0일 방문";
        //holderY.infos[3].GetComponent<Image>().sprite = ;
        //holderY.infos[4].GetComponent<Image>().sprite = ;
        holderY.infos[5].GetComponent<TextMeshProUGUI>().text = "경기 00시 00구 00로 00";
        // UI 활성화
        Props_UI.instance.PropsUISetting(true, 1);
    }


    //Props_UI.instance.propModeling.GetComponent<MeshRenderer>().material = trs.GetComponent<MeshRenderer>().material;   // 모델링
    //Props_UI.instance.contents[0].GetChild(0).GetComponent<TextMeshProUGUI>().text = trs.name;                          // 이름
    //Props_UI.instance.contents[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = "o o o";                           // 난이도
    //// Props_UI.instance.contents[7].                                                                                   // 장소 사진
    //Props_UI.instance.contents[8].GetChild(0).GetComponent<TextMeshProUGUI>().text = "Address";                         // 장소 정보
    ////Props_UI.instance.contents[8].GetChild(1)                                                                         // 장소 링크

    //Props_UI.instance.contents[2].gameObject.SetActive(false);  // 방문일자
    //Props_UI.instance.contents[3].gameObject.SetActive(false);  // 업적
    //Props_UI.instance.contents[4].gameObject.SetActive(true);   // 유저와의 거리
    //Props_UI.instance.contents[5].gameObject.SetActive(false);  // 찍은 사진 1
    //Props_UI.instance.contents[6].gameObject.SetActive(false);  // 찍은 사진 2
    //Props_UI.instance.contents[9].GetChild(0).GetComponent<TextMeshProUGUI>().text = "Quest";               // 구분선
    //Props_UI.instance.contents[10].GetChild(1).GetComponent<TextMeshProUGUI>().text = "Take a Picture !";    // 퀘스트

    //Props_UI.instance.contents[2].gameObject.SetActive(true);  // 방문일자
    //Props_UI.instance.contents[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = "2024.08.25";
    //Props_UI.instance.contents[4].gameObject.SetActive(false);  // 유저와의 거리

    //// ******************** 퀘스트 달성여부에 따라 사진 
    //Props_UI.instance.contents[5].gameObject.SetActive(true);   // 찍은 사진 1
    //Props_UI.instance.contents[6].gameObject.SetActive(false);  // 찍은 사진 2
    //Props_UI.instance.contents[9].GetChild(0).GetComponent<TextMeshProUGUI>().text = "PlusQuest";
    //Props_UI.instance.contents[10].GetChild(1).GetComponent<TextMeshProUGUI>().text = "Let's Look Arond !";

    // REVEAL
    //if (trs.GetComponent<tmpPropReveal>().state == tmpPropReveal.State.REVEAL)
    //{
    //    //Props_UI.instance.contents[3].gameObject.SetActive(true);
    //    //// Props_UI.instance.contents[3].GetComponent<>().          // 어떤 업적인지

    //    ////if ( 추가 퀘스트 모두 완료 했으면 )
    //    ////{
    //    ////    Props_UI.instance.contents[3].GetChild(0).GetComponent<Image>().color = Color.green;
    //    ////}

    //    //// 그게 아니라면
    //    //Props_UI.instance.contents[3].GetChild(0).GetComponent<Image>().color = Color.red;

    //}
}
