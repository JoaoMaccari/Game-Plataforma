using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    public float forcaPulo = 5.0f;
    public int health;

    private bool isJumping;
    private bool doubleJump;
    private bool isAttacking;
    private bool recovery;

    public Animator anim;

    public Transform point;
    public float radius;

    public LayerMask enemyLayer;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        jump();
        attack();
    }

    private void FixedUpdate() {
        move();
    }

    void move() {
        //o getAxis horizontal vai retornar um valor positivo quando o personagem vai pra direita e negativa pra esquerda
        float movement = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2 (movement * speed, rb.velocity.y);


        if (movement > 0 && !isAttacking) {

            if (!isJumping) {
                anim.SetInteger("transition", 1);
            }

            
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (movement < 0 && !isAttacking) {

            if (!isJumping) {
                anim.SetInteger("transition", 1);
            }

            transform.eulerAngles = new Vector3 (0, 180, 0);
        }

        //só vai executar o idle se o movimento for 0, não estiver pulando e não estiver atackando
        if (movement == 0 && !isJumping && !isAttacking) {
            anim.SetInteger("transition", 0);
        }
    }

    void jump() {
        if (Input.GetButtonDown("Jump") ) {

            if (!isJumping) {
                anim.SetInteger("transition", 2);
                rb.AddForce(Vector2.up * forcaPulo, ForceMode2D.Impulse);
                isJumping = true;
                doubleJump = true;
            }else if (doubleJump) {

                rb.AddForce(Vector2.up * forcaPulo, ForceMode2D.Impulse);
                doubleJump = false;

            }
            
        }
    }

    void attack() {

        

        if (Input.GetButtonDown("Fire1")) {
            isAttacking = true;
            anim.SetInteger("transition", 3);
            
            //Collider 2d é um tipo de variavel de colisão, ou seja. Vai armazenar um colisor na variavel

            //cria uma area ao redor do meu objeto hit
            //especifico a posição que começa o hit, o raio e em qual layer precisa bater para funcionar
            Collider2D hit = Physics2D.OverlapCircle(point.position, radius, enemyLayer);

            if (hit != null) {
                //pega a classe slime para poder acessar seu método de ataque
                hit.GetComponent<slime>().onHit();
            }

            
            
            //a corotina faz o transition receber o valor de 0 novamente quando o atack é encerrado
            //se eu tentar passar o valor de false para o atack de forma direta, não ia dar tempo de ocorrer a animação do atack
            StartCoroutine(onAttack());
        }
        

    }

    //método controlado por tempo, depois que der o atack eu desativo o atack com false fazendo o idle ser lido novamente
    IEnumerator onAttack() {
        //vai aguardar 0.33segundos (tempo da animação) para dai setar o attack como false e o transition voltar a ser 0, mantendo o personagem em idle
        yield return new WaitForSeconds(0.33f);
        isAttacking = false;
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(point.position, radius);
    }

    void OnCollisionEnter2D(Collision2D collisor) {
        if (collisor.gameObject.layer == 3) {
            isJumping = false;
        }
    }

    float recoveryCount;
    public void onHit() {

        recoveryCount += Time.deltaTime;

        if (recoveryCount >= 2f) {
            
            anim.SetTrigger("takeHit");
            health--;
            
            
            recoveryCount = 0f;
        }

        if (health <= 0 && !recovery) {

            recovery = true;
            speed = 0;
            anim.SetTrigger("death");
            Destroy(gameObject, 1f);
        }
    }


    void OnCollisionEnter2D(Collider2D coll) {
        if (coll.gameObject.layer == 6) {
            isJumping=false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == 6) {
            onHit();
        }
    }
}
