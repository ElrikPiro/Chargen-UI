using Domain.Character.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Character;

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

        private static ModuleModel ResolveMockModule(ModuleModel module)
        {
            module.IsResolved = true;
            return module;
        }

        private static void ResolveInputNameModule(Character character)
        {
            character.modules["InputName"].IsResolved = true;
        }

        public static Character ResolveModules(Character character)
        {
            character.modules["MockModule"] = MockModuleResolver.ResolveMockModule(character.modules["MockModule"]);
            MockModuleResolver.ResolveInputNameModule(character);
            return character;
        }

    }

}
