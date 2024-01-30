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
        Dictionary<string, string> solvedDependencies;
        
        public ModuleModel(Dictionary<string, object> moduleConfig)
        {
            this.moduleConfig = moduleConfig ?? new Dictionary<string, object>();
            this.solvedDependencies = new Dictionary<string, string>();
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

        public virtual List<string> GetDependencies()
        {
            return ModuleConfig.ContainsKey("dependencies") ? ModuleConfig["dependencies"] as List<string> : new List<string>();
        }

        public virtual void SetDependency(string key, string value)
        {
            solvedDependencies[key] = value;
        }

        public virtual Dictionary<string, string> GetSolvedDependencies()
        {
            return solvedDependencies;
        }

    }
}
