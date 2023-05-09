using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    private Transform player;

    public static PlayerPos instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        //vai colocar na variavel player o gameobject com a tag "Player" 
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null) {
            checkPoint();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void checkPoint() {
        //passo em qual posição o player vai spawnar
        Vector3 playerPos = transform.position;
        playerPos.z = 0f;

        player.position = playerPos;
    }
}
