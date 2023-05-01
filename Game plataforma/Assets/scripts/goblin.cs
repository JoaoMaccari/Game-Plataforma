using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    private Rigidbody2D rig;
    private Animator anim;

    private bool isFront;
    public bool isRight;
    public float stopDistance;

    private Vector2 direction;

    public float speed;
    public float maxVision;

    public Transform point;
    public Transform behind;

    // Start is called before the first frame update
    void Start()

    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

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
       
    }


    void FixedUpdate() {
        getPlayer();
        onMove();

        
    }

    void onMove() {

        if (isFront) {

            anim.SetInteger("transicao", 1);

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


    void getPlayer(){
        RaycastHit2D hit = Physics2D.Raycast(point.position, direction, maxVision);

        //vai detectar o colider do objeto
        if (hit.collider != null) {
            
            //verifica se a tag do objeto é o player
            if (hit.transform.CompareTag("Player")) {

                isFront = true;

                //variavel que usa metodo que mostra a distancia entre 2 objetos. Estou passando a posição de cada um em cena como referencia
                float distance = Vector2.Distance(transform.position, hit.transform.position);

                if (distance <= stopDistance) {//distancia pra atacar
                    isFront = false;
                    rig.velocity = Vector2.zero;

                    anim.SetInteger("transicao", 2);
                    hit.transform.GetComponent<Player>().onHit();
                }


                Debug.Log("viu player");
            }
        }

        RaycastHit2D behindHit = Physics2D.Raycast(behind.position, -direction, maxVision);

        if (behindHit.collider != null) {

            if (behindHit.transform.CompareTag("Player")) {

                isRight = !isRight;
                Debug.Log("atrás");
            }

        }
    }


    private void OnDrawGizmos() {
        Gizmos.DrawRay(point.position, direction * maxVision);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawRay(point.position, -direction * maxVision);
    }
}
