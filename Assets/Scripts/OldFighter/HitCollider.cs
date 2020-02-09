using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public string punchName;
    public bool kick;
    public bool punch;
    public int damage;
    public GameObject fighter;

    Animator myAnimator;

    void Start()
    {
        myAnimator = fighter.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherObject = other.gameObject;
        IDamageable damageable = otherObject.GetComponent<Enemy>() as IDamageable;
        bool canDamage = fighter.GetComponent<oldFighter>().CanIDamage();

        if (otherObject != null && otherObject != fighter && damageable != null && canDamage == true)
        {
            //Patadas
            if (kick)
            {
               // print(punchName);
                if (myAnimator.GetBool("Attack2") || myAnimator.GetBool("Attack3"))
                {
                    damageable.OnDamage(damage);
                    fighter.GetComponent<oldFighter>().CantDamage();
                }
            }
            //Puñetazos
            else if (punch)
            {
                //print("puño");
                if (myAnimator.GetBool("Attack1"))
                {
                    damageable.OnDamage(damage);
                    fighter.GetComponent<oldFighter>().CantDamage();
                }
            }
        }
    }
}
