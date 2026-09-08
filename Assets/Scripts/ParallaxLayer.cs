using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("Parallax Settings")]

    [Tooltip("0 = ثابت (آسمان دوردست) | 1 = هم‌سرعت با دوربین (پیش‌زمینه)")]
    [Range(0f, 1f)]
    public float parallaxFactorX = 0.5f;

    [Space(4)]
    [Tooltip("اگه لایه باید به صورت عمودی هم پارالاکس داشته باشه فعال کن")]
    public bool useVerticalParallax = false;

    [Tooltip("0 = ثابت عمودی | 1 = هم‌سرعت عمودی با دوربین")]
    [Range(0f, 1f)]
    public float parallaxFactorY = 0.2f;

    private Transform cam;
    private Vector2 lastCamPos;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.position;
    }

    void LateUpdate()
    {
        float deltaX = cam.position.x - lastCamPos.x;
        float deltaY = cam.position.y - lastCamPos.y;

        float moveX = deltaX * parallaxFactorX;
        float moveY = useVerticalParallax ? deltaY * parallaxFactorY : 0f;

        transform.position += new Vector3(moveX, moveY, 0f);

        lastCamPos = cam.position;
    }
}
