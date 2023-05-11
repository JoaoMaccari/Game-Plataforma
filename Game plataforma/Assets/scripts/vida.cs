using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class vida : MonoBehaviour {
    public int health;

    //totol de cora��es que aparecem na tela
    public int heartsCount;

    public Image[] hearts;// guarda as imagens dos cora��es
    public Sprite heart;//sprite cora��o full
    public Sprite noHeart;//sprite cora��o vazio

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //vai passar por  todos os cora��es dentro do arrey
        for (int i = 0; i<hearts.Length; i++) {

            if (i < health) {// verifica a vida do personagem
                //se o contador do for � menor que o numero total de vidas
                hearts[i].sprite = heart;
            }
            else {
                hearts[i].sprite = noHeart;
            }

            //vai verificar se o meu contador � menor que o numero de cora��es
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
