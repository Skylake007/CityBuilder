using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResNameParser
{
	public static ResourceName FromString(string name)
	{
		//Debug.Log("parser" + name);
		//name = name.ToLower();
		return (ResourceName)Enum.Parse(typeof(ResourceName), name);
	}
}

public enum ResourceName
{

	noResource = 0,
	//Money
	gold = 1,
	diamond = 2,

	//Maretial level 1
	water = 1000,
	logwood = 1001,
	ironOre = 1002,

	//Material lever 2
	blank = 2001,
	ironIgnot = 2002,

}
