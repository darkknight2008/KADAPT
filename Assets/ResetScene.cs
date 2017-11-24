using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour {

    //public GameObject Hero;
	// Use this for initialization
	void Start () {
        //DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update () {

	}
    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //SceneManager.LoadScene(KeepData.keepLevelName, LoadSceneMode.Single);
        //SceneManager.LoadScene("B4");
        //Hero.GetComponent<Animation>().Rewind("Idle");
        //Time.timeScale = 1;
    }
}
