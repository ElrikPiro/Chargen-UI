using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Character
{
    public class ModuleModel
    {
        Dictionary<string, object> moduleConfig;
        
        public ModuleModel(Dictionary<string, object> moduleConfig)
        {
            this.moduleConfig = moduleConfig ?? new Dictionary<string, object>();
        }

        public bool IsResolved
        {
            get => moduleConfig.ContainsKey("resolved") && (moduleConfig["resolved"] == null ? false : (bool)moduleConfig["resolved"]);
            set
            {
                moduleConfig["resolved"] = value;
            }
        }

        public Dictionary<string, object> ModuleConfig
        {
            get => moduleConfig;
            set => moduleConfig = value;
        }

    }
}
