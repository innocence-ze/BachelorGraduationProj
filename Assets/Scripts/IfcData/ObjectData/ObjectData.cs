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

    Dictionary<string,string> GetGeneralProperties();
    void SetGeneralProperties();

    Dictionary<string, Dictionary<string, string>> GetProperties();
}

public class ObjectData : MonoBehaviour, IObjectData
{
    private IIfcObject thisObject;
    private IObjectData relatingObject = null;
    private List<IObjectData> relatedObjects = new List<IObjectData>();

    private string _name;
    private int label;
    private string globalID;

    protected Dictionary<string, string> generalProperties = new Dictionary<string, string>();
    private Dictionary<string, Dictionary<string, string>> properties = new Dictionary<string, Dictionary<string, string>>();

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

    public virtual Dictionary<string,Dictionary<string,string>> GetProperties()
    {
        return properties;
    }

    protected virtual void SetProperties(IIfcObject ifcObj)
    {
        //get define relationship
        foreach(var define in ifcObj.IsDefinedBy)
        {
            // is defined by property?
            if (define is IIfcRelDefinesByProperties property)
            {
                //only ifcpropertyset have data
                if (property.RelatingPropertyDefinition is IIfcPropertySet propSet)
                {
                    // one set is a set of properties
                    Dictionary<string, string> propDatas = new Dictionary<string, string>();
                    //name of the set
                    var propName = propSet.Name;
                    //traversal properties in the set
                    foreach (var d in propSet.HasProperties)
                    {
                        if (d is IIfcPropertySingleValue data)
                        {
                            if (data.NominalValue.Value != null)
                                propDatas.Add(data.Name, data.NominalValue.Value.ToString());
                            else
                                propDatas.Add(data.Name, "Unknown");
                        }
                    }

                    if (properties.ContainsKey(propName))
                        foreach (var p in propDatas)
                            properties[propName].Add(p.Key + " ", p.Value);
                    else
                        properties.Add(propName.ToString(), propDatas);
                }
            }
        }
    }

    public virtual Dictionary<string, string> GetGeneralProperties()
    {
        return generalProperties;
    }

    public virtual void SetGeneralProperties()
    {
        generalProperties.Add("Name", _name);
        generalProperties.Add("Label", label.ToString());
        generalProperties.Add("GlobalID", globalID);
    }

    public virtual void InitialObject(IIfcObject ifcObj)
    {
        thisObject = ifcObj;

        SetProperties(thisObject);

        _name = thisObject.Name;
        label = thisObject.EntityLabel;
        globalID = thisObject.GlobalId;

        SetGeneralProperties();
    }
}
