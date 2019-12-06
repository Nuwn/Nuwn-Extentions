using UnityEngine;
using UnityEngine.SceneManagement;
using Nuwn.Essentials;
using System;
using System.Collections.Generic;
using Nuwn.Extensions;

namespace Nuwn
{
    public abstract class GameManager : MonoBehaviour
    {
        
        protected virtual void OnEnable()
        {
            Debug.Log("enabled");
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        #region SceneManagement
        public List<Scene> activeScenes = new List<Scene>();
        protected virtual void OnSceneUnloaded(Scene arg0)
        {
            activeScenes.Remove(arg0);
        }
        protected virtual void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            activeScenes.Add(arg0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="oldScene">-1 if you want to unload manually</param>
        public void LoadScene(int i)
        {
            StartCoroutine(Nuwn_Essentials.LoadNewScene(i, -1));
        }
        public void LoadScene(int i, int oldScene)
        {
            StartCoroutine(Nuwn_Essentials.LoadNewScene(i, oldScene));
        }
        public void LoadScene(int i, Action<Scene> Callback)
        {
            StartCoroutine(Nuwn_Essentials.LoadNewScene(i, -1, (v) => { if (v) Callback(SceneManager.GetSceneByBuildIndex(i)); }));
        }
        public void AddScene(int i)
        {
            StartCoroutine(Nuwn_Essentials.AddNewScene(i));
        }
        public void AddScene(int i, int oldScene)
        {
            StartCoroutine(Nuwn_Essentials.AddNewScene(i, oldScene));
        }
        public void AddScene(int i, Action<Scene> Callback)
        {
            StartCoroutine(Nuwn_Essentials.AddNewScene(i, -1, (v) => { if (v) Callback(SceneManager.GetSceneByBuildIndex(i)); }));
        }
        public void UnloadScene(int scene)
        {
            SceneManager.UnloadSceneAsync(scene);
        }
        public void SetActiveScene(Scene scene)
        {
            SceneManager.SetActiveScene(scene);
        }
        public void ReloadScene(int i)
        {
            StartCoroutine(Nuwn_Essentials.LoadNewScene(i, i));
        }
        #endregion


        #region PauseHandler
        protected virtual void OnApplicationPause(bool pause)
        {
            PauseGame(pause);
        }
        bool isPaused = false;
        float prevTime;
        private void PauseGame(bool v)
        {
            if (!isPaused && v)
            {
                prevTime = Time.timeScale;
                Time.timeScale = 0;
                SetPause(true);
            }
            else if (isPaused && !v)
            {
                Time.timeScale = prevTime;
                SetPause(false);
            }
            isPaused = v;
        }

        public void Pause()
        {
            PauseGame(true);
        }
        public void Resume()
        {
            PauseGame(false);
        }
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        #endregion

        #region Pausable
        static IPausable[] Pausables = new IPausable[0];

        public static void RegisterPausable(IPausable pausable)
        {
            Pausables = Pausables.Add(pausable);
        }
        public static void UnRegisterPausable(IPausable pausable)
        {
            Pausables = Pausables.Remove(pausable);
        }

        private void SetPause(bool v)
        {
            for (int i = 0; i < Pausables.Length; i++)
            {
                Pausables[i].OnPause(v);
            }       
        }
        #endregion
    }
}
