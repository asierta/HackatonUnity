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

    private Shake shake;

    Animator myAnimator;
    AudioSource punchAudio;
    AudioSource kickAudio;

    void Start()
    {
        myAnimator = fighter.GetComponent<Animator>();
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();
        punchAudio = fighter.GetComponents<AudioSource>()[0];
        kickAudio = fighter.GetComponents<AudioSource>()[2];
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherObject = other.gameObject;
        IDamageable damageable = otherObject.GetComponent<Enemy>() as IDamageable;
        bool canDamage = fighter.GetComponent<oldFighter>().CanIDamage();

        if (otherObject != null && otherObject != fighter && damageable != null && canDamage == true)
        {
            shake.CamShake();

            //Patadas
            if (kick)
            {
               // print(punchName);
                if (myAnimator.GetBool("Attack2") || myAnimator.GetBool("Attack3"))
                {
                    kickAudio.Play();
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
                    punchAudio.Play();
                    damageable.OnDamage(damage);
                    fighter.GetComponent<oldFighter>().CantDamage();
                }
            }
        }
    }
}
