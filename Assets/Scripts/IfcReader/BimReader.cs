using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Xbim.Common.Geometry;
using Xbim.Common.XbimExtensions;
using Xbim.Ifc;
using Xbim.Ifc2x3.Interfaces;

public class BimReader
{
    #region GetSpatialHierarchy
    /// <summary>
    /// return projectData of file, which contains lists of subSpatialStructure and subProduct  
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static IProjectData GetBimSpatialStructure(string fileName)
    {
        using (var model = IfcStore.Open(fileName))
        {
            var proj = model.Instances.FirstOrDefault<IIfcProject>();
            var go = new GameObject();
            var pd = go.AddComponent<ProjectData>();
            pd.InitialObject(proj);

            foreach (var item in proj.IsDecomposedBy.SelectMany(r => r.RelatedObjects))
                pd.AddRelatedObjects(GetSpatialSturcture(item));

            //var spatialProducts = SomeValue.Elements.FindAll(p => p.HaveSpatialStructure == false);
            //AddGeoProductToSpatialStructure(SomeValue.spatialStructures, spatialProducts);

            return pd;
        }
    }

    /// <summary>
    /// return spatial structure of cur IfcObject
    /// </summary>
    /// <param name="father"></param>
    /// <param name="cur"></param>
    /// <returns></returns>
    private static ISpatialData GetSpatialSturcture(IIfcObjectDefinition cur)
    {
        ISpatialData spd = default;
        //whether cur is spatial structure or not
        if (cur is IIfcSpatialStructureElement spatialElement)
        {
            //get spatialElement's spatialData
            spd = InstantiateCurSpatial(spatialElement);
            if (spd != null)
            {
                //get elements by using IfcRelContainedInSpatialElement 
                var containedElements = spatialElement.ContainsElements.SelectMany(rel => rel.RelatedElements);
                if (containedElements.Count() > 0)
                {
                    foreach (var element in containedElements)
                    {
                        if (element is IIfcElement)
                        {
                            var eled = InstantiateCurElement(element as IIfcElement);
                            spd.AddRelatedObjects(eled);
                            if (element.IsDecomposedBy != null && element.IsDecomposedBy.Count() > 0)
                            {
                                eled.SetDecomposeProduct(element.IsDecomposedBy);
                            }
                        }
                    }
                }
            }
        }

        //use IfcRelAggregares to obtain sub spatial structure
        foreach (var item in cur.IsDecomposedBy.SelectMany(r => r.RelatedObjects))
            spd.AddRelatedObjects(GetSpatialSturcture(item));
        return spd;
    }


    /// <summary>
    /// instantiate spatialData by ifcSpatialStructureElement
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static ISpatialData InstantiateCurSpatial(IIfcSpatialStructureElement s)
    {
        var go = new GameObject();
        ISpatialData sp;
        switch (s)
        {
            case IIfcSite _:
                sp = go.AddComponent<SiteData>();
                break;
            case IIfcBuilding _:
                sp = go.AddComponent<BuildingData>();
                break;
            case IIfcBuildingStorey _:
                sp = go.AddComponent<BuildingStoreyData>();
                break;
            case IIfcSpace _:
                sp = go.AddComponent<SpaceData>();
                break;
            default:
                sp = null;
                break;
        }
      
        if (sp != null)
        {
            sp.InitialObject(s);
        }
        return sp;
    }
   
    public static IElementData InstantiateCurElement(IIfcElement e)
    {
        var go = new GameObject();
        IElementData ele;
        switch (e)
        {
            case IIfcBeam _:
                ele = go.AddComponent<BeamData>();
                break;
            case IIfcBuildingElementProxy _:
                ele = go.AddComponent<BuildingElementProxyData>();
                break;
            case IIfcColumn _:
                ele = go.AddComponent<ColumnData>();
                break;
            case IIfcCovering _:
                ele = go.AddComponent<CoveringData>();
                break;
            case IIfcCurtainWall _:
                ele = go.AddComponent<CurtainWallData>();
                break;
            case IIfcDoor _:
                ele = go.AddComponent<DoorData>();
                break;
            case IIfcFlowTerminal _:
                ele = go.AddComponent<FlowTerminalData>();
                break;
            case IIfcFooting _:
                ele = go.AddComponent<FootingData>();
                break;
            case IIfcFurnishingElement _:
                ele = go.AddComponent<FurnitureData>();
                break;
            case IIfcMember _:
                ele = go.AddComponent<MemberData>();
                break;
            case IIfcPile _:
                ele = go.AddComponent<PileData>();
                break;
            case IIfcPlate _:
                ele = go.AddComponent<PlateData>();
                break;
            case IIfcRailing _:
                ele = go.AddComponent<RailingData>();
                break;
            case IIfcRamp _:
                ele = go.AddComponent<RampData>();
                break;
            case IIfcRampFlight _:
                ele = go.AddComponent<RampFlightData>();
                break;
            case IIfcRoof _:
                ele = go.AddComponent<RoofData>();
                break;
            case IIfcSlab _:
                ele = go.AddComponent<SlabData>();
                break;
            case IIfcStair _:
                ele = go.AddComponent<StairData>();
                break; 
            case IIfcStairFlight _:
                ele = go.AddComponent<StairFlightData>();
                break;
            case IIfcWall _:
                ele = go.AddComponent<WallData>();
                break;
            case IIfcWindow _:
                ele = go.AddComponent<WindowData>();
                break;
            default:
                ele = go.AddComponent<ElementData>();
                break;
        }

        if(ele!=null)
        {
            ele.InitialObject(e);
        }
        return ele;
    }

    #endregion

    /// <summary>
    /// read geomotry data from wexbim file
    /// </summary>
    /// <param name="fileName"></param>
    public static void ReadWexbimFile(string fileName)
    {
        Vector3 offsite;
        using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
            using (var br = new BinaryReader(fs))
            {
                var magicalNum = br.ReadInt32();
                var version = br.ReadByte();
                var shapeCount = br.ReadInt32();
                var vertexCount = br.ReadInt32();
                var triangleCount = br.ReadInt32();
                var matrixCount = br.ReadInt32();
                var productCount = br.ReadInt32();
                var styleCount = br.ReadInt32();
                var scale = br.ReadSingle();
                var regionCount = br.ReadInt16();

                //Region
                for (int i = 0; i < regionCount; i++)
                {
                    var population = br.ReadInt32();
                    var centreX = br.ReadSingle(); centreX /= scale;
                    var centreY = br.ReadSingle(); centreY /= scale;
                    var centreZ = br.ReadSingle(); centreZ /= scale;
                    var boundsBytes = br.ReadBytes(6 * sizeof(float));
                    var bounds = XbimRect3D.FromArray(boundsBytes);
                    bounds.X /= scale; bounds.Y /= scale; bounds.Z /= scale;
                    bounds.SizeX /= scale; bounds.SizeY /= scale; bounds.SizeZ /= scale;
                    MyBimGeomorty.regions.Add(new MyBimRegion(population, centreX, centreY, centreZ, (float)bounds.SizeX, (float)bounds.SizeY, (float)bounds.SizeZ));
                }
                offsite = MyBimGeomorty.regions[0].position;

                //texture
                for (int i = 0; i < styleCount; i++)
                {
                    var styleId = br.ReadInt32();
                    var red = br.ReadSingle();
                    var green = br.ReadSingle();
                    var blue = br.ReadSingle();
                    var alpha = br.ReadSingle();
                    MyBimGeomorty.colors.Add(new MyBimColor(styleId, red, green, blue, alpha));
                }

                //product
                for (int i = 0; i < productCount; i++)
                {
                    var entityLabel = br.ReadInt32();
                    var typeId = br.ReadInt16();
                    var boxBytes = br.ReadBytes(6 * sizeof(float));
                    XbimRect3D bb = XbimRect3D.FromArray(boxBytes);
                    //float x = (float)bb.X, y = (float)bb.Y, z = (float)bb.Z;
                    //float sizeX = (float)bb.SizeX, sizeY = (float)bb.SizeY, sizeZ = (float)bb.SizeZ;
                    MyBimGeomorty.products.Add(new MyBimProduct(entityLabel, typeId));
                }

                //shape
                for (int i = 0; i < shapeCount; i++)
                {
                    var shapeRepetition = br.ReadInt32();
                    MyBimShapeInstance si;
                    if (shapeRepetition > 1)
                    {
                        List<MyBimShapeInstance> curShapeInstances = new List<MyBimShapeInstance>();
                        for (int j = 0; j < shapeRepetition; j++)
                        {
                            var ifcProductLabel = br.ReadInt32();
                            var ifcTypeId = br.ReadInt16();
                            var instanceLabel = br.ReadInt32();
                            var styleLabel = br.ReadInt32();
                            var transform = XbimMatrix3D.FromArray(br.ReadBytes(sizeof(double) * 16));

                            si = new MyBimShapeInstance(ifcProductLabel, ifcTypeId, instanceLabel, styleLabel, transform);
                            //MyBimGeomorty.shapeInstances.Add(si);
                            curShapeInstances.Add(si);
                            var p = MyBimGeomorty.products.Find(product => product.entityLabel == ifcProductLabel);
                            p.AddShapeInstance(si);
                        }
                        var triangulation = br.ReadShapeTriangulation();

                        foreach (var csi in curShapeInstances)
                        {
                            var tri = new MyBimTriangulation(triangulation, offsite, scale, csi.transform, true);
                            csi.AddTriangulation(tri);
                            //MyBimGeomorty.triangulations.Add(tri);
                        }
                    }
                    else if (shapeRepetition == 1)
                    {
                        var ifcProductLabel = br.ReadInt32();
                        var ifcTypeId = br.ReadInt16();
                        var instanceLabel = br.ReadInt32();
                        var styleLabel = br.ReadInt32();

                        si = new MyBimShapeInstance(ifcProductLabel, ifcTypeId, instanceLabel, styleLabel);
                        //MyBimGeomorty.shapeInstances.Add(si);
                        var p = MyBimGeomorty.products.Find(product => product.entityLabel == ifcProductLabel);
                        p.AddShapeInstance(si);

                        XbimShapeTriangulation triangulation = br.ReadShapeTriangulation();
                        var tri = new MyBimTriangulation(triangulation, offsite, scale);
                        //MyBimGeomorty.triangulations.Add(tri);

                        si.AddTriangulation(tri);
                    }
                }
            }
        }
    }
}
