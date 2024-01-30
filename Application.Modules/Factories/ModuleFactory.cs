using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Modules.Interfaces;
using Domain.Character;

namespace Application.Modules.Factories
{
    public enum ModuleIndexes
    {
        InputName,
        Culture,
        CulturalName,
        GenderModule
    }


    public static class ModuleFactory
    {
        public static Dictionary<string, ModuleIndexes> ModuleNames = new Dictionary<string, ModuleIndexes>
        {
            {ModuleIndexes.InputName.ToString(), ModuleIndexes.InputName},
            {ModuleIndexes.Culture.ToString(), ModuleIndexes.Culture},
            {ModuleIndexes.CulturalName.ToString(), ModuleIndexes.CulturalName},
            {ModuleIndexes.GenderModule.ToString(), ModuleIndexes.GenderModule}
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
                case ModuleIndexes.Culture:
                    var mockupCulture = "mockup";
                    retval = new JsonCulture(
                        new Dictionary<string, object>
                        {
                            { "culture", mockupCulture as object }
                        }
                    );
                    break;
                case ModuleIndexes.CulturalName:
                    retval = new ModuleModel(
                        new Dictionary<string, object>
                        {
                            { "checked", true },
                            { "dependencies", new List<string>{ ModuleIndexes.Culture.ToString() } }
                        }
                    );
                    break;
                case ModuleIndexes.GenderModule:
                    retval = new ModuleModel(
                        new Dictionary<string, object>
                        {
                            { "gender", "" }
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
