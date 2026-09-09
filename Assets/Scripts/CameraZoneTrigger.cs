using UnityEngine;

/// <summary>
/// Put this on a trigger collider (Is Trigger = true) in the scene.
/// When the player enters, the camera smoothly moves to follow using
/// cameraAnchor's framing/rotation. When the player exits, the camera
/// smoothly returns to normal follow.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class CameraZoneTrigger : MonoBehaviour
{
    [Tooltip("An empty GameObject placed in the scene marking the desired camera position/angle for this zone. Its Z position is ignored (camera Z is always locked to the camera's own Z).")]
    public Transform cameraAnchor;

    public float transitionSpeed = 3f;

    void Reset()
    {
        // Helpful reminder in the Inspector/console if someone forgets to set this up.
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (cameraAnchor == null)
        {
            Debug.LogWarning($"CameraZoneTrigger on '{gameObject.name}' has no cameraAnchor assigned.", this);
            return;
        }

        if (CameraFollow.Instance == null)
        {
            Debug.LogWarning("CameraZoneTrigger: no CameraFollow.Instance found in the scene.", this);
            return;
        }

        CameraFollow.Instance.EnterZone(cameraAnchor, transitionSpeed);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (CameraFollow.Instance == null) return;

        CameraFollow.Instance.ExitZone();
    }
}