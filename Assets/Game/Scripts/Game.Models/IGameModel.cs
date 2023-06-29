using Game.Data;

namespace Game.Models
{
    public interface IGameModel
    {
        BoardDeckType DeckType { get; set; }
    }
}