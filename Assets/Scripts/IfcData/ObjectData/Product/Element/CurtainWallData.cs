using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface ICurtainWallData : IElementData
{
    string PredefinedType { get; set; }
}

public class CurtainWallData : ElementData, ICurtainWallData
{
    private IIfcCurtainWall thisCurtainWall;
    private string predefinedType;

    public string PredefinedType { get => predefinedType; set => predefinedType = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisCurtainWall = ifcObj as IIfcCurtainWall;

        if (objType != null)
        {
            IIfcCurtainWallType curtainWallType = objType as IIfcCurtainWallType;
            predefinedType = curtainWallType.PredefinedType.ToString();
        }
        SetGeneralProperties();
    }

    public new void SetGeneralProperties()
    {
        if (objType != null)
        {
            generalProperties.Add("PredefinedType", predefinedType);
        }
    }
}
