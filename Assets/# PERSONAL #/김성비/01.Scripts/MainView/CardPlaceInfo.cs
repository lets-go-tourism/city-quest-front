using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using TMPro;
using System;

public class CardPlaceInfo : MonoBehaviour
{
    public Transform[] info;

    // Ž��/��Ž��
    public enum Type
    {
        REVEAL,
        UNREVEAL
    }
    public Type type;

    // ����/�̼���
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
        // ���� �̹��� ũ��
        float originalWidth = texture.width;
        float originalHeight = texture.height;

        // ���� �̹��� ���� ���
        float originalAspect = originalWidth / originalHeight;

        // ��ǥ ũ�� ���
        float targetWidth = 240;
        float targetHeight = 240;

        // ���� ���̰� �� �� ���
        if (originalWidth > originalHeight)
        {
            // ���� ���̸� 240���� ���߰� ������ ���� ���� ���� ���
            targetWidth = targetHeight * originalAspect;
            targetHeight = 240;
        }
        else
        {
            // ���� ���̰� �� �� ���
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
        //KJY�߰�
        SettingManager.instance.EffectSound_ButtonTouch();
        // �̼��� -> ����
        if (state == State.UnSelected)
        {
            // ������ �̼���
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
            // ���� �͸� ����
            transform.GetChild(0).GetComponent<Image>().sprite = GetComponent<SpritesHolder>().sprites[1];
            Selected(true);

            // ȭ�� �̵�
            Prop targetProp = PropsController.Instance.ServerAdventurePlaceWorldDic[this.ServerAdventurePlace];
            MapCameraController.Instance.StartCameraMoveToTarget(targetProp.PropObj.transform.TransformPoint(targetProp.GetBoundsCenter()));

            // ������ũ ƾƮ
            PropsController.Instance.TintProp = targetProp;
        }

        // ���� -> �̼���
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
