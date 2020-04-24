using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces; 
public interface ISiteData : ISpatialData
{
    string Latitude { get; set; }
    string Longitude { get; set; }
    string Elevation { get; set; }
    string LandTitleNum { get; set; }
} 
public class SiteData : SpatialData, ISiteData
{
    private IIfcSite thisSite;
    private string latitude;
    private string longitude;
    private string elevation;
    private string landTitleNum;

    public string Latitude { get => latitude; set => latitude = value; }
    public string Longitude { get => longitude; set => longitude = value; }
    public string Elevation { get => elevation; set => elevation = value; }
    public string LandTitleNum { get => landTitleNum; set => landTitleNum = value; }


    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisSite = ThisStructure as IIfcSite;
        if(thisSite.RefLatitude.HasValue)
            latitude = thisSite.RefLongitude.Value.AsDouble.ToString();
        if (thisSite.RefLongitude.HasValue)
            longitude = thisSite.RefLongitude.Value.AsDouble.ToString();
        if (thisSite.RefElevation.HasValue)
            elevation = thisSite.RefElevation.Value.ToString();
        if (thisSite.LandTitleNumber.HasValue)
            landTitleNum = thisSite.LandTitleNumber.Value.ToString();

        ThisGameObject.name = Name + "[" + TypeName + "]#" + EntityLabel;
        SomeValue.spatialStructures.Add(this);
        SetGeneralProperties();
    }

    public new void SetGeneralProperties()
    {
        generalProperties.Add("Latitude", latitude);
        generalProperties.Add("Longitude", longitude);
        generalProperties.Add("Elevation", elevation);
        generalProperties.Add("LandTitleNum", landTitleNum);
    }
}
