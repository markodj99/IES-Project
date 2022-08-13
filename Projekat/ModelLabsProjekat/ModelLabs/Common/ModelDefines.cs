using System;
using System.Collections.Generic;
using System.Text;

namespace FTN.Common
{
	
	public enum DMSType : short
	{		
		MASK_TYPE							         = unchecked((short)0xFFFF),

        CONNECTIVITYNODECONTAINER                    = 0x0001,
        TOPOLOGICALNODE                              = 0x0002,
        CONNECTIVITYNODE                             = 0x0003,
        BASEVOLTAGE                                  = 0x0004,
        SWITCH                                       = 0x0005,
        TERMINAL                                     = 0x0006
    }

    [Flags]
	public enum ModelCode : long
	{
        IDENTIFIEDOBJECT                             = 0x1000000000000000,
        IDENTIFIEDOBJECT_GLOBALID                    = 0x1000000000000104,
        IDENTIFIEDOBJECT_ALIASNAME                   = 0x1000000000000207,
        IDENTIFIEDOBJECT_MRID                        = 0x1000000000000307,
        IDENTIFIEDOBJECT_NAME                        = 0x1000000000000407,

		POWERSYSTEMRESOURCE                          = 0x1100000000000000,

        BASEVOLTAGE							         = 0x1200000000040000,
		BASEVOLTAGE_CONDUCTINGEQUIPMENTS             = 0x1200000000040119,

        CONNECTIVITYNODECONTAINER                    = 0x1300000000010000,
		CONNECTIVITYNODECONTAINER_CONNECTIVITYNODES  = 0x1300000000010119,

        CONNECTIVITYNODE                             = 0x1400000000030000,
        CONNECTIVITYNODE_TOPOLOGICALNODE             = 0x1400000000030109,
        CONNECTIVITYNODE_CONNECTIVITYNODECONTAINER   = 0x1400000000030209,
        CONNECTIVITYNODE_TERMINALS                   = 0x1400000000030319,

        TERMINAL                                     = 0x1500000000060000,
        TERMINAL_CONNECTIVITYNODE                    = 0x1500000000060109,
        TERMINAL_CONDUCTINGEQUIPMENT                 = 0x1500000000060209,

        EQUIPMENT                                    = 0x1110000000000000,
        EQUIPMENT_AGGREGATE                          = 0x1110000000000101,
        EQUIPMENT_NORMALLYINSERVICE                  = 0x1110000000000201,

        CONDUCTINGEQUIPMENT                          = 0x1111000000000000,
        CONDUCTINGEQUIPMENT_BASEVOLTAGE              = 0x1111000000000109,
        CONDUCTINGEQUIPMENT_TERMINALS                = 0x1111000000000219,

        SWITCH                                       = 0x1111100000050000,
        SWITCH_NORMALOPEN                            = 0x1111100000050101,
        SWITCH_RATEDCURRENT                          = 0x1111100000050205,
        SWITCH_RETAINED                              = 0x1111100000050301,
        SWITCH_SWITCHONCOUNT                         = 0x1111100000050403,
        SWITCH_SWITCHONDATE                          = 0x1111100000050508,

        TOPOLOGICALNODE                              = 0x1600000000020000,
        TOPOLOGICALNODE_CONNECTIVITYNODES            = 0x1600000000020119,
    }

    [Flags]
	public enum ModelCodeMask : long
	{
		MASK_TYPE			                         = 0x00000000ffff0000,
		MASK_ATTRIBUTE_INDEX                         = 0x000000000000ff00,
		MASK_ATTRIBUTE_TYPE	                         = 0x00000000000000ff,

		MASK_INHERITANCE_ONLY                        = unchecked((long)0xffffffff00000000),
		MASK_FIRSTNBL		                         = unchecked((long)0xf000000000000000),
		MASK_DELFROMNBL8	                         = unchecked((long)0xfffffff000000000),		
	}																		
}


