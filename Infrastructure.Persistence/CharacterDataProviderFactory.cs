using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Persistence.Interfaces;

namespace Infrastructure.Persistence
{

    public enum CharacterDataProviderType
    {
        MOCK_CHARACTER_DATA_PROVIDER = 0
    }

    public class CharacterDataProviderFactory
    {
        private CharacterDataProviderFactory() { }

        public static ICharacterDataProvider BuildCharacterDataProvider(CharacterDataProviderType type, params object[] args)
        {

            switch (type)
            {
                case CharacterDataProviderType.MOCK_CHARACTER_DATA_PROVIDER:
                    return new MockCharacterDataProvider();
                default:
                    throw new Exception("Invalid Character data Type");
            }

        }

    }

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

    }

}
