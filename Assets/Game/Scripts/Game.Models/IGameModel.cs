using Game.Data;

namespace Game.Models
{
    public interface IGameModel
    {
        GameMode GameMode { get; set; }
        BoardMode BoardMode { get; set; }
        BoardDeckType DeckType { get; set; }
    }
}