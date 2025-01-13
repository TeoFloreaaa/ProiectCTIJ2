using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu; // Referință la meniul de pauză (Canvas sau imagine)

    private bool isPaused = false;

    void Update()
    {
        // Verificăm dacă tasta TAB este apăsată
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TogglePause();
        }
    }

    // Funcție pentru a comuta între pauză și joc
    public void TogglePause()
    {
        isPaused = !isPaused; // Comutăm între pauză și reluare

        // Activăm sau dezactivăm meniul de pauză
        pauseMenu.SetActive(isPaused);

        // Oprim timpul jocului dacă este pauză, reluăm dacă nu este
        Time.timeScale = isPaused ? 0 : 1;
    }

    // Funcție pentru butonul Resume
    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1; // Reluăm timpul jocului
    }

    // Funcție pentru butonul Exit
    public void ExitGame()
    {
        Time.timeScale = 1; // Asigurăm că timpul jocului este reluat înainte de a părăsi
        Debug.Log("Se închide jocul..."); // Mesaj în consola editorului (opțional)
        Application.Quit(); // Închide aplicația
    }
}
