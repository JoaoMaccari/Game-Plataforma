using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class controlador : MonoBehaviour
{

    public int score;
    public Text scoreText;


    public static controlador instance;

    private void Awake() {
        instance = this;

        //verifica o valor que esta armazenado no arquivo de texto
        if (PlayerPrefs.GetInt("scoreBin") != null) {

            //meu score recebe o valoar que está armazenado em scoreBin
            score = PlayerPrefs.GetInt("scoreBin");
            //passo o valor visualmente ao meu texto na cena
            scoreText.text = "X " + score.ToString();
        }
       
    }

    public void start() {
        getCoin();
    }

    public void getCoin() {
        score++;
        //como o scoreText é do tipo Text. Ele está acessando o texto que fica dentrodo meu canvas em cena
        //em seguida transformo o score em string para passar para o scoreText
        scoreText.text = "X " +  score.ToString();

        //salva localmente o valor da variavel score dentro de um binario (nesse caso no scoreBin)
        PlayerPrefs.SetInt("scoreBin", score);
    }

    public void NextLvl() {
        SceneManager.LoadScene(1);
    }
  
}
