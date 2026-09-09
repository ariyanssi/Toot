using UnityEngine;

/// <summary>
/// Controls the main camera: normal side-scrolling follow of the player,
/// plus an optional "zone mode" that smoothly moves the camera to a fixed
/// anchor (with its own rotation) while the player is inside a trigger zone.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance;

    [Header("References")]
    public Transform player;

    [Header("Normal Follow Settings")]
    public float smoothSpeedX = 5f;
    public float smoothSpeedY = 2f;
    public float yDeadZone = 1.5f;

    // Offset between camera and player on X/Y (Z is handled separately, see cameraZ).
    private Vector3 offset;
    private float fixedY;

    // The camera's own Z position. Locked once at Start and NEVER changed,
    // no matter what any zone anchor's Z is. This is what prevents the
    // "scene breaks / camera goes inside everything" bug.
    private float cameraZ;

    // The camera's rotation before ever entering a zone (normally identity).
    // After exiting a zone we lerp back to this instead of staying tilted.
    private Quaternion defaultRotation;

    [Header("Zone Exit Settings")]
    [Tooltip("How fast rotation eases back to normal after leaving a zone.")]
    public float rotationReturnSpeed = 4f;

    // --- Zone mode state ---
    private bool inZoneMode = false;
    private Transform zoneAnchor;
    private float zoneSpeed = 3f;
    private Vector3 zoneOffset; // anchor position minus player position, captured on entry

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("CameraFollow: 'player' is not assigned in the Inspector.");
            return;
        }

        cameraZ = transform.position.z; // lock camera's Z once, forever
        offset = transform.position - player.position;
        offset.z = 0f; // never let Z leak into the X/Y offset math
        fixedY = transform.position.y;
        defaultRotation = transform.rotation; // usually Quaternion.identity
    }

    void LateUpdate()
    {
        if (player == null) return;

        if (inZoneMode && zoneAnchor != null)
        {
            FollowInZone();
            return;
        }

        FollowNormally();

        // Ease rotation back to how it was before ever entering a zone.
        // Harmless no-op once it arrives (Lerp toward a rotation you're
        // already at doesn't change anything), so it's safe to run always.
        if (transform.rotation != defaultRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, defaultRotation, rotationReturnSpeed * Time.deltaTime);
        }
    }

    private void FollowNormally()
    {
        float targetX = player.position.x + offset.x;
        float newX = Mathf.Lerp(transform.position.x, targetX, smoothSpeedX * Time.deltaTime);

        float targetY = fixedY;
        if (Mathf.Abs(player.position.y - fixedY) > yDeadZone)
        {
            targetY = player.position.y + offset.y;
        }
        float newY = Mathf.Lerp(transform.position.y, targetY, smoothSpeedY * Time.deltaTime);
        fixedY = newY;

        transform.position = new Vector3(newX, newY, cameraZ);
    }

    private void FollowInZone()
    {
        // Camera keeps the same offset from the player that it had the moment
        // it entered the zone, so it still tracks player movement instead of
        // freezing on a single world-space point.
        Vector3 targetPos = player.position + zoneOffset;
        targetPos.z = cameraZ; // Z is ALWAYS locked, regardless of anchor's Z

        transform.position = Vector3.Lerp(transform.position, targetPos, zoneSpeed * Time.deltaTime);

        // Only take the anchor's rotation around Z (2D games shouldn't tilt on X/Y).
        Quaternion targetRot = Quaternion.Euler(0f, 0f, zoneAnchor.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, zoneSpeed * Time.deltaTime);
    }

    /// <summary>Called by CameraZoneTrigger when the player enters a zone.</summary>
    public void EnterZone(Transform anchor, float speed)
    {
        if (anchor == null)
        {
            Debug.LogWarning("CameraFollow.EnterZone called with a null anchor.");
            return;
        }

        inZoneMode = true;
        zoneAnchor = anchor;
        zoneSpeed = speed;

        Vector3 rawOffset = anchor.position - player.position;
        rawOffset.z = 0f; // Z is never taken from the anchor
        zoneOffset = rawOffset;
    }

    /// <summary>Called by CameraZoneTrigger when the player exits a zone.</summary>
    public void ExitZone()
    {
        inZoneMode = false;
        zoneAnchor = null;

        // Deliberately DON'T touch offset/fixedY here. They still hold the
        // exact values from before the player entered the zone, so
        // FollowNormally() will smoothly Lerp the camera back to precisely
        // the position/framing it had pre-zone, rather than resuming from
        // wherever it happened to be inside the zone.
    }
}