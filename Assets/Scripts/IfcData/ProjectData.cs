using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;
public interface IProjectData
{
    GameObject ThisGameObject { get; }
    IIfcProject ThisProject { get; set; }
    List<ISpatialData> RelatedProducts { get; set; }

    string Name { get; set; }
    string TypeName { get; set; }
    int EntityLabel { get; set; }
    string GlobalID { get; set; }
    string LongName { get; set; }
    string Phase { get; set; }

    void InitialProject(IIfcProject ifcProj);
    void AddRelatedProduct(ISpatialData spat);
}
public class ProjectData : MonoBehaviour, IProjectData
{
    [SerializeField]
    private IIfcProject thisProject;
    [SerializeField]
    private List<ISpatialData> relatedProducts = new List<ISpatialData>();

    [SerializeField]
    private string _name;
    [SerializeField]
    private string typeName;
    [SerializeField]
    private string globalID;
    [SerializeField]
    private string longName;
    [SerializeField]
    private string phase;
    [SerializeField]
    private int label;


    public IIfcProject ThisProject { get => thisProject; set { if (value is IIfcProject) thisProject = value; } }
    public List<ISpatialData> RelatedProducts { get => relatedProducts; set => relatedProducts = value; }
    public GameObject ThisGameObject => gameObject;

    public string Name { get => _name; set => _name = value; }
    public string TypeName { get => typeName; set => typeName = value; }
    public int EntityLabel { get => label; set => label = value; }
    public string GlobalID { get => globalID; set => globalID = value; }
    public string LongName { get => longName; set => longName = value; }
    public string Phase { get => phase; set => phase = value; }

    public void AddRelatedProduct(ISpatialData spat)
    {
        relatedProducts.Add(spat);
        spat.ThisGameObject.transform.parent = ThisGameObject.transform;
    }

    public void InitialProject(IIfcProject ifcProj)
    {
        thisProject = ifcProj;
        _name = thisProject.Name;
        typeName = thisProject.GetType().ToString();
        label = thisProject.EntityLabel;
        globalID = thisProject.GlobalId;
        longName = thisProject.LongName;
        phase = thisProject.Phase;

        SomeValue.project = this;
        ThisGameObject.name = Name;
    }
}
