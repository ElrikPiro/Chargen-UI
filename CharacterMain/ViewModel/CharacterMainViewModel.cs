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
using Domain.Character;
using Domain.Character.Interfaces;
using System.Windows;
using Application.Modules.Factories;
using System.Collections.ObjectModel;

namespace CharacterMain.ViewModel
{
    public class CharacterMainViewModel : ViewModelBase
    {
        private List<string> _characterList;
        private bool _enableButtons;
        private int _characterIndex;
        private ICharacterDataProvider _characterDataProvider;
        Character _characterModel;
        private System.Collections.ObjectModel.ObservableCollection<string> _allModuleList;
        private string _selectedModule;
        private Dictionary<string, ModuleModel> _activeModules;
        private System.Windows.Visibility _inputNameModuleVisibility;

        public System.Windows.Visibility InputNameModuleVisibility 
        { 
            get
            {
                return HasModule(ModuleIndexes.InputName.ToString()) ? Visibility.Visible : Visibility.Collapsed;
            }
            
        }

        public System.Windows.Visibility GenderModuleVisibility
        {
            get
            {
                return HasModule(ModuleIndexes.GenderModule.ToString()) ? Visibility.Visible : Visibility.Collapsed;
            }

        }

        public CharacterMainViewModel(ICharacterDataProvider characterDataProvider)
        {
            _enableButtons = false;
            _characterIndex = 0;
            _characterDataProvider = characterDataProvider;
            _characterModel = new Character("");
            _inputNameModuleVisibility = System.Windows.Visibility.Collapsed;
            _allModuleList = new System.Collections.ObjectModel.ObservableCollection<string>(ModuleFactory.ModuleNames.Keys);
            _selectedModule = _allModuleList.First();
        }

        public string SelectedModule { 
            get => _selectedModule; 
            set => SetProperty(ref _selectedModule, value); 
        }

        public System.Collections.ObjectModel.ObservableCollection<string> ModuleList {
            get {
                return _allModuleList;
            }
        }

        public bool IsCharacterLoaded { 
            
            get => _characterIndex >= 0;
        
        }

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
                if(value >= 0 && value < _characterList.Count)
                {
                    Task.Run(LoadSelectedCharacter);
                    RaisePropertyChanged(nameof(SelectedCharacterIndex));
                    RaisePropertyChanged(nameof(EnableDeleteButton));
                }
                RaisePropertyChanged(nameof(IsCharacterLoaded));
            }
        }

        private async Task LoadSelectedCharacter()
        {
            Dictionary<string, ModuleModel> characterData = await Task.Run(() => { return _characterDataProvider.LoadCharacterModules(CharacterList.ElementAt(_characterIndex)); });
            _characterModel = new Character(CharacterList.ElementAt(_characterIndex), characterData);
            RefreshProperties();
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
            set
            {
                _characterModel.modules = value;
                RefreshProperties();
            }
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
                SelectedCharacterIndex = CharacterList.Count > indexAfterLoading ? indexAfterLoading : -1;
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
            RefreshProperties();

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
                _characterModel = await Task.Run(() => { return _characterDataProvider.ResolveCharacter(_characterModel); });
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Couldn't resolve the character. Cause: " + e.Message);
            }

            await LoadCharacterList();
        }

        private ActionCommand addModuleCommand;

        public ICommand AddModuleCommand
        {
            get
            {
                if (addModuleCommand == null)
                {
                    addModuleCommand = new ActionCommand(AddSelectedModule);
                }

                return addModuleCommand;
            }
        }

        private void AddSelectedModule()
        {
            addModule(SelectedModule);
            RefreshProperties();
        }

        private void addModule(string ModuleName)
        {
            if(HasModule(ModuleName))
            {
                System.Windows.MessageBox.Show("Current character already has this module");
            }
            else
            {
                _characterModel.modules[ModuleName] = ModuleFactory.GetDefaultModule(ModuleName);
            }
        }

        private bool HasModule(string moduleName)
        {
            return _characterModel.modules.Keys.Contains(moduleName);
        }

        private void RefreshProperties()
        {
            RaisePropertyChanged(nameof(InputNameModuleVisibility));
            RaisePropertyChanged(nameof(InputNameModuleValue));
            RaisePropertyChanged(nameof(InputNameModuleUnresolved));
            RaisePropertyChanged(nameof(GenderModuleVisibility));
            RaisePropertyChanged(nameof(GenderModuleValue));
        }

        public string InputNameModuleValue {
            get
            {
                try
                {
                    return _characterModel.modules[ModuleIndexes.InputName.ToString()].ModuleConfig["name"] as string;
                }
                catch(Exception e)
                {
                    return string.Empty;
                }
            }
            set 
            {
                _characterModel.modules[ModuleIndexes.InputName.ToString()].ModuleConfig["name"] = value;
                RaisePropertyChanged(nameof(InputNameModuleValue));
            }
        }

        public string GenderModuleValue
        {

            get
            {
                try
                {
                    return _characterModel.modules[ModuleIndexes.GenderModule.ToString()].ModuleConfig["gender"] as string;
                }
                catch (Exception e)
                {
                    return string.Empty;
                }
            }

            set
            {
                _characterModel.modules[ModuleIndexes.GenderModule.ToString()].ModuleConfig["gender"] = value;
                _characterModel.modules[ModuleIndexes.GenderModule.ToString()].ModuleConfig["export"] = value;
                RaisePropertyChanged(nameof(GenderModuleValue));
            }
        }

        public bool InputNameModuleUnresolved 
        { 
            get => _characterModel.modules.ContainsKey(ModuleIndexes.InputName.ToString()) && !_characterModel.modules[ModuleIndexes.InputName.ToString()].IsResolved; 
        }

        public ObservableCollection<string> GenderModuleOptions { 
            get
            {
                return new ObservableCollection<string> { "Male", "Female" };
            } 
        }

    }
}
