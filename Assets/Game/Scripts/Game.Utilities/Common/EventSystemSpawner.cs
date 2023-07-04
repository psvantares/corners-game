using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Utilities
{
    public class EventSystemSpawner : MonoBehaviour
    {
        private void OnEnable()
        {
            var sceneEventSystem = FindObjectOfType<EventSystem>();

            if (sceneEventSystem != null)
            {
                return;
            }

            var eventSystem = new GameObject("EventSystem");

            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
    }
}