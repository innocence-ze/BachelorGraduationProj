using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface IColumnData : IElementData
{
    string PredefinedType { get; set; }
}

public class ColumnData : ElementData, IColumnData
{
    private IIfcColumn thisColumn;
    private string predefinedType;

    public string PredefinedType { get => predefinedType; set => predefinedType = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisColumn = ifcObj as IIfcColumn;

        if (objType != null)
        {
            IIfcColumnType columnType = objType as IIfcColumnType;
            predefinedType = columnType.PredefinedType.ToString();
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