using Domain.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Interfaces
{
    public abstract class ICultureModule : ModuleModel
    {
        public ICultureModule(Dictionary<string, object> moduleConfig) : base(moduleConfig)
        {
            ModuleConfig["culture"] = moduleConfig.ContainsKey("culture") ? moduleConfig["culture"] : string.Empty;
        }

        public virtual string GenerateCulturalName()
        {
            throw new Exception("Invalid instance for Culture Module");
        }

        public string Culture
        {
            get => ModuleConfig["culture"] as string;
            set => ModuleConfig["culture"] = value;
        }

    }

    public class MockupCulture : ICultureModule
    {
        public MockupCulture(Dictionary<string, object> moduleConfig) : base(moduleConfig)
        {
            this.Culture = "mockup";
        }

        override public string GenerateCulturalName()
        {
            var data = new byte[1];
            RandomNumberGenerator.Create().GetBytes(data, 0, 1);
            var isOdd = (data[0] & 0x01) == 0x01;
            return isOdd ? "Foo" : "Bar";
        }

    }

}
