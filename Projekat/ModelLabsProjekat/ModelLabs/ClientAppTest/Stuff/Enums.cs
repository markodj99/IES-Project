using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientAppTest.Stuff
{
    public static class Enums
    {
        public enum Methods
        {
            GetValues,
            GetExtentValues,
            GetRelatedValues,
            Unknown
        }

        public static Methods GetMethodEnum(string method)
        {
            switch (method)
            {
                case "GetValues":
                    return Methods.GetValues;
                case "GetExtentValues":
                    return Methods.GetExtentValues;
                case "GetRelatedVlaues":
                    return Methods.GetRelatedValues;
                default:
                    return Methods.Unknown;
            }
        }
    }
}
