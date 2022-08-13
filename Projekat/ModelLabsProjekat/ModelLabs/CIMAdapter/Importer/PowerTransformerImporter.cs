using System;
using System.Collections.Generic;
using CIM.Model;
using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter.Manager;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	/// <summary>
	/// PowerTransformerImporter
	/// </summary>
	public class PowerTransformerImporter
	{
		/// <summary> Singleton </summary>
		private static PowerTransformerImporter ptImporter = null;
		private static object singletoneLock = new object();

		private ConcreteModel concreteModel;
		private Delta delta;
		private ImportHelper importHelper;
		private TransformAndLoadReport report;

		#region Properties
		public static PowerTransformerImporter Instance
		{
			get
			{
				if (ptImporter == null)
				{
					lock (singletoneLock)
					{
						if (ptImporter == null)
						{
							ptImporter = new PowerTransformerImporter();
							ptImporter.Reset();
						}
					}
				}
				return ptImporter;
			}
		}

		public Delta NMSDelta
		{
			get 
			{
				return delta;
			}
		}
		#endregion Properties

		public void Reset()
		{
			concreteModel = null;
			delta = new Delta();
			importHelper = new ImportHelper();
			report = null;
		}

		public TransformAndLoadReport CreateNMSDelta(ConcreteModel cimConcreteModel)
		{
			LogManager.Log("Importing PowerTransformer Elements...", LogLevel.Info);
			report = new TransformAndLoadReport();
			concreteModel = cimConcreteModel;
			delta.ClearDeltaOperations();

			if ((concreteModel != null) && (concreteModel.ModelMap != null))
			{
				try
				{
					// convert into DMS elements
					ConvertModelAndPopulateDelta();
				}
				catch (Exception ex)
				{
					string message = string.Format("{0} - ERROR in data import - {1}", DateTime.Now, ex.Message);
					LogManager.Log(message);
					report.Report.AppendLine(ex.Message);
					report.Success = false;
				}
			}
			LogManager.Log("Importing PowerTransformer Elements - END.", LogLevel.Info);
			return report;
		}

		/// <summary>
		/// Method performs conversion of network elements from CIM based concrete model into DMS model.
		/// </summary>
		private void ConvertModelAndPopulateDelta()
		{
			LogManager.Log("Loading elements and creating delta...", LogLevel.Info);

			//// import all concrete model types (DMSType enum)
            ImportConnectivityNodeContainers();
            ImportTopologicalNodes();
            ImportConnectivityNodes();
            ImportBaseVoltages();
            ImportSwitches();
            ImportTerminals();

            LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
		}

		#region Import

        private void ImportConnectivityNodeContainers()
        {
			SortedDictionary<string, object> cimConnectivityNodeContainers = concreteModel.GetAllObjectsOfType("FTN.ConnectivityNodeContainer");
            if (cimConnectivityNodeContainers != null)
            {
                foreach (KeyValuePair<string, object> cimConnectivityNodeContainerPair in cimConnectivityNodeContainers)
                {
                    FTN.ConnectivityNodeContainer cimConnectivityNodeContainer = cimConnectivityNodeContainerPair.Value as FTN.ConnectivityNodeContainer;

                    ResourceDescription rd = CreateConnectivityNodeContainerDescription(cimConnectivityNodeContainer);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Base Voltage ID = ").Append(cimConnectivityNodeContainer.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Base Voltage ID = ").Append(cimConnectivityNodeContainer.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
		}

        private ResourceDescription CreateConnectivityNodeContainerDescription(FTN.ConnectivityNodeContainer cimConnectivityNodeContainer)
        {
            ResourceDescription rd = null;
            if (cimConnectivityNodeContainer != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.CONNECTIVITYNODECONTAINER, importHelper.CheckOutIndexForDMSType(DMSType.CONNECTIVITYNODECONTAINER));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimConnectivityNodeContainer.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateConnectivityNodeContainerProperties(cimConnectivityNodeContainer, rd);
            }
            return rd;
        }

        private void ImportTopologicalNodes()
        {
            SortedDictionary<string, object> cimTopologicalNodes = concreteModel.GetAllObjectsOfType("FTN.TopologicalNode");
            if (cimTopologicalNodes != null)
            {
                foreach (KeyValuePair<string, object> cimTopologicalNodePair in cimTopologicalNodes)
                {
                    FTN.TopologicalNode cimTopologicalNode = cimTopologicalNodePair.Value as FTN.TopologicalNode;

                    ResourceDescription rd = CreateTopologicalNodeDescription(cimTopologicalNode);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Topological Node ID = ").Append(cimTopologicalNode.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Topological Node ID = ").Append(cimTopologicalNode.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
		}

        private ResourceDescription CreateTopologicalNodeDescription(FTN.TopologicalNode cimTopologicalNode)
        {
			ResourceDescription rd = null;
            if (cimTopologicalNode != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.TOPOLOGICALNODE, importHelper.CheckOutIndexForDMSType(DMSType.TOPOLOGICALNODE));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimTopologicalNode.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateTopologicalNodeProperties(cimTopologicalNode, rd);
            }
            return rd;
		}

		private void ImportConnectivityNodes()
        {
            SortedDictionary<string, object> cimConnectivityNodes = concreteModel.GetAllObjectsOfType("FTN.ConnectivityNode");
            if (cimConnectivityNodes != null)
            {
                foreach (KeyValuePair<string, object> cimConnectivityNodePair in cimConnectivityNodes)
                {
                    FTN.ConnectivityNode cimConnectivityNode = cimConnectivityNodePair.Value as FTN.ConnectivityNode;

                    ResourceDescription rd = CreateConnectivityNodeDescription(cimConnectivityNode);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Connectivity Node ID = ").Append(cimConnectivityNode.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Connectivity Node ID = ").Append(cimConnectivityNode.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
		}

        private ResourceDescription CreateConnectivityNodeDescription(FTN.ConnectivityNode cimConnectivityNode)
        {
            ResourceDescription rd = null;
            if (cimConnectivityNode != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.CONNECTIVITYNODE, importHelper.CheckOutIndexForDMSType(DMSType.CONNECTIVITYNODE));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimConnectivityNode.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateConnectivityNodeProperties(cimConnectivityNode, rd, importHelper, report);
            }
            return rd;
        }

        private void ImportBaseVoltages()
        {
            SortedDictionary<string, object> cimBaseVoltages = concreteModel.GetAllObjectsOfType("FTN.BaseVoltage");
            if (cimBaseVoltages != null)
            {
                foreach (KeyValuePair<string, object> cimBaseVoltagePair in cimBaseVoltages)
                {
                    FTN.BaseVoltage cimBaseVoltage = cimBaseVoltagePair.Value as FTN.BaseVoltage;

                    ResourceDescription rd = CreateBaseVoltageResourceDescription(cimBaseVoltage);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Base Voltage ID = ").Append(cimBaseVoltage.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Base Voltage ID = ").Append(cimBaseVoltage.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateBaseVoltageResourceDescription(FTN.BaseVoltage cimBaseVoltage)
        {
            ResourceDescription rd = null;
            if (cimBaseVoltage != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.BASEVOLTAGE, importHelper.CheckOutIndexForDMSType(DMSType.BASEVOLTAGE));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimBaseVoltage.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateBaseVoltageProperties(cimBaseVoltage, rd);
            }
            return rd;
        }

        private void ImportSwitches()
        {
            SortedDictionary<string, object> cimSwitches = concreteModel.GetAllObjectsOfType("FTN.Switch");
            if (cimSwitches != null)
            {
                foreach (KeyValuePair<string, object> cimSwitchPair in cimSwitches)
                {
                    FTN.Switch cimSwitch = cimSwitchPair.Value as FTN.Switch;

                    ResourceDescription rd = CreateSwitchDescription(cimSwitch);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Switch ID = ").Append(cimSwitch.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Switch ID = ").Append(cimSwitch.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
		}

        private ResourceDescription CreateSwitchDescription(FTN.Switch cimSwitch)
        {
            ResourceDescription rd = null;
            if (cimSwitch != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.SWITCH, importHelper.CheckOutIndexForDMSType(DMSType.SWITCH));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimSwitch.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateSwitchProperties(cimSwitch, rd, importHelper, report);
            }
            return rd;
        }

        private void ImportTerminals()
        {
            SortedDictionary<string, object> cimTerminals = concreteModel.GetAllObjectsOfType("FTN.Terminal");
            if (cimTerminals != null)
            {
                foreach (KeyValuePair<string, object> cimTerminalPair in cimTerminals)
                {
                    FTN.Terminal cimTerminal = cimTerminalPair.Value as FTN.Terminal;

                    ResourceDescription rd = CreateTerminalDescription(cimTerminal);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Terminal ID = ").Append(cimTerminal.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Terminal ID = ").Append(cimTerminal.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateTerminalDescription(FTN.Terminal cimTerminal)
        {
            ResourceDescription rd = null;
            if (cimTerminal != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.TERMINAL, importHelper.CheckOutIndexForDMSType(DMSType.TERMINAL));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimTerminal.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateTerminalProperties(cimTerminal, rd, importHelper, report);
            }
            return rd;
        }

        #endregion Import
    }
}

