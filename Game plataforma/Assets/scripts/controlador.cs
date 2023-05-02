using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class controlador : MonoBehaviour
{

    public int score;
    public Text scoreText;

    public static controlador instance;

    private void Awake() {
        instance = this; 
       
    }

    public void Update() {
        getCoin();
    }

    public void getCoin() {
        score++;
        //como o scoreText é do tipo Text. Ele está acessando o texto que fica dentrodo meu canvas em cena
        //em seguida transformo o score em string para passar para o scoreText
        scoreText.text = "X " +  score.ToString();
    }
  
}
