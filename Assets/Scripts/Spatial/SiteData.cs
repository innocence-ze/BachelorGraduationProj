using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc4.Interfaces; 
public interface ISiteData : ISpatialData
{

} 
public class SiteData : MonoBehaviour, ISiteData
{
    [SerializeField]
    private IIfcSite thisSite;
    [SerializeField]
    private string _name;
    [SerializeField]
    private List<IProductData> products = new List<IProductData>();
    [SerializeField]
    private List<ISpatialData> subSpatials = new List<ISpatialData>();

    public IIfcSpatialStructureElement ThisStructure { get => thisSite; set { if (value is IIfcSite) thisSite = (IIfcSite)value; } }
    public string Name { get => _name; set => _name = value; }
    public List<IProductData> SubProducts { get => products; set => products = value; }
    public GameObject ThisGameObject { get => gameObject; }
    public List<ISpatialData> SubSpatials { get => subSpatials; set => subSpatials = value; }
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
