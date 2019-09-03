using UnityEngine;
using UnityEngine.SceneManagement;
using Nuwn.Essentials;

namespace Nuwn
{
    namespace Extras
    {
        public abstract class GameManager : MonoBehaviour
        {
            private void OnEnable()
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
            }

            protected virtual void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) { }

            public bool gameIsPaused { get; set; }
            public void PauseGame()
            {
                Time.timeScale = 0;
                gameIsPaused = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            public void ResumeGame()
            {
                Time.timeScale = 1;
                gameIsPaused = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            public void RestartGame()
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            public void LoadScene(int i)
            {
                Nuwn_Essentials.LoadNewScene(i, SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
