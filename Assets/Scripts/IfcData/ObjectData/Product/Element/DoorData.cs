using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface IDoorData
{
    string Height { get; set; }
    string Width { get; set; }

    string ConstructionType { get; set; }
    string OperationType { get; set; }
    string ParameterTakesPrecedence { get; set; }
    string Sizeable { get; set; }
}

public class DoorData : ElementData, IDoorData
{
    private IIfcDoor thisDoor;
    private string height;
    private string width;

    private string constructionType;
    private string operationType;
    private string parameterTakesPrecedence;
    private string sizeable;

    public string Height { get => height; set => height = value; }
    public string Width { get => width; set => width = value; }

    public string ConstructionType { get => constructionType; set => constructionType = value; }
    public string OperationType { get => operationType; set => operationType = value; }
    public string ParameterTakesPrecedence { get => parameterTakesPrecedence; set => parameterTakesPrecedence = value; }
    public string Sizeable { get => sizeable; set => sizeable = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisDoor = ifcObj as IIfcDoor;
        if (thisDoor.OverallHeight.HasValue)
            height = thisDoor.OverallHeight.Value.ToString();
        if (thisDoor.OverallWidth.HasValue)
            width = thisDoor.OverallWidth.Value.ToString();
        if (objType != null)
        {
            IIfcDoorStyle doorType = objType as IIfcDoorStyle;
            constructionType = doorType.ConstructionType.ToString();
            operationType = doorType.OperationType.ToString();
            parameterTakesPrecedence = doorType.ParameterTakesPrecedence.ToString();
            sizeable = doorType.Sizeable.ToString();
        }
        SetGeneralProperties();
    }

    public new void SetGeneralProperties()
    {
        generalProperties.Add("Height", height);
        generalProperties.Add("Width", width);
    }
}
