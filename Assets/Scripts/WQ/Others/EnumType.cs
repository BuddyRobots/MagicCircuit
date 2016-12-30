using UnityEngine;
using System.Collections;



public enum LevelProgress
{

	Todo=0,
	Doing,
	Done
}

public  enum  FromPanelFlag
{
	START=1,
	LEVELSELECT,
	PHOTOTAKING,
	PHOTORECOGNIZE

}

public enum ItemType
{
	Battery,              // class 1
	Switch,               // class 2
	LightActSwitch,       // class 3
	VoiceOperSwitch,      // class 4
	VoiceTimedelaySwitch, // class 5
	SPDTSwitch,           // class 6
	Bulb,		          // class 7
	Loudspeaker,          // class 8
	InductionCooker,      // class 9

	CircuitLine
}
