using CookBook.ViewModel.Interfaces;
using CookBookData.Model;
using CookBookData.Model.DbActions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookBook.ViewModel
{
    class IngredientsViewModel
    {
        private DbActions dbActions;

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

            DeleteIngredientCommand = new RelayCommand(DeleteIngredient);
            EditIngredientCommand = new RelayCommand(EditIngredient);

        }

        private void DeleteIngredient(Object ingredient)
        {
            dbActions = new DbActions();

            if (selectedIngredient != null)
            {
                if (MessageBox.Show("Do you want to delete the selected ingredient?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    if (dbActions.DeleteIngredient(new CookBookData.Model.Ingredient { Id = selectedIngredient.Id }))
                    {
                        ingredientItems.Remove(selectedIngredient);
                    }
                }
            }
        }

        private void EditIngredient(object ingredient)
        {
            dbActions = new DbActions();

            if(selectedIngredient != null && !string.IsNullOrEmpty(selectedIngredient.name) && !string.IsNullOrWhiteSpace(selectedIngredient.name))
            {
                if(dbActions.EditIngredient(new CookBookData.Model.Ingredient { Id = selectedIngredient.Id, name = selectedIngredient.name}))
                {
                    MessageBox.Show("Ingredient updated", "Ingredient updated", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                }
            }
        }


        public ICommand DeleteIngredientCommand { get; set; }
        public ICommand EditIngredientCommand { get; set; }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion

    }

}
