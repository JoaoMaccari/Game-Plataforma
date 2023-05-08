using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleButtom : MonoBehaviour

   
{

    private Animator anim;
    public Animator barrierAnim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onPressed() {
        anim.SetBool("isPressed", true);
        barrierAnim.SetBool("down", true);
    }

    void onExit() {
        anim.SetBool("isPressed", false);
        barrierAnim.SetBool("down", false);

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("stone")) {
            onPressed();
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("stone")) {
            onExit();
        }
    }
}
