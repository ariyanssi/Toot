using UnityEngine;
using System.Collections;

public class TootMover : MonoBehaviour
{
    [Tooltip("کامپوننت Animator خودِ کاراکتر Toot")]
    [SerializeField] private Animator anim;

    public IEnumerator MoveTo(Vector3 destination, float speed)
    {
        if (anim != null)
            anim.SetBool("isWalking", true);
    
    
        // چرخوندن کاراکتر بر اساس جهت حرکت (چپ/راست)
        bool movingRight = destination.x > transform.position.x;
        Vector3 scale = transform.localScale;
        scale.x = movingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;

        while (Vector3.Distance(transform.position, destination) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = destination;

        if (anim != null)
            anim.SetBool("isWalking", false);
    }
}