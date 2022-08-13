using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;

namespace FTN.Services.NetworkModelService.DataModel.Topology
{
    public class TopologicalNode : IdentifiedObject
    {
        private List<long> connectivityNodes = new List<long>();

        public TopologicalNode(long globalId) : base(globalId) { }

        private List<long> ConnectivityNodes { get => connectivityNodes; set => connectivityNodes = value; }

		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
                TopologicalNode x = (TopologicalNode)obj;
				return CompareHelper.CompareLists(x.ConnectivityNodes, this.ConnectivityNodes);
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#region IAccess implementation

		public override bool HasProperty(ModelCode property)
		{
			switch (property)
			{
                case ModelCode.TOPOLOGICALNODE_CONNECTIVITYNODES:
					return true;

				default:
					return base.HasProperty(property);
			}
		}

		public override void GetProperty(Property prop)
		{
			switch (prop.Id)
			{
				case ModelCode.TOPOLOGICALNODE_CONNECTIVITYNODES:
					prop.SetValue(connectivityNodes);
					break;
                default:
					base.GetProperty(prop);
					break;
			}
		}

		public override void SetProperty(Property property) => base.SetProperty(property);

        #endregion IAccess implementation

		#region IReference implementation

		public override bool IsReferenced => connectivityNodes.Count != 0 || base.IsReferenced;

		public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
		{
            if (connectivityNodes != null && connectivityNodes.Count != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
			{
				references[ModelCode.TOPOLOGICALNODE_CONNECTIVITYNODES] = connectivityNodes.GetRange(0, connectivityNodes.Count);
			}

			base.GetReferences(references, refType);
		}

		public override void AddReference(ModelCode referenceId, long globalId)
		{
			switch (referenceId)
			{
				case ModelCode.CONNECTIVITYNODE_TOPOLOGICALNODE:
                    connectivityNodes.Add(globalId);
					break;

				default:
					base.AddReference(referenceId, globalId);
					break;
			}
		}

		public override void RemoveReference(ModelCode referenceId, long globalId)
		{
			switch (referenceId)
			{
				case ModelCode.CONNECTIVITYNODE_TOPOLOGICALNODE:

					if (connectivityNodes.Contains(globalId))
					{
                        connectivityNodes.Remove(globalId);
					}
					else
					{
						CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
					}

					break;

				default:
					base.RemoveReference(referenceId, globalId);
					break;
			}
		}

		#endregion IReference implementation
	}
}
