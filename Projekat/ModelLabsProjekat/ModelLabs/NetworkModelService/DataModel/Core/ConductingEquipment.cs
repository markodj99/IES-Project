using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using FTN.Common;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
	public class ConductingEquipment : Equipment
	{
        private long baseVoltage = 0;
        private List<long> terminals = new List<long>();

		public ConductingEquipment(long globalId) : base(globalId) { }
        
		public long BaseVoltage { get => baseVoltage; set => baseVoltage = value; }
        public List<long> Terminals { get => terminals; set => terminals = value; }

		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
				ConductingEquipment x = (ConductingEquipment)obj;
				return ((x.BaseVoltage == this.BaseVoltage) && (CompareHelper.CompareLists(x.Terminals, this.Terminals)));
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
				case ModelCode.CONDUCTINGEQUIPMENT_BASEVOLTAGE:				
				case ModelCode.CONDUCTINGEQUIPMENT_TERMINALS:
                    return true;

				default:
					return base.HasProperty(property);
			}
		}

		public override void GetProperty(Property prop)
		{
			switch (prop.Id)
			{
                case ModelCode.CONDUCTINGEQUIPMENT_TERMINALS:
					prop.SetValue(terminals);
					break;
                case ModelCode.CONDUCTINGEQUIPMENT_BASEVOLTAGE:
					prop.SetValue(baseVoltage);
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
                case ModelCode.CONDUCTINGEQUIPMENT_BASEVOLTAGE:
					baseVoltage = property.AsReference();
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
            if (baseVoltage != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.CONDUCTINGEQUIPMENT_BASEVOLTAGE] = new List<long> {baseVoltage};
            }


			if (terminals != null && terminals.Count != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.CONDUCTINGEQUIPMENT_TERMINALS] = terminals.GetRange(0, terminals.Count);
            }

			base.GetReferences(references, refType);
		}

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.TERMINAL_CONDUCTINGEQUIPMENT:
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
                case ModelCode.TERMINAL_CONDUCTINGEQUIPMENT:

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
