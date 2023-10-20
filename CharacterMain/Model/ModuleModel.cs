using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterMain.Model
{
    public class ModuleModel
    {
        Dictionary<string, object> moduleConfig;
        
        public ModuleModel(Dictionary<string, object> moduleConfig)
        {
            this.moduleConfig = moduleConfig ?? new Dictionary<string, object>();
        }
    }
}
