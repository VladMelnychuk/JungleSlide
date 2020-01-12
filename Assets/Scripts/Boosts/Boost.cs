using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Boosts
{
    public abstract class Boost : MonoBehaviour
    {
        // 4, 5
        [SerializeField] private Vector3 spawnPosition;
        [SerializeField] private Vector3 spawnPositionUI;

        [SerializeField] private Button interact;
        [SerializeField] private Button spawnBoost;

        [SerializeField] protected RectTransform gfxComponent;

        [SerializeField] protected Board gameBoard;

        private void Start()
        {
            spawnPositionUI = Camera.main.WorldToScreenPoint(spawnPosition);
            interact.onClick.AddListener(Interact);
            spawnBoost.onClick.AddListener(SpawnBoost);
        }

        protected abstract void Interact();

        private void SpawnBoost()
        {
            gfxComponent.gameObject.SetActive(true);
            gfxComponent.position = spawnPositionUI;
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            if (gfxComponent.gameObject.activeSelf)
            {
                StartCoroutine(MovingBlock());
            }
            
        }

        private IEnumerator MovingBlock()
        {
            yield return new WaitForSeconds(.5f);
            Game.GameState = GameState.Paused;
            while (Input.GetMouseButton(0))
            {
                var fingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);;
                var intPos = Board.Vector3ToVector2Int(fingerPos);
                var newPos = new Vector3 {x = intPos.x, y = intPos.y};
                gfxComponent.position = Camera.main.WorldToScreenPoint(newPos);
                yield return null;
            }
            Game.GameState = GameState.Playing;
        }
    }
}