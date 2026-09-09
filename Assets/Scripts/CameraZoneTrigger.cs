using UnityEngine;

public class CameraZoneTrigger : MonoBehaviour
{
    public Transform cameraAnchor;   // یه گیم‌آبجکت خالی که موقعیت + زاویه‌ی دلخواه دوربین رو نشون میده
    public float transitionSpeed = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraFollow.Instance.EnterZone(cameraAnchor, transitionSpeed);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraFollow.Instance.ExitZone();
        }
    }
}