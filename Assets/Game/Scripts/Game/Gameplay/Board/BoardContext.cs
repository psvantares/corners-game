using Game.Models;

namespace Game.Gameplay
{
    public class BoardContext
    {
        public readonly IGameModel GameModel;
        public readonly NetworkGameController NetworkGameController;
        public readonly BoardConfig Config;
        public readonly PoolController Pool;

        public BoardContext
        (
            IGameModel gameModel,
            NetworkGameController networkGameController,
            BoardConfig config,
            PoolController pool
        )
        {
            GameModel = gameModel;
            NetworkGameController = networkGameController;
            Config = config;
            Pool = pool;
        }
    }
}