using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface IObjectData
{
    IIfcObject ThisObject { get; set; }
    IObjectData RelatingObject { get; set; }
    List<IObjectData> RelatedObjects { get; set; }
    GameObject ThisGameObject { get; }

    string Name { get; set; }
    int EntityLabel { get; set; }
    string GlobalID { get; set; }
    void InitialObject(IIfcObject obj);
    void AddRelatedObjects(IObjectData obj);
}

public class ObjectData : MonoBehaviour, IObjectData
{
    private IIfcObject thisObject;
    private IObjectData relatingObject = null;
    private List<IObjectData> relatedObjects = new List<IObjectData>();

    private string _name;
    private int label;
    private string globalID;


    public IIfcObject ThisObject { get => thisObject; set => thisObject = value; }
    public GameObject ThisGameObject => gameObject;
    public IObjectData RelatingObject { get => relatingObject; set => relatingObject = value; }
    public List<IObjectData> RelatedObjects { get => relatedObjects; set => relatedObjects = value; }


    public string Name { get => _name; set => _name = value; }
    public int EntityLabel { get => label; set => label = value; }
    public string GlobalID { get => globalID; set => globalID = value; }

    public void AddRelatedObjects(IObjectData obj)
    {
        relatedObjects.Add(obj);
        obj.RelatingObject = this;
        obj.ThisGameObject.transform.parent = ThisGameObject.transform;
    }

    public virtual void InitialObject(IIfcObject ifcObj)
    {
        thisObject = ifcObj;

        _name = thisObject.Name;
        label = thisObject.EntityLabel;
        globalID = thisObject.GlobalId;
    }
}
