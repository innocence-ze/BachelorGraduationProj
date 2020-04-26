using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface IMemberData : IElementData
{
    string PredefinedType { get; set; }
}

public class MemberData : ElementData, IMemberData
{
    private IIfcMember thisMember;
    private string predefinedType;

    public string PredefinedType { get => predefinedType; set => predefinedType = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisMember = ifcObj as IIfcMember;

        if (objType != null)
        {
            IIfcMemberType memberType = objType as IIfcMemberType;
            predefinedType = memberType.PredefinedType.ToString();
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
