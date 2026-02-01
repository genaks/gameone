using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class FlexibleGridLayout : MonoBehaviour
    {
        [Header("Grid Settings")]
        [SerializeField] private int columns = 3;
        [SerializeField] private int rows = 3;

        [Header("Spacing")]
        [SerializeField] private float cellSpacing = 10f;
        [SerializeField] private Vector2 padding = new Vector2(10f, 10f);

        [Header("Aspect Ratio")]
        [SerializeField] private bool maintainCellAspectRatio = true;
        [SerializeField] private float cellAspectRatio = 1f; // Width / Height ratio

        [Header("Auto Update")]
        [SerializeField] private bool updateOnScreenResize = true;

        private RectTransform _rectTransform;
        private Vector2 _lastScreenSize;
        private List<RectTransform> _childElements = new List<RectTransform>();

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _lastScreenSize = new Vector2(Screen.width, Screen.height);
        }

        private void Start()
        {
            UpdateGrid();
        }

        private void Update()
        {
            if (updateOnScreenResize)
            {
                Vector2 currentScreenSize = new Vector2(Screen.width, Screen.height);
                if (currentScreenSize != _lastScreenSize)
                {
                    _lastScreenSize = currentScreenSize;
                    UpdateGrid();
                }
            }
        }
        
        private void UpdateGrid()
        {
            CollectChildren();
            CalculateAndApplyLayout();
        }
        
        private void CollectChildren()
        {
            _childElements.Clear();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.gameObject.activeSelf)
                {
                    RectTransform childRect = child.GetComponent<RectTransform>();
                    if (childRect != null)
                    {
                        _childElements.Add(childRect);
                    }
                }
            }
        }
        
        private void CalculateAndApplyLayout()
        {
            if (columns <= 0 || rows <= 0)
            {
                Debug.LogWarning("Columns and rows must be greater than 0");
                return;
            }

            // Get available space
            Vector2 availableSize = _rectTransform.rect.size;
            float availableWidth = availableSize.x - (padding.x * 2) - (cellSpacing * (columns - 1));
            float availableHeight = availableSize.y - (padding.y * 2) - (cellSpacing * (rows - 1));

            // Calculate cell size
            float cellWidth = availableWidth / columns;
            float cellHeight = availableHeight / rows;

            // Maintain aspect ratio if enabled
            if (maintainCellAspectRatio)
            {
                float targetWidth = cellHeight * cellAspectRatio;
                float targetHeight = cellWidth / cellAspectRatio;

                if (targetWidth <= cellWidth)
                {
                    cellWidth = targetWidth;
                }
                else
                {
                    cellHeight = targetHeight;
                }
            }

            Vector2 cellSize = new Vector2(cellWidth, cellHeight);

            // Position each child
            for (int i = 0; i < _childElements.Count; i++)
            {
                int row = i / columns;
                int column = i % columns;

                // Calculate position (from top-left)
                float xPos = padding.x + (column * (cellWidth + cellSpacing)) + (cellWidth * 0.5f);
                float yPos = -padding.y - (row * (cellHeight + cellSpacing)) - (cellHeight * 0.5f);

                RectTransform child = _childElements[i];

                // Set anchors to top-left
                child.anchorMin = new Vector2(0, 1);
                child.anchorMax = new Vector2(0, 1);
                child.pivot = new Vector2(0.5f, 0.5f);

                // Set size and position
                child.sizeDelta = cellSize;
                child.anchoredPosition = new Vector2(xPos, yPos);
            }
        }
        
        public void SetColumns(int newColumns)
        {
            if (newColumns > 0)
            {
                columns = newColumns;
                UpdateGrid();
            }
        }
        
        public void SetRows(int newRows)
        {
            if (newRows > 0)
            {
                rows = newRows;
                UpdateGrid();
            }
        }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Update in editor when values change
        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();
        
        UpdateGrid();
    }
#endif
    }
}

