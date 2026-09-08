using UnityEngine;
using UnityEngine.Events;

public class DialoguePlayer : MonoBehaviour
{
    [Header("پنل‌های دیالوگ به ترتیب نمایش")]
    [SerializeField] private GameObject[] dialoguePanels;

    [Header("دنبال‌کردن کاراکتر")]
    [Tooltip("کاراکتری که پنل باید بالای سرش نشون داده بشه. می‌تونه توت باشه یا هر کاراکتر دیگه‌ای.")]
    [SerializeField] private Transform followTarget;
    [Tooltip("همون Canvasی که پنل‌های دیالوگ زیرش قرار دارن.")]
    [SerializeField] private Canvas targetCanvas;
    [Tooltip("چقدر بالاتر از Transform کاراکتر (واحد دنیای بازی)، مثلاً بالای سرش.")]
    [SerializeField] private float heightAboveHead = 2f;
    [Tooltip("دوربینی که صحنه رو رندر می‌کنه. اگه خالی بذاری، خودش Camera.main رو استفاده می‌کنه.")]
    [SerializeField] private Camera worldCamera;

    [Header("فعال‌سازی با برخورد پلیر (اختیاری)")]
    [SerializeField] private bool startOnPlayerTrigger = false;
    [SerializeField] private string playerTag = "Player";

    [Header("تنظیمات")]
    [SerializeField] private bool onlyPlayOnce = true;

    [Header("پنل‌هایی که خودکار (بدون کلید) به بعدی می‌رن")]
    [SerializeField] private int[] autoAdvanceIndexes;
    [SerializeField] private float autoAdvanceDelay = 2f;

    [Header("رویداد پایان دیالوگ (اختیاری)")]
    [SerializeField] private UnityEvent onFinished;

    private int index = 0;
    private bool isPlaying = false;
    private bool hasPlayed = false;

    // اسکریپت‌های دیگه (مثل TutorialSequence) با این می‌فهمن دیالوگ هنوز داره پخش می‌شه یا تموم شده
    public bool IsPlaying => isPlaying;

    private void Start()
    {
        for (int i = 0; i < dialoguePanels.Length; i++)
        {
            if (dialoguePanels[i] != null)
                dialoguePanels[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (!isPlaying)
            return;

        // هر فریم موقعیت پنل فعلی رو آپدیت کن تا اگه کاراکتر یا
        // دوربین حرکت کرد، پنل درست بالای سرش بمونه
        UpdateFollowPosition(dialoguePanels[index]);

        if (IsAutoAdvance(index))
            return; // این پنل با تایمر جلو میره، نه با کلید

        if (AnyKeyboardKeyDown())
        {
            Next();
        }
    }

    // بررسی می‌کند که آیا هر دکمه‌ای از کیبورد (بدون ماوس و جوی‌استیک) فشرده شده یا نه
    private bool AnyKeyboardKeyDown()
    {
        foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (kcode >= KeyCode.Mouse0 && kcode <= KeyCode.Mouse6)
                continue; // دکمه‌های ماوس رد شوند

            if (kcode >= KeyCode.JoystickButton0)
                continue; // دکمه‌های دسته رد شوند

            if (Input.GetKeyDown(kcode))
                return true;
        }
        return false;
    }

    // بررسی می‌کند که آیا ایندکس داده‌شده جزو پنل‌های خودکار هست یا نه
    private bool IsAutoAdvance(int i)
    {
        foreach (int idx in autoAdvanceIndexes)
            if (idx == i) return true;
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!startOnPlayerTrigger)
            return;

        if (!other.CompareTag(playerTag))
            return;

        Play();
    }

    public void Play()
    {
        if (dialoguePanels == null || dialoguePanels.Length == 0)
        {
            Debug.LogError("[DialoguePlayer] هیچ پنلی توی Dialogue Panels قرار داده نشده!");
            return;
        }

        if (onlyPlayOnce && hasPlayed)
            return;

        hasPlayed = true;
        isPlaying = true;
        index = 0;

        Show(index);
    }

    private void Next()
    {
        Hide(index);
        index++;

        if (index < dialoguePanels.Length)
        {
            Show(index);
        }
        else
        {
            Finish();
        }
    }

    private void Show(int i)
    {
        GameObject panel = dialoguePanels[i];

        if (panel == null)
        {
            Debug.LogWarning("[DialoguePlayer] Element " + i + " توی Dialogue Panels خالیه (None).");
            return;
        }

        UpdateFollowPosition(panel);
        panel.SetActive(true);

        if (IsAutoAdvance(i))
        {
            StartCoroutine(AutoAdvanceCoroutine());
        }
    }

    // بعد از autoAdvanceDelay ثانیه، خودش به دیالوگ بعدی می‌ره
    private System.Collections.IEnumerator AutoAdvanceCoroutine()
    {
        yield return new WaitForSeconds(autoAdvanceDelay);
        Next();
    }

    // موقعیت دنیای بازیِ کاراکتر رو به مختصات محلیِ Canvas تبدیل
    // می‌کنه و روی RectTransform پنل می‌ذاره
    private void UpdateFollowPosition(GameObject panel)
    {
        if (panel == null || followTarget == null || targetCanvas == null)
            return;

        RectTransform panelRect = panel.GetComponent<RectTransform>();
        if (panelRect == null)
            return;

        Camera cam = worldCamera != null ? worldCamera : Camera.main;
        if (cam == null)
            return;

        Vector3 worldPos = followTarget.position + Vector3.up * heightAboveHead;
        Vector2 screenPoint = cam.WorldToScreenPoint(worldPos);

        RectTransform canvasRect = targetCanvas.GetComponent<RectTransform>();
        Camera uiCamera = targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : targetCanvas.worldCamera;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, uiCamera, out Vector2 localPoint))
        {
            panelRect.anchoredPosition = localPoint;
        }
    }

    private void Hide(int i)
    {
        GameObject panel = dialoguePanels[i];
        if (panel != null)
            panel.SetActive(false);
    }

    private void Finish()
    {
        isPlaying = false;
        Debug.Log("[DialoguePlayer] دیالوگ تمام شد.");
        onFinished?.Invoke();
    }
}