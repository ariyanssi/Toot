using UnityEngine;

public class ColorChangeItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerSkinManager skinManager = other.GetComponent<PlayerSkinManager>();
            if (skinManager != null)
            {
                skinManager.ChangeToWhiteSkin();
            }

            Destroy(gameObject); // آیتم بعد از برخورد حذف بشه (اختیاری)
        }
    }
}