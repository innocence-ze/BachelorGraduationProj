using System;
using System.Collections.Generic;
using UnityEngine;

public static class SomeValue
{
#if UNITY_EDITOR
    public static string processName = Application.dataPath + "/Plugins/IfcToWexBim.exe";
#elif UNITY_STANDALONE_WIN
    public static string processName = Application.dataPath + "/Managed/IfcToWexBim.exe";
#endif

    public static string fileName = default;
    public static string WexbimFileName => fileName + ".wexBIM";
    public static string BimFileName => fileName + ".ifc";


    public static IProjectData project;
    public static List<IElementData> Elements = new List<IElementData>();
    public static List<ISpatialData> spatialStructures = new List<ISpatialData>();

}
