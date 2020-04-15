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
    public delegate GameObject ProductSelectEventHandler(int id);
    public void ProductSelected(int id)
    {
        if (OnProductsSelected != null)
        {
            var delArray = OnProductsSelected.GetInvocationList();
            selectedProduct = null;
            foreach(var del in delArray)
            {
                //ProductSelectEventHandler handler = del as ProductSelectEventHandler;
                selectedProduct = del.DynamicInvoke(id) as GameObject;
                if (selectedProduct != null)
                    break;
            }
        }
    }

    /*
    public event Func<int, GameObject> OnUiTreeButtonClicked;
    public void ButtonClicked(int id)
    {
        if (OnUiTreeButtonClicked != null)
        {
            var delArray = OnUiTreeButtonClicked.GetInvocationList();
            selectedProduct = null;
            foreach (var del in delArray)
            {
                //ProductSelectEventHandler handler = del as ProductSelectEventHandler;
                selectedProduct = del.DynamicInvoke(id) as GameObject;
                if (selectedProduct != null)
                    break;
            }
        }
    }*/

}
