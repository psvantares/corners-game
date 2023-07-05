using Game.Models;

namespace Game.Gameplay
{
    public class BoardContext
    {
        public readonly IGameModel GameModel;
        public readonly BoardConfig Config;
        public readonly PoolController Pool;

        public BoardContext
        (
            IGameModel gameModel,
            BoardConfig config,
            PoolController pool
        )
        {
            GameModel = gameModel;
            Config = config;
            Pool = pool;
        }
    }
}