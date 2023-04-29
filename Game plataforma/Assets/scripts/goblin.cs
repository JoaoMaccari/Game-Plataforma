using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class goblin : MonoBehaviour
    
{

    private Rigidbody2D rig;

    public bool isFrtont;

    public float maxVision;
    public float speed;
    public Vector2 direction;

    public Transform point;

    public bool isRight;

    // Start is called before the first frame update
    void Start()
    {
        rig.GetComponent<Rigidbody2D>();

        
            if (isRight) {
                transform.eulerAngles = new Vector2(0, 0);
                direction = Vector2.right;
                
               
            }
            else {
                transform.eulerAngles = new Vector2(0, 180);
                direction = Vector2.left;
              
            }
       



    }

    // Update is called once per frame
    void Update()
    {


        /*if (isRight) {
            transform.eulerAngles = new Vector2(0, 0);
            direction = Vector2.right;


        }
        else {
            transform.eulerAngles = new Vector2(0, 180);
            direction = Vector2.left;

        }*/

    }

    void FixedUpdate() {
        getPlayer();    
        onMove();
    }

    void onMove() {
        //só vai andar se o player estiver no ponto de visao do raycast

        //
        if (isFrtont) {

            if (isRight) {
                transform.eulerAngles = new Vector2(0, 0);
                direction = Vector2.right;
                rig.velocity = new Vector2(speed, rig.velocity.y);
            }
            else {
                transform.eulerAngles = new Vector2(0, 180);
                direction = Vector2.left;
                 rig.velocity = new Vector2(-speed, rig.velocity.y);
            }
        }
         
    }

    void getPlayer() {
        //passo o ponto de origem do raycast, direção, e a distancia de alcançe 
        RaycastHit2D hit = Physics2D.Raycast(point.position, direction, maxVision);

        //verifica se o hit está batendo em algum colisor
        if (hit.collider != null) {

            if (hit.transform.CompareTag("Player")) {
                isFrtont = true;    
                Debug.Log("viu o player");
                
            }
        }
    }

    //só vai mostrar o gizmo no objeto selecionado
    private void OnDrawGizmosSelected() {
        Gizmos.DrawRay(point.position, Vector2.right * maxVision);
    }
}
