using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface IRampFlightData : IElementData
{
    string PredefinedType { get; set; }
}

public class RampFlightData : ElementData, IRampFlightData
{
    private IIfcRampFlight thisRampFlight;
    private string predefinedType;

    public string PredefinedType { get => predefinedType; set => predefinedType = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisRampFlight = ifcObj as IIfcRampFlight;

        if (objType != null)
        {
            IIfcRampFlightType rampFlightType = objType as IIfcRampFlightType;
            predefinedType = rampFlightType.PredefinedType.ToString();
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
