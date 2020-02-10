using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oldFighter : MonoBehaviour, IDamageable
{
    Animator myAnimator;   
    
    AudioSource audioData;

    public float health = 100;
    public  AudioClip generic, punch, kick1, kick2;
    public KeyCode attackKey;

    public GameObject[] colliders;

    int attackIndex = 0;
    float timeSinceLastAttack = 0;

    public float attacMaxkDelay;

    List<KeyCode[]> combos = new List<KeyCode[]>();

    bool next = true;
    bool firstAttackDone = false;
    //Se activa cada vez que se pulsa la tecla de ataque. 
    //Se desactiva una vez ese ataque haya colisionado con algo Damageable. 
    //De esta forma se evita que un mismo ataque quite vida al enemigo varias veces
    bool canDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = this.GetComponent<Animator>();
        audioData = GetComponent<AudioSource>();

        combos.Add(new KeyCode[]{
            KeyCode.Z,
            KeyCode.Z,
            KeyCode.Z});

        combos.Add(new KeyCode[]{
            KeyCode.Z,
            KeyCode.X,
            KeyCode.Z});
    }

    public bool isDead()
    {
        return health <= 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead())
        {
            return;
        }
        timeSinceLastAttack += Time.deltaTime; //Tiempo que tiene para concatenar ataques.
        UpdateButtons();
    }

    void UpdateButtons()
    {
        if (Input.GetKeyDown(attackKey))
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
                        canDamage = true;
                        next = false;
                        //colliders[0].GetComponent<HitCollider>().enableParticles();
                    }

                    if (attackIndex == 1)
                    {
                        myAnimator.SetBool("Attack1", true);
                        PlaySound(generic);
                        colliders[0].GetComponent<HitCollider>().enableParticles();


                    }
                    attackIndex = Mathf.Clamp(attackIndex, 0, 3);
                }
                else
                {
                    attackIndex++;
                    canDamage = true;
                    myAnimator.SetBool("Attack1", true);
                    PlaySound(generic);
                    colliders[0].GetComponent<HitCollider>().enableParticles();


                }
            }
            timeSinceLastAttack = 0;
        }
    }

      

    //Collider functions
    public bool CanIDamage()
    {
        return canDamage;
    }

    public void CantDamage()
    {
        canDamage = false;
    }


    //Sound
    public  void PlaySound(AudioClip sonido) {

        audioData.clip = sonido;
        audioData.Play();
    }

    public  void PlayOneShotSound(AudioClip sonido)
    {
        audioData.PlayOneShot(sonido,1);
    }

    public void PlayGeneric()
    {
        audioData.PlayOneShot(generic, 1);
    }


    //To connect more than one hit
    public void HabilitarNext(float time)
    {
        next = true;
    }

    //Animation Finicsh Events
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
        colliders[0].GetComponent<HitCollider>().disableParticles();

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

    public void OnDamage(int value)
    {
        if (health - value <= 0)
        {
            myAnimator.SetBool("isDead", true);
            health -= value;
        }
        else 
        { 
            health -= value;
        }
    }



    public void Death()
    {
        myAnimator.SetBool("isDead", false);
    }
}
