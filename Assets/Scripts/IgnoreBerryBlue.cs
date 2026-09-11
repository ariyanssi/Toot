using UnityEngine;

public class IgnoreBerryBlue : MonoBehaviour
{
    private Collider2D myCollider;
    private Collider2D playerCollider;

    void Start()
    {
        myCollider = GetComponent<Collider2D>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerCollider = player.GetComponent<Collider2D>();

        Debug.Log("Player found: " + (player != null) + " | playerCollider: " + (playerCollider != null));
    }

    void FixedUpdate()
    {
        if (playerCollider == null || myCollider == null) return;

        GameObject blueSkin = GameObject.FindGameObjectWithTag("BerryBlue");
        bool isBlueActive = (blueSkin != null && blueSkin.activeSelf);

        Physics2D.IgnoreCollision(myCollider, playerCollider, isBlueActive);

        Debug.Log("isBlueActive: " + isBlueActive + " | blueSkin found: " + (blueSkin != null));
    }
}