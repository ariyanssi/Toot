using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Buttons (Drag and Drop Here)")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    [Header("Scene Settings")]
    [SerializeField] private string mainGameSceneName = "GameScene";

    // کلید ذخیره‌سازی موقتی برای دکمه‌ی «ادامه» — تا وقتی سیستم Save واقعی ساخته بشه
    private const string LastSceneKey = "LastScene";

    // بخش کانواس تنظیمات (هم هدر و هم متغیر فعلاً کامنت شدند تا ارور ندهد)
    // [Header("Panels")]
    // [SerializeField] private GameObject settingsCanvas;

    private void Awake()
    {
        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);

        if (startButton != null)
            startButton.onClick.AddListener(OnStartClicked);

        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettingsClicked);

        if (exitButton != null)
            exitButton.onClick.AddListener(OnExitClicked);
    }

    private void OnDestroy()
    {
        if (continueButton != null)
            continueButton.onClick.RemoveListener(OnContinueClicked);

        if (startButton != null)
            startButton.onClick.RemoveListener(OnStartClicked);

        if (settingsButton != null)
            settingsButton.onClick.RemoveListener(OnSettingsClicked);

        if (exitButton != null)
            exitButton.onClick.RemoveListener(OnExitClicked);
    }

    private void OnContinueClicked()
    {
        // فعلاً تا سیستم Save واقعی ساخته بشه، از PlayerPrefs استفاده می‌کنیم
        if (PlayerPrefs.HasKey(LastSceneKey))
        {
            string savedScene = PlayerPrefs.GetString(LastSceneKey);
            SceneManager.LoadScene(savedScene);
        }
        else
        {
            // اگر هیچ پیشرفتی ذخیره نشده، فعلاً مثل «شروع» عمل می‌کنیم
            Debug.Log("پیشرفت ذخیره‌شده‌ای پیدا نشد؛ بازی از ابتدا شروع می‌شود.");
            SceneManager.LoadScene(mainGameSceneName);
        }
    }

    private void OnStartClicked()
    {
        // شروع بازی جدید یعنی هر ذخیره‌ی قبلی پاک شود
        PlayerPrefs.DeleteKey(LastSceneKey);
        SceneManager.LoadScene(mainGameSceneName);
    }

    private void OnSettingsClicked()
    {
        // هنوز صحنه یا پنل تنظیمات ساخته نشده — این خط فقط برای یادآوری است
        Debug.Log("دکمه‌ی تنظیمات فعلاً غیرفعال است — صحنه یا پنل تنظیمات هنوز ساخته نشده.");

        // if (settingsCanvas != null)
        // {
        //     settingsCanvas.SetActive(true);
        // }
    }

    private void OnExitClicked()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // این تابع را از جایی که پیشرفت بازیکن ذخیره می‌شود صدا بزنید
    // (مثلاً وقتی وارد یک checkpoint جدید می‌شود)
    public static void SaveCurrentScene(string sceneName)
    {
        PlayerPrefs.SetString(LastSceneKey, sceneName);
        PlayerPrefs.Save();
    }
}