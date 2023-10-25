using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Character.Interfaces;

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

}
