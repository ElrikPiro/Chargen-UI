using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Character
{
    public class Character
    {
        public string id_;
        public Dictionary<string, ModuleModel> modules;

        public Character(string id_, Dictionary<string, ModuleModel> modules = null)
        {
            this.id_ = id_;
            this.modules = modules ?? new Dictionary<string, ModuleModel>();
        }

        //override public string ToString() 
        //{
        //    StringBuilder stringBuilder = new StringBuilder();
        //    stringBuilder.Append("{");
        //    stringBuilder.Append("id_: \"" + id_ + "\"");

        //    return stringBuilder.ToString();
        //}

    }
}
