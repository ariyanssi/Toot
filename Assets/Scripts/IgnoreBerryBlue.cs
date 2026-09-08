using UnityEngine;

public class IgnoreBerryBlue : MonoBehaviour
{
    private Collider2D myCollider;

    void Start()
    {
        myCollider = GetComponent<Collider2D>();

        // 
        GameObject[] berries = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject berry in berries)
        {
            Collider2D col = berry.GetComponent<Collider2D>();
            if (col != null)
                Physics2D.IgnoreCollision(myCollider, col);
        }
    }

    //
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("berryblue"))
            Physics2D.IgnoreCollision(myCollider, collision.collider);
    }
}
