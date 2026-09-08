using System.Collections;
using UnityEngine;
public class TutorialSequence : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        [Tooltip("نقطه‌ای که توت باید تا اونجا بره")]
        public Transform stopPoint;

        [Tooltip("دیالوگی که بعد از رسیدن به این نقطه پخش می‌شه (اختیاری، می‌تونی خالی بذاری)")]
        public DialoguePlayer dialoguePlayer;
    }

    [Header("Toot")]
    [Tooltip("اسکریپت TootMover که روی کاراکتر Toot قرار داره")]
    [SerializeField] private TootMover tootMover;
    [SerializeField] private float moveSpeed = 3f;

    [Header("مراحل (به ترتیب اجرا)")]
    [SerializeField] private TutorialStep[] steps;

    [Header("تنظیمات")]
    [SerializeField] private bool startAutomatically = true;

    private bool started = false;

    private void Start()
    {
        if (startAutomatically)
            StartCoroutine(Run());
    }

    public IEnumerator Run()
    {
        if (started)
            yield break;

        started = true;

        for (int i = 0; i < steps.Length; i++)
        {
            TutorialStep step = steps[i];

            if (step == null)
                continue;

            // اول حرکت به نقطه‌ی این مرحله
            if (tootMover != null && step.stopPoint != null)
            {
                yield return StartCoroutine(tootMover.MoveTo(step.stopPoint.position, moveSpeed));
            }
            else
            {
                Debug.LogWarning("[TutorialSequence] مرحله " + i + ": Toot Mover یا Stop Point خالیه.");
            }

            // بعد اگه دیالوگی برای این مرحله تعریف شده، پخشش کن و صبر کن تموم بشه
            if (step.dialoguePlayer != null)
            {
                step.dialoguePlayer.Play();
                yield return new WaitUntil(() => !step.dialoguePlayer.IsPlaying);
            }
        }
    }
}