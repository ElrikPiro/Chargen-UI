﻿using Microsoft.Expression.Interactivity.Core;
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

        public List<string> CharacterList 
        {
            get => _characterList;
            
            set
            {
                SetProperty(ref _characterList, value, "CharacterList");
                RaisePropertyChanged("CharacterList");
            }
        }

        public int SelectedCharacterIndex { 
            get => _characterIndex; 
            set
            {
                SetProperty(ref _characterIndex, value, nameof(SelectedCharacterIndex));
                RaisePropertyChanged(nameof(SelectedCharacterIndex));
            }
        }

        public bool EnableButtons
        {
            get => _enableButtons;

            set
            {
                SetProperty(ref _enableButtons, value, "EnableButtons");
                RaisePropertyChanged("EnableButtons");
            }
        }

        public CharacterMainViewModel(ICharacterDataProvider characterDataProvider)
        {
            _enableButtons = false;
            _characterIndex = -1;
            _characterDataProvider = characterDataProvider;
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
    }
}
