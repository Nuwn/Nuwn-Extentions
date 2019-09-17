using UnityEngine;
using UnityEngine.SceneManagement;
using Nuwn.Essentials;
using System;
using System.Collections.Generic;
using Nuwn.Extensions;

namespace Nuwn
{
    public abstract class GameManager : Singleton<MonoBehaviour>
    {   
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected virtual void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) { }

        bool isPaused = false;
        float prevTime;
        private void PauseGame(bool v)
        {
            if(!isPaused && v)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="oldScene">-1 if you want to unload manually</param>
        public void LoadScene(int i)
        {
            Nuwn_Essentials.LoadNewScene(i, -1);
        }
        public void LoadScene(int i, int oldScene)
        {
            Nuwn_Essentials.LoadNewScene(i, oldScene);
        }
        public void LoadScene(int i, Action Callback)
        {
            Nuwn_Essentials.LoadNewScene(i, -1, (v) => { if (v) Callback(); });
        }
        public void UnloadScene(int scene)
        {
            SceneManager.UnloadSceneAsync(scene);
        }
        private void OnApplicationPause(bool pause)
        {
            PauseGame(pause);
        }


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
