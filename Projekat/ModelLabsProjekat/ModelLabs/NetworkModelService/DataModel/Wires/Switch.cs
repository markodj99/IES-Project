using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class Switch : ConductingEquipment
    {
        private bool normalOpen = false;
        private float ratedCurrent = 0.0f;
        private bool retained = false;
        private int switchOnCount = 0;
        private DateTime switchOnDate = DateTime.MinValue;

        public Switch(long globalId) : base(globalId) { }

        public bool NormalOpen { get => normalOpen; set => normalOpen = value; }
        private float RatedCurrent { get => ratedCurrent; set => ratedCurrent = value; }
        private bool Retained { get => retained; set => retained = value; }
        private int SwitchOnCount { get => switchOnCount; set => switchOnCount = value; }
        private DateTime SwitchOnDate { get => switchOnDate; set => switchOnDate = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                Switch x = (Switch)obj;
                return x.NormalOpen == this.NormalOpen && x.RatedCurrent == this.RatedCurrent &&
                        x.Retained == this.Retained && x.SwitchOnCount == this.SwitchOnCount && x.SwitchOnDate == x.SwitchOnDate;
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
                case ModelCode.SWITCH_NORMALOPEN:
                case ModelCode.SWITCH_RATEDCURRENT:
                case ModelCode.SWITCH_RETAINED:
                case ModelCode.SWITCH_SWITCHONCOUNT:
                case ModelCode.SWITCH_SWITCHONDATE:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.SWITCH_NORMALOPEN:
                    property.SetValue(normalOpen);
                    break;

                case ModelCode.SWITCH_RATEDCURRENT:
                    property.SetValue(ratedCurrent);
                    break;

                case ModelCode.SWITCH_RETAINED:
                    property.SetValue(retained);
                    break;

                case ModelCode.SWITCH_SWITCHONCOUNT:
                    property.SetValue(switchOnCount);
                    break;
                case ModelCode.SWITCH_SWITCHONDATE:
                    property.SetValue(switchOnDate);
                    break;
                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.SWITCH_NORMALOPEN:
                    normalOpen = property.AsBool();
                    break;

                case ModelCode.SWITCH_RATEDCURRENT:
                    ratedCurrent = property.AsFloat();
                    break;

                case ModelCode.SWITCH_RETAINED:
                    retained = property.AsBool();
                    break;

                case ModelCode.SWITCH_SWITCHONCOUNT:
                    switchOnCount = property.AsInt();
                    break;
                case ModelCode.SWITCH_SWITCHONDATE:
                    switchOnDate = property.AsDateTime();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation
	}
}
