﻿using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

namespace MagicCircuit
{
    public class CurrentFlow : MonoBehaviour
    {
        private List<CircuitItem> circuitItems;
        private List<List<int>> circuitBranch;  // Store the branches of the whole circuit for CircuitCompare

        private Correctness correctness;        // Determine whether circuit is correct

        private Connectivity[,] connectivity;   // Will modify this when handling switch on/off
        private Connectivity[,] L_Matrix;
        private Connectivity[,] R_Matrix;
        private Connectivity[,] originalConn;   // Connectivity for the whole circuit when all switches on
        private Connectivity[,] currentConn;    // Store the current state of connectivity for switchOn/Off

        private bool[] isOpened;                // Store information of which item is open circuited
        private int count;
        private int boundary;                   // ID of the first CircuitLine


		public 	CurrentFlow()
		{
			
			circuitBranch = new List<List<int>> ();
			correctness = new Correctness ();

		}



        public bool compute(ref List<CircuitItem> itemList, int level)
        {
            //@FIXME
            // Deep copy itemList to circuitItems
            circuitItems = itemList;

            if (computeCircuitBranch())
                Debug.Log("Working Circuit!");
            else
            {
                Debug.Log("Not working Circuit!");
                return false;
            }

            // Determine whether circuit is correct
            if (!correctness.computeCorrectness(itemList, level, circuitBranch))
                return false;

            /// Display CircuitBranch
            Debug.Log("=========CircuitBranch============");
            for (var i = 0; i < circuitBranch.Count; i++)
            {
                for (var j = 0; j < circuitBranch[i].Count; j++)
                    Debug.Log(circuitBranch[i][j]);
                Debug.Log("----");
            }

            /////////////////////////////////////
            // Return if no switches
            bool haveSwitch = false;
            for (var i = 0; i < boundary; i++)
                if (circuitItems[i].type == ItemType.Switch ||
                    circuitItems[i].type == ItemType.LightActSwitch ||
                    circuitItems[i].type == ItemType.VoiceOperSwitch ||
                    circuitItems[i].type == ItemType.VoiceTimedelaySwitch)
                {
                    haveSwitch = true;
                    break;
                }                    
            if (!haveSwitch) return true;

            /////////////////////////////////////
            // Deal with switches
            // Restore connectivity to compute circuitItems
            Array.Copy(originalConn, connectivity, originalConn.Length);

            // Reset all circuitItems.powered to false
            for (var i = 0; i < count; i++)
                circuitItems[i].powered = false;

            // Turn all the switches off
            for (var i = 0; i < boundary; i++)
                if (circuitItems[i].type == ItemType.Switch ||
                    circuitItems[i].type == ItemType.LightActSwitch ||
                    circuitItems[i].type == ItemType.VoiceOperSwitch ||
                    circuitItems[i].type == ItemType.VoiceTimedelaySwitch)
                    switchOff(i);

            // Save to currentConn
            Array.Copy(connectivity, currentConn, connectivity.Length);

            // Will modify connectivity & generate circuitItems as result
            process();

            /////////////////////////////////////
            //@ Result is the start state of circuit when all the switches are off
            //@ Use CurrentFlow.circuitItems here

            //@FIXME
            // Deep Copy circuitItems to itemList
            itemList = circuitItems;

            /// Display circuitItems.list
            for (var i = boundary; i < count; i++)
            {
                //Debug.Log(i + " " + circuitItems[i].list[0] + " " + circuitItems[i].list[2] + " " + circuitItems[i].powered);
            }

			/////////////////////////////////////  
			return true;                 
		}

        public void switchOnOff(int ID, bool state) // State true: on false: off
        {
            // Restore connectivity to current state
            Array.Copy(currentConn, connectivity, currentConn.Length);

            // Reset all circuitItems.powered to false
            for (var i = 0; i < count; i++)
            {
                circuitItems[i].powered = false;
            }

            if (state)
                switchOn(ID);
            else
                switchOff(ID);

            // Save new current state
            Array.Copy(connectivity, currentConn, connectivity.Length);

            // Will modify connectivity & generate circuitItems as result
            process();
        }

        // Only call this method once!
        private bool computeCircuitBranch()
        {
            //circuitItems = XmlCircuitItemCollection.Load(Path.Combine(Application.dataPath, "CircuitItems.xml")).toCircuitItems();

            circuitBranch = new List<List<int>>();
            circuitBranch.Add(new List<int>());

            count = circuitItems.Count;

            // Find boundary between cards & lines
            boundary = 0;
            while (boundary < count)
            {
                if (circuitItems[boundary].type == ItemType.CircuitLine)
                    break;
                boundary++;
            }

            connectivity = new Connectivity[count, count];
            originalConn = new Connectivity[count, count];
            currentConn = new Connectivity[count, count];
            L_Matrix = new Connectivity[boundary, boundary];
            R_Matrix = new Connectivity[boundary, boundary];

            // Set default value of matrices to zeros
            for (var i = 0; i < count; i++)
                for (var j = 0; j < count; j++)
                    connectivity[i, j] = Connectivity.zero;
            for (var i = 0; i < boundary; i++)
                for (var j = 0; j < boundary; j++)
                {
                    L_Matrix[i, j] = Connectivity.zero;
                    R_Matrix[i, j] = Connectivity.zero;
                }
            // Compute connectivity matrix
            computeConnectivity();

            // Remove open circuit
            simplifyCircuit();

            // Deal with multiple batteries
            if (!combineBattery()) return false;

            // Store the connectivity when all switches on            
            Array.Copy(connectivity, originalConn, connectivity.Length);

            // Will modify connectivity & generate circuitBranch as result
            if (!process())
                return false;

            return true;
        }

        private bool process()
        {
            // Remove open circuit
            simplifyCircuit();

            // Compute L&R matrix
            if (!computeLRMat()) return false; // If have error when computing L/R_Matrix

            // Traverse all the components to get current flow direction
            Vector2 next = new Vector2(0, 0);  // next : (next, current) ->

            Stack<Vector2> parrallel_start = new Stack<Vector2>();

            // Main circuit
            do
            {
                circuitItems[(int)next.x].powered = true;
                circuitBranch[0].Add((int)next.x);

                // Modify Connectivity & L & R_Matrix
                if (R_Matrix[(int)next.y, (int)next.x] == Connectivity.r)
                    flipComponent((int)next.x);

                parrallel_start = findR(next);

                // Branch circuit
                if (parrallel_start.Count > 1)
                {
                    // next = deParrallel
                    if (!deParrallel(ref parrallel_start, ref next))
                        return false;
                }
                else if (parrallel_start.Count == 1)
                {
                    next = parrallel_start.Pop();
                }
                else // (parrallel_start.Count == 0)
                    return false;

            } while ((int)next.x != 0);

            // Determine current flow direcion of lines
            flipLineFromComponent();
            while (flipLineFromLine()) ;

            for (var i = 0; i < count; i++)
                if (isOpened[i])
                    circuitItems[i].powered = false;

            return true;
        }

        private void computeConnectivity()
        {
            // For each card, check which line is attached
            // Improve performance?
            for (var i = 0; i < boundary; i++)
                for (var j = boundary; j < count; j++)
                {
                    if (inRegion(circuitItems[i].connect_left, circuitItems[j].connect_left))
                    {
                        connectivity[i, j] = Connectivity.l;
                        connectivity[j, i] = Connectivity.s;
                        circuitItems[j].list.Insert(0, circuitItems[i].list[0]);
                        continue;
                    }
                    if (inRegion(circuitItems[i].connect_right, circuitItems[j].connect_left))
                    {
                        connectivity[i, j] = Connectivity.r;
                        connectivity[j, i] = Connectivity.s;
                        circuitItems[j].list.Insert(0, circuitItems[i].list[0]);
                        continue;
                    }
                    if (inRegion(circuitItems[i].connect_left, circuitItems[j].connect_right))
                    {
                        connectivity[i, j] = Connectivity.l;
                        connectivity[j, i] = Connectivity.e;
                        circuitItems[j].list.Add(circuitItems[i].list[0]);
                        continue;
                    }
                    if (inRegion(circuitItems[i].connect_right, circuitItems[j].connect_right))
                    {
                        connectivity[i, j] = Connectivity.r;
                        connectivity[j, i] = Connectivity.e;
                        circuitItems[j].list.Add(circuitItems[i].list[0]);
                        continue;
                    }
                };

            // For each line, check which line it is connected to
            for (var i = boundary; i < count; i++)
            {
                for (var j = boundary; j < count; j++)
                {
                    if (i == j) continue;
                    if (isConnected(circuitItems[i].connect_left, circuitItems[j].connect_left))
                    { connectivity[i, j] = Connectivity.s; connectivity[j, i] = Connectivity.s; }
                    if (isConnected(circuitItems[i].connect_left, circuitItems[j].connect_right))
                    { connectivity[i, j] = Connectivity.s; connectivity[j, i] = Connectivity.e; }
                }
                for (var j = boundary; j < count; j++)
                {
                    if (i == j) continue;
                    if (isConnected(circuitItems[i].connect_right, circuitItems[j].connect_left))
                    { connectivity[i, j] = Connectivity.e; connectivity[j, i] = Connectivity.s; }
                    if (isConnected(circuitItems[i].connect_right, circuitItems[j].connect_right))
                    { connectivity[i, j] = Connectivity.e; connectivity[j, i] = Connectivity.e; }
                }
            }
        }

        // Check if linePoint is in a box region of cardPoint
        private bool inRegion(Vector2 cardPoint, Vector2 linePoint)
        {
            if ((linePoint.x > (cardPoint.x - Constant.POINT_CONNECT_REGION)) && (linePoint.x < (cardPoint.x + Constant.POINT_CONNECT_REGION))
                && (linePoint.y > (cardPoint.y - Constant.POINT_CONNECT_REGION)) && (linePoint.y < (cardPoint.y + Constant.POINT_CONNECT_REGION)))
                return true;
            else
                return false;
        }

        private bool isConnected(Vector2 point_1, Vector2 point_2)
        {
            if (point_1.x == point_2.x && point_1.y == point_2.y)
                return true;
            else
                return false;
        }

        private bool isNotValid(int i)
        {
            bool haveStart = false;
            bool haveEnd = false;

            for (var j = 0; j < count; j++)
            {
                if (connectivity[i, j] == Connectivity.l || connectivity[i, j] == Connectivity.s)
                    haveStart = true;
                if (connectivity[i, j] == Connectivity.r || connectivity[i, j] == Connectivity.e)
                    haveEnd = true;
            }

            // We consider 0 0 and 1 1 as valid
            //             1 0 and 0 1 as invalid
            // So return true when invalid
            if (haveStart ^ haveEnd) // XOR operator
                return true;
            else
                return false;
        }

        private bool simplifyCircuit()
        {
            bool flag;
            bool[] delete = new bool[count];
            bool haveOpenCircuit = false;

            // Initialize isOpened
            isOpened = new bool[count];
            for (var i = 0; i < count; i++)
                isOpened[i] = false;

            do
            {
                flag = false;
                // Initialize delete[] to all false            
                for (var i = 0; i < count; i++)
                    delete[i] = false;

                for (var i = 0; i < count; i++)
                    if (isNotValid(i))
                    {
                        delete[i] = true;
                        isOpened[i] = true;
                        flag = true;
                        haveOpenCircuit = true;
                    }

                for (var i = 0; i < count; i++)
                    if (delete[i])
                        for (var j = 0; j < count; j++)
                        {
                            connectivity[i, j] = Connectivity.zero;
                            connectivity[j, i] = Connectivity.zero;
							circuitItems[i].powered = false;
                        }

            } while (flag);

            if (haveOpenCircuit) return false;
            else return true;
        }

        private bool computeLRMat()
        {
            L_Matrix = new Connectivity[boundary, boundary];
            R_Matrix = new Connectivity[boundary, boundary];

            bool LCorrect = false;
            bool RCorrect = false;
            for (var i = 0; i < boundary; i++)
            {
                LCorrect = compute1ComponentLR(i, Connectivity.l);
                RCorrect = compute1ComponentLR(i, Connectivity.r);
                if (!(LCorrect || RCorrect))
                    return false;
            }
            return (LCorrect && RCorrect);
        }

        private bool compute1ComponentLR(int ID, Connectivity dir)
        {
            Stack<Vector2> stack = new Stack<Vector2>();

            bool[] visited = new bool[count];
            for (var i = 0; i < count; i++)
                visited[i] = false;

            for (var j = boundary; j < count; j++)
                if (connectivity[ID, j] == dir)
                    stack.Push(new Vector2(j, ID));

            while (stack.Count > 0)
            {
                Vector2 v = stack.Pop();

                if (visited[(int)v.x])
                    continue;
                visited[(int)v.x] = true;

                for (var i = 0; i < count; i++)
                {
                    if (i == (int)v.y || i == ID) continue;

                    if (connectivity[i, (int)v.x] != Connectivity.zero)
                    {
                        if (i < boundary)
                        {
                            if (dir == Connectivity.l)
                            {
                                if (L_Matrix[ID, i] == Connectivity.r) return false; // Short circuit
                                L_Matrix[ID, i] = connectivity[i, (int)v.x];
                            }
                            if (dir == Connectivity.r)
                            {
                                if (R_Matrix[ID, i] == Connectivity.l) return false; // Short circuit
                                R_Matrix[ID, i] = connectivity[i, (int)v.x];
                            }
                        }
                        else
                            stack.Push(new Vector2(i, (int)v.x));
                    }
                }
            }
            return true;
        }

        private Stack<Vector2> findR(Vector2 ID)
        {
            Stack<Vector2> res = new Stack<Vector2>();
            for (var j = 0; j < boundary; j++)
            {
                if (R_Matrix[(int)ID.x, j] != Connectivity.zero)
                    res.Push(new Vector2(j, (int)ID.x));
            }
            return res;
        }

        private void flipComponent(int ID)
        {
            // Modify IDth col of R_Matrix
            for (var i = 0; i < boundary; i++)
            {
                if (R_Matrix[i, ID] == Connectivity.l)
                    R_Matrix[i, ID] = Connectivity.r;
                else if (R_Matrix[i, ID] == Connectivity.r)
                    R_Matrix[i, ID] = Connectivity.l;
            }
            // Modify IDth col of L_Matrix
            for (var i = 0; i < boundary; i++)
            {
                if (L_Matrix[i, ID] == Connectivity.l)
                    L_Matrix[i, ID] = Connectivity.r;
                else if (L_Matrix[i, ID] == Connectivity.r)
                    L_Matrix[i, ID] = Connectivity.l;
            }
            // Switch IDth row of L/R_Matrix
            Connectivity[] tmp = new Connectivity[boundary];
            for (var j = 0; j < boundary; j++)
            {
                tmp[j] = R_Matrix[ID, j];
                R_Matrix[ID, j] = L_Matrix[ID, j];
                L_Matrix[ID, j] = tmp[j];
            }
            // Modify IDth row of Connectivity
            for (var j = 0; j < count; j++)
            {
                if (connectivity[ID, j] == Connectivity.l)
                    connectivity[ID, j] = Connectivity.r;
                else if (connectivity[ID, j] == Connectivity.r)
                    connectivity[ID, j] = Connectivity.l;
            }
        }

        private bool deParrallel(ref Stack<Vector2> parrallel_start, ref Vector2 next)
        {
            List<int> parrallel_end = new List<int>();

            while (parrallel_start.Count > 0)
            {
                // In a new branch
                circuitBranch.Add(new List<int>());
                // Add compnents to new branch in deSeries
                int end = deSeries(parrallel_start.Pop());
                
                parrallel_end.Add(end);
            }

            // Construct result matrix
            int[,] result = new int[parrallel_end.Count + 1, boundary];

            for (var i = 0; i < parrallel_end.Count; i++)
                for (var j = 0; j < boundary; j++)
                {
                    if (R_Matrix[parrallel_end[i], j] != Connectivity.zero)
                        result[i, j] = 1;
                    else
                        result[i, j] = 0;
                }

            for (var j = 0; j < boundary; j++)
            {
                result[parrallel_end.Count, j] = 0;

                for (var i = 0; i < parrallel_end.Count; i++)
                    result[parrallel_end.Count, j] += result[i, j];
            }

            // Determine whether this is a correct parrallel circuit
            int[] statistics = new int[3] { 0, 0, 0 };

            for (var j = 0; j < boundary; j++)
            {
                if (result[parrallel_end.Count, j] == parrallel_end.Count)
                {
                    next = new Vector2(j, parrallel_end[0]);
                    statistics[0]++;
                }
                if (result[parrallel_end.Count, j] == parrallel_end.Count - 1)
                    statistics[1]++;
                if (result[parrallel_end.Count, j] == 0)
                    statistics[2]++;
            }

            if (statistics[0] == 1 &&
                statistics[1] == parrallel_end.Count &&
                statistics[2] == (boundary - parrallel_end.Count - 1))
                return true;
            else
                return false;
        }

        // Branch circuit
        private int deSeries(Vector2 next)
        {
            Stack<Vector2> stack = new Stack<Vector2>();

            while (true)
            {
                circuitItems[(int)next.x].powered = true;
                circuitBranch[circuitBranch.Count - 1].Add((int)next.x);

                if (R_Matrix[(int)next.y, (int)next.x] == Connectivity.r)
                    flipComponent((int)next.x);

                stack = findR(next);

                if (stack.Count > 1)
                    break;
                else if (stack.Count == 1)
                    next = stack.Pop();
            }
            return (int)next.x;
        }

