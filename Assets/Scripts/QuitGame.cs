using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    [SerializeField] GameObject quitGameObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(quitGameObject.activeInHierarchy == false)
            {
                quitGameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                quitGameObject.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }
}
