using System.Collections;
using UnityEngine;

namespace Boosts
{
    public class TNT : MonoBehaviour
    {
        void Start()
        {
        }

        void Update()
        {
        }

        private void KeepInGrid()
        {
        }

        private IEnumerator MovingBlock(GameObject blockGameObject)
        {
            yield break;
//            var gameObjectPos = blockGameObject.transform.position;
//            var blockInitPosition =
//                new Vector2Int(Mathf.RoundToInt(gameObjectPos.x), Mathf.RoundToInt(gameObjectPos.y));
//
//            var block = _grid[blockInitPosition.x, blockInitPosition.y];
//
//            var borders = GetBorders(block);
//
//            while (Input.GetMouseButton(0))
//            {
//                var newX = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
//
//                if (newX >= borders.x && newX <= borders.y)
//                {
//                    // can move
//                    var newPos = new Vector3(newX, block.gridPosition.y, 0);
//                    block.transform.position = newPos;
//                }
//
//                yield return null;
//            }
//
//            if (gameObjectPos != block.transform.position)
//            {
//                // move completed
//                StartCoroutine(CompleteMove(block));
//            }
        }
    }
}