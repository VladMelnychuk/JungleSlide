using System;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LevelCreation
{
    public class LevelCreator : MonoBehaviour
    {
        private GameObject _one;
        private GameObject _two;
        private GameObject _three;
        private GameObject _four;

        [SerializeField] private Transform blocksParent;
        [SerializeField] private int blockLayerId;
        [SerializeField] private Button saveLevel;
        [SerializeField] private InputField levelName;

        [Header("Element picker")] [SerializeField]
        private Button waterMode;

        [SerializeField] private Button earthMode;
        [SerializeField] private Button airMode;

        [Header("Spawn Buttons")] [SerializeField]
        private Button oneButton;

        [SerializeField] private Button twoButton;
        [SerializeField] private Button threeButton;
        [SerializeField] private Button fourButton;

        [Header("Prefabs")]

        #region Prefabs Serialization

        [SerializeField]
        private GameObject waterPrefab1;

        [SerializeField] private GameObject waterPrefab2;
        [SerializeField] private GameObject waterPrefab3;
        [SerializeField] private GameObject waterPrefab4;

        [SerializeField] private GameObject earthPrefab1;
        [SerializeField] private GameObject earthPrefab2;
        [SerializeField] private GameObject earthPrefab3;
        [SerializeField] private GameObject earthPrefab4;

        [SerializeField] private GameObject airPrefab1;
        [SerializeField] private GameObject airPrefab2;
        [SerializeField] private GameObject airPrefab3;
        [SerializeField] private GameObject airPrefab4;

        #endregion

        private void Start()
        {
            blockLayerId = LayerMask.NameToLayer("block");

            oneButton.onClick.AddListener(() =>
                Instantiate(_one, blocksParent.position, Quaternion.identity, blocksParent));
            twoButton.onClick.AddListener(() =>
                Instantiate(_two, blocksParent.position, Quaternion.identity, blocksParent));
            threeButton.onClick.AddListener(() =>
                Instantiate(_three, blocksParent.position, Quaternion.identity, blocksParent));
            fourButton.onClick.AddListener(() =>
                Instantiate(_four, blocksParent.position, Quaternion.identity, blocksParent));

            waterMode.onClick.AddListener(() =>
            {
                _one = waterPrefab1;
                _two = waterPrefab2;
                _three = waterPrefab3;
                _four = waterPrefab4;
            });
            earthMode.onClick.AddListener(() =>
            {
                _one = earthPrefab1;
                _two = earthPrefab2;
                _three = earthPrefab3;
                _four = earthPrefab4;
            });
            airMode.onClick.AddListener(() =>
            {
                _one = airPrefab1;
                _two = airPrefab2;
                _three = airPrefab3;
                _four = airPrefab4;
            });

            airMode.onClick.Invoke();

            saveLevel.onClick.AddListener(SaveLevel);
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit.collider && hit.collider.gameObject.layer == blockLayerId)
            {
                print(hit.collider.gameObject.name);
                StartCoroutine(MovingBlock(hit.collider.gameObject));
            }
            else
            {
                print("missed?");
            }
        }

        private IEnumerator MovingBlock(GameObject blockGameObject)
        {
            while (Input.GetMouseButton(0))
            {
                var newX = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
                var newY = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

                blockGameObject.transform.position = new Vector3(newX, newY, 0);

                yield return null;
            }
        }

        private void SaveLevel()
        {
            var levelJson = new JObject();
            var array = new JArray();

            foreach (Transform block in blocksParent)
            {
                var blockTransform = block.transform.position;
                var obj = new JObject
                {
                    ["name"] = block.gameObject.name.Substring(0, block.gameObject.name.Length - 7),
                    ["x"] = blockTransform.x,
                    ["y"] = blockTransform.y
                };

                array.Add(obj);
            }

            var fileTag = levelName.text;

            levelJson[fileTag] = array;
            print(levelJson.ToString());

            using (var file = File.CreateText("Assets/Levels/" + fileTag))
            using (var writer = new JsonTextWriter(file))
            {
                levelJson.WriteTo(writer);
            }
        }

        private void OnDestroy()
        {
//            SaveLevel();
        }
    }
}