using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private Image _backgroundDark;
    [SerializeField] private GameObject _toastMsg;
    [SerializeField] private Image _invisibleNonTouch;

    public delegate void TutorialStart();
    public delegate void TutorialEnd();

    public TutorialStart _tutorialStartDelegate;
    public TutorialEnd _tutorialEndDelegate;

    private void Start()
    {
        _backgroundDark.enabled = true;
        _toastMsg.SetActive(true);
        _invisibleNonTouch.enabled = false;
        _toastMsg.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(OnClickYes);
        _toastMsg.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(OnClickNo);
    }

    private void OnClickYes()
    {
        _backgroundDark.enabled = false;
        _toastMsg.SetActive(false);

        // 튜토리얼 시작
        StartTutorial();
    }

    private void OnClickNo()
    {
        _backgroundDark.enabled = false;
        _toastMsg.SetActive(false);
    }

    private void StartTutorial()
    {
        if (_tutorialStartDelegate != null)
            _tutorialStartDelegate.Invoke();

        _invisibleNonTouch.enabled = true;
        MapCameraController.Instance.StartCameraMoveToTarget(new Vector3(-206, 0, 355.5f));

    }
}
