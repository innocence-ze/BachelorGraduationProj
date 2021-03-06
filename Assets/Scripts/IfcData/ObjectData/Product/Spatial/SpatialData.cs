﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;
 
public interface ISpatialData: IProductData
{
    /*project
     #site
      building
      buildingStorey
     #space*/
     string LongName { get; set; }
    string CompositionType { get; set; }
    IIfcSpatialStructureElement ThisStructure { get; set; }
}

public class SpatialData : ProductData, ISpatialData
{
    private string longName;
    private IIfcSpatialStructureElement thisStructure;
    private string compositionType;

    public string LongName { get => longName; set => longName = value; }
    public IIfcSpatialStructureElement ThisStructure { get => thisStructure; set => thisStructure = value; }
    public string CompositionType { get => compositionType; set => compositionType = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisStructure = ifcObj as IIfcSpatialStructureElement;
        compositionType = ThisStructure.CompositionType.ToString();
        longName = thisStructure.LongName;

        SetGeneralProperties();
    }

    public new void SetGeneralProperties()
    {
        generalProperties.Add("LongName", longName);
        generalProperties.Add("CompositionType", compositionType);
    }
}