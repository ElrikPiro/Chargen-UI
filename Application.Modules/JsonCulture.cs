using Application.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Application.Modules
{

    public class Culture
    {
        public List<string> AttributeModuleDependencies { get; set; }
        public Dictionary<string, List<string>> CulturalNamesByAttribute { get; set; }
    }

    public class CultureDb
    {
        public Dictionary<string, Culture> Cultures { get; set; }
    }

    public class JsonCulture : ICultureModule
    {

        CultureDb cultureDb;
        
        Culture CultureData
        {
            get => cultureDb.Cultures[Culture];
        }

        public JsonCulture(Dictionary<string, object> moduleConfig) : base(moduleConfig)
        {
            Culture = moduleConfig["culture"] as string;

            //Load json data
            string jsonFile = System.IO.File.ReadAllText("config\\cultures.json");
            cultureDb = JsonConvert.DeserializeObject<CultureDb>(jsonFile);

        }

        override public string GenerateCulturalName()
        {
            //check if cultureDb has the selected culture
            if (!cultureDb.Cultures.ContainsKey(Culture))
            {
                throw new Exception("cultures.json has no culture named '" + Culture + "'");
            }

            List<string> elegibleNames = new List<string>();

            //se pide cada una de las dependencias, que estarán como un atributo dentro del json del modulo
            foreach (var dependency in CultureData.AttributeModuleDependencies)
            {
                var dependencyValue = GetSolvedDependencies()[dependency];
                elegibleNames = elegibleNames.Concat(CultureData.CulturalNamesByAttribute[dependencyValue]).ToList();
            }

            //se resuelve el nombre cultural con el atributo de la referencia
            var random = new Random();
            var randomElement = elegibleNames[random.Next(elegibleNames.Count)];

            return randomElement;
        }

        override public List<string> GetDependencies()
        {
            return CultureData.AttributeModuleDependencies;
        }

    }
}
