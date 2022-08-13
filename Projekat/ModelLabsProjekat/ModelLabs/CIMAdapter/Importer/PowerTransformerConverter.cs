namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	using FTN.Common;

	/// <summary>
	/// PowerTransformerConverter has methods for populating
	/// ResourceDescription objects using PowerTransformerCIMProfile_Labs objects.
	/// </summary>
	public static class PowerTransformerConverter
	{

		#region Populate ResourceDescription

		public static void PopulateIdentifiedObjectProperties(FTN.IdentifiedObject cimIdentifiedObject, ResourceDescription rd)
		{
			if ((cimIdentifiedObject != null) && (rd != null))
			{
				if (cimIdentifiedObject.MRIDHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID, cimIdentifiedObject.MRID));
				}
				if (cimIdentifiedObject.NameHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDENTIFIEDOBJECT_NAME, cimIdentifiedObject.Name));
				}
				if (cimIdentifiedObject.AliasNameHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDENTIFIEDOBJECT_ALIASNAME, cimIdentifiedObject.AliasName));
				}
			}
		}

        public static void PopulatePowerSystemResourceProperties(FTN.PowerSystemResource cimPowerSystemResource, ResourceDescription rd)
        {
            if (cimPowerSystemResource != null && rd != null)
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimPowerSystemResource, rd);
            }
        }

        public static void PopulateEquipmentProperties(FTN.Equipment cimEquipment, ResourceDescription rd)
        {
            if ((cimEquipment != null) && (rd != null))
            {
                PowerTransformerConverter.PopulatePowerSystemResourceProperties(cimEquipment, rd);

                if (cimEquipment.AggregateHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.EQUIPMENT_AGGREGATE, cimEquipment.Aggregate));
                }
                if (cimEquipment.NormallyInServiceHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.EQUIPMENT_NORMALLYINSERVICE, cimEquipment.NormallyInService));
                }
            }
        }

        public static void PopulateBaseVoltageProperties(FTN.BaseVoltage cimBaseVoltage, ResourceDescription rd)
        {
            if ((cimBaseVoltage != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimBaseVoltage, rd);
            }
        }

        public static void PopulateConductingEquipmentProperties(FTN.ConductingEquipment cimConductingEquipment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimConductingEquipment != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateEquipmentProperties(cimConductingEquipment, rd);

                if (cimConductingEquipment.BaseVoltageHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimConductingEquipment.BaseVoltage.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimConductingEquipment.GetType().ToString()).Append(" rdfID = \"").Append(cimConductingEquipment.ID);
                        report.Report.Append("\" - Failed to set reference to BaseVoltage: rdfID \"").Append(cimConductingEquipment.BaseVoltage.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.CONDUCTINGEQUIPMENT_BASEVOLTAGE, gid));
                }
            }
        }

        public static void PopulateSwitchProperties(FTN.Switch cimSwitch, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimSwitch != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateConductingEquipmentProperties(cimSwitch, rd, importHelper, report);

                if (cimSwitch.NormalOpenHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_NORMALOPEN, cimSwitch.NormalOpen));
				}
                if (cimSwitch.RatedCurrentHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_RATEDCURRENT, cimSwitch.RatedCurrent));
                }
                if (cimSwitch.RetainedHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_RETAINED, cimSwitch.Retained));
                }
                if (cimSwitch.SwitchOnCountHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_SWITCHONCOUNT, cimSwitch.SwitchOnCount));
                }
                if (cimSwitch.SwitchOnDateHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_SWITCHONDATE, cimSwitch.SwitchOnDate));
                }
            }
        }

        public static void PopulateConnectivityNodeContainerProperties(FTN.ConnectivityNodeContainer cimConnectivityNodeContainer, ResourceDescription rd)
        {
            if ((cimConnectivityNodeContainer != null) && (rd != null))
            {
                PowerTransformerConverter.PopulatePowerSystemResourceProperties(cimConnectivityNodeContainer, rd);
            }
        }

        public static void PopulateConnectivityNodeProperties(FTN.ConnectivityNode cimConnectivityNode, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimConnectivityNode != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimConnectivityNode, rd);

                if (cimConnectivityNode.TopologicalNodeHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimConnectivityNode.TopologicalNode.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimConnectivityNode.GetType().ToString()).Append(" rdfID = \"").Append(cimConnectivityNode.ID);
                        report.Report.Append("\" - Failed to set reference to Topological Node: rdfID \"").Append(cimConnectivityNode.TopologicalNode.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.CONNECTIVITYNODE_TOPOLOGICALNODE, gid));
                }

                if (cimConnectivityNode.ConnectivityNodeContainerHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimConnectivityNode.ConnectivityNodeContainer.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimConnectivityNode.GetType().ToString()).Append(" rdfID = \"").Append(cimConnectivityNode.ID);
                        report.Report.Append("\" - Failed to set reference to Connectivity Node Container: rdfID \"").Append(cimConnectivityNode.ConnectivityNodeContainer.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.CONNECTIVITYNODE_CONNECTIVITYNODECONTAINER, gid));
                }
			}
		}

        public static void PopulateTopologicalNodeProperties(FTN.TopologicalNode cimTopologicalNode, ResourceDescription rd)
        {
            if ((cimTopologicalNode != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimTopologicalNode, rd);
            }
        }

        public static void PopulateTerminalProperties(FTN.Terminal cimTerminal, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
            if ((cimTerminal != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimTerminal, rd);

                if (cimTerminal.ConnectivityNodeHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimTerminal.ConnectivityNode.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimTerminal.GetType().ToString()).Append(" rdfID = \"").Append(cimTerminal.ID);
                        report.Report.Append("\" - Failed to set reference to Connectivity Node: rdfID \"").Append(cimTerminal.ConnectivityNode.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.TERMINAL_CONNECTIVITYNODE, gid));
                }

                if (cimTerminal.ConductingEquipmentHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimTerminal.ConductingEquipment.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimTerminal.GetType().ToString()).Append(" rdfID = \"").Append(cimTerminal.ID);
                        report.Report.Append("\" - Failed to set reference to Conducting Equipment: rdfID \"").Append(cimTerminal.ConductingEquipment.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.TERMINAL_CONDUCTINGEQUIPMENT, gid));
                }
			}
        }

        #endregion Populate ResourceDescription

		#region Enums convert

		#endregion Enums convert
	}
}
