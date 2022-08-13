using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTN.Common;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class Terminal : IdentifiedObject
    {
        private long connectivityNode = 0;
        private long conductingEquipment = 0;

        public Terminal(long globalId) : base(globalId) { }

        public long ConnectivityNode { get => connectivityNode; set => connectivityNode = value; }
        public long ConductingEquipment { get => conductingEquipment; set => conductingEquipment = value; }

        public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
                Terminal x = (Terminal)obj;
				return x.ConnectivityNode == this.ConnectivityNode && x.ConductingEquipment == this.ConductingEquipment;
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
				case ModelCode.TERMINAL_CONNECTIVITYNODE:
				case ModelCode.TERMINAL_CONDUCTINGEQUIPMENT:
                    return true;

				default:
					return base.HasProperty(property);
			}
		}

		public override void GetProperty(Property prop)
		{
			switch (prop.Id)
			{
				case ModelCode.TERMINAL_CONNECTIVITYNODE:
					prop.SetValue(connectivityNode);
					break;
                case ModelCode.TERMINAL_CONDUCTINGEQUIPMENT:
					prop.SetValue(conductingEquipment);
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
				case ModelCode.TERMINAL_CONNECTIVITYNODE:
					connectivityNode = property.AsReference();
					break;
				case ModelCode.TERMINAL_CONDUCTINGEQUIPMENT:
					conductingEquipment = property.AsReference();
					break;
				default:
					base.SetProperty(property);
					break;
			}
		}

		#endregion IAccess implementation

		#region IReference implementation

		public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
		{
			if (connectivityNode != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
			{
				references[ModelCode.TERMINAL_CONNECTIVITYNODE] = new List<long> { connectivityNode };
			}

			if (conductingEquipment != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
			{
				references[ModelCode.TERMINAL_CONDUCTINGEQUIPMENT] = new List<long> { conductingEquipment };
			}

            base.GetReferences(references, refType);
		}

		#endregion IReference implementation
	}
}
