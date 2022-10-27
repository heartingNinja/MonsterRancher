using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OneChacterNoLife : MonoBehaviour
{
    [SerializeField] GameObject winnerLeft;
    [SerializeField] GameObject winnerRight;

    public EnemyHealth enemyHealthLeft;
    public EnemyHealth enemyHealthRight;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if( enemyHealthLeft.currentHealth <= 0)
        {
            winnerRight.SetActive(true);
            Time.timeScale = 0;
        }

        if(enemyHealthRight.currentHealth <=0)
        {
            winnerLeft.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ResetGame()
    {
        //SceneManager.LoadScene("FightMonsterRanchers");
        StartCoroutine(LoadYourAsyncScene());
       
    }

    IEnumerator LoadYourAsyncScene()
    {
       
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("FightMonsterRanchers");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
