using System.Collections;
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
    IIfcSpatialStructureElement ThisStructure { get; set; }
    void InitialSpatialElement(IIfcSpatialStructureElement ifcSE);
    void AddRelatedProduct(IElementData ele);
    void AddRelatedProduct(ISpatialData spat);
}