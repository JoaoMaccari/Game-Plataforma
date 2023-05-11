using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class vida : MonoBehaviour {
    public int health;

    //totol de corações que aparecem na tela
    public int heartsCount;

    public Image[] hearts;// guarda as imagens dos corações
    public Sprite heart;//sprite coração full
    public Sprite noHeart;//sprite coração vazio

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //vai passar por  todos os corações dentro do arrey
        for (int i = 0; i<hearts.Length; i++) {

            if (i < health) {// verifica a vida do personagem
                //se o contador do for é menor que o numero total de vidas
                hearts[i].sprite = heart;
            }
            else {
                hearts[i].sprite = noHeart;
            }

            //vai verificar se o meu contador é menor que o numero de corações
            //se for significa que o sprite tem que ficar ativo
            if (i< heartsCount) {
                hearts[i].enabled = true;

            }
            else {
                hearts[i].enabled=false;
            }
        }
    }
}
