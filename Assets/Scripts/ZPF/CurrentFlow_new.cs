using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

namespace MagicCircuit
{
    public class CurrentFlow : MonoBehaviour
    {
        private List<CircuitItem> circuitItemList;
        private List<List<int>> circuitBranch;  // Store the branches of the whole circuit for CircuitCompare

        private Connectivity[,] connectivity;   // Will modify this when handling switch on/off
        private Connectivity[,] L_Matrix;
        private Connectivity[,] R_Matrix;
        private Connectivity[,] originalConn;   // Connectivity for the whole circuit when all switches on
        
        private int count;                      // Total number of items
        private int boundary;                   // ID of the first CircuitLine


        public bool compute(ref List<CircuitItem> itemList, int level)
        {
            circuitItemList = itemList;

            initCountBoundary(circuitItemList);

            allPowerOff(circuitItemList);


        }

        private void initCountBoundary(List<CircuitItem> itemList)
		{
			count = itemList.Count;
			// Find boundary between cards & lines
			boundary = 0;
			while (boundary < count)
			{
				if (itemList[boundary].type == ItemType.CircuitLine)
					break;
				boundary++;
			}
		}

        private void allPowerOff(List<CircuitItem> itemList)
        {
            for (var i = 0; i < count; i++)
            {
                itemList[i].powered = false;
            }
        }

        private void allPowerOn()
        {

        }



    }
}