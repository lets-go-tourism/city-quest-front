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
                SettingPropInfo.instance.PropInfoSetting(hit.transform);

                // �� �̻� ������ ��ġ�� �� ������!!
                for (int i = 0; i < Props_UI.instance.props.Length; i++)
                    Props_UI.instance.props[i].GetComponent<BoxCollider>().enabled = false;
            }
            else if (Physics.Raycast(ray, out hit, LayerMask.GetMask("UI")))
            {
                print("dfjsldjkfhsdkj");
                if (hit.transform.CompareTag("BSPlace"))
                {
                    Props_UI.instance.tags[0].sprite = hit.transform.GetComponent<SpritesHolder>().sprites[0];
                    Props_UI.instance.tags[1].sprite = hit.transform.GetComponent<SpritesHolder>().sprites[1];
                    print("0000000000000000000000");
                }
                else if (hit.transform.CompareTag("BSTour"))
                {
                    Props_UI.instance.tags[0].sprite = hit.transform.GetComponent<SpritesHolder>().sprites[1];
                    Props_UI.instance.tags[1].sprite = hit.transform.GetComponent<SpritesHolder>().sprites[0];
                    print("1111111111111111111111111");
                }

            }
        }
    }
}