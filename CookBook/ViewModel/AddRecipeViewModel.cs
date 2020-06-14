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
    public class AddRecipeViewModel: INotifyPropertyChanged
    {
        public ICommand AddRecipeToDBCommand { get; set; }
        private ObservableCollection<Recipe> _recipeItems;
        private DbActions dbActions;
        
        private string _name;
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private int _prepTime;
        public int prepTime
        {
            get { return _prepTime; }
            set
            {
                _prepTime = value;
                OnPropertyChanged();
            }
        }

        public AddRecipeViewModel(DbActions dbActions, ObservableCollection<Recipe> recipeItems)
        {
            this.dbActions = dbActions;
            _recipeItems = recipeItems;

            AddRecipeToDBCommand = new RelayCommand(AddRecipe);
        }


        public void AddRecipe(object obj)
            // ToDo Validate prep time input
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                if (MessageBox.Show("Enter recipe name name", "Invalid recipe name", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK) == MessageBoxResult.OK)
                {
                    name = "";
                }
            }
            else
            {
                if (this.dbActions.AddRecipe(new CookBookData.Model.Recipe { name = name, prepTime = prepTime }))
                {
                    Console.WriteLine("Recipe added");

                    if (MessageBox.Show("Recipe added", "Recipe added", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK) == MessageBoxResult.OK)
                    {


                        // We added a new Recipe, but we dont know the id so
                        // to update the view, we must read the recently created 
                        // Recipe and add it to the listview source

                        var readRecipe = this.dbActions.ReadRecipe(new CookBookData.Model.Recipe { name = name });

                        // update collection in the view model
                        var recipeItem = new CookBookData.Model.Recipe
                        {
                            Id = ((CookBookData.Model.Recipe)readRecipe).Id,
                            name = ((CookBookData.Model.Recipe)readRecipe).name,
                            prepTime = ((CookBookData.Model.Recipe)readRecipe).prepTime
                        };

                        this._recipeItems.Add(recipeItem);
                        name = "";
                    }
                }
            }

        }









        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
