using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    private PlayerMovementHandler player;
    private bool playerExiting;
    [SerializeField] private Transform exitPoint;
    [SerializeField] float playerMoveSpeed;
    [SerializeField] bool nextScene;
    [SerializeField] bool previousScene;


    void Awake()
    {
        player = PlayerHealthController.instance.GetComponent<PlayerMovementHandler>();
    }

    void Update()
    {
        if (playerExiting)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, exitPoint.position, playerMoveSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!playerExiting)
            {
                player.canMove = false;
                StartCoroutine(UseDoor());
            }
        }
    }

    IEnumerator UseDoor()
    {
        playerExiting = true;
        player.animator.enabled = false;
 
        yield return new WaitForSeconds(1.5f);
      
        player.canMove = true;
        player.animator.enabled = true;

        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        int previousIndex = SceneManager.GetActiveScene().buildIndex - 1;

        if (nextScene)
        {
            if (SceneManager.sceneCountInBuildSettings > nextIndex)
            {
                SceneManager.LoadScene(nextIndex); 
            }
            else
            {
                SceneManager.LoadScene(0);
            }
            
        }
        else if (previousScene)
        {
            SceneManager.LoadScene(previousIndex); 
        }
    }
    
}
