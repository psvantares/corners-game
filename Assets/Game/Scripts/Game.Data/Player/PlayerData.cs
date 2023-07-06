using UnityEngine;

namespace Game.Data
{
    public class PlayerData : MonoBehaviour
    {
        private string nickName;

        private void Start()
        {
            var count = FindObjectsOfType<PlayerData>().Length;

            if (count > 1)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        public void SetNickName(string nickName)
        {
            this.nickName = nickName;
        }

        public string GetNickName()
        {
            if (string.IsNullOrWhiteSpace(nickName))
            {
                nickName = GetRandomNickName();
            }

            return nickName;
        }

        public static string GetRandomNickName()
        {
            var playerNumber = Random.Range(0, 9999);
            return $"Player {playerNumber:0000}";
        }
    }
}