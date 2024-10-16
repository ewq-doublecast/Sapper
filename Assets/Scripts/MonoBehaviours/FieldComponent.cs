using Core;
using UnityEngine;

namespace MonoBehaviours
{
    public class FieldComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _cellPrefab;
        [SerializeField] private float _spacing = 0.1f;
        [SerializeField] Vector3 _startCreatePosition = new Vector3(0, 0, 0);
    
        private readonly Field _field = new Field();

        private Vector3 _createPosition;

        private void Start()
        {
            _field.InitializeCompleted += OnFieldBuilt;

            _createPosition = _startCreatePosition;
            
            _field.Initialize();
        }

        private void OnDestroy()
        {
            _field.InitializeCompleted -= OnFieldBuilt;
        }

        private void OnFieldBuilt(Cell[,] grid)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                _createPosition.y = _startCreatePosition.y;
                
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    GameObject instantiate = Instantiate(_cellPrefab, _createPosition, Quaternion.identity);
                    instantiate.GetComponent<CellComponent>().Cell = grid[x, y];

                    _createPosition.y -= (_cellPrefab.transform.localScale.y + _spacing);
                }

                _createPosition.x += (_cellPrefab.transform.localScale.x + _spacing);
            }
        }
    }
}