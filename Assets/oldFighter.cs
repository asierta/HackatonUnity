using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oldFighter : MonoBehaviour
{
    Animator myAnimator;
    int attackIndex=0;
    float timeSinceLastAttack=0;

    public float attacMaxkDelay;


    bool next =false;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = this.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Buttons();
        timeSinceLastAttack += Time.deltaTime;
       
    }

    void Buttons() {



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
            else
            {
                attackIndex = 1;
                myAnimator.SetBool("Attack1", true);
            }
            timeSinceLastAttack = 0;

        }
    }





    public void HabilitarNext(float time) {
         next = true;
    }



    public void return1()
    {
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
