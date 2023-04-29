using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slime : MonoBehaviour
{
    private Rigidbody2D rig;
    public float speed;
    public Animator anim;
    public int health;

    public Transform point;
    public float radius;
    public LayerMask layer;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();    
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        rig.velocity = new Vector2(speed, rig.velocity.y);
        OnCollision();
    }

    void OnCollision() {
        Collider2D hit = Physics2D.OverlapCircle(point.position, radius, layer);

        if (hit != null) {
            speed = -speed;

            // só é chamado quando o inimigo bate em um objeto com a layer selecionada
            if (transform.eulerAngles.y ==0) {
                transform.localEulerAngles = new Vector3(0, 180,0);
            }
            else {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }

    }

    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(point.position, radius);
    }

    public void onHit() {
        anim.SetTrigger("hit");
        health--;

        if (health <= 0) {
            speed = 0;
            anim.SetTrigger("death");
            Destroy(gameObject, 1f);
        }
    }
}
