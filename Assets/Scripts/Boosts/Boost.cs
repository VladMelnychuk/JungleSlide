using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Boosts
{
    public abstract class Boost : MonoBehaviour
    {
        // 4, 5
        [SerializeField] protected Vector3 spawnPosition;
        [SerializeField] protected Vector3 spawnPositionUI;

        [SerializeField] private Button activateBoost;
        [SerializeField] private Button spawnBoost;

        [SerializeField] protected Transform gfxComponent;
        [SerializeField] protected RectTransform uiComponent;

        [SerializeField] protected Board gameBoard;
        
        [SerializeField] protected int boostLayerId = 10;
        
        protected const string BoostTag = "boost";

        protected void Awake()
        {
            spawnPositionUI = Camera.main.WorldToScreenPoint(spawnPosition);
            activateBoost.onClick.AddListener(Interact);
            spawnBoost.onClick.AddListener(SpawnBoost);
            boostLayerId = LayerMask.NameToLayer("boost");
        }

        protected abstract void Interact();

        private void SpawnBoost()
        {
            gfxComponent.gameObject.SetActive(true);
            uiComponent.gameObject.SetActive(true);
            gfxComponent.position = spawnPosition;
        }

        private void Update()
        {

            if (gfxComponent.gameObject.activeSelf)
            {
                // make UI follow sprite
                uiComponent.position = Camera.main.WorldToScreenPoint(gfxComponent.position);
            }
            else
            {
                return;
            }
            
            if (!Input.GetMouseButtonDown(0)) return;
            
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(pos, Vector2.zero);
            
            if (hit.collider && hit.collider.gameObject.layer == boostLayerId)
            {
                StartCoroutine(MovingBlock());
            }
            
        }

        private IEnumerator MovingBlock()
        {
            Game.GameState = GameState.Paused;
            while (Input.GetMouseButton(0))
            {
                var fingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);;
                var intPos = Board.Vector3ToVector2Int(fingerPos);
                intPos.x = Mathf.Clamp(intPos.x, 0, 7);
                intPos.y = Mathf.Clamp(intPos.y, 0, 9);
                var newPos = new Vector3 {x = intPos.x, y = intPos.y};
//                gfxComponent.position = Camera.main.WorldToScreenPoint(newPos);
                gfxComponent.position = newPos;
                yield return null;
            }
            Game.GameState = GameState.Playing;
        }
    }
}