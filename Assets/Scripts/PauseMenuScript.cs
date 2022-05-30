using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject PauseMenuUi;
    public Slider LoadingMenuSlider;
    public TMP_Text progressText;
    public GameObject loadingPanel;

    public GameObject gameOverScreen;
    public void Resume()
    {
        Time.timeScale = 1f;
        PauseMenuUi.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !gameOverScreen.activeSelf)
        {
            Pause();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void Pause()
    {
        PauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;

      //  DisableOrEnableComponents(false);
    }

 /*   private void DisableOrEnableComponents(bool _isTrue)
    {
        player.GetComponent<GhostMovement>().enabled = _isTrue;
        player.GetComponent<ShootProjectile>().enabled = _isTrue;
        player.GetComponent<CharacterController>().enabled = _isTrue;
        _animator.enabled = _isTrue;
        cam.SetActive(_isTrue);

        DisableOrEnableEnemy(_isTrue);
    }

    public static void DisableOrEnableEnemy(bool _isTrue)
    {
        GameObject[] ghostObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in ghostObjects)
        {
            go.GetComponent<NavMeshAgent>().enabled = _isTrue;
            go.GetComponent<Enemy>().enabled = _isTrue;
        }
    }
 */


    public void PlayGameAfterLoading()
    {
        loadingPanel.SetActive(true);
        StartCoroutine(LoadAsynchronously(1));
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator LoadAsynchronously(int a)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(a);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingMenuSlider.value = progress;
            float p1 = progress * 100f;
            progressText.text = (int)p1 + "%";
            yield return null;
        }
    }
}
