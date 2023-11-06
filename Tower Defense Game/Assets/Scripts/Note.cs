using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    internal Enemy Target;

    internal bool activate = false;

    internal float damage;
    
    private float speed = 8f;

    private void moveNote()
    {
        transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, Target.transform.position) <= .1f)
        {
            Target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!activate) { return; }

        if (Target)
        {
            moveNote();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
