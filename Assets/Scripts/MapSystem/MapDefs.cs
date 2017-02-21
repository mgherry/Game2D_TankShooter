using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDefs {
	
	public class LabirinthMap
	{
		public int height = 10;
		public int width = 10;

		public bool loaded = false;
		public bool[] checkboard;

		public void ResetMap(int nHeight = int.MinValue, int nWidth = int.MinValue)
		{
			if (nHeight != int.MinValue)
				height = nHeight;

			if (nWidth != int.MinValue)
				width = nWidth;

			checkboard = new bool[height * width];

			for (int h = 0; h < height; h++)
				for (int w = 0; w < width; w++)
					checkboard[h*w] = false;

			loaded = false;
		}

		//public void 
	}

	public class TreeMapNode
	{
		public TreeMapNode parent;
		public List<TreeMapNode> children;

		public bool wall = false;

	}

}