        private void flipLineFromComponent()
        {
            for (var i = 0; i < boundary; i++)
                for (var j = boundary; j < count; j++)
                {
                    circuitItems[j].powered = true;

                    if (connectivity[i, j] == Connectivity.r)
                    {
                        if (connectivity[j, i] == Connectivity.e)
                            flipLine(j);
                    }
                    else if (connectivity[i, j] == Connectivity.l)
                    {
                        if (connectivity[j, i] == Connectivity.s)
                            flipLine(j);
                    }
                }
        }

        private bool flipLineFromLine()
        {
            bool modified = false;

            for (var i = boundary; i < count; i++)
                if (!circuitItems[i].powered)
                {
                    circuitItems[i].powered = true;
                    modified = true;

                    int flip = 0; // For voting
                    int dont_flip = 0;

                    for (var j = boundary; j < count; j++)
                    {
                        if (!circuitItems[j].powered) continue;

                        if (connectivity[j, i] == Connectivity.e)
                        {
                            if (connectivity[i, j] == Connectivity.e)
                                flip++;
                            else
                                dont_flip++;
                        }
                        else if (connectivity[j, i] == Connectivity.s)
                        {
                            if (connectivity[i, j] == Connectivity.s)
                                flip++;
                            else
                                dont_flip++;
                        }
                    }
                    if (flip > dont_flip)
                        flipLine(i);
                }



            return modified;
        }

        private void flipLine(int ID)
        {
            circuitItems[ID].list.Reverse();

            for (var j = 0; j < count; j++)
            {
                if (connectivity[ID, j] == Connectivity.s)
                    connectivity[ID, j] = Connectivity.e;
                else if (connectivity[ID, j] == Connectivity.e)
                    connectivity[ID, j] = Connectivity.s;
            }
        }

        private bool combineBattery()
        {
            int num = 0;
            for (var i = 0; i < boundary; i++)
                if (circuitItems[i].type == ItemType.Battery)
                    num++;

            switch (num)
            {
                case 0:
                    return false;
                case 1:
                    return true;
                case 2:
                    Connectivity c = Connectivity.zero;
                    int lineID = 0;
                    if (!batteryAdjacency(ref c, ref lineID)) return false;

                    deleteBattery1(c, lineID);
                    circuitBranch[0].Add(1);

                    return true;
                default:
                    return false;
            }
        }

        private bool batteryAdjacency(ref Connectivity c, ref int lineID)
        {
            int num = 0;
            bool flag = false;
            c = Connectivity.zero;
            lineID = 0;

            // Adjacent right
            for (var j = boundary; j < count; j++)
                if (connectivity[0, j] == Connectivity.r)
                {
                    num++;
                    if (connectivity[1, j] == Connectivity.l)
                    {
                        c = Connectivity.r;
                        lineID = j;
                        flag = true;
                    }
                }
            if (num == 1 && flag)
                return true;

            // Adjacent left
            num = 0;
            flag = false;
            c = Connectivity.zero;
            lineID = 0;

            for (var j = boundary; j < count; j++)
                if (connectivity[0, j] == Connectivity.l)
                {
                    num++;
                    if (connectivity[1, j] == Connectivity.r)
                    {
                        c = Connectivity.l;
                        lineID = j;
                        flag = true;
                    }
                }
            if (num == 1 && flag)
                return true;

            return false;
        }

        private void deleteBattery1(Connectivity c, int lineID)
        {
            for (var j = boundary; j < count; j++)
                if (connectivity[1, j] == c)
                {
                    connectivity[lineID, j] = connectivity[lineID, 1];
                    connectivity[j, lineID] = connectivity[j, 1];
                }
            for (var j = boundary; j < count; j++)
            {
                connectivity[1, j] = Connectivity.zero;
                connectivity[j, 1] = Connectivity.zero;
            }
        }

        private void switchOff(int ID)
        {
            for (var i = 0; i < count; i++)
            {
                connectivity[ID, i] = Connectivity.zero;
                connectivity[i, ID] = Connectivity.zero;
            }
        }

        private void switchOn(int ID)
        {
            for (var i = 0; i < count; i++)
            {
                connectivity[ID, i] = originalConn[ID, i];
                connectivity[i, ID] = originalConn[i, ID];
            }
        }
    }
}
