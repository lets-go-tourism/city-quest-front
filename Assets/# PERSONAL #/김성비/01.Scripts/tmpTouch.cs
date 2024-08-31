using TMPro;
using UnityEngine;

public class tmpTouch : MonoBehaviour
{
    /// <summary>
    /// ��ǥ : ���� ���� ������ ��ġ�ϸ� �ش� ������ ������ �޾ƿ� UI�� ǥ���Ѵ�.
    /// 1. ������ ��ġ�ϸ�
    ///     1-1. ���� ����(layer)
    ///          - ���� ����
    ///     1-2. ������ ȹ�� ����
    ///     1-3. ������ �̸�, ���̵�, ���� ������ �Ÿ� ����
    ///     1-4. �߰� ����Ʈ ����
    ///     
    /// 2. 1�� ������ UI�� ǥ���Ѵ�.
    /// </summary>



    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ������ ��ġ���� ��
            if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Prop")))
            {
                // ���� ������ �����ͼ� �˾�â ����
                // �� �� : �� ���� �߿� ���� ���� ������ �ִ� ��ũ��Ʈ ��������
                // �� ��ũ��Ʈ �߿� Adventure No �� ������ ���
                // �Ʒ� �ڵ� �����ϱ�
                SettingPropInfo.instance.PropInfoSetting(hit.transform);

                // �� �̻� ������ ��ġ�� �� ������!!
                for (int i = 0; i < Props_UI.instance.props.Length; i++)
                    Props_UI.instance.props[i].GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}