using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using TMPro;
using System;

public class CardTourInfo : MonoBehaviour
{
    public Transform[] info;

    public enum Type
    {
        TouristSpot,
        CulturalFacilities,
        Festival,
        TravelCource,
        LeisureSports,
        Lodgment,
        Shopping,
        Food
    }
    public Type type;

    public enum State
    {
        UnSelected,
        Selectd
    }
    public State state;

    public ServerTourInfo ServerTourInfo { get; private set; }

    public IEnumerator UpateDistance()
    {
        int num = 0;
        while (num == 0)
        {
            string meter = ConvertDistance(GPS.Instance.GetDistToUserInRealWorld(double.Parse(ServerTourInfo.latitude), double.Parse(ServerTourInfo.longitude))).ToString();
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

    // 이미지 세팅
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

    // 타입 세팅
    public void SettingTourType(string str)
    {
        int no = 0;
        no = int.Parse(str);

        if (no == 12) { type = Type.TouristSpot; }
        else if (no == 14) { type = Type.CulturalFacilities; }
        else if (no == 15) { type = Type.Festival; }
        else if (no == 25) { type = Type.TravelCource; }
        else if (no == 28) { type = Type.LeisureSports; }
        else if (no == 32) { type = Type.Lodgment; }
        else if (no == 38) { type = Type.Shopping; }
        else if (no == 39) { type = Type.Food; }
    }


    public void InputTourList(ServerTourInfo serverTourInfo)
    {
        this.ServerTourInfo = serverTourInfo;
    }

    public void SendTourInfo()
    {
        //KJY추가
        SettingManager.instance.EffectSound_ButtonTouch();

        // 미선택 -> 선택
        if (state == State.UnSelected)
        {
            for (int j = 0; j < BottomSheetManager.instance.contentPlace.childCount; j++)
            {
                BottomSheetManager.instance.contentPlace.GetChild(j).GetChild(0).GetComponent<Image>().sprite = BottomSheetManager.instance.contentPlace.GetChild(j).GetComponent<SpritesHolder>().sprites[0];
                BottomSheetManager.instance.contentPlace.GetChild(j).GetComponent<CardPlaceInfo>().Selected(false);
            }

            for (int i = 0; i < BottomSheetManager.instance.contentTour.childCount; i++)
            {
                BottomSheetManager.instance.contentTour.GetChild(i).GetChild(0).GetComponent<Image>().sprite = GetComponent<SpritesHolder>().sprites[0];
                BottomSheetManager.instance.contentTour.GetChild(i).GetComponent<CardTourInfo>().Selected(false);
            }
            transform.GetChild(0).GetComponent<Image>().sprite = GetComponent<SpritesHolder>().sprites[1];
            Selected(true);

            TourData targetTour = TourDataController.Instance.TourInfoWordList[ServerTourInfo];
            MapCameraController.Instance.StartCameraMoveToTarget(targetTour.transform.position);

            PropsController.Instance.TintTourData = targetTour;
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
            state = State.Selectd;
        }
        else
        {
            state = State.UnSelected;
        }
    }
}