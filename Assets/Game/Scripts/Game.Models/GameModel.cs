using System;
using Game.Data;

namespace Game.Models
{
    [Serializable]
    public class GameModel : IGameModel
    {
        public GameMode GameMode { get; set; }
        public BoardMode BoardMode { get; set; }
        public BoardDeckType DeckType { get; set; }
        public bool AiOpponent { get; set; }
    }
}