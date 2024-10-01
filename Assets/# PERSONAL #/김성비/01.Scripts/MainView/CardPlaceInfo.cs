using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using TMPro;
using System;

public class CardPlaceInfo : MonoBehaviour
{
    public Transform[] info;

    // 탐험/미탐험
    public enum Type
    {
        REVEAL,
        UNREVEAL
    }
    public Type type;

    // 선택/미선택
    public enum State
    {
        UnSelected,
        Selected
    }
    public State state;

    public ServerAdventurePlace ServerAdventurePlace { get; private set; }
    public ServerProp ServerProp { get; private set; }

    public IEnumerator Start()
    {
        int num = 0;
        while (num == 0)
        {
            string meter = ConvertDistance(GPS.Instance.GetDistToUserInRealWorld(ServerProp.latitude, ServerProp.longitude)).ToString();
            info[1].GetComponent<TextMeshProUGUI>().text = meter;
            yield return new WaitForSeconds(5);
        }
    }


    string ConvertDistance(double distance)
    {
        string result = string.Empty;

        double tmp = distance;
        double a = 1000;
        if (tmp > a)
        {
            double calcultate = Math.Round(tmp / a, 1);
            result = calcultate.ToString() + "km";
        }
        else
        {
            result = tmp.ToString() + "m";
        }

        return result;
    }

    public IEnumerator GetTexture(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            CropImage(texture);
        }
    }

    private void CropImage(Texture2D texture)
    {
        // 원본 이미지 크기
        float originalWidth = texture.width;
        float originalHeight = texture.height;

        // 원본 이미지 비율 계산
        float originalAspect = originalWidth / originalHeight;

        // 목표 크기 계산
        float targetWidth = 240;
        float targetHeight = 240;

        // 가로 길이가 더 긴 경우
        if (originalWidth > originalHeight)
        {
            // 세로 길이를 240으로 맞추고 비율에 맞춰 가로 길이 계산
            targetWidth = targetHeight * originalAspect;
            targetHeight = 240;
        }
        else
        {
            // 세로 길이가 더 긴 경우
            targetWidth = 240;
            targetHeight = targetWidth / originalAspect;
        }

        if (info[2].GetComponent<RectTransform>() != null)
        {
            info[2].GetComponent<RectTransform>().sizeDelta = new Vector2(targetWidth, targetHeight);
        }

        info[2].GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, originalWidth, originalHeight), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect);
    }

    public void SettingPlaceType(bool reveal)
    {
        if (reveal) { type = Type.REVEAL; }
        else if (!reveal) { type = Type.UNREVEAL; }
    }

    public void SetServerProp(ServerAdventurePlace prop)
    {
        this.ServerAdventurePlace = prop;
    }

    public void setPlaceProp(ServerProp serverProp)
    {
        this.ServerProp = serverProp;
    }

    public void SendPlaceInfo()
    {
        //KJY추가
        SettingManager.instance.EffectSound_ButtonTouch();
        // 미선택 -> 선택
        if (state == State.UnSelected)
        {
            // 나머지 미선택
            for (int j = 0; j < BottomSheetManager.instance.contentTour.childCount; j++)
            {
                BottomSheetManager.instance.contentTour.GetChild(j).GetChild(0).GetComponent<Image>().sprite = BottomSheetManager.instance.contentTour.GetChild(j).GetComponent<SpritesHolder>().sprites[0];
                BottomSheetManager.instance.contentTour.GetChild(j).GetComponent<CardTourInfo>().Selected(false);
            }
            for (int i = 0; i < BottomSheetManager.instance.contentPlace.childCount; i++)
            {
                BottomSheetManager.instance.contentPlace.GetChild(i).GetChild(0).GetComponent<Image>().sprite = GetComponent<SpritesHolder>().sprites[0];
                BottomSheetManager.instance.contentPlace.GetChild(i).GetComponent<CardPlaceInfo>().Selected(false);
            }
            // 누른 것만 선택
            transform.GetChild(0).GetComponent<Image>().sprite = GetComponent<SpritesHolder>().sprites[1];
            Selected(true);

            // 화면 이동
            Prop targetProp = PropsController.Instance.ServerAdventurePlaceWorldDic[this.ServerAdventurePlace];
            MapCameraController.Instance.StartCameraMoveToTarget(targetProp.PropObj.transform.TransformPoint(targetProp.GetBoundsCenter()));

            // 네임태크 틴트
            PropsController.Instance.TintProp = targetProp;
        }

        // 선택 -> 미선택
        else
        {
            Selected(false);
            transform.GetChild(0).GetComponent<Image>().sprite = GetComponent<SpritesHolder>().sprites[0];
        }
    }

    public void Selected(bool isSelected)
    {
        if (isSelected)
        {
            state = State.Selected;
        }
        else
        {
            state = State.UnSelected;
        }
    }

    public void ChangeType()
    {
        type = Type.REVEAL;
    }

    ///public class HomeAdventurePlace
    ///{
    ///    public long adventureNo;
    ///    public string name;
    ///    public string difficulty;
    ///    public string imageUrl;
    ///    public double distance;
    ///    public bool status;
    ///}
    ///
}
