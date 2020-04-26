using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface IBeamData : IElementData
{
    string PredefinedType { get; set; }
}

public class BeamData : ElementData, IBeamData
{
    private IIfcBeam thisBeam;
    private string predefinedType;

    public string PredefinedType { get => predefinedType; set => predefinedType = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisBeam = ifcObj as IIfcBeam;

        if (objType != null)
        {
            IIfcBeamType beamType = objType as IIfcBeamType;
            predefinedType = beamType.PredefinedType.ToString();
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
