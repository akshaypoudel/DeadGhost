using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject PauseMenuUi;
    public Slider LoadingMenuSlider;
    public TMP_Text progressText;
    public GameObject loadingPanel;
    public GameObject player;
    public GameObject cam;
    public Animator _animator;
    public GameObject enemyPrefab;
    public void Resume()
    {
        PauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        DisableOrEnableComponents(true);
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Pause()
    {
        //Time.timeScale = 0f;
        PauseMenuUi.SetActive(true);
        DisableOrEnableComponents(false);
    }

    private void DisableOrEnableComponents(bool _isTrue)
    {
        player.GetComponent<GhostMovement>().enabled = _isTrue;
        player.GetComponent<ShootProjectile>().enabled = _isTrue;
        player.GetComponent<CharacterController>().enabled = _isTrue;
        _animator.enabled = _isTrue;
        //enemyPrefab.GetComponent<Enemy>().enabled = _isTrue;
        cam.SetActive(_isTrue);
    }

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
