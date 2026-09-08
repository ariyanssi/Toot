using UnityEngine;

// این اسکریپت مخصوص لایه‌ی آسمان است: آسمان را دقیقاً هم‌سرعت با دوربین
// نگه می‌دارد، بدون هیچ‌گونه افکت پارالاکس یا تأخیر.
[DefaultExecutionOrder(100)]
public class SkyFollow : MonoBehaviour
{
    [Tooltip("اگر خالی بماند، به‌صورت خودکار دوربین اصلی صحنه پیدا می‌شود")]
    public Transform cameraTransform;

    private Vector3 offset;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        // فاصله‌ی اولیه‌ی آسمان نسبت به دوربین را ذخیره می‌کنیم
        offset = transform.position - cameraTransform.position;
    }

    void LateUpdate()
    {
        transform.position = cameraTransform.position + offset;
    }
}