using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUIController : MonoBehaviour
{
    public static MapUIController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    // ������ �ϴܿ� ���̴� UI ���� ��ũ��Ʈ
    public NameTagContainer NameTagContainer { get { return nameTagContainer; } private set { NameTagContainer = value; } }
    [SerializeField] private NameTagContainer nameTagContainer;

    // �÷��̾��� ���� ��ġ�� ��� �������� �˷��ִ� ��ũ��Ʈ
    public MyPosArrowUI MyPosArrowUI { get { return myPosArrowUI; } private set { myPosArrowUI = value; } }
    [SerializeField] private MyPosArrowUI myPosArrowUI;
}
