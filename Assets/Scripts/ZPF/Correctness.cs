using System.Collections.Generic;
using UnityEngine;

namespace MagicCircuit
{
    public class Correctness
    {
        private List<CircuitItem> itemList;
        private List<List<int>> circuitBranch;
            

        public bool computeCorrectness(List<CircuitItem> _itemList, int level, List<List<int>> _circuitBranch)
        {
            itemList = _itemList;
            circuitBranch = _circuitBranch;

            switch (level)
            {
                case 1:  return level_1();
                case 2:  return level_2();
                case 3:  return level_3();
                case 4:  return level_4();
                case 5:  return level_5();
                case 6:  return level_6();
                case 7:  return level_7();
                case 8:  return level_8();
                case 9:  return level_9();
                case 10: return level_10();
                case 11: return level_11();
                case 12: return level_12();
                case 13: return level_13();
                case 14: return level_14();
                default: return false;
            }
        }

        private bool level_1()
        {
            //  -----Bulb------
            // |               |
            //  -0--Battery----

            // Only 1 branch
            if (circuitBranch.Count != 1) return false;
            // Have 2 components
            if (circuitBranch[0].Count != 2) return false;
            // Have 1 Battery & 1 Bulb
            bool haveBattery = false;
            bool haveBulb = false;
            for (var i = 0; i < circuitBranch[0].Count; i++)
            {
                if (itemList[circuitBranch[0][i]].type == ItemType.Battery) haveBattery = true;
                if (itemList[circuitBranch[0][i]].type == ItemType.Bulb) haveBulb = true;
            }
            if (haveBattery && haveBulb) return true;
            else return false;
        }

        private bool level_2()
        {
            //  -----Bulb------
            //                 |
            //      Battery----

            // Same circuit as level_1
            return level_1();
        }

        private bool level_3()
        {
            //  -----------Bulb----------
            // |                         |
            //  -0--Battery----Switch----

            // Only 1 branch
            if (circuitBranch.Count != 1) return false;
            // Have 3 components
            if (circuitBranch[0].Count != 3) return false;
            // Have 1 Battery & 1 Bulb & 1 Switch
            bool haveBattery = false;
            bool haveBulb = false;
            bool haveSwitch = false;
            for (var i = 0; i < circuitBranch[0].Count; i++)
            {
                if (itemList[circuitBranch[0][i]].type == ItemType.Battery) haveBattery = true;
                if (itemList[circuitBranch[0][i]].type == ItemType.Bulb) haveBulb = true;
                if (itemList[circuitBranch[0][i]].type == ItemType.Switch) haveSwitch = true;
            }
            if (haveBattery && haveBulb && haveSwitch) return true;
            else return false;
        }

        private bool level_4()
        {
            //  -----Bulb-----Speaker----
            // |                         |
            //  -0--Battery----Switch----

            // Only 1 branch
            if (circuitBranch.Count != 1) return false;
            // Have 4 components
            if (circuitBranch[0].Count != 4) return false;
            // Have 1 Battery & 1 Bulb & 1 Switch & 1 Speaker
            bool haveBattery = false;
            bool haveBulb = false;
            bool haveSwitch = false;
            bool haveSpeaker = false;
            for (var i = 0; i < circuitBranch[0].Count; i++)
            {
                if (itemList[circuitBranch[0][i]].type == ItemType.Battery) haveBattery = true;
                if (itemList[circuitBranch[0][i]].type == ItemType.Bulb) haveBulb = true;
                if (itemList[circuitBranch[0][i]].type == ItemType.Switch) haveSwitch = true;
                if (itemList[circuitBranch[0][i]].type == ItemType.Loudspeaker) haveSpeaker = true;
            }
            if (haveBattery && haveBulb && haveSwitch && haveSpeaker) return true;
            else return false;
        }

        private bool level_5()
        {
            //  ----------Bulb------Speaker--------
            // |                                   |
            //  -0--Battery----Switch----Switch----

            // Only 1 branch
            if (circuitBranch.Count != 1) return false;
            // Have 5 components
            if (circuitBranch[0].Count != 5) return false;
            // Have 1 Battery & 1 Bulb & 2 Switch & 1 Speaker
            bool haveBattery = false;
            bool haveBulb = false;
            bool haveSpeaker = false;
            int countSwitch = 0;
            for (var i = 0; i < circuitBranch[0].Count; i++)
            {
                if (itemList[circuitBranch[0][i]].type == ItemType.Battery) haveBattery = true;
                if (itemList[circuitBranch[0][i]].type == ItemType.Bulb) haveBulb = true;
                if (itemList[circuitBranch[0][i]].type == ItemType.Switch) countSwitch++;
                if (itemList[circuitBranch[0][i]].type == ItemType.Loudspeaker) haveSpeaker = true;
            }
            if (haveBattery && haveBulb && (countSwitch == 2) && haveSpeaker) return true;
            else return false;
        }

        private bool level_6()
        {
            //  -2---Bulb------Switch----
            // |                         |
            //  -1--Speaker----Switch----
            // |                         |
            //  -0-------Battery---------

            // have 3 branches
            if (circuitBranch.Count != 3) return false;
            /// Main Circuit :
            // Have 1 Battery
            if (circuitBranch[0].Count != 1) return false;
            bool haveBattery = false;
            bool checkMainCircuit = false;         
            for (var i = 0; i < circuitBranch[0].Count; i++)
                if (itemList[circuitBranch[0][i]].type == ItemType.Battery) haveBattery = true;
            if (!haveBattery) return false;
            checkMainCircuit = true;
            /// Branch Circuit :
            // Have 1 Bulb & 1 Switch or 1 Speaker & 1 Switch
            bool checkBranch_1 = false;
            bool checkBranch_2 = false;
            for (var i = 1; i < circuitBranch.Count; i++)
            {
                bool haveBulb = false;
                bool haveSpeaker = false;
                bool haveSwitch = false;

                for (var j = 0; j < circuitBranch[i].Count; j++)
                {
                    if (itemList[circuitBranch[i][j]].type == ItemType.Bulb) haveBulb = true;
                    if (itemList[circuitBranch[i][j]].type == ItemType.Loudspeaker) haveSpeaker = true;
                    if (itemList[circuitBranch[i][j]].type == ItemType.Switch) haveSwitch = true;                    
                }
                if (haveSpeaker && haveSwitch && (circuitBranch[i].Count == 2)) checkBranch_1 = true;
                else if (haveBulb && haveSwitch && (circuitBranch[i].Count == 2)) checkBranch_2 = true;
                else return false;                
            }

            if (checkMainCircuit && checkBranch_1 && checkBranch_2) return true;
            else return false;
        }

        private bool level_7()
        {
            //  -2---Bulb------Switch------
            // |                           |
            //  -1--Speaker----Switch------
            // |                           |
            //  -0--Battery----Battery-----

            // have 3 branches
            if (circuitBranch.Count != 3) return false;
            /// Main Circuit :
            // Have 2 Batteries
            if (circuitBranch[0].Count != 2) return false;
            int countBattery = 0;
            bool checkMainCircuit = false;
            for (var i = 0; i < circuitBranch[0].Count; i++)
                if (itemList[circuitBranch[0][i]].type == ItemType.Battery) countBattery++;
            if (countBattery != 2) return false;
            checkMainCircuit = true;
            /// Branch Circuit :
            // Have 1 Bulb & 1 Switch or 1 Speaker & 1 Switch
            bool checkBranch_1 = false;
            bool checkBranch_2 = false;
            for (var i = 1; i < circuitBranch.Count; i++)
            {
                bool haveBulb = false;
                bool haveSpeaker = false;
                bool haveSwitch = false;

                for (var j = 0; j < circuitBranch[i].Count; j++)
                {
                    if (itemList[circuitBranch[i][j]].type == ItemType.Bulb) haveBulb = true;
                    if (itemList[circuitBranch[i][j]].type == ItemType.Loudspeaker) haveSpeaker = true;
                    if (itemList[circuitBranch[i][j]].type == ItemType.Switch) haveSwitch = true;                    
                }
                if (haveSpeaker && haveSwitch && (circuitBranch[i].Count == 2)) checkBranch_1 = true;
                else if (haveBulb && haveSwitch && (circuitBranch[i].Count == 2)) checkBranch_2 = true;
                else return false;
            }

            if (checkMainCircuit && checkBranch_1 && checkBranch_2) return true;
            else return false;
        }

