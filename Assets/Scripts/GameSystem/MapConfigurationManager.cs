using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapConfigurationManager : IManagerBase<MapConfigurationManager> {

	public string MapFileName = "Labirinth1";

	public string GetMapFile()
	{
		return MapFileName;
	}

}
