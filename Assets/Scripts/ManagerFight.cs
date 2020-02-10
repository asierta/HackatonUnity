using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ManagerFight : MonoBehaviour
{

    public GameObject player1, player2;
    public Slider P1, P2;

    public AudioClip[] narratorAudios;

    AudioSource ambient, narrator;


    // Start is called before the first frame update
    void Start()
    {
        ambient = this.GetComponents<AudioSource>()[0];
        ambient.volume = 0.1f;
        narrator = this.GetComponents<AudioSource>()[1];
        StartCoroutine(playSoundWithDelay(narratorAudios[0], 1));
        StartCoroutine(setVolumeWithDelay(ambient, 0.5f, 7f));
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

    IEnumerator playSoundWithDelay(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        narrator.PlayOneShot(clip);
    }
    IEnumerator setVolumeWithDelay(AudioSource sources, float volume, float delay)
    {
        yield return new WaitForSeconds(delay);
        sources.volume = volume;
    }
}
