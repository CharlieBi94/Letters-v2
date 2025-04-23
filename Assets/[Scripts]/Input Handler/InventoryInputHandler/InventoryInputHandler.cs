using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class used to handle inputs from the inventory slots
/// </summary>
public class InventoryInputHandler : Singleton<InventoryInputHandler>
{
    [SerializeField]
    PlayAreaController playArea;

    [SerializeField]
    RectTransform mainUI;

    [SerializeField]
    Animator animator;

    [SerializeField]
    Vector2 leftOffset;

    [SerializeField]
    Vector2 rightOffset;

    // Events
    public Action<RowController, bool> LetterAdded;

    // These are populated when a player 'selects' the letter from the inventory
    private GameObject tile;
    private RectTransform tileRect;
    private LayoutElement layout;
    private bool canPlaceMiddle;

    // Remembers row rect of last frame
    RowController prevRow;

    // Remembers the tile position
    RowController tileOriginalParent;
    int tileOriginalIndex = -1;

    [SerializeField]
    RectTransform canvas;
    [SerializeField]
    Camera mainCamera;

    private void Start()
    {
        if(canvas == null)
        {
            canvas = FindFirstObjectByType<Canvas>().GetComponent<RectTransform>();
        }
    }
    /// <summary>
    /// Moves the selected item to a valid location depending on mouse location and board state
    /// </summary>
    public void PlaceTile()
    {
        // don't do anything unless there is a gameobject populated
        if (tile == null) { return; }

        RowController targetRow;
        int targetIndex;

        // First try to find tile as Target
        Collider2D collider = GetNearestCollider(1 << 6);
        if(collider != null)
        {
            targetRow = collider.gameObject.GetComponentInParent<RowController>();

            // Check to see if we care about placing things between tiles in the row
            if(canPlaceMiddle)
            {
                Tile targetTile = collider.gameObject.GetComponent<Tile>();
                // If we need to set a specific position, then we need to find the index of target tile
                targetIndex = targetRow.GetPositionIndex(targetTile);
                // Check to see if mouse is left or right of the target tile
                if (!IsLeft(collider.gameObject.GetComponent<RectTransform>()))
                {
                    // Need to add 1 to the position if we want to place it to the right
                    targetIndex++;
                }
            }
            else
            {
                // Regardless of where we are going to position it, we need to get the parent rect
                RectTransform rowRect = targetRow.gameObject.GetComponent<RectTransform>();
                // Check to see if mouse is left or right of the center of the row (parent)
                if (IsLeft(rowRect))
                {
                    targetIndex = 0;
                }
                else
                {
                    targetIndex = targetRow.Count(tile.GetComponent<Tile>());
                }
            }
            prevRow = targetRow;
            playArea.AddTile(new(tile.GetComponent<Tile>(), targetRow, targetIndex));
            return;
        }

        // If we didn't find a tile, try to find a row instead
        collider = GetNearestCollider(1 << 7);
        if (collider != null)
        {

            targetRow = collider.GetComponent<RowController>();
            // If we found a row, then first check if the row is the same as last time we tried to position tile
            if (prevRow == targetRow) return;
            RectTransform rowRect = collider.GetComponent<RectTransform>();            
            // Determine if we should place the tile to the left or right
            if (IsLeft(rowRect))
            {
                targetIndex = 0;
            }
            else
            {
                targetIndex = targetRow.Count(tile.GetComponent<Tile>());
            }
            prevRow = targetRow;
            playArea.AddTile(new(tile.GetComponent<Tile>(), targetRow, targetIndex));            
            return;
        }
        // If we can't find a row or a tile, then make the tile follow the mouse
        else
        {
            
            prevRow = null;
            PlaceAtMouse();
        }
    }

    private void PlaceAtMouse()
    {

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas,
            Input.mousePosition,
            Camera.main,
            out Vector2 pos);

        // if setting it to the canvas, ignore the layout and display its preferred size
        
