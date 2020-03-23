using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc4.Interfaces;
public interface IBuildingData: ISpatialData
{

}
public class BuildingData : MonoBehaviour, IBuildingData
{
    [SerializeField]
    private IIfcBuilding thisBuilding;
    [SerializeField]
    private string _name;
    [SerializeField]
    private List<IProductData> products = new List<IProductData>();
    [SerializeField]
    private List<ISpatialData> subSpatials = new List<ISpatialData>();

    public IIfcSpatialStructureElement ThisStructure { get => thisBuilding; set { if (value is IIfcBuilding) thisBuilding = (IIfcBuilding)value; } }
    public string Name { get => _name; set => _name = value; }
    public List<IProductData> SubProducts { get => products; set => products = value; }
    public List<ISpatialData> SubSpatials { get => subSpatials; set => subSpatials = value; }
    public GameObject ThisGameObject { get => gameObject; }
    public int EntityLabel { get => ThisStructure.EntityLabel; }
    public void AddProduct(IProductData p)
    {
        products.Add(p);
    }
    public void AddSubSpatial(ISpatialData s)
    {
        subSpatials.Add(s);
    }
}
