using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Bootstrap
{
    public class BootstrapEntry : MonoBehaviour
    {
        private void Awake()
        {
            try
            {
                SceneManager.LoadScene(1);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}