using Game.Data;

namespace Game.Models
{
    public interface IGameModel
    {
        GameplayMode GameplayMode { get; set; }
        BoardMode BoardMode { get; set; }
        BoardDeckType DeckType { get; set; }
    }
}