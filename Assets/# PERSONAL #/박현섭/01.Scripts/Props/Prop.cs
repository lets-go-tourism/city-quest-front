using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Todo : ���� �����͸� �����ϴ� Ŭ������ �����
// 1. ���� �浵, �ּ�, �̸�, 
public class Prop : MonoBehaviour
{
    public PropData PropData { get; private set; }
    private int id;

}

public class PropData
{
    // ������ ���� ��ȣ (�������̸� ����)
    public int id;

    // ���� �̸� ��: ���� ȭ�� �ڹ���
    public string name;

    // ���� �ּ� ��: ��� ������ �ȴޱ� â���� 21
    public string addrees;

    // ���� ����
    public float latitude;

    // ���� �浵
    public float longitude;

    // ���� ������ ������� ������� true
    public bool isGet;

    // ������
    public Image propPicture;
}

public class QuestData
{
    //������ ���� ��ȣ
    public int id;
}

public enum QuestType
{
    picture,

}
