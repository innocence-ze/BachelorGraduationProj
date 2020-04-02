using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    public GameObject selectedProduct;

    private void Awake()
    {
        current = this;
    }

    public event Func<int,GameObject> OnProductsSelected;
    public void ProductSelected(int id)
    {
        if (OnProductsSelected != null)
        {
            selectedProduct = OnProductsSelected(id);
        }
    }

}
