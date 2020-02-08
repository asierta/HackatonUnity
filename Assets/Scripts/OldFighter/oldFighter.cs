using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oldFighter : MonoBehaviour
{
    Animator myAnimator;
    int attackIndex = 0;
    float timeSinceLastAttack = 0;

    public float attacMaxkDelay;

    List<KeyCode[]> combos = new List<KeyCode[]>();

    bool next = true;
    bool firstAttackDone = false;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = this.GetComponent<Animator>();

        combos.Add(new KeyCode[]{
            KeyCode.Z,
            KeyCode.Z,
            KeyCode.Z});

        combos.Add(new KeyCode[]{
            KeyCode.Z,
            KeyCode.X,
            KeyCode.Z});
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastAttack += Time.deltaTime; //Tiempo que tiene para concatenar ataques.
        Buttons();

    }

    void Buttons()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //Aseguramos que siempre podamos hacer un ataque desde el inicio
            if (!firstAttackDone)
            {
                firstAttackDone = true;
                timeSinceLastAttack = 0;
            }

            if (firstAttackDone == true)
            {
              //  print("Next: " + next);
               // print("Attack index: " + attackIndex);

                if (timeSinceLastAttack < attacMaxkDelay)
                {
                    if (next == true)
                    {
                        attackIndex++;
                        next = false;
                    }

                    if (attackIndex == 1)
                    {
                        myAnimator.SetBool("Attack1", true);
                    }
                    attackIndex = Mathf.Clamp(attackIndex, 0, 3);
                }
                else
                {
                    attackIndex++;
                    myAnimator.SetBool("Attack1", true);
                }
            }

            timeSinceLastAttack = 0;
        }
    }

    void Buttons2()
    {
        //    foreach (KeyCode[] combo in combos)
        //    {
        //        if (attackIndex <= combo.Length - 1)
        //        { 
        //            print(combo[attackIndex]);
        //        }
        //    }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (timeSinceLastAttack < attacMaxkDelay)
            {
                if (next == true)
                {
                    attackIndex++;
                    next = false;
                }

                if (attackIndex == 1)
                {
                    myAnimator.SetBool("Attack1", true);
                }
                attackIndex = Mathf.Clamp(attackIndex, 0, 3);
            }
            timeSinceLastAttack = 0;
        }
    }


    public void HabilitarNext(float time)
    {
        next = true;
    }



    public void return1()
    {
       // print("Se llamo al return1: ");

        if (attackIndex >= 2)
        {
            myAnimator.SetBool("Attack2", true);
        }
        else
        {
            myAnimator.SetBool("Attack1", false);
            attackIndex = 0;
        }
    }

    public void return2()
    {
        if (attackIndex >= 3)
        {
            myAnimator.SetBool("Attack3", true);
        }
        else
        {
            myAnimator.SetBool("Attack2", false);
            myAnimator.SetBool("Attack1", false);
            attackIndex = 0;
        }
    }

    public void return3()
    {
        myAnimator.SetBool("Attack1", false);
        myAnimator.SetBool("Attack2", false);
        myAnimator.SetBool("Attack3", false);
        attackIndex = 0;
    }

}
