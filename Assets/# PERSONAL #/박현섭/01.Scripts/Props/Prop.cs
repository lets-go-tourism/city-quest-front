using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Todo : ���� �����͸� �����ϴ� Ŭ������ �����
// 1. ���� �浵, �ּ�, �̸�, 
public class Prop : MonoBehaviour
{
    public PropData PropData { get; private set; }
    [SerializeField] private GameObject propObj;
    private int id;

    public float OffsetY { get; private set; }

    public bool PropActive { get { return propActive; } 
        private set 
        {
            if (propActive == value)
                return;

            propActive = value;

            if(value)
            {
                propObj.SetActive(true);
                MapUIController.Instance.NameTagContainer.AddTarget(this);
            }
            else
            {
                propObj.SetActive(false);
                MapUIController.Instance.NameTagContainer.RemoveTarget(this);
            }
        } 
    }
    private bool propActive = false;

    private void Start()
    {
        propObj.SetActive(false);
        OffsetY = -35f;
    }

    private void Update()
    {
        CheckDistToCamera();

        if (PropActive == false)
            return;

        transform.Rotate(new Vector3(0, 10f * Time.deltaTime, 0), Space.Self);
    }

    private void CheckDistToCamera()
    {
        if ((Camera.main.transform.position - new Vector3(0, 500, 0) - transform.position).sqrMagnitude > 100000)
            PropActive = false;
        else
            PropActive = true;
    }
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
