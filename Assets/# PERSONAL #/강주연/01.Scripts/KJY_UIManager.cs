using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KJY_UIManager : MonoBehaviour
{
    [Header("splash_onBoard")]
    [SerializeField] private GameObject splash_onBoardObject;
    [SerializeField] private RectTransform logoFirstPosition;
    [SerializeField] private RectTransform logoSecondPosition;
    [SerializeField] private GameObject logo;
    [SerializeField] private TextMeshProUGUI explain;
    [SerializeField] private GameObject kakaoBtn;


    [Header("Term&Confirm")]
    [SerializeField] private GameObject confrimObject;
    [SerializeField] private GameObject confirmBtn;
    [SerializeField] private ScrollRect confirmScrollRect;
    private bool isConfirmView = false;

    [Header("Authorization")]
    [SerializeField] private GameObject AuthorizationObject;
    [SerializeField] private Button checkBox1;
    [SerializeField] private Button checkBox2;
    [SerializeField] private Button checkBox3;



    // Start is called before the first frame update
    void Start()
    {
        explain.DOFade(0f, 0f);
        kakaoBtn.SetActive(false);
        confirmBtn.SetActive(false);
        logo.transform.position = logoFirstPosition.position;
        StartCoroutine(Splash());
    }

    private void Update()
    {
        if (confirmScrollRect.verticalNormalizedPosition == 0 && isConfirmView == true)
        {
            confirmBtn.SetActive(true);
            isConfirmView = false;
        }
    }
    #region splash_onBoard2-1-1
    private IEnumerator Splash()
    {
        yield return new WaitForSeconds(0.5f);
        yield return logo.transform.DOMove(logoSecondPosition.position, 1f);
        explain.DOFade(1f, 1f);
        explain.gameObject.SetActive(true);
        kakaoBtn.gameObject.SetActive(true);
        //���⼭ �α��ε��ִ��� �ȵ��ִ����� ��ū ������ ���� �ٷ� �̵��������� �ƴϸ� �º������� �̵����� ����
        //if ()
        //{

        //}
        //else
        //{

        //}
    }
    #endregion

    #region onBoard2-2-2

    #endregion

    #region newCustomer
    //ȸ�������Ͻ�
    public void ShowConfirmScrollView()
    {
        splash_onBoardObject.SetActive(false);
        //���⼭ �α����ߴ��� ���ߴ����� ���� �޶���
        confirmBtn.SetActive(true);
        isConfirmView = true;
    }

    public void OnClickConfirmButton()
    {
        confrimObject.SetActive(false);
        AuthorizationObject.SetActive(true);
    }
    #endregion

}
