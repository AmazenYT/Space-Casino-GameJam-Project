using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPotrol : MonoBehaviour
{
    public GameObject PointA;
    public GameObject PointB;
    private Rigidbody2D rb;
    // private Animator anim;
    private Transform currentPoint;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        currentPoint = PointB.transform;
        //anim.SetBool("isRunning", true);

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == PointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 3f && currentPoint == PointB.transform)
        {
            //flip();
            currentPoint = PointA.transform;
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 3f && currentPoint == PointA.transform)
        {
            //flip();
            currentPoint = PointB.transform;
        }
    }

    private void flip()
    {
        Vector3 localscale = transform.localScale;
        localscale.x *= -1;
        transform.localScale = localscale;

    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(PointA.transform.position, 4f);
        Gizmos.DrawSphere(PointB.transform.position, 4f);
        Gizmos.DrawLine(PointA.transform.position, PointB.transform.position);

    }




}
