using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance;

    public Transform player;
    public float smoothSpeedX = 5f;
    public float smoothSpeedY = 2f;
    public float yDeadZone = 1.5f;

    private Vector3 offset;
    private float fixedY;

    // --- حالت zone ---
    private bool inZoneMode = false;
    private Transform zoneAnchor;
    private float zoneSpeed = 3f;
    private Vector3 zoneOffset;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        offset = transform.position - player.position;
        fixedY = transform.position.y;
    }

    void LateUpdate()
    {
        if (inZoneMode && zoneAnchor != null)
        {
            // دوربین با همون فریم‌بندی anchor حرکت می‌کنه، ولی همچنان دنبال پلیر می‌ره
            Vector3 targetPos = player.position + zoneOffset;
            transform.position = Vector3.Lerp(transform.position, targetPos, zoneSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, zoneAnchor.rotation, zoneSpeed * Time.deltaTime);
            return;
        }

        // --- حالت عادی دنبال‌کردن پلیر ---
        float targetX = player.position.x + offset.x;
        float newX = Mathf.Lerp(transform.position.x, targetX, smoothSpeedX * Time.deltaTime);

        float targetY = fixedY;
        if (Mathf.Abs(player.position.y - fixedY) > yDeadZone)
        {
            targetY = player.position.y + offset.y;
        }
        float newY = Mathf.Lerp(transform.position.y, targetY, smoothSpeedY * Time.deltaTime);
        fixedY = newY;

        transform.position = new Vector3(newX, newY, transform.position.z);
    }

    public void EnterZone(Transform anchor, float speed)
    {
        inZoneMode = true;
        zoneAnchor = anchor;
        zoneSpeed = speed;
        // فاصله‌ی anchor نسبت به پلیر در لحظه‌ی ورود رو ذخیره می‌کنه
        // تا فریم‌بندی حفظ بشه ولی دوربین همچنان با پلیر حرکت کنه
        zoneOffset = anchor.position - player.position;
    }

    public void ExitZone()
    {
        inZoneMode = false;
        // دنبال‌کردن عادی رو با موقعیت فعلی دوربین ریست می‌کنه تا پرش نداشته باشیم
        offset = transform.position - player.position;
        fixedY = transform.position.y;
    }
}