using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Interfaces
{
    public interface ICharacterDataProvider
    {
        List<string> LoadCharacterList();
        void CreateCharacter();
        void DeleteCharacter(string characterId);
    }
}