        tile.transform.SetParent(canvas, false);
        layout.ignoreLayout = true;
        tile.transform.position = canvas.TransformPoint(pos);
        tileRect.sizeDelta = new Vector2(layout.preferredWidth, layout.preferredHeight);    
    }

    private Collider2D GetNearestCollider(int layerMask)
    {
        // Get mouse position in screen space
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Convert mouse screen position to a ray from the camera
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPosition);

        // Visualize the ray in the Scene view
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);

        // Perform the raycast
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, layerMask);
        if(hit.collider == null){ return null; }
        return hit.collider;
    }

    /// <summary>
    /// A function that calculates if mouse is to the right or left of the target rect
    /// </summary>
    /// <param name="target">The rect of the obj the tile will be parented under</param>
    /// <returns>True if left, false if right</returns>
    private bool IsLeft(RectTransform target)
    {
        RectTransform rectParent = target.GetComponentInParent<RectTransform>();
        //Get the mouse position and convert it to position of parent rect
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectParent,
            Input.mousePosition,
            Camera.main,
            out Vector2 mousePos);
        return mousePos.x < 0;
    }

    public void HandleInventoryPointerDown(string content, bool placeMiddle)
    {
        // Only allow the player to add an tile when in play or in god mode
        if (GameManager.Instance.CurrentState != GameManager.GameState.IN_PLAY && GameManager.Instance.CurrentState != GameManager.GameState.GOD_MODE) return;
        // Check to ensure current tile is empty
        if(tile != null)
        {
            CancelInventoryBehaviour();
        }
        // Populate all the properties of the tile
        tile = TileFactory.Instance.SpawnTile(canvas, true, content[0]);
        PopulateFields(tile, false, placeMiddle);
    }

    private void CancelInventoryBehaviour()
    {
        if (tile != null)
        {
            // Check to see if the position is valid by ensuring its parent is the row
            var parentRow = tile.GetComponentInParent<RowController>();
            if(parentRow != null)
            {
                // If position is valid, lock it in by setting the tile to active
                tile.GetComponent<Tile>().SetState(Tile.TileState.IN_PLAY);
                tile.layer = 6;
                if(tileOriginalParent != null)
                {
                    LetterAdded(parentRow, false);
                    // Also notify original parent (if they exist) that a tile was removed
                    LetterAdded(tileOriginalParent, false);
                }
                else
                {
                    LetterAdded(parentRow, true);
                }
                // Calculate position:
                Vector2 tilePos = tile.GetComponent<RectTransform>().position;
                // Figure out if tile position is left or right of the screen
                if(tilePos.x < 0)
                {
                    Vector3 scale = animator.transform.localScale;
                    if(scale.x > 0)
                    {
                        scale.x *= -1;
                    }
                    animator.transform.localScale = scale;
                    animator.transform.position = tilePos + leftOffset;
                }
                else
                {
                    Vector3 scale = animator.transform.localScale;
                    if (scale.x < 0)
                    {
                        scale.x *= -1;
                    }
                    animator.transform.localScale = scale;
                    animator.transform.position = tilePos + rightOffset;
                }

                //Play Animation                
                animator.Play("CatSlap", -1, 0f);
                

            }
            else if (tileOriginalParent != null)
            {
                tile.transform.SetParent(tileOriginalParent.GetContainer(), false);
                tile.transform.SetSiblingIndex(tileOriginalIndex);
                tile.GetComponent<Tile>().SetState(Tile.TileState.IN_PLAY);
                tile.layer = 6;
                layout.ignoreLayout = false;
                // Reset these fields after reverting tile back to its original position
                tileOriginalParent = null;
                tileOriginalIndex = -1;
            }
            else
            {
                Destroy(tile);
            }
            // removes reference to tile
            tile = null;
            // removes reference to tileRect
            tileRect = null;
            // removes reference to layout component
            layout = null;
            // resets to default behaviour
            canPlaceMiddle = false;
            // Stops remembering prev row
            prevRow = null;
        }
    }
    public void HandleInventoryPointerUp()
    {
        CancelInventoryBehaviour();
        //Destroy(tile);
    }

    /// <summary>
    /// Tries to move the target tile
    /// </summary>
    /// <param name="t">Target tile</param>
    /// <returns>True if able to move, else False</returns>
    public bool HandleTilePointerDown(Tile t)
    {
        // Can only move tiles in GOD-MODE
        if (GameManager.Instance.CurrentState != GameManager.GameState.GOD_MODE) return false;
        // Cannot move system placed tiles
        if (!t.playerAdded) return false;        
        tile = t.gameObject;
        // You can always place the tile anywhere
        PopulateFields(t.gameObject, true, true);
        // Remove the tile (temporarily) from hiearchy, this well stop weird placement behaviour 
        tileOriginalParent.RemoveTile(t);
        return true;
    }

    private void PopulateFields(GameObject tile, bool existingTile, bool placeMiddle)
    {
        if (existingTile)
        {
            Tile t = tile.GetComponent<Tile>();
            // Remember the orginal index and parent
            tileOriginalParent = t.GetParentRow();
            tileOriginalIndex = tileOriginalParent.GetPositionIndex(t);
        }
        tileRect = tile.GetComponent<RectTransform>();
        layout = tile.GetComponent<LayoutElement>();
        canPlaceMiddle = placeMiddle;
        if(!placeMiddle)
        {
            // Check to see if we are in god mode, if so, we can place any tile between other tiles locations
            if (GameManager.Instance.CurrentState == GameManager.GameState.GOD_MODE)
            {
                canPlaceMiddle = true;
            }
        }
        // Sets the phantom tile to default layer so it isn't being detected by raycast
        tile.layer = 0;
    }
    public void HandleTilePointerUp()
    {
        CancelInventoryBehaviour();
    }

}
