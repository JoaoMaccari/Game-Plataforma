using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerAudio playerAudio;
    public float speed;
    public float forcaPulo = 5.0f;
    

    private bool isJumping;
    private bool doubleJump;
    private bool isAttacking;
    private bool recovery;

    public Text scoreText;
    public GameObject gameover;

    private vida healthSystem;
    public float recoveryTime;

    public Animator anim;

    public Transform point;
    public float radius;

    public LayerMask enemyLayer;


    public static Player instance;
    private void Awake() {

        
        if (instance == null) {

            instance = this;
            DontDestroyOnLoad(gameObject);

        }else if (instance != this) {

            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<PlayerAudio>();
        rb = GetComponent<Rigidbody2D>();
        healthSystem = GetComponent<vida>();
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
                playerAudio.PlaySFX(playerAudio.jumpSound);
            }else if (doubleJump) {

                rb.AddForce(Vector2.up * forcaPulo, ForceMode2D.Impulse);
                doubleJump = false;
                playerAudio.PlaySFX(playerAudio.jumpSound);


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
            //o tipo collider armazena um colisor, neste caso vai pegar o colisor do inimigo
            Collider2D hit = Physics2D.OverlapCircle(point.position, radius, enemyLayer);

            playerAudio.PlaySFX(playerAudio.hitSound);

            if (hit != null) {

                //se o hit não for null, eu checo qual script contem dentro dele
                //para poder acessar o método de levar dano
                if (hit.GetComponent<slime>()) {
                    hit.GetComponent<slime>().onHit();
                }

                if (hit.GetComponent<Goblin>()) {
                    hit.GetComponent<Goblin>().onHit();
                }



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


        //coloco um colisor em um lugar que cause a morte do personagem
        //aqui eu chamo um singleton  que chama o checkpoint
        if (collisor.gameObject.layer == 8) {
            PlayerPos.instance.checkPoint();
        }
    }

    
    public void onHit() {
        //o personagem só toma dano quando o recovery for falso
        //por padrão ele já vai tomar dano, logo que ele toma dano o recovery fica true por 2 s (colldown do inimigo)

        //por padrão o recovery é falso
        
        if (!recovery) {

            //caso esteja falso recebe o hit e diminui a vida
            anim.SetTrigger("takeHit");
            healthSystem.health--;

            //checa se o score vida cheou a zero e gera o game over
            if (healthSystem.health <= 0 ) {

                recovery = true;
                speed = 0;
                anim.SetTrigger("death");
                //Destroy(gameObject, 1f);
                controlador.instance.showGameOver();
            }
            else {//se nao for zero checa a coroutina que vai abrir o cool down para n tomar dano por 2s
                StartCoroutine(Recover());
            }
        }

    }

    //a coroutina vai manter o recovery true pelo tempo que o recoveyTime for estipulado
    private IEnumerator Recover() {
        recovery = true;
        yield return new WaitForSeconds(recoveryTime);
        recovery = false;
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

        if (collision.CompareTag("Coin")) {

            playerAudio.PlaySFX(playerAudio.coinSound);
            //pega o componente animatro da moeda e ativa o pjarametro hit
            collision.GetComponent<Animator>().SetTrigger("hit");
            Destroy(collision.gameObject, 0.5f);
            controlador.instance.getCoin();
        }

        
    }
}
