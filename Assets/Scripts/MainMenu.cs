using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Update()
    {
#if UNITY_EDITOR
        UseMouseInput();
#elif UNITY_ANDROID
        UseTouchScreenInput();
#endif
    }

    private void UseTouchScreenInput()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        string mode = hit.transform.gameObject.name;
                        if (mode == "Easy") DataHolder.enemyCount = 2;
                        else if (mode == "Medium") DataHolder.enemyCount = 3;
                        else if (mode == "Hard") DataHolder.enemyCount = 4;
                        LoadGame();
                    }
                }
            }
        }
    }

    private void UseMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                string mode = hit.transform.gameObject.name;
                if (mode == "Easy") DataHolder.enemyCount = 2;
                else if (mode == "Medium") DataHolder.enemyCount = 3;
                else if (mode == "Hard") DataHolder.enemyCount = 4;
                LoadGame();
            }
        }
    }

    private void LoadGame()
    {
        SceneManager.LoadScene("FightScene");
    }
}
