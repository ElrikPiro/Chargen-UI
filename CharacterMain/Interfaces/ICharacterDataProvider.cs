using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CharacterMain.Model;

namespace Infrastructure.Persistence.Interfaces
{
    public interface ICharacterDataProvider
    {
        List<string> LoadCharacterList();
        void CreateCharacter();
        void DeleteCharacter(string characterId);
        Dictionary<string, ModuleModel> LoadCharacterModules(string characterIndex);
        Dictionary<string, ModuleModel> ResolveCharacter(Dictionary<string, ModuleModel> modules);
    }
}
