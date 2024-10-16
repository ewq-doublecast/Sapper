using Core;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace MonoBehaviours
{
    public class CellComponent : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tmpText;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private float _rotateDuration = 0.5f;

        public Cell Cell;

        private void Start()
        {
            Cell.MineActivated += OnMineActivated;
            Cell.CellOpened += OnCellOpened;
            Cell.FlagSet += OnFlagSet;
            Cell.FlagRemoved += OnFlagRemoved;
            Cell.MessageReceived += OnMessageReceived;
        }

        private void OnDestroy()
        {
            Cell.MineActivated -= OnMineActivated;
            Cell.CellOpened -= OnCellOpened;
            Cell.FlagSet -= OnFlagSet;
            Cell.FlagRemoved -= OnFlagRemoved;
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cell.Open();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Cell.SetFlag();
            }
        }

        private void OnCellOpened(Cell cell)
        {
            SetState(Color.green, new Vector3(0, 180, 0));
        }

        private void OnMineActivated()
        {
            SetState(Color.red, new Vector3(0, 180, 0));
        }

        private void OnFlagSet()
        {
            SetState(Color.yellow, new Vector3(0, 180, 0));
        }

        private void OnFlagRemoved()
        {
            _tmpText.text = "";
            SetState(Color.white, new Vector3(0, 0, 0));
        }

        private void SetState(Color color, Vector3 rotateValue)
        {
            _renderer.material.color = color;
            transform.DORotate(rotateValue, _rotateDuration);
        }

        private void OnMessageReceived(string message)
        {
            _tmpText.text = message;
        }
    }
}