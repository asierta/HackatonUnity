using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ManagerFight : MonoBehaviour
{

    public GameObject player1, player2;
    public Slider P1, P2;




    public 

    // Start is called before the first frame update
    void Start()
    {
        P1.value = Mathf.Clamp01(player1.GetComponent<oldFighter>().health / 100f);
        P2.value = Mathf.Clamp01(player2.GetComponent<oldFighter>().health / 100f);
    }

    // Update is called once per frame
    void Update()
    {
        //Seleccion

        //In puase

        //In game
        P1.value = Mathf.Clamp01(player1.GetComponent<oldFighter>().health / 100f);
        P2.value = Mathf.Clamp01(player2.GetComponent<oldFighter>().health / 100f);
    }
}
