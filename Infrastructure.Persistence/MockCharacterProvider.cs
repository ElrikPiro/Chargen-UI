using Domain.Character.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Character;
using System.Collections;
using Application.Modules.Factories;
using Application.Modules.Interfaces;

namespace Infrastructure.Persistence
{
    public class MockCharacterDataProvider : ICharacterDataProvider
    {
        private readonly Dictionary<string, Character> _characterList;

        public MockCharacterDataProvider()
        {
            _characterList = new Dictionary<string, Character>();
        }

        public List<string> LoadCharacterList()
        {
            return new List<string>(_characterList.Keys);
        }

        public void CreateCharacter()
        {
            string newCharacter = DateTime.Now.ToString("yyyyMMddHHmmss");
            if (!_characterList.ContainsKey(newCharacter))
            {
                var character = new Character(newCharacter, GenerateNewCharacterModules());
                _characterList.Add(newCharacter, character);
            }
            else
            {
                throw new Exception("Character already exists.");
            }
        }

        public void DeleteCharacter(string characterId)
        {
            if (!_characterList.ContainsKey(characterId))
            {
                throw new Exception("Character does not exist.");
            }
            _characterList.Remove(characterId);
        }

        public Character ResolveCharacter(Character character)
        {
            return MockModuleResolver.ResolveModules(character);
        }

        public Dictionary<string, ModuleModel> GenerateNewCharacterModules()
        {

            var retval = new Dictionary<string, ModuleModel>();

            retval.Add("MockModule", GenerateMockModule());

            return retval;
        }

        public Dictionary<string, ModuleModel> LoadCharacterModules(string characterIndex)
        {
            return _characterList[characterIndex].modules;
        }

        private ModuleModel GenerateMockModule()
        {
            var mockModuleRaw = new Dictionary<string, object>();
            mockModuleRaw.Add("module", "MockModule");
            mockModuleRaw.Add("version", "v0.231025.1750");

            return new ModuleModel(mockModuleRaw);
        }
    }

    static class MockModuleResolver
    {

        private static void ResolveModule(Character character, string moduleName)
        {
            if (!character.modules[moduleName].IsResolved)
            {
                //has dependencies?
                if (character.modules[moduleName].ModuleConfig.ContainsKey("dependencies") || character.modules[moduleName].GetDependencies().Count > 0)
                {
                    List<string> unsolvedDependencies = new List<string>();
                    
                    //resolve dependencies
                    foreach (string dependency in character.modules[moduleName].GetDependencies())
                    {
                        if(character.modules.ContainsKey(dependency))
                        {
                            ResolveModule(character, dependency);
                            if (character.modules[dependency].ModuleConfig.ContainsKey("export"))
                            {
                                character.modules[moduleName].SetDependency(
                                    dependency, 
                                    character.modules[dependency].ModuleConfig["export"] as string
                                );
                            }
                        }
                        else
                        {
                            unsolvedDependencies.Add(dependency);
                        }
                    }

                    if(unsolvedDependencies.Count > 0)
                    {
                        throw new Exception("Unmet module dependencies: " + string.Join(", ", unsolvedDependencies));
                    }
                }

                //resolution
                if(moduleName.Equals(ModuleIndexes.CulturalName.ToString()))
                {
                    string INPUT_NAME_MODULE = ModuleIndexes.InputName.ToString();

                    var cultureModule = character.modules[ModuleIndexes.Culture.ToString()] as ICultureModule;
                    var generatedName = cultureModule.GenerateCulturalName();
                    var inputNameModule = ModuleFactory.GetDefaultModule(INPUT_NAME_MODULE);

                    inputNameModule.ModuleConfig["name"] = generatedName;
                    character.modules[INPUT_NAME_MODULE] = inputNameModule;
                    ResolveModule(character, INPUT_NAME_MODULE);
                    character.modules[moduleName].ModuleConfig["export"] = generatedName;
                }
                
                character.modules[moduleName].IsResolved = true;
                
            }
        }

        public static Character ResolveModules(Character character)
        {
            //TODO: Implementar la resolución de submódulos bien.

            var cachedCollection = character.modules.Keys.ToArray();

            foreach(string moduleName in cachedCollection)
            {
                ResolveModule(character, moduleName);
            }

            return character;
        }

    }

}
