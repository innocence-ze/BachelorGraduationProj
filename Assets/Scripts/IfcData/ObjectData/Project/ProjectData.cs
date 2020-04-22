using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;
public interface IProjectData:IObjectData
{
    IIfcProject ThisProject { get; set; }

    string TypeName { get; set; }
    string LongName { get; set; }
    string Phase { get; set; }

}
public class ProjectData : ObjectData, IProjectData
{
    [SerializeField]
    private IIfcProject thisProject;

    [SerializeField]
    private string typeName;
    [SerializeField]
    private string longName;
    [SerializeField]
    private string phase;

    public IIfcProject ThisProject { get => thisProject; set =>thisProject = value;  }

    public string TypeName { get => typeName; set => typeName = value; }
    public string LongName { get => longName; set => longName = value; }
    public string Phase { get => phase; set => phase = value; }


    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisProject = ifcObj as IIfcProject;
        typeName = thisProject.GetType().Name;
        longName = thisProject.LongName;
        phase = thisProject.Phase;

        SomeValue.project = this;
        ThisGameObject.name = Name + "[" + TypeName + "]#" + EntityLabel;

        SetGeneralProperties();
    }

    public new void SetGeneralProperties()
    {
        generalProperties.Add("TypeName", typeName);
        generalProperties.Add("LongName", longName);
        generalProperties.Add("Phase", phase);
    }
}
