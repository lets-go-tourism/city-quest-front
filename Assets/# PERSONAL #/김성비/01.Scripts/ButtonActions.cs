using UnityEngine;
using UnityEngine.UI;

public class ButtonActions : MonoBehaviour
{
    Button btn;
    public Transform bs_Tour;

    private void Start()
    {
        btn = GetComponent<Button>();
    }

    public void DoAction(int no)
    {
        // Ž��/��Ž�� ���� �˾�â ����
        if (no == 0)
        {
            Props_UI.instance.PropsUISetting(false, 0);

            // ������ ��ġ�� �� �ֵ���!!
            for (int i = 0; i < Props_UI.instance.props.Length; i++)
                Props_UI.instance.props[i].GetComponent<BoxCollider>().enabled = true;
        }

        // ���� ������ ����
        else if (no == 1)
        {
            //Props_UI.instance.PropsUISetting(false, 0);
            Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);
            Props_UI.instance.props[0].gameObject.SetActive(false);
            Props_UI.instance.CanvasCamera.enabled = true;
        }

        // ���� ��� ���ư���
        else if(no == 2)
        {
            //Props_UI.instance.PropsUISetting(true, 0);
            Props_UI.instance.props[0].gameObject.SetActive(true);
            Props_UI.instance.CanvasCamera.enabled = false;
        }
    }

    // �±� ��������Ʈ �� ���� �ٲٱ�
    public void ChangeBottomSheet(int num)
    {
        // ��� �˾� ����
        if (num == 0)
        {
            Props_UI.instance.tags[0].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[0];
            Props_UI.instance.tags[1].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[1];
            bs_Tour.gameObject.SetActive(false);
            //SortingBottomSheet.instance.SortingPlace();
        }

        // �������� �˾� ����
        if (num == 1)
        {
            Props_UI.instance.tags[1].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[0];
            Props_UI.instance.tags[0].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[1];
            bs_Tour.gameObject.SetActive(true);
            //SortingBottomSheet.instance.SortingTour();
        }
    }
}
