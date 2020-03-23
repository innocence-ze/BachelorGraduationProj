using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc4.Interfaces;
public interface IProjectData
{
    GameObject ThisGameObject { get; }
    IIfcProject ThisProject { get; set; }
    string Name { get; set; }
    List<IProductData> Products { get; set; }
    List<ISpatialData> SubSpatials { get; set; }
}
public class ProjectData : MonoBehaviour, IProjectData
{
    [SerializeField]
    private IIfcProject thisProject;
    [SerializeField]
    private string _name;
    [SerializeField]
    private List<IProductData> products = new List<IProductData>();
    [SerializeField]
    private List<ISpatialData> subSpatials = new List<ISpatialData>();

    public IIfcProject ThisProject { get => thisProject; set { if (value is IIfcProject) thisProject = value; } }
    public string Name { get => _name; set => _name = value; }
    public List<IProductData> Products { get => products; set => products = value; }
    public List<ISpatialData> SubSpatials { get => subSpatials; set => subSpatials = value; }
    public GameObject ThisGameObject => gameObject;
    public void AddProduct(IProductData p)
    {
        products.Add(p);
    }
    public void AddSubSpatial(ISpatialData s)
    {
        subSpatials.Add(s);
    }
}
