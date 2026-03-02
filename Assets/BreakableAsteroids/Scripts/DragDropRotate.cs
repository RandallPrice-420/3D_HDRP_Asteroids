using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


// This ensures that any GameObject with this script will have a Collider
[RequireComponent(typeof(Collider))]
public class DragDropRotate : MonoBehaviour
{
    // =========================================================================
    // Instructions:
    //   Add this script to each of the GameObjects you want to drag and drop.
    //   
    //   The GameObjects you want to drag and drop must have a Collider component
    //   (it can be any of the Collider components).
    //
    // Input:
    //   Unity Input System (New)
    //
    // Reference:
    //   Bing Copilot Search:  unity drag and drop 3d using input system
    // =========================================================================


    // -------------------------------------------------------------------------
    // Public Variables:
    // -----------------
    //   MouseDragSpeed
    //   RotateSpeed
    // -------------------------------------------------------------------------

    #region .  Public Variables  .

    public float MouseDragSpeed = 0.1f;
    public float RotateSpeed    = 20f;

    #endregion



    // -------------------------------------------------------------------------
    // SerializeField Variables:
    // -------------------------
    //   _mouseLeftClick
    //   _mouseRightClick
    //   _mousePosition
    // -------------------------------------------------------------------------

    #region .  SerializeField Variables  .

    [Header("Input Actions")]
    [SerializeField] private InputAction _mouseLeftClick;
    [SerializeField] private InputAction _mouseRightClick;
    [SerializeField] private InputAction _mousePosition;

    #endregion



    // -------------------------------------------------------------------------
    // Private Properties:
    // -------------------
    //   _curScreenPosition
    //   _isDragging
    //   _isRotating
    //   _mainCamera
    //   _meshCollider
    //   _startMousePosition
    //   _TMP_ObjectName
    //
    //   _isClickedOn
    //   _worldPosition
    // -------------------------------------------------------------------------

    #region .  Private Properties  .

    private Vector3      _curScreenPosition;
    private bool         _isDragging;
    private bool         _isRotating;
    private Camera       _mainCamera;
    private MeshCollider _meshCollider;
    private float        _startMousePosition;
    private TMP_Text     _TMP_ObjectName = null;

    private bool _isClickedOn
    {
        get
        {
            Ray ray = this._mainCamera.ScreenPointToRay(this._curScreenPosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.transform == this.transform;
            }
            return false;
        }
    }

    private Vector3 _worldPosition
    {
        get
        {
            float   z = this._mainCamera.WorldToScreenPoint(this.transform.position).z;
            Vector3 w = this._mainCamera.ScreenToWorldPoint(this._curScreenPosition + new Vector3(0, 0, z));

            return w;
        }
    }

    #endregion



    // -------------------------------------------------------------------------
    // Private Methods:
    // ----------------
    //   Awake()
    //   DoMousePress()
    //   DragUpdate()
    //   OnDisable()
    //   OnEnable()
    //   RotateUpdate()
    // -------------------------------------------------------------------------

    #region .  Awake()  .
    // -------------------------------------------------------------------------
    //   Method.......:  Awake()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Vector3
    // -------------------------------------------------------------------------
    private void Awake()
    {
        this._mainCamera   = Camera.main;
        this._meshCollider = GetComponent<MeshCollider>();

        try
        {
            this._TMP_ObjectName      = GameObject.Find("TMP_ObjectName").GetComponent<TMP_Text>();
            this._TMP_ObjectName.text = "";
        }
        catch { }

    }   // Awake()
    #endregion


    #region .  DoMousePress()  .
    // -------------------------------------------------------------------------
    //   Method.......:  DoMousePress()
    //   Description..:  
    //   Parameters...:  None
    //   Notes........:  Uses new Input System instead of old Input Manager.
    //   Returns......:  Vector3
    // -------------------------------------------------------------------------
    private void DoMousePress(string coroutineName)
    {
        Ray ray = this._mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Physics.Raycast(ray, out RaycastHit hit);

        if (hit.collider != null)
        {
            if (this._TMP_ObjectName != null)
            {
                this._TMP_ObjectName.text = hit.collider.name;
            }

            StartCoroutine(coroutineName, hit.collider.gameObject);
        }

    }   // DoMousePress()
    #endregion


    #region .  DragUpdate()  .
    // -------------------------------------------------------------------------
    //   Method.......:  DragUpdate()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Nothing
    // -------------------------------------------------------------------------
    private IEnumerator DragUpdate()
    {
        this._isDragging = true;

        float   yCoordinate = this.transform.position.y;
        Vector3 offset      = this.transform.position - this._worldPosition;

        while (this._isDragging)
        {
            Vector3 newPosition     = this._worldPosition + offset;
            newPosition.y           = yCoordinate;
            this.transform.position = newPosition;

            yield return null;
        }

    }   // DragUpdate()
    #endregion


    #region .  OnDisable()  .
    // -------------------------------------------------------------------------
    //   Method.......:  OnDisable()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Vector3
    // -------------------------------------------------------------------------
    private void OnDisable()
    {
        this._mouseLeftClick.Disable();
        this._mouseLeftClick.performed -= _ => { if (this._isClickedOn) DoMousePress("DragUpdate"); };
        this._mouseLeftClick.canceled  -= _ => { this._isDragging = false; };

        this._mouseRightClick.canceled -= _ => { this._isRotating = false; };

        this._mousePosition.Disable();
        this._mousePosition.performed  -= context => { this._curScreenPosition = context.ReadValue<Vector2>(); };

    }   // OnDisable()
    #endregion


    #region .  OnEnable()  .
    // -------------------------------------------------------------------------
    //   Method.......:  OnEnable()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Vector3
    // -------------------------------------------------------------------------

    private void OnEnable()
    {
        this._mouseLeftClick.Enable();
        this._mouseLeftClick.performed  += _ => { if (this._isClickedOn) DoMousePress("DragUpdate"); };
        this._mouseLeftClick.canceled   += _ => { this._isDragging = false; };

        this._mouseRightClick.Enable();
        this._mouseRightClick.performed += _ => { if (this._isClickedOn) DoMousePress("RotateUpdate"); };
        this._mouseRightClick.canceled  += _ => { this._isRotating = false; };

        this._mousePosition.Enable();
        this._mousePosition.performed   += context => { this._curScreenPosition = context.ReadValue<Vector2>(); };

    }   // OnEnable()
    #endregion


    #region .  RotateUpdate()  .
    // -------------------------------------------------------------------------
    //   Method.......:  RotateUpdate()
    //   Description..:  Rotates the given GameObject around the X axis based on
    //                   the mouse movement.  If FreezeRotationY is true, the
    //                   rotation will be frozen.
    //   Parameters...:  GameObject:  the object to rotate
    //   Notes........:  Uses new Input System instead of old Input Manager.
    //   Returns......:  Vector3
    // -------------------------------------------------------------------------
    private IEnumerator RotateUpdate(GameObject clickedObject)
    {
        while (_mouseRightClick.ReadValue<float>() != 0)
        {
            float mousePositionX = Mouse.current.position.ReadValue().x;
            float mouseMovement  = mousePositionX - this._startMousePosition;

            Vector3 direction = (this._meshCollider == null)
                              ? Vector3.up
                              : Vector3.forward;

            clickedObject.transform.Rotate(direction, -mouseMovement * this.RotateSpeed * Time.deltaTime);
            this._startMousePosition = mousePositionX;

            yield return null;
        }

    }   // RotateUpdate()
    #endregion


}   // class DragDropRotate
