using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface IWindowData : IElementData
{
    string Height { get; set; }
    string Width { get; set; }

    string ConstructionType { get; set; }
    string OperationType { get; set; }
    string ParameterTakesPrecedence { get; set; }
    string Sizeable { get; set; }

}

public class WindowData : ElementData, IWindowData
{
    private IIfcWindow thisWindow;
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
        thisWindow = ifcObj as IIfcWindow;
        if (thisWindow.OverallHeight.HasValue)
            height = thisWindow.OverallHeight.Value.ToString();
        if (thisWindow.OverallWidth.HasValue)
            width = thisWindow.OverallWidth.Value.ToString();

        if (objType != null)
        {
            IIfcWindowStyle windowType = objType as IIfcWindowStyle;
            constructionType = windowType.ConstructionType.ToString();
            operationType = windowType.OperationType.ToString();
            parameterTakesPrecedence = windowType.ParameterTakesPrecedence.ToString();
            sizeable = windowType.Sizeable.ToString();
        }
        SetGeneralProperties();
    }

    public new void SetGeneralProperties()
    {
        generalProperties.Add("Height", height);
        generalProperties.Add("Width", width);
        if(objType != null)
        {
            generalProperties.Add("ConstructionType", constructionType);
            generalProperties.Add("OperationType", operationType);
            generalProperties.Add("ParameterTakesPrecedence", parameterTakesPrecedence);
            generalProperties.Add("Sizeable", sizeable);
        }

    }
}
