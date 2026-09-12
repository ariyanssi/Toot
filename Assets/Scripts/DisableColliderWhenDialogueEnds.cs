using UnityEngine;

public class DisableColliderWhenDialogueEnds : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject colliderObject;

    private bool wasDialogueActive = false;

    void Update()
    {
        bool isDialogueActive = dialoguePanel.activeSelf;

        // وقتی دیالوگ از فعال به غیرفعال تغییر کرد
        if (wasDialogueActive && !isDialogueActive)
        {
            colliderObject.SetActive(false);
        }

        wasDialogueActive = isDialogueActive;
    }
}