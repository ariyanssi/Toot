using System;
using UnityEngine;

public class TriggerDialogueActivator : MonoBehaviour
{
    [Header("UI Canvas Dialogue")]
    [Tooltip("دقیقاً خود آبجکت پنل دیالوگ (نه کل Canvas)")]
    [SerializeField] private GameObject dialogueUI;

    [Header("Follow Settings")]
    [Tooltip("کاراکتری که پنل باید بالای سرش نشون داده بشه")]
    [SerializeField] private Transform followTarget;
    [Tooltip("همون Canvasی که پنل دیالوگ زیرمجموعه آن است")]
    [SerializeField] private Canvas targetCanvas;
    [Tooltip("دوربینی که صحنه را رندر می‌کند (خالی باشد خودکار دوربین اصلی را می‌گیرد)")]
    [SerializeField] private Camera worldCamera;
    [Tooltip("فاصله به سمت بالا از کاراکتر")]
    [SerializeField] private float heightAboveHead = 2f;

    [Header("Trigger Settings")]
    [SerializeField] private Transform triggerPoint;
    [SerializeField] private float triggerDistance = 2f;
    [SerializeField] private Transform player;

    [Header("Options")]
    [SerializeField] private bool onlyPlayOnce = true;

    private bool isPlaying = false;
    private bool hasPlayed = false;

    private void Start()
    {
        // در شروع بازی دیالوگ غیرفعال باشد
        if (dialogueUI != null)
        {
            dialogueUI.SetActive(false);
        }

        // اگر پلیر دستی داده نشده بود، با تگ پیدا کند
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    private void Update()
    {
        // بخش ۱: اگر هنوز فعال نشده، بررسی فاصله تا تریگر
        if (!isPlaying)
        {
            if (onlyPlayOnce && hasPlayed) return;
            if (player == null || triggerPoint == null || dialogueUI == null) return;

            float distance = Vector3.Distance(player.position, triggerPoint.position);

            if (distance <= triggerDistance)
            {
                Show();
            }
            return;
        }

        // بخش ۲: وقتی پاپ‌آپ باز است، با زدن هر دکمه‌ای از کیبورد بسته شود
        if (isPlaying && AnyKeyboardKeyDown())
        {
            Hide();
        }
    }

    private void Show()
    {
        isPlaying = true;
        hasPlayed = true;
        dialogueUI.SetActive(true);
        UpdateFollowPosition(dialogueUI);
    }

    private void Hide()
    {
        isPlaying = false;
        if (dialogueUI != null)
        {
            dialogueUI.SetActive(false);
        }
    }

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

    // استفاده از LateUpdate برای هماهنگی با حرکت کاراکتر/دوربین و جلوگیری از لرزش پنل
    private void LateUpdate()
    {
        if (isPlaying && dialogueUI != null && dialogueUI.activeSelf)
        {
            UpdateFollowPosition(dialogueUI);
        }
    }

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

        // مختصات بالای سر کاراکتر در دنیای سه‌بعدی
        Vector3 worldPos = followTarget.position + Vector3.up * heightAboveHead;

        // تبدیل به مختصات صفحه نمایش
        Vector2 screenPoint = cam.WorldToScreenPoint(worldPos);

        RectTransform canvasRect = targetCanvas.GetComponent<RectTransform>();
        Camera uiCamera = targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : targetCanvas.worldCamera;

        // تبدیل به مختصات محلی درون Canvas
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, uiCamera, out Vector2 localPoint))
        {
            panelRect.anchoredPosition = localPoint;
        }
    }
}
