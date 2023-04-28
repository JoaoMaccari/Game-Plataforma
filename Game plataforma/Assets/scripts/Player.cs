using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    public float forcaPulo = 5.0f;

    private bool isJumping;
    private bool doubleJump;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        jump();
    }

    private void FixedUpdate() {
        move();
    }

    void move() {
        float movement = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2 (movement * speed, rb.velocity.y);

        if (movement > 0) {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (movement < 0) {
            transform.eulerAngles = new Vector3 (0, 180, 0);
        }
    }

    void jump() {
        if (Input.GetButtonDown("Jump") ) {

            if (!isJumping) {
                rb.AddForce(Vector2.up * forcaPulo, ForceMode2D.Impulse);
                isJumping = true;
                doubleJump = true;
            }else if (doubleJump) {

                rb.AddForce(Vector2.up * forcaPulo, ForceMode2D.Impulse);
                doubleJump = false;

            }
            
        }
    }

    void OnCollisionEnter2D(Collision2D collisor) {
        if (collisor.gameObject.layer == 3) {
            isJumping = false;
        }
    }
}
