using UnityEngine;


public class Fracture : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Public Variables:
    // -----------------
    //   Fractured
    // -------------------------------------------------------------------------

    #region .  Public Variables  .

    [Tooltip("\"Fractured\" is the object that this will break into")]
    public GameObject Fractured;

    #endregion



    // -------------------------------------------------------------------------
    // Public Methods:
    // ---------------
    //   FractureObject
    // -------------------------------------------------------------------------

    #region .  FractureObject()  .
    // -------------------------------------------------------------------------
    //   Method.......:  FractureObject()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Vector3
    // -------------------------------------------------------------------------
    public void FractureObject()
    {
        //print($"Fracture.FractureObject():  Fractured.name = {Fractured.name}, transform.position = {transform.position}");

        // Spawn the broken version of this asteroid.
        Instantiate(Fractured, transform.position, transform.rotation);

        // Hide this asteroid so it doesn't get in the way.
        //gameObject.SetActive(false);

        // Destroy this asteroid so it doesn't get in the way.
        Destroy(gameObject);

    }   // FractureObject()
    #endregion


}   // class Fracture
