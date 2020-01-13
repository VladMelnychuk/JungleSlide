using System;
using UnityEngine;

namespace Boosts
{
    public class ExtraBlock : Boost
    {

        [SerializeField] private GameObject boostBlock;

        private void Start()
        {
            gameBoard.objectpool.AddObjectPool(BoostTag, boostBlock, gameBoard.blocksHolder, 5);
        }

        protected override void Interact()
        {
            gameBoard.objectpool[BoostTag].Spawn(spawnPosition, Quaternion.identity);
        }

    }
}
