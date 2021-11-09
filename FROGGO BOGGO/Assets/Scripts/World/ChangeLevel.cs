using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Sprite Open_Bud;
    private GameMaster gm;
    public int sceneToSwitchTo;

    public AudioSource jingle;

    void Start()
    {
        //gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.sprite = Open_Bud;
            StartCoroutine(waitBeforeSwitchingToNextLevel());
        }
    }

    IEnumerator waitBeforeSwitchingToNextLevel()
    {

        jingle.Play();
        yield return new WaitForSeconds(4.2f);
        SceneManager.LoadScene(sceneToSwitchTo);
        
  
    }
}
