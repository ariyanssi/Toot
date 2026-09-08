using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeedX = 5f;
    public float smoothSpeedY = 2f;       // آروم‌تر از X
    public float yDeadZone = 1.5f;        // تا این مقدار پرش، دوربین اصلاً تکون نمی‌خوره

    private Vector3 offset;
    private float fixedY;

    void Start()
    {
        offset = transform.position - player.position;
        fixedY = transform.position.y;
    }

    void LateUpdate()
    {
        // X: نرم دنبال پلیر می‌ره
        float targetX = player.position.x + offset.x;
        float newX = Mathf.Lerp(transform.position.x, targetX, smoothSpeedX * Time.deltaTime);

        // Y: فقط اگه پلیر از یه محدوده مشخص خارج شد، دوربین تکون می‌خوره
        float targetY = fixedY;
        if (Mathf.Abs(player.position.y - fixedY) > yDeadZone)
        {
            targetY = player.position.y + offset.y;
        }
        float newY = Mathf.Lerp(transform.position.y, targetY, smoothSpeedY * Time.deltaTime);
        fixedY = newY;

        transform.position = new Vector3(newX, newY, transform.position.z);
    }
}