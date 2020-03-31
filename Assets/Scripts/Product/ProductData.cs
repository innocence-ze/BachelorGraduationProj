using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc4.Interfaces;
[SerializeField]
public interface IProductData
{
     MyBimProduct ProductGeoData { get; set; }
     string TypeName { get; set; }
     string ProductName { get; set; }
    GameObject ThisGameObject { get; }
    bool HaveSpatialStructure { get; set; }
    IIfcProduct ThisProduct { get; set; }
    List<IProductData> DecomposedProducts { get; set; }
}
[SerializeField]
public class ProductData : MonoBehaviour, IProductData
{
    [SerializeField]
    private MyBimProduct product;
    [SerializeField]
    private string typeName;
    [SerializeField]
    private string productName;
    [SerializeField]
    private bool haveSpatialStructure = false;
    [SerializeField]
    private IIfcProduct thisProduct;
    [SerializeField]
    private List<IProductData> decomposedProducts = new List<IProductData>();

    public MyBimProduct ProductGeoData { get => product; set => product = value; }
    public string TypeName { get => typeName; set => typeName = value; }
    public string ProductName { get => productName; set => productName = value; }
    public GameObject ThisGameObject => gameObject;
    public bool HaveSpatialStructure { get => haveSpatialStructure; set => haveSpatialStructure = value; }
    public IIfcProduct ThisProduct { get => thisProduct; set => thisProduct = value; }
    public List<IProductData> DecomposedProducts { get => decomposedProducts; set => decomposedProducts = value; }
}
