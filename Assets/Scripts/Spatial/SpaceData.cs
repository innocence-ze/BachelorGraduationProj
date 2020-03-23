using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc4.Interfaces; 
public interface ISpaceData : ISpatialData
{
} 
public class SpaceData : MonoBehaviour, ISpaceData/*,IIfcSpace*/
{
    [SerializeField]
    private IIfcSpace thisSpace;
    [SerializeField]
    private string _name;
    [SerializeField]
    private List<IProductData> products = new List<IProductData>();
    [SerializeField]
    private List<ISpatialData> none = new List<ISpatialData>();

    public IIfcSpatialStructureElement ThisStructure { get => thisSpace; set { if (value is IIfcSpace) thisSpace = (IIfcSpace)value; } }
    public string Name { get => _name; set => _name = value; }
    public List<IProductData> SubProducts { get => products; set => products = value; }
    public List<ISpatialData> SubSpatials { get => none; set => none = value; }
    public GameObject ThisGameObject { get => gameObject; }
    public int EntityLabel { get => ThisStructure.EntityLabel; }
    public void AddProduct(IProductData p)
    {
        products.Add(p);
    }
    public void AddSubSpatial(ISpatialData s)
    {
        
    }
}
