using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface IStairFlightData : IElementData
{
    string NumberOfRisers { get; set; }
    string NumberOfTreads { get; set; }
    string RiserHeight { get; set; }
    string TreadLength { get; set; }
}

public class StairFlightData : ElementData, IStairFlightData
{
    private IIfcStairFlight thisStairFlight;
    private string predefinedType;
    private string numberOfRisers;
    private string numberOfTreads;
    private string riserHeight;
    private string treadLength;

    public string PredefinedType { get => predefinedType; set => predefinedType = value; }
    public string NumberOfRisers { get => numberOfRisers; set => numberOfRisers = value; }
    public string NumberOfTreads { get => numberOfTreads; set => numberOfTreads = value; }
    public string RiserHeight { get => riserHeight; set => riserHeight = value; }
    public string TreadLength { get => treadLength; set => treadLength = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisStairFlight = ifcObj as IIfcStairFlight;

        if (thisStairFlight.NumberOfRiser.HasValue)
            numberOfRisers = thisStairFlight.NumberOfRiser.Value.ToString();
        if (thisStairFlight.NumberOfTreads.HasValue)
            numberOfTreads = thisStairFlight.NumberOfTreads.Value.ToString();
        if (thisStairFlight.RiserHeight.HasValue)
            riserHeight = thisStairFlight.RiserHeight.Value.ToString();
        if (thisStairFlight.TreadLength.HasValue)
            treadLength = thisStairFlight.TreadLength.Value.ToString();

        if (objType != null)
        {
            IIfcStairFlightType stairFlightType = objType as IIfcStairFlightType;
            predefinedType = stairFlightType.PredefinedType.ToString();
        }
        SetGeneralProperties();
    }

    public new void SetGeneralProperties()
    {
        generalProperties.Add("NumberOfRisers", numberOfRisers);
        generalProperties.Add("NumberOfTreads", numberOfTreads);
        generalProperties.Add("RiserHeight", riserHeight);
        generalProperties.Add("TreadLength", treadLength);
        if (objType != null)
        {
            generalProperties.Add("PredefinedType", predefinedType);
        }
    }
}
