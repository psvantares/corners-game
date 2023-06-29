using System;
using Game.Data;

namespace Game.Models
{
    [Serializable]
    public class GameModel : IGameModel
    {
        public BoardDeckType DeckType { get; set; }
    }
}