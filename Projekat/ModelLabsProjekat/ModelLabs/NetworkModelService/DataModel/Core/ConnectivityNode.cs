using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTN.Common;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class ConnectivityNode : IdentifiedObject
    {
        private long topologicalNode = 0;
        private long connectivityNodeContainer = 0;
        private List<long> terminals = new List<long>();

        public ConnectivityNode(long globalId) : base(globalId) { }

        public long TopologicalNode { get => topologicalNode; set => topologicalNode = value; }
        public long ConnectivityNodeContainer { get => connectivityNodeContainer; set => connectivityNodeContainer = value; }
        public List<long> Terminals { get => terminals; set => terminals = value; }

        public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
                ConnectivityNode x = (ConnectivityNode)obj;
				return x.TopologicalNode == this.TopologicalNode
                       && x.ConnectivityNodeContainer == this.ConnectivityNodeContainer
                       && CompareHelper.CompareLists(x.Terminals, this.Terminals);
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
				case ModelCode.CONNECTIVITYNODE_TOPOLOGICALNODE:
				case ModelCode.CONNECTIVITYNODE_CONNECTIVITYNODECONTAINER:
                case ModelCode.CONNECTIVITYNODE_TERMINALS:
					return true;

				default:
					return base.HasProperty(property);
			}
		}

		public override void GetProperty(Property prop)
		{
			switch (prop.Id)
			{
				case ModelCode.CONNECTIVITYNODE_TOPOLOGICALNODE:
					prop.SetValue(topologicalNode);
					break;

				case ModelCode.CONNECTIVITYNODE_CONNECTIVITYNODECONTAINER:
					prop.SetValue(connectivityNodeContainer);
					break;

                case ModelCode.CONNECTIVITYNODE_TERMINALS:
                    prop.SetValue(terminals);
                    break;

				default:
					base.GetProperty(prop);
					break;
			}
		}

		public override void SetProperty(Property property)
		{
			switch (property.Id)
			{
                case ModelCode.CONNECTIVITYNODE_TOPOLOGICALNODE:
                    topologicalNode = property.AsReference();
                    break;
                case ModelCode.CONNECTIVITYNODE_CONNECTIVITYNODECONTAINER:
                    connectivityNodeContainer = property.AsReference();
                    break;
                default:
					base.SetProperty(property);
					break;
			}
		}

		#endregion IAccess implementation

		#region IReference implementation

		public override bool IsReferenced => terminals.Count != 0 || base.IsReferenced;

		public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
		{
			if (topologicalNode != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
			{
				references[ModelCode.CONNECTIVITYNODE_TOPOLOGICALNODE] = new List<long> { topologicalNode };
			}

            if (connectivityNodeContainer != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.CONNECTIVITYNODE_CONNECTIVITYNODECONTAINER] = new List<long> { connectivityNodeContainer };
            }

			if (terminals != null && terminals.Count != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
			{
				references[ModelCode.CONNECTIVITYNODE_TERMINALS] = terminals.GetRange(0, terminals.Count);
			}

			base.GetReferences(references, refType);
		}

		public override void AddReference(ModelCode referenceId, long globalId)
		{
			switch (referenceId)
			{
				case ModelCode.TERMINAL_CONNECTIVITYNODE:
					terminals.Add(globalId);
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
				case ModelCode.TERMINAL_CONNECTIVITYNODE:

					if (terminals.Contains(globalId))
					{
						terminals.Remove(globalId);
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
