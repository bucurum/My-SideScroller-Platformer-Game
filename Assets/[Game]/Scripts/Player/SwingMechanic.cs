using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingMechanic : MonoBehaviour
{
    public LayerMask swingLayer; //sallanabilir nesnelerin Layer'ı
    [SerializeField] Transform anchorPoint;
    private bool anchorPoints;
    [SerializeField] float jointDistance;
    public float swingForce = 5f; //sallanma kuvveti
    public float maxSwingAngle = 45f; //maksimum sallanma açısı

    private bool isSwinging = false; //sallanma durumu
    private Rigidbody2D rb;
    [SerializeField] private DistanceJoint2D swingJoint;
    private Vector2 previousVelocity; //önceki hız
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        swingJoint.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            if (!isSwinging)
            {
                //Sallanacak objeyi bul
                anchorPoints = Physics2D.OverlapCircle(anchorPoint.position, jointDistance, swingLayer);


                if (anchorPoints)
                {
                   
                    //Sallanma ekle
                    isSwinging = true;
                    rb.isKinematic = true;
                    rb.velocity = Vector2.zero;

                    swingJoint.enabled = true;
                    swingJoint.connectedAnchor = new Vector2(0,0);
                    swingJoint.distance = Vector2.Distance(transform.position, anchorPoint.position);

                    previousVelocity = rb.velocity;
                }
            }
            else
            {
                //Sallanma kontrolü
                    float angle = Vector2.Angle(rb.velocity, Vector2.up);
                    if (angle > maxSwingAngle) 
                    {
                        swingJoint.breakForce = 0;

                    }
                    
                    Vector2 swingDirection = (swingJoint.connectedAnchor - (Vector2)transform.position).normalized;
                    rb.AddForce(swingDirection * swingForce);
                
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            //Sallanmadan çık
            isSwinging = false;
            rb.isKinematic = false;
            swingJoint.enabled = false;

            //Önceki hızı ayarla
            rb.velocity = previousVelocity;
        }
    }

     void OnDrawGizmosSelected()
    {
        if (anchorPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(anchorPoint.position, jointDistance);
    }
}
