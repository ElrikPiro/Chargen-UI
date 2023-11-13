using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Character.Factories
{
    public enum ModuleIndexes
    {
        InputName,
    }

    

    public static class ModuleFactory
    {
        public static Dictionary<string, ModuleIndexes> ModuleNames = new Dictionary<string, ModuleIndexes>
        {
            {ModuleIndexes.InputName.ToString(), ModuleIndexes.InputName}
        };

        public static ModuleModel GetDefaultModule(string key)
        {
            var index = ModuleNames[key];
            ModuleModel retval = null;

            switch(index)
            {
                case ModuleIndexes.InputName:
                    retval = new ModuleModel(
                        new Dictionary<string, object>
                        {
                            { "name", "" }
                        }
                    );
                    break;
                default:
                    break;
            }

            return retval;
        }

    }
}
