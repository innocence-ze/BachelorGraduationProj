using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc4.Interfaces;
 
public interface ISpatialData
{
    /*project
     #site
      building
      buildingStorey
     #space*/
    IIfcSpatialStructureElement ThisStructure { get; set; }
    string Name { get; set; }
    List<IProductData> SubProducts { get; set; }
    List<ISpatialData> SubSpatials { get; set; }
    GameObject ThisGameObject { get; }
    void AddProduct(IProductData p);
    void AddSubSpatial(ISpatialData s);
    int EntityLabel { get; }
}