using UnityEngine;

public class SequenceManager : MonoBehaviour
{
    [Header("اسکریپت‌ها به ترتیب اجرا")]
    [Tooltip("اسکریپت‌های روی همین آبجکت را به ترتیب اینجا قرار دهید")]
    [SerializeField] private MonoBehaviour[] sequences;

    private int currentIndex = 0;

    private void Start()
    {
        // در شروع، همه اسکریپت‌ها به جز اولی را غیرفعال کن
        for (int i = 0; i < sequences.Length; i++)
        {
            if (sequences[i] != null)
            {
                sequences[i].enabled = (i == 0);
            }
        }
    }

    /// <summary>
    /// هر اسکریپت وقتی کارش تمام شد، این متد را صدا می‌زند
    /// </summary>
    public void NextSequence()
    {
        // غیرفعال کردن اسکریپت فعلی
        if (currentIndex < sequences.Length && sequences[currentIndex] != null)
        {
            sequences[currentIndex].enabled = false;
        }

        currentIndex++;

        // فعال کردن اسکریپت بعدی
        if (currentIndex < sequences.Length && sequences[currentIndex] != null)
        {
            sequences[currentIndex].enabled = true;
            Debug.Log("[SequenceManager] اسکریپت فعال شد: " + sequences[currentIndex].GetType().Name);
        }
        else
        {
            Debug.Log("[SequenceManager] تمام توالی‌ها به پایان رسیدند.");
        }
    }
}
