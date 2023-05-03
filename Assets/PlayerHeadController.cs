using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadController : MonoBehaviour
{
    AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SweetTreat"))
        {
            Debug.Log("collided with player head");
            SweetTreat treat = other.GetComponent<SweetTreat>();
            if (treat.owner != null)
            {
                Debug.Log("owner not null");
                // Free up the hand holding the treat, then let the treat be eaten
                treat.owner.FreeHand();
                treat.owner.removeTreat(treat);
                PlayerManager player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
                player.EatTreat(treat);

                StartCoroutine("EatingAudio");
                
            }
        }
    }

    private IEnumerator EatingAudio()
    {
        audio.PlayOneShot(audio.clip);

        float time = 0f;
        int plays = 1;
        float interval = 0.2f;

        while (plays < 3)
        {
            time += Time.deltaTime;

            if (time >= interval)
            {
                audio.PlayOneShot(audio.clip);
                time -= interval;

                plays++;
            }

            yield return new WaitForEndOfFrame();
        }

        StopCoroutine("EatingAudio");
    }
}