        private bool level_8()
        {
            //  -3---Bulb------Switch------
            // |                           |
            //  -2--Speaker----Switch------
            // |                           |
            //  -1--Cooker-----Switch------
            // |                           |
            //  -0--Battery----Battery-----

            // have 4 branches
            if (circuitBranch.Count != 4) return false;
            /// Main Circuit :
            // Have 2 Batteries
            if (circuitBranch[0].Count != 2) return false;
            int countBattery = 0;
            bool checkMainCircuit = false;
            for (var i = 0; i < circuitBranch[0].Count; i++)
                if (itemList[circuitBranch[0][i]].type == ItemType.Battery) countBattery++;
            if (countBattery != 2) return false;
            checkMainCircuit = true;
            /// Branch Circuit :
            // Have 1 Bulb & 1 Switch or 1 Speaker & 1 Switch or 1 Cooker & 1 Switch
            bool checkBranch_1 = false;
            bool checkBranch_2 = false;
            bool checkBranch_3 = false;
            for (var i = 1; i < circuitBranch.Count; i++)
            {
                bool haveBulb = false;
                bool haveSpeaker = false;
                bool haveSwitch = false;
                bool haveCooker = false;

                for (var j = 0; j < circuitBranch[i].Count; j++)
                {
                    if (itemList[circuitBranch[i][j]].type == ItemType.Bulb) haveBulb = true;
                    if (itemList[circuitBranch[i][j]].type == ItemType.Loudspeaker) haveSpeaker = true;
                    if (itemList[circuitBranch[i][j]].type == ItemType.Switch) haveSwitch = true;
                    if (itemList[circuitBranch[i][j]].type == ItemType.InductionCooker) haveCooker = true;                    
                }
                if (haveCooker && haveSwitch && (circuitBranch[i].Count == 2)) checkBranch_1 = true;
                else if (haveSpeaker && haveSwitch && (circuitBranch[i].Count == 2)) checkBranch_2 = true;
                else if (haveBulb && haveSwitch && (circuitBranch[i].Count == 2)) checkBranch_3 = true;
                else return false;
            }

            if (checkMainCircuit && checkBranch_1 && checkBranch_2 && checkBranch_3) return true;
            else return false;
        }

        private bool level_9()
        {
            //  -3---Bulb----Bulb----Switch----
            // |                               |
            //  -2-----Speaker----Switch-------
            // |                               |
            //  -1-----Cooker-----Switch-------
            // |                               |
            //  -0-----Battery----Battery------

            // have 4 branches
            if (circuitBranch.Count != 4) return false;
            /// Main Circuit :
            // Have 2 Batteries
            if (circuitBranch[0].Count != 2) return false;
            int countBattery = 0;
            bool checkMainCircuit = false;
            for (var i = 0; i < circuitBranch[0].Count; i++)
                if (itemList[circuitBranch[0][i]].type == ItemType.Battery) countBattery++;
            if (countBattery != 2) return false;
            checkMainCircuit = true;
            /// Branch Circuit :
            // Have 2 Bulbs & 1 Switch or 1 Speaker & 1 Switch or 1 Cooker & 1 Switch
            bool checkBranch_1 = false;
            bool checkBranch_2 = false;
            bool checkBranch_3 = false;
            for (var i = 1; i < circuitBranch.Count; i++)
            {
                int countBulb = 0;
                bool haveSpeaker = false;
                bool haveSwitch = false;
                bool haveCooker = false;

                for (var j = 0; j < circuitBranch[i].Count; j++)
                {
                    if (itemList[circuitBranch[i][j]].type == ItemType.Bulb) countBulb++;
                    if (itemList[circuitBranch[i][j]].type == ItemType.Loudspeaker) haveSpeaker = true;
                    if (itemList[circuitBranch[i][j]].type == ItemType.Switch) haveSwitch = true;
                    if (itemList[circuitBranch[i][j]].type == ItemType.InductionCooker) haveCooker = true;                    
                }
                if (haveCooker && haveSwitch && (circuitBranch[i].Count == 2)) checkBranch_1 = true;
                else if (haveSpeaker && haveSwitch && (circuitBranch[i].Count == 2)) checkBranch_2 = true;
                else if ((countBulb == 2) && haveSwitch && (circuitBranch[i].Count == 3)) checkBranch_3 = true;
                else return false;
            }

            if (checkMainCircuit && checkBranch_1 && checkBranch_2 && checkBranch_3) return true;
            else return false;
        }
        
        private bool level_10()
        {
            //  -----------Bulb----------
            // |                         |
            //  -0--Battery----Switch----

            // Same as level 3
            return level_3();
        }

        private bool level_11()
        {
            //  -----------Bulb------------
            // |                           |
            //  -0--Battery----VOSwitch----

            // Only 1 branch
            if (circuitBranch.Count != 1) return false;
            // Have 3 components
            if (circuitBranch[0].Count != 3) return false;
            // Have 1 Battery & 1 Bulb & 1 VoiceOperSwitch
            bool haveBattery = false;
            bool haveBulb = false;
            bool haveSwitch = false;
            for (var i = 0; i < circuitBranch[0].Count; i++)
            {
                if (itemList[circuitBranch[0][i]].type == ItemType.Battery) haveBattery = true;
                if (itemList[circuitBranch[0][i]].type == ItemType.Bulb) haveBulb = true;
                if (itemList[circuitBranch[0][i]].type == ItemType.VoiceOperSwitch) haveSwitch = true;
            }
            if (haveBattery && haveBulb && haveSwitch) return true;
            else return false;
        }

        private bool level_12()
        {
            //  -----------Bulb------------
            // |                           |
            //  -0--Battery----LASwitch----

            // Only 1 branch
            if (circuitBranch.Count != 1) return false;
            // Have 3 components
            if (circuitBranch[0].Count != 3) return false;
            // Have 1 Battery & 1 Bulb & 1 LightActSwitch
            bool haveBattery = false;
            bool haveBulb = false;
            bool haveSwitch = false;
            for (var i = 0; i < circuitBranch[0].Count; i++)
            {
                if (itemList[circuitBranch[0][i]].type == ItemType.Battery) haveBattery = true;
                if (itemList[circuitBranch[0][i]].type == ItemType.Bulb) haveBulb = true;
                if (itemList[circuitBranch[0][i]].type == ItemType.LightActSwitch) haveSwitch = true;
            }
            if (haveBattery && haveBulb && haveSwitch) return true;
            else return false;
        }

        private bool level_13()
        {
            //  -----Bulb------VOSwitch----
            // |                           |
            //  -0--Battery----LASwitch----

            // Only 1 branch
            if (circuitBranch.Count != 1) return false;
            // Have 4 components
            if (circuitBranch[0].Count != 4) return false;
            // Have 1 Battery & 1 Bulb & 1 LightActSwitch & 1 VoiceOperSwitch
            bool haveBattery = false;
            bool haveBulb = false;
            bool haveLASwitch = false;
            bool haveVOSwitch = false;
            for (var i = 0; i < circuitBranch[0].Count; i++)
            {
                if (itemList[circuitBranch[0][i]].type == ItemType.Battery) haveBattery = true;
                if (itemList[circuitBranch[0][i]].type == ItemType.Bulb) haveBulb = true;
                if (itemList[circuitBranch[0][i]].type == ItemType.LightActSwitch) haveLASwitch = true;
                if (itemList[circuitBranch[0][i]].type == ItemType.VoiceOperSwitch) haveVOSwitch = true;                
            }
            if (haveBattery && haveBulb && haveLASwitch && haveVOSwitch) return true;
            else return false;
        }

        private bool level_14()
        {
            //  -----Bulb------VTDSwitch----
            // |                            |
            //  -0--Battery----LASwitch-----

            // Only 1 branch
            if (circuitBranch.Count != 1) return false;
            // Have 4 components
            if (circuitBranch[0].Count != 4) return false;
            // Have 1 Battery & 1 Bulb & 1 LightActSwitch & 1 VoiceTimeDelaySwitch
            bool haveBattery = false;
            bool haveBulb = false;
            bool haveLASwitch = false;
            bool haveVTDSwitch = false;
            for (var i = 0; i < circuitBranch[0].Count; i++)
            {
                if (itemList[circuitBranch[0][i]].type == ItemType.Battery) haveBattery = true;
                if (itemList[circuitBranch[0][i]].type == ItemType.Bulb) haveBulb = true;
                if (itemList[circuitBranch[0][i]].type == ItemType.LightActSwitch) haveLASwitch = true;
                if (itemList[circuitBranch[0][i]].type == ItemType.VoiceTimedelaySwitch) haveVTDSwitch = true;
            }
            if (haveBattery && haveBulb && haveLASwitch && haveVTDSwitch) return true;
            else return false;
        }
    }
}