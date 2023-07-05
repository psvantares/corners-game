using Game.Utilities;
using UnityEngine;

namespace Game.Gameplay
{
    public class CheckersPool : ObjectPool<Checker>
    {
        private readonly Checker checker;
        private readonly Transform parent;

        public CheckersPool(Checker checker, Transform parent)
        {
            this.checker = checker;
            this.parent = parent;
        }

        protected override Checker CreateInstance()
        {
            var instance = Object.Instantiate(checker, parent, false);
            return instance;
        }
    }
}