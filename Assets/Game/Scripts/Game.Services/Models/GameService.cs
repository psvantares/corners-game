using Game.Core;
using Game.Models;

namespace Game.Services
{
    public class GameService : IGameService
    {
        public readonly IGameModel GameModel;

        public GameService(IGameModel gameModel)
        {
            GameModel = gameModel;
        }
    }
}