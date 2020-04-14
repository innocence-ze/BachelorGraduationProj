using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductController : MonoBehaviour
{
    public int id;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.OnProductsSelected += OnSelected;
    }

    GameObject OnSelected(int id)
    {
        if (id != this.id)
            return null;

        Debug.Log(gameObject.name);
        return gameObject;
    }

    private void OnDestroy()
    {
        GameEvents.current.OnProductsSelected -= OnSelected;
    }

    private void OnMouseDown()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;
        GameEvents.current.ProductSelected(id);
    }
}
