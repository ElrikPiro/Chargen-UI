using Infrastructure.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CharacterMain.Model;

namespace Infrastructure.Persistence
{
    public class MockCharacterDataProvider : ICharacterDataProvider
    {
        private readonly List<string> _characterList;

        public MockCharacterDataProvider()
        {
            _characterList = new List<string>();
        }

        public List<string> LoadCharacterList()
        {
            return _characterList;
        }

        public void CreateCharacter()
        {
            string newCharacter = DateTime.Now.ToString("yyyyMMddHHmmss");
            if (!_characterList.Contains(newCharacter))
            {
                _characterList.Add(newCharacter);
            }
            else
            {
                throw new Exception("Character already exists.");
            }
        }

        public void DeleteCharacter(string characterId)
        {
            if (!_characterList.Contains(characterId))
            {
                throw new Exception("Character does not exist.");
            }
            _characterList.Remove(characterId);
        }

        public Dictionary<string, ModuleModel> ResolveCharacter(Dictionary<string, ModuleModel> modules)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, ModuleModel> LoadCharacterModules(string characterIndex)
        {
            throw new NotImplementedException();
        }
    }
}
