using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Defeat : MonoBehaviour
{
    public GameObject gameOver;
    public GameObject totalScore;
    public GameObject record;

    private void OnEnable()
    {
        totalScore.GetComponent<Text>().text = GameObject.Find("Score").GetComponent<Text>().text;
        gameOver.SetActive(true);
        totalScore.SetActive(true);
        StartCoroutine("LoadMenu");
        string mode = DataHolder.enemyCount == 2 ? "easy" : DataHolder.enemyCount == 3 ? "medium" : "hard";
        int score = int.Parse(totalScore.GetComponent<Text>().text.Split(' ')[1]);
        if (PlayerPrefs.GetInt(mode) < score)
        {
            record.SetActive(true);
            PlayerPrefs.SetInt(mode, score);
        }
    }

    private IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(9f);
        SceneManager.LoadScene("MainMenu");
    }
}
