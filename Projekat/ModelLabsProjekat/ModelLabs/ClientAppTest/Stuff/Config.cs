using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace ClientAppTest.Stuff
{
    public class Config
    {

        private string resultDirecotry = string.Empty;

        public string ResultDirecotry => resultDirecotry;

        private Config()
        {
            resultDirecotry = ConfigurationManager.AppSettings["ResultDirecotry"];

            if (!Directory.Exists(resultDirecotry))
            {
                Directory.CreateDirectory(resultDirecotry);
            }
        }

        #region Static members

        private static Config instance = null;

        public static Config Instance => instance ?? (instance = new Config());

        #endregion Static members
    }
}