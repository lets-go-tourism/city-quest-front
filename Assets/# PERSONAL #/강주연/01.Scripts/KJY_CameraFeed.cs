using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CameraFeed : MonoBehaviour
{
    public static CameraFeed Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    [Header("Setting")]
    public Vector2 requestedRatio;
    public int requestedFPS;

    [Header("Component")]
    public RawImage webCamRawImage;
    public RectTransform webCam;

    [Header("Data")]
    public WebCamTexture webCamTexture;

    private bool useFrontCamera = false;
    //private bool tutorial = true;
    //private bool notTutorial = false;

    [Header("UI")]
    [SerializeField] private GameObject captureObject;
    [SerializeField] private RectTransform captureRect;
    [SerializeField] private GameObject checkObject;
    [SerializeField] private RectTransform checkRect;
    [SerializeField] private Canvas camCanvas;
    [SerializeField] private GameObject tipObject;
    [SerializeField] private Image tipImage;
    [SerializeField] private List<Sprite> tipSprite;

    private RectTransform rawImageTransform;
    private Vector3 originalPos;
    private Vector3 originalCapRect;
    private Vector3 originalCheckRect;

    public long questNo;
    [SerializeField] private Canvas cameraCanvas;
    [SerializeField] private Camera cam;
    public Vector2 referenceResolution = new Vector2(1080, 2340);

    [Header("Tutorial")]
    public bool isTutorial = false;
    [SerializeField] private Sprite tutorialImage_notCrop;
    [SerializeField] private Sprite tutorialImage_crop;
    [SerializeField] private Image tutorialImage;
    [SerializeField] private Image tutorialImage_2;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject tutorialObject;
    [SerializeField] private GameObject shutter_Dialog;
    [SerializeField] private GameObject photoUse_Dialog;
    [SerializeField] private List<Button> buttonList;


    private void Start()
    {
        rawImageTransform = webCamRawImage.rectTransform;
        originalPos = rawImageTransform.localPosition;
        originalCapRect = captureRect.localPosition;
        originalCheckRect = checkRect.localPosition;
        animator.enabled = false;
    }


    private IEnumerator OnTip()
    {
        QuestData quest = DataManager.instance.GetQuestInfo();

        if (quest.propNo == 1)
        {
            tipImage.sprite = tipSprite[0];
        }
        else if (quest.propNo == 2)
        {
            tipImage.sprite = tipSprite[1];
        }
        else if (quest.propNo == 3)
        {
            tipImage.sprite = tipSprite[2];
        }
        else if (quest.propNo == 4)
        {
            tipImage.sprite = tipSprite[3];
        }
        else
        {
            tipImage.sprite = tipSprite[4];
        }

        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].interactable = false;
        }

        yield return new WaitForSeconds(0.5f);
        tipObject.SetActive(true);
        tipImage.GetComponent<RectTransform>().DOAnchorPosY(0, 0.38f).SetEase(Ease.OutBack);
    }

    public void OffTip()
    {
        StartCoroutine(OffTipCoroutine());
    }

    private IEnumerator OffTipCoroutine()
    {
        tipImage.GetComponent<RectTransform>().DOAnchorPosY(-2600, 0.38f);
        SettingManager.instance.EffectSound_PopDown();
        yield return new WaitForSeconds(0.5f);
        if (isTutorial == true)
        {
            animator.enabled = true;
        }
        else
        {
            for (int i = 0; i < buttonList.Count; i++)
            {
                buttonList[i].interactable = true;
            }
        }
        tipObject.SetActive(false);
    }

    public void SetWebCam()
    {
        SettingManager.instance.EffectSound_ButtonTouch();
        SettingManager.instance.BackGrorundSound_Change(0, 2);

        tipImage.GetComponent<RectTransform>().DOAnchorPosY(-2600f, 0);
        if (camCanvas.enabled == false)
        {
            camCanvas.enabled = true;
            captureObject.SetActive(true);
            checkObject.SetActive(false);
            StartCoroutine(OnTip());
        }

        #region notUse_Now     
        //webCam.localPosition = originalPos;
        //captureRect.localPosition = originalCapRect;
        //checkRect.localPosition = originalCheckRect;

        //AdjustUIPosition(originalPos, webCam);
        //AdjustUIPosition(captureRect.localPosition, captureRect);
        //AdjustUIPosition(originalCheckRect, checkRect);

        //if (Screen.width > 1440)
        //{
        //    float ratio = Screen.width / 1080;
        //    float newY =  125 * ratio;
        //    float newY2 =  250 * ratio;

        //    Vector3 tmp1 = webCam.localPosition;
        //    Vector3 tmp2 = captureRect.localPosition;
        //    Vector3 tmp3 = checkRect.localPosition;

        //    tmp1.y += newY;
        //    tmp2.y += newY2;
        //    tmp3.y += newY2;

        //    webCam.localPosition = tmp1;
        //    captureRect.localPosition = tmp2;
        //    checkRect.localPosition = tmp3;
        //}
        #endregion
        
        if (isTutorial)
        {
            TutorialStart();
        }
        else
        {
            CreateWebCamTexture();
        }
    }

    public void SwitchCamera()
    {
        SettingManager.instance.EffectSound_ButtonTouch();
        StartCoroutine(DestroyWebCamTextureCoroutine());
        useFrontCamera = !useFrontCamera;
        CreateWebCamTexture();
    }

    private void CreateWebCamTexture(string permissionName = null)
    {
        if (webCamTexture)
        {
            StartCoroutine(DestroyWebCamTextureCoroutine());
        }
        else
        {
            InitializeWebCamTexture();
        }
    }

    private IEnumerator DestroyWebCamTextureCoroutine()
    {
        webCamTexture.Stop();
        Destroy(webCamTexture);

        // 현재 프레임이 끝날 때까지 기다림
        yield return new WaitForEndOfFrame();

        webCamTexture = null;

        // Destroy가 완료된 후 새로운 웹캠 텍스처를 초기화
        InitializeWebCamTexture();
    }

    private void InitializeWebCamTexture()
    {
        WebCamDevice[] webCamDevices = WebCamTexture.devices;
        if (webCamDevices.Length == 0) return;

        int cameraIndex = -1;
        for (int i = 0, l = webCamDevices.Length; i < l; ++i)
        {
            if (webCamDevices[i].isFrontFacing == useFrontCamera)
            {
                cameraIndex = i;
                break;
            }
        }

        if (cameraIndex != -1)
        {
            int requestWidth = Screen.width;
            int requestHeight = Screen.height;

            // 스크린의 해상도와 요청된 비율을 비교하여 적절한 해상도를 선택
            float screenRatio = (float)Screen.width / Screen.height;
            float targetRatio = requestedRatio.x / requestedRatio.y;

            if (Mathf.Abs(screenRatio - targetRatio) > 0.01f)
            {
                if (screenRatio > targetRatio)
                {
                    requestWidth = Mathf.RoundToInt(Screen.height * targetRatio);
                    requestHeight = Screen.height;
                }
                else
                {
                    requestWidth = Screen.width;
                    requestHeight = Mathf.RoundToInt(Screen.width / targetRatio);
                }
            }

            webCamTexture = new WebCamTexture(webCamDevices[cameraIndex].name, requestWidth, requestHeight, requestedFPS);
            webCamTexture.filterMode = FilterMode.Trilinear;
            webCamTexture.Play();

            webCamRawImage.texture = webCamTexture;
            captureObject.SetActive(true);
            checkObject.SetActive(false);
        }
    }

    private string GetAspectRatio(int width, int height, bool allowPrtrait = false)
    {
        if (!allowPrtrait && width < height)
        {
            Swap(ref width, ref height);
        }
        float r = (float)width / height;
        return r.ToString("F2");
    }

    private void Swap<T>(ref T a, ref T b)
    {
        T tmp = a;
        a = b;
        b = tmp;
    }

    private void Update()
    {
        UpdateWebCamRawImage();
    }

    private void UpdateWebCamRawImage()
    {
        if (!webCamTexture) return;

        int videoRotAngle = webCamTexture.videoRotationAngle;

        webCamRawImage.transform.localEulerAngles = new Vector3(0, 0, -videoRotAngle);


        int width, height;
        if (Screen.orientation == ScreenOrientation.Portrait ||
            Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            width = Screen.width;
            height = Screen.width * webCamTexture.width / webCamTexture.height;
        }
        else
        {
            height = Screen.height;
            width = Screen.width * webCamTexture.width / webCamTexture.height;
        }

        if (Mathf.Abs(videoRotAngle) % 180 != 0f)
        {
            Swap(ref width, ref height);
        }
        webCamRawImage.rectTransform.sizeDelta = new Vector2(width, height);
    }

    public void CapturePhoto()
    {
        SettingManager.instance.EffectSound_Camera();

        if (isTutorial)
        {
            captureObject.SetActive(false);
            checkObject.SetActive(true);
            StartCoroutine(nameof(Shutter));
        }
        else
        {
            StartCoroutine(CapturePhotoCoroutine());
        }
    }

    private IEnumerator CapturePhotoCoroutine()
    {
        yield return new WaitForEndOfFrame(); 

        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        Texture2D rotatedPhoto;
        if (useFrontCamera)
        {
            rotatedPhoto = RotateTexture(photo, true);
        }
        else
        {
            rotatedPhoto = RotateTexture(photo, false);
        }
        webCamRawImage.texture = rotatedPhoto;

        webCamTexture.Stop();
        captureObject.SetActive(false);
        checkObject.SetActive(true);
    }

    private Texture2D RotateTexture(Texture2D originalTexture, bool clockwise)
    {
        Texture2D rotatedTexture = new Texture2D(originalTexture.height, originalTexture.width);
        Color32[] originalPixels = originalTexture.GetPixels32();
        Color32[] rotatedPixels = new Color32[originalPixels.Length];

        int w = originalTexture.width;
        int h = originalTexture.height;

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                if (clockwise)
                {
                    rotatedPixels[j + i * h] = originalPixels[(h - j - 1) * w + i];
                }
                else
                {
                    rotatedPixels[j + i * h] = originalPixels[j * w + (w - i - 1)];
                }
            }
        }

        rotatedTexture.SetPixels32(rotatedPixels);
        rotatedTexture.Apply();

        Destroy(originalTexture);

        return rotatedTexture;
    }



    public void RotatePhoto()
    {
        SettingManager.instance.EffectSound_ButtonTouch();

        Texture2D currentPhoto = webCamRawImage.texture as Texture2D;

        if (currentPhoto != null)
        {
            Texture2D rotatedPhoto = RotateTexture(currentPhoto, true); // 시계 방향 90도 회전
            webCamRawImage.texture = rotatedPhoto;
        }
    }

    public void UploadImage()
    {
        SettingManager.instance.EffectSound_ButtonTouch();

        if (isTutorial)
        {
            // Sprite sprite = tutorialImage_crop;
            // Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            // Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
            //                                              (int)sprite.textureRect.y,
            //                                              (int)sprite.textureRect.width,
            //                                              (int)sprite.textureRect.height);
            // newText.SetPixels(newColors);
            // newText.Apply();
            //KJY_ConnectionTMP.instance.OnClickTest(newText);

            KJY_ConnectionTMP.instance.questNoPicture = 3;
            TutorialFinish();
            TutorialUI.Instance.OnNonTouch();
        }
        else
        {
            int width = webCamRawImage.texture.width;
            int height = webCamRawImage.texture.height;

            RenderTexture currentRenderTexture = RenderTexture.active;
            RenderTexture copiedRenderTexture = new RenderTexture(width, height, 0);

            Graphics.Blit(webCamRawImage.texture, copiedRenderTexture);

            RenderTexture.active = copiedRenderTexture;


            Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGB24, false);

            texture2D.ReadPixels(new UnityEngine.Rect(0, 0, width, height), 0, 0);
            texture2D.Apply();

            RenderTexture.active = currentRenderTexture;

            KJY_ConnectionTMP.instance.OnClickTest(texture2D);
            TutorialUI.Instance.OnNonTouch();
        }
    }

    public void CameraOff()
    {
        SettingManager.instance.BackGrorundSound_Change(0.4f, 2);

        if (webCamTexture != null)
        {
            webCamTexture.Stop(); 
            Destroy(webCamTexture);
            webCamTexture = null;
        }

        cameraCanvas.enabled = false;
    }

    public void CameraResolution()
    {
        Rect viewport = cam.rect;

        float screenAspectRatio = (float)(Screen.width / Screen.height);
        float targetAspectRation = 9f / 19.5f;

        if (screenAspectRatio < targetAspectRation)
        {
            viewport.height = screenAspectRatio / targetAspectRation;
            viewport.y = (1f - viewport.height) / 2f;
        }
        else
        {
            viewport.width = targetAspectRation / screenAspectRatio;
            viewport.x = (1f - viewport.width) / 2f;
        }

        cam.rect = viewport;
    }

    #region Tutoral
    public void TutorialStart()
    {
        TutorialUI.Instance.EndTutorial1();
        
        animator.Rebind();
        tutorialImage_2.enabled = false;
        tutorialImage.enabled = true;

        tutorialObject.SetActive(true);
    }

    public void TutorialShutter()
    {
        shutter_Dialog.SetActive(true);
        buttonList[2].interactable = true;
    }

    private IEnumerator Shutter()
    {
        tutorialImage.enabled = false;
        shutter_Dialog.SetActive(false);
        tutorialImage_2.enabled = true;
        yield return tutorialImage_2.DOFade(0, 0.5f);
        //tutorialImage.rectTransform.sizeDelta = new Vector2(1081, 1081);
        //tutorialImage.sprite = tutorialImage_crop;
        yield return tutorialImage_2.DOFade(1, 1f);
        yield return new WaitForSeconds(1);
        photoUse_Dialog.SetActive(true);
    }

    public void TutorialFinish()
    {
        tutorialObject.SetActive(false);
        animator.enabled = false;
        photoUse_Dialog.SetActive(false);
        CameraOff();
        ButtonActions.Instance.StartCoroutine(nameof(ButtonActions.Instance.QuestDone));

        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].interactable = true;
        }
    }
    #endregion
    //private IEnumerator Tutorial()
    //{
    //    float shakeDuration = 1.0f;
    //    float shakeAmount = 1.0f;
    //    float decreaseFactor = 1.0f;
    //    float currentShakeDuration = 0f;
    //    webCamRawImage.texture = Resources.Load<Texture>("testpicture");

    //    while (tutorial)
    //    {
    //        if (currentShakeDuration > 0)
    //        {
    //            rawImageTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

    //            currentShakeDuration -= Time.deltaTime * decreaseFactor;
    //        }
    //        else
    //        {
    //            currentShakeDuration = 0f;
    //            rawImageTransform.localPosition = originalPos;
    //        }

    //        yield return new WaitForSeconds(shakeDuration);

    //        currentShakeDuration = shakeDuration;
    //    }
    //}
}
