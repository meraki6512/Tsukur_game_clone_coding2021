﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferScene : MonoBehaviour {

    public string transferMapName;

    private PlayerManager thePlayer;

	// Use this for initialization
	void Start () {
        thePlayer = FindObjectOfType<PlayerManager>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "thePlayer")
        {
            thePlayer.currentMapName = transferMapName;
            SceneManager.LoadScene(transferMapName);
        }
    }
}
