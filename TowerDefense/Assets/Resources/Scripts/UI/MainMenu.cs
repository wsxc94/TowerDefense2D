using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    private GameObject optionMenu=null;

    [SerializeField]
    private GameObject mainMenu=null;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Options()
    {
        
            if (optionMenu.activeSelf)
            {
                  mainMenu.SetActive(true);
                  optionMenu.SetActive(false);
            }
            else
            {
               mainMenu.SetActive(false);
               optionMenu.SetActive(true);

             }

        
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
        
    
}
