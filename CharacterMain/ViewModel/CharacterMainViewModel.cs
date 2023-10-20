using Microsoft.Expression.Interactivity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using MakCraft.ViewModels;
using System.Windows.Input;
using CharacterMain.Model;
using Infrastructure.Persistence.Interfaces;

namespace CharacterMain.ViewModel
{
    public class CharacterMainViewModel : ViewModelBase
    {
        private List<string> _characterList;
        private bool _enableButtons;
        private int _characterIndex;
        private ICharacterDataProvider _characterDataProvider;
        Character _characterModel;

        public List<string> CharacterList
        {
            get => _characterList;

            set
            {
                SetProperty(ref _characterList, value, "CharacterList");
                RaisePropertyChanged("CharacterList");
                RaisePropertyChanged(nameof(EnableDeleteButton));
            }
        }

        public int SelectedCharacterIndex {
            get => _characterIndex;
            set
            {
                SetProperty(ref _characterIndex, value, nameof(SelectedCharacterIndex));
                Task.Run(LoadSelectedCharacter);
                RaisePropertyChanged(nameof(SelectedCharacterIndex));
                RaisePropertyChanged(nameof(EnableDeleteButton));
            }
        }

        private async Task LoadSelectedCharacter()
        {
            Dictionary<string, ModuleModel> characterData = await Task.Run(() => { return _characterDataProvider.LoadCharacterModules(CharacterList.ElementAt(_characterIndex)); });
            _characterModel = new Character(CharacterList.ElementAt(_characterIndex) , characterData);
        }

        public bool EnableButtons
        {
            get => _enableButtons;

            set
            {
                SetProperty(ref _enableButtons, value, "EnableButtons");
                RaisePropertyChanged(nameof(EnableButtons));
                RaisePropertyChanged(nameof(EnableDeleteButton));
            }
        }

        public bool EnableDeleteButton
        {
            get => _enableButtons && CanDeleteCharacter();
        }

        public Dictionary<string, ModuleModel> Modules
        {
            get => _characterModel.modules;
            set => _characterModel.modules = value;
        }

        public CharacterMainViewModel(ICharacterDataProvider characterDataProvider)
        {
            _enableButtons = false;
            _characterIndex = -1;
            _characterDataProvider = characterDataProvider;
            _characterModel = new Character("");
        }

        public async Task LoadCharacterList()
        {

            EnableButtons = false;

            var indexAfterLoading = _characterIndex != -1 ? _characterIndex : -1;

            _characterList = new List<string>
            {
                "Loading..."
            };
            CharacterList = _characterList;

            try
            {
                CharacterList = await Task.Run(_characterDataProvider.LoadCharacterList);
                SelectedCharacterIndex = CharacterList.Count >= indexAfterLoading ? indexAfterLoading : -1;
            }
            catch(Exception e)
            {
                throw e;
            }
            
            // --- Implementation details that should not be at this level --- 
            //var client = new HttpClient();
            //HttpResponseMessage response = await client.GetAsync("http://localhost:8000/getCharacterIds");
            //response.EnsureSuccessStatusCode();

            //string responseBody = await response.Content.ReadAsStringAsync();

            //if (!string.IsNullOrEmpty(responseBody))
            //{
            //    return JsonSerializer.Deserialize<List<string>>(responseBody);
            //}

            EnableButtons = true;

        }

        private ActionCommand createCharacterCommand;

        public ICommand CreateCharacterCommand
        {
            get
            {
                if (createCharacterCommand == null)
                {
                    createCharacterCommand = new ActionCommand(CreateCharacter);
                }

                return createCharacterCommand;
            }
        }

        private ActionCommand _deleteCharacterCommand;
        public System.Windows.Input.ICommand DeleteCharacterCommand
        {
            get
            {
                if (_deleteCharacterCommand == null)
                {
                    _deleteCharacterCommand = new ActionCommand(DeleteCharacter);
                }
                return _deleteCharacterCommand;
            }
        }

        private bool CanDeleteCharacter()
        {
            return _characterIndex >= 0 && _characterIndex < _characterList.Count;
        }

        private async void DeleteCharacter()
        {
            if (CanDeleteCharacter())
            {
                await Task.Run(() => { _characterDataProvider.DeleteCharacter(_characterList.ElementAt(_characterIndex)); });
                await LoadCharacterList();
            }
        }


        StringContent getEncodedJsonFromCharacter(Character character)
        {
            var obj = new
            {
                id_ = character.id_,
                modules = character.modules
            };
            var serializedObj = JsonSerializer.Serialize(obj);
            return new StringContent(serializedObj, Encoding.UTF8, "application/json");
        }

        string getCurrentTimeString()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        async private void CreateCharacter()
        {
            // --- Implementation details that should not be at this level --- 
            //var client = new HttpClient();
            //var content = getEncodedJsonFromCharacter( new Character(
            //        getCurrentTimeString()
            //    )
            //);
            //HttpResponseMessage response = await client.PostAsync("http://localhost:8000/buildCharacter", content);
            //response.EnsureSuccessStatusCode();

            //string responseBody = await response.Content.ReadAsStringAsync();

            try
            {
                await Task.Run(_characterDataProvider.CreateCharacter);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Couldn't create the character. Cause: " + e.Message);
            }

            await LoadCharacterList();
        }

        private ActionCommand resolveCharacterCommand;

        public ICommand ResolveCharacterCommand
        {
            get
            {
                if (resolveCharacterCommand == null)
                {
                    resolveCharacterCommand = new ActionCommand(ResolveCharacter);
                }

                return resolveCharacterCommand;
            }
        }

        async private void ResolveCharacter()
        {
            try
            {
                Modules = await Task.Run(() => { return _characterDataProvider.ResolveCharacter(Modules); });
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Couldn't resolve the character. Cause: " + e.Message);
            }

            await LoadCharacterList();
        }
    }
}
