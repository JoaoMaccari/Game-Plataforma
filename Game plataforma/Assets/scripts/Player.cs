using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerAudio playerAudio;
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


    private static Player instance;
    private void Awake() {
        DontDestroyOnLoad(this);// mantem um objeto em cena

        if (instance == null) { //vai checar na cena seguinte se o instance � nulo(se j� existe outro player na cena)

            instance = this;//caso n�o tenha nenhum objeto player, instance recebe o objeto player. O this passa a receber a classe Player
        }
        else {
            Destroy(gameObject);//se existir outra classe Player na cena ele destro o bjeto mantento apenas um player
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<PlayerAudio>();
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

        //s� vai executar o idle se o movimento for 0, n�o estiver pulando e n�o estiver atackando
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
            
            //Collider 2d � um tipo de variavel de colis�o, ou seja. Vai armazenar um colisor na variavel

            //cria uma area ao redor do meu objeto hit
            //especifico a posi��o que come�a o hit, o raio e em qual layer precisa bater para funcionar
            //o tipo collider armazena um colisor, neste caso vai pegar o colisor do inimigo
            Collider2D hit = Physics2D.OverlapCircle(point.position, radius, enemyLayer);

            playerAudio.PlaySFX(playerAudio.hitSound);

            if (hit != null) {

                //se o hit n�o for null, eu checo qual script contem dentro dele
                //para poder acessar o m�todo de levar dano
                if (hit.GetComponent<slime>()) {
                    hit.GetComponent<slime>().onHit();
                }

                if (hit.GetComponent<Goblin>()) {
                    hit.GetComponent<Goblin>().onHit();
                }



            }



            //a corotina faz o transition receber o valor de 0 novamente quando o atack � encerrado
            //se eu tentar passar o valor de false para o atack de forma direta, n�o ia dar tempo de ocorrer a anima��o do atack
            StartCoroutine(onAttack());
        }
        

    }

    //m�todo controlado por tempo, depois que der o atack eu desativo o atack com false fazendo o idle ser lido novamente
    IEnumerator onAttack() {
        //vai aguardar 0.33segundos (tempo da anima��o) para dai setar o attack como false e o transition voltar a ser 0, mantendo o personagem em idle
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

        if (collision.CompareTag("Coin")) {
            playerAudio.PlaySFX(playerAudio.coinSound);
            //pega o componente animatro da moeda e ativa o pjarametro hit
            collision.GetComponent<Animator>().SetTrigger("hit");
            Destroy(collision.gameObject, 0.5f);
            controlador.instance.getCoin();
        }

        if (collision.gameObject.layer == 7) {
            controlador.instance.NextLvl();
        }
    }
}
