using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitCollider : MonoBehaviour
{
    public string punchName;
    public bool kick;
    public bool punch;
    public int damage;


    [HideInInspector]
    public GameObject fighter;

    private Shake shake;

    Animator myAnimator;

    oldFighter oldScript;
    new ParticleSystem particleSystem;


    void Awake()
    {
        if (punchName == "Puño")
        {
            particleSystem = this.gameObject.GetComponent<ParticleSystem>();
            if (particleSystem)
            {
                disableParticles();
            }
        }

        fighter = transform.root.gameObject;

        //print(fighter.name);

        oldScript = fighter.GetComponent<oldFighter>();
    }

    void Start()
    {

        myAnimator = fighter.GetComponent<Animator>();
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();


        //punchAudio = fighter.GetComponents<AudioSource>()[0];
        //kickAudio = fighter.GetComponents<AudioSource>()[2];
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherObject = other.gameObject;
        IDamageable damageable = otherObject.GetComponent<oldFighter>() as IDamageable;
        bool canDamage = fighter.GetComponent<oldFighter>().CanIDamage();

        if (otherObject != null && otherObject != fighter && damageable != null && canDamage == true)
        {
            //Patadas
            if (kick)
            {
                // print(punchName);
                if (myAnimator.GetBool("Attack2") || myAnimator.GetBool("Attack3"))
                {
                    //kickAudio.Play();
                    shake.CamShake("shake2");
                    oldScript.PlayOneShotSound(oldScript.kick1);
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
                    //punchAudio.Play();
                    shake.CamShake("shake");
                    oldScript.PlayOneShotSound(oldScript.punch);
                    damageable.OnDamage(damage);
                    fighter.GetComponent<oldFighter>().CantDamage();
                }
            }
        }
    }

    public void enableParticles()
    {
        particleSystem.Play();
    }

    public void disableParticles()
    {
        particleSystem.Stop();
    }



}
