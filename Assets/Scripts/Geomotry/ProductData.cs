using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProductData
{
     MyBimProduct ProductGeoData { get; set; }
     string TypeName { get; set; }
     string ProductName { get; set; }
}

public class ProductData : MonoBehaviour, IProductData
{
    [SerializeField]
    private MyBimProduct product;
    [SerializeField]
    private string typeName;
    [SerializeField]
    private string productName;

    public MyBimProduct ProductGeoData { get => product; set => product = value; }
    public string TypeName { get => typeName; set => typeName = value; }
    public string ProductName { get => productName; set => productName = value; }
}
