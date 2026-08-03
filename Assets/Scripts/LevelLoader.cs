using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private Animator transition;
    [SerializeField] private TMP_Text loadingText;

    public void LoadNextScene(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    private IEnumerator LoadLevel(string name)
    {
        yield return null; //Lets the animator finish initializing this frame

        transition.SetTrigger("Start");

        float timer = 0f;
        float dotTimer = 0f;
        int dotCount = 1;

        // Wait for transitionTime seconds, cycling "Loading." / ".." / "..."
        while (timer < transitionTime)
        {
            dotTimer += Time.deltaTime;
            if (dotTimer >= 0.3f) 
            {
                dotTimer = 0f;
                dotCount = dotCount % 3 + 1;
                if (loadingText != null)
                    loadingText.text = "Loading" + new string('.', dotCount);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(name);
    }
}