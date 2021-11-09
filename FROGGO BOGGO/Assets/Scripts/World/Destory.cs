using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Destory : MonoBehaviour
{
    public AudioSource deathSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            StartCoroutine(waitBeforeReload());
        }
    }

    IEnumerator waitBeforeReload()
    {
        deathSound.Play();
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
