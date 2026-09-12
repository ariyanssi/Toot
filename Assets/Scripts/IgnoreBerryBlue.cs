using UnityEngine;

public class IgnoreBerryBlue : MonoBehaviour
{
    [SerializeField] private Collider2D playerCollider;
    private Collider2D myCollider;

    void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        if (playerCollider == null || myCollider == null) return;

        GameObject blueSkin = GameObject.FindGameObjectWithTag("berryblue");
        bool isBlueActive = (blueSkin != null && blueSkin.activeSelf);

        Physics2D.IgnoreCollision(myCollider, playerCollider, isBlueActive);
    }
}