﻿using CookBookData.Model;
using CookBookData.Model.DbActions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.ViewModel
{
    class IngredientsViewModel
    {
        private readonly DbActions dbActions;

        public ObservableCollection<Ingredient> ingredientItems { get; set; }

        private Ingredient _selectedIngredient;

        public Ingredient selectedIngredient
        {
            get
            {
                return _selectedIngredient;
            }
            set
            {
                _selectedIngredient = value;
                OnPropertyChanged();
            }
        }


        public IngredientsViewModel()
        {
            dbActions = new DbActions();

            var allIngredients = this.dbActions.BrowseIngredients();

            ingredientItems = new ObservableCollection<Ingredient>(
                allIngredients.Select(obj =>
                {
                    var ingredient = (CookBookData.Model.Ingredient)obj;
                    return new Ingredient { Id = ingredient.Id, name = ingredient.name };
                })
                );
            //Console.WriteLine(ingredientItems[0].name);

        }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion

    }

}
