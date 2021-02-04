using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(RectTransform), typeof(GridLayoutGroup))]
public class DynamicGrid : MonoBehaviour
{
    private RectTransform parent;
    private GridLayoutGroup grid;

    private float lastWidth;
    private float lastHeight;
    private float cellAspectRatio;

    public bool scaleOnlyOnStartup = true;
    public bool keepAspectRatio = false;
    public bool keepSpacingZero = true;

    void Start()
    {
        parent = GetComponent<RectTransform>();
        grid = GetComponent<GridLayoutGroup>();
        if (parent == null || grid == null)
        {
            Debug.LogError("UIFlexibleGridController couldn't find a RectTransform or a GridLayoutGroup");
            return;
        }
        lastWidth = parent.rect.width;
        lastHeight = parent.rect.height;
        cellAspectRatio = parent.rect.width / parent.rect.height;
        if (grid.constraint == GridLayoutGroup.Constraint.FixedColumnCount || grid.constraint == GridLayoutGroup.Constraint.FixedRowCount)
        {
            UpdateCellSizes();
        }
        else
        {
            Debug.LogWarning("GridLayoutGroup contraints do not make this UIFlexibleGridController necessary. Consider removing it.");
            return;
        }
    }
    void Update()
    {
        if (scaleOnlyOnStartup) return;
        if (!HasSizedChanged()) return;
        lastWidth = parent.rect.width;
        lastHeight = parent.rect.height;
        UpdateCellSizes();
    }
    private bool HasSizedChanged()
    {
        return lastHeight != parent.rect.height || lastWidth != parent.rect.width;
    }
    private string GameObjectPathName(Transform t)
    {
        if (t.parent == null)
        {
            return t.name;
        }
        else
        {
            return GameObjectPathName(t.parent) + "/" + t.name;
        }
    }
    private void UpdateCellSizes()
    {
        float w = 0f;
        float h = 0f;
        float sx = 0f;
        float sy = 0f;
        w = (parent.rect.width / (float)grid.constraintCount);
        h = (parent.rect.height / (float)grid.constraintCount);
        if (w == 0 || h == 0)
        {
            Debug.LogError(string.Format("Invalid width ({0}) or height ({1}) at {2}", w, h, GameObjectPathName(this.transform)));
            return;
        }
        if (!keepSpacingZero)
        {
            w = (int)w;
            h = (int)h;
        }
        if (keepAspectRatio)
        {
            h = w * cellAspectRatio;
        }
        Vector2 newSize = grid.cellSize;
        newSize.x = w * 2;
        newSize.y = h;
        grid.cellSize = newSize;
        if (!keepSpacingZero && grid.constraintCount != 1)
        {
            sx = (parent.rect.width - (w * grid.constraintCount)) / (float)(grid.constraintCount - 1);
            sy = (parent.rect.height - (w * grid.constraintCount)) / (float)(grid.constraintCount - 1);
        }
        Vector2 newSpacing = grid.spacing;
        newSpacing.x = sx;
        newSpacing.y = sy;
        grid.spacing = newSpacing;
    }
}
