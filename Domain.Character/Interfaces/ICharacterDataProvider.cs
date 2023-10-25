using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Character;

namespace Domain.Character.Interfaces
{
    public interface ICharacterDataProvider
    {
        List<string> LoadCharacterList();
        void CreateCharacter();
        void DeleteCharacter(string characterId);
        Dictionary<string, ModuleModel> LoadCharacterModules(string characterIndex);
        Character ResolveCharacter(Character modules);
    }
}
