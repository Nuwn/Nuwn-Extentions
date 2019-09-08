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
        public void LoadScene(int i)
        {
            Nuwn_Essentials.LoadNewScene(i, SceneManager.GetActiveScene().buildIndex);
        }




        #region Pausable
        static PausableMonoBehaviour[] Pausables = new PausableMonoBehaviour[0];
        static PausableScriptableObject[] PausableScriptableObjects = new PausableScriptableObject[0];

        public static void RegisterPausable(object pausable)
        {
            switch (pausable)
            {
                case PausableMonoBehaviour pm:
                    Pausables = Pausables.Add(pm);
                    break;
                case PausableScriptableObject ps:
                    PausableScriptableObjects = PausableScriptableObjects.Add(ps);
                    break;
            }
        }
        public static void UnRegisterPausable(object pausable)
        {
            switch (pausable)
            {
                case PausableMonoBehaviour pm:
                    Pausables = Pausables.Remove(pm);
                    break;
                case PausableScriptableObject ps:
                    PausableScriptableObjects = PausableScriptableObjects.Remove(ps);
                    break;
            }
        }

        private void SetPause(bool v)
        {
            for (int i = 0; i < PausableScriptableObjects.Length; i++)
            {
                if (PausableScriptableObjects[i].HasMethod("OnPause"))
                {
                    PausableScriptableObjects[i].OnPause(v);
                }
            }
            for (int i = 0; i < Pausables.Length; i++)
            {
                if (Pausables[i].HasMethod("OnPause"))
                {
                    Pausables[i].OnPause(v);
                }
            }       
        }

        #endregion
    }
}
