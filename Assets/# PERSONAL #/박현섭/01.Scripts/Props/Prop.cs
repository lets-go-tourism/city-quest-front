using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Todo : 프랍 데이터를 저장하는 클래스를 만들기
// 1. 위도 경도, 주소, 이름, 
public class Prop : MonoBehaviour
{
    public PropData PropData { get; private set; }
    private int id;

}

public class PropData
{
    // 프랍의 고유 번호 (관리용이를 위한)
    public int id;

    // 프랍 이름 예: 수원 화성 박물관
    public string name;

    // 프랍 주소 예: 경기 수원시 팔달구 창룡대로 21
    public string addrees;

    // 프랍 위도
    public float latitude;

    // 프랍 경도
    public float longitude;

    // 현재 프랍을 얻었는지 얻었을시 true
    public bool isGet;

    // 인증샷
    public Image propPicture;
}

public class QuestData
{
    //프랍의 고유 번호
    public int id;
}

public enum QuestType
{
    picture,

}
