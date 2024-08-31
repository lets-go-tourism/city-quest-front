using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Todo : ���� �����͸� �����ϴ� Ŭ������ �����
// 1. ���� �浵, �ּ�, �̸�, 
public class Prop : MonoBehaviour
{
    public HomeProps PropData { get; private set; }
    public HomeAdventurePlace HomeAdventurePlaceData { get; private set; }

    public GameObject PropObj { get { return propObj; } private set { propObj = value; } }
    [SerializeField] private GameObject propObj;

    public float OffsetY { get; private set; } = -35f;

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

    public void Init(HomeProps propData, HomeAdventurePlace homeAdventurePlace)
    {
        this.PropData = propData;
        this.HomeAdventurePlaceData = homeAdventurePlace;
    }

    private void Start()
    {
        propObj.SetActive(false);
        originPos = transform.position;
    }

    private void Update()
    {
        CheckDistToCamera();

        if (PropActive == false)
            return;


        �յ�();
        //transform.Rotate(new Vector3(0, 10f * Time.deltaTime, 0), Space.Self);
    }

    public AnimationCurve curve;

    private Vector3 originPos;

    private float time;

    public float time2 = 1;
    public float �յ�value = 30;
    private void �յ�()
    {
        time += Time.deltaTime;
        if (time > time2)
            time -= time2;

        transform.position = originPos + Vector3.up * curve.Evaluate(time / time2) * �յ�value + new Vector3(0, 60, 0);
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

public enum QuestType
{
    picture,

}
