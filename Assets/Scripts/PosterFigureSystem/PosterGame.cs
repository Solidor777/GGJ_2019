﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosterGame : MonoBehaviour
{

    public static PosterGame pg;
    public GameObject gameScreen; //False wall for mini-game background

    public GameObject currentPoster; //Poster currently held by player to place next
    public GameObject[] posterPrefabs; //The minigame will go through all the posters before finishing

    public GameObject placementBoard; //Object used as container for scaling the posters down from mini-game to player's room
    public int currentLayer = 1; //Keeps track of current poster layer so they can overlap

    public bool hasPoster = false;

    private void Awake()
    {
        pg = this;
    }

    // Use this for initialization
    void Start()
    {
        SwitchPoster(0); //Just debugging to start it, it should be called via interaction with PosterGame.pg.PosterMinigame()
    }

    // Update is called once per frame
    void Update()
    {
        if (hasPoster) //bool to stop the object from moving away from minigame
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            transform.position += move * 5f * Time.deltaTime; //Simple movement for the posters in the game. Probably will need colliders to stop from leaving the screen.

            if (Input.GetKeyUp(KeyCode.Space)) // Places the poster and parents it from the controller to the placementBoard for moving later
            {
                currentPoster.transform.SetParent(placementBoard.transform);
                hasPoster = false;
                if ((currentLayer - 1) < posterPrefabs.Length)
                {
                    SwitchPoster(currentLayer - 1);
                }
                else
                {
                    print("Oh No! You're out of large sheets of graphics!");
                    //screen goes black
                    gameScreen.SetActive(false);
                    placementBoard.transform.position = new Vector3(-.72f, 1.6f, 0.75f); // Bad hardcoding of values for testing. 
                    placementBoard.transform.rotation = Quaternion.Euler(0, -24.7f, 1.9f); // But honestly will probably still hardcode in final scene.
                    placementBoard.transform.localScale = new Vector3(20, 15, 1); // It works tho.
                }
            }
        }
    }

    void SwitchPoster(int nextPost)
    {
        GameObject curPost = Instantiate(posterPrefabs[nextPost], pg.gameObject.transform.position, Quaternion.identity) as GameObject; //Instantiates next poster for the poster minigame
        curPost.transform.SetParent(pg.gameObject.transform); // Assigns it to move along with the controller
        curPost.GetComponent<SpriteRenderer>().sortingOrder = currentLayer; // Allows posters to overlap
        hasPoster = true; 
        currentPoster = curPost; // Reference for the Update function to use
        currentLayer += 1;
    }

    public void PosterMinigame() // Call this from the interactions to start the minigame.
    {
        StartCoroutine(StartPosters()); 
    }

    IEnumerator StartPosters()
    {
        //camera will go black and move to minigame
        yield return new WaitForSeconds(1f);
        SwitchPoster(0);
    }
}