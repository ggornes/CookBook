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
    public class AddRecipeIngredientViewModel: INotifyPropertyChanged
    {
        public ICommand AddRecipeIngredientToDBCommand { get; set; }
        private DbActions dbActions;
        private ObservableCollection<CookBookData.RecipeIngredientItem> _recipeIngredientItems;
        private int _recipeId;
        public int recipeId
        {
            get { return _recipeId; }
            set
            {
                _recipeId = value;
                OnPropertyChanged();
            }
        }

        private string _amount;
        public string amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                OnPropertyChanged();
            }
        }

        // Ingredients Combo Box
        private ObservableCollection<Ingredient> _allIngredientItems { get; set; }
        public ObservableCollection<Ingredient> allIngredientItems
        {
            get { return _allIngredientItems; }
            set
            {
                if (_allIngredientItems != value)
                {
                    _allIngredientItems = value;
                    OnPropertyChanged();
                }
            }
        }

        private Ingredient _trueSelectedRecipeIngredient;
        public Ingredient trueSelectedRecipeIngredient
        {
            get { return _trueSelectedRecipeIngredient; }
            set
            {
                _trueSelectedRecipeIngredient = value;
                OnPropertyChanged();
            }
        }

        // Measures combo box
        private ObservableCollection<Measure> _allMeasureItems { get; set; }
        public ObservableCollection<Measure> allMeasureItems
        {
            get { return _allMeasureItems; }
            set
            {
                if (_allMeasureItems != value)
                {
                    _allMeasureItems = value;
                    OnPropertyChanged();
                }
            }
        }

        private Measure _selectedRecipeMeasure;
        public Measure selectedRecipeMeasure
        {

            get { return _selectedRecipeMeasure; }
            set
            {
                _selectedRecipeMeasure = value;
                OnPropertyChanged();
            }
        }


        public AddRecipeIngredientViewModel(DbActions dbActions, ObservableCollection<CookBookData.Model.Ingredient> ingredientItems, ObservableCollection<CookBookData.Model.Measure> measureItems, ObservableCollection<CookBookData.RecipeIngredientItem> recipeIngredientItems, int recipeId)
        {
            this.dbActions = dbActions;
            _allIngredientItems = ingredientItems;
            _allMeasureItems = measureItems;
            _recipeId = recipeId;

            _recipeIngredientItems = recipeIngredientItems;

            AddRecipeIngredientToDBCommand = new RelayCommand(AddRecipeIngredient);
        }

        public void AddRecipeIngredient(object obj)
        {
            //Console.WriteLine("Selected Recipe ID: {0}", recipeId);
            //Console.WriteLine("Selected Ingredient ID: {0} | Name: {1}", trueSelectedRecipeIngredient.Id, trueSelectedRecipeIngredient.name);
            //Console.WriteLine("Selected Measure ID: {0} | Name: {1}", selectedRecipeMeasure.Id, selectedRecipeMeasure.name);
            //Console.WriteLine("Amount: {0}", amount);

            int intAmount;

            // basic validations
            if (string.IsNullOrEmpty(amount) || string.IsNullOrWhiteSpace(amount) || !Int32.TryParse(amount, out intAmount))
            {
                if (MessageBox.Show("Enter a valid recipe ingredient amount", "Invalid recipe ingredient amount", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK) == MessageBoxResult.OK)
                {
                    amount = "";
                }

            }
            else
            {
                if (selectedRecipeMeasure == null)
                {
                    if (this.dbActions.AddRecipeIngredient(new CookBookData.Model.RecipeIngredient { recipeId = recipeId, ingredientId = trueSelectedRecipeIngredient.Id, measureId = null, amount = Int32.Parse(amount) }))
                    {
                        Console.WriteLine("Recipe Ingredient added");
                        if (MessageBox.Show("Recipe Ingredient added", "Recipe Ingredient added", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK) == MessageBoxResult.OK)
                        {
                            // update the recipe-ingredient list view
                            //var readRecipeIngredient = this.dbActions.ReadRecipeIngredient(new CookBookData.Model.RecipeIngredient { recipeId = recipeId, ingredientId = trueSelectedRecipeIngredient.Id });
                            var readRecipeIngredient = this.dbActions.ReadIngredient(new CookBookData.Model.Ingredient { name = trueSelectedRecipeIngredient.name });
                            //var readRecipeMeasure = this.dbActions.ReadMeasure(new CookBookData.Model.Measure { Id = selectedRecipeMeasure.Id });

                            //var readIngredientName = this.dbActions.ReadIngredient()

                            var theId = ((CookBookData.Model.Ingredient)readRecipeIngredient).Id;
                            var theIngName = ((CookBookData.Model.Ingredient)readRecipeIngredient).name;
                            //var theMeasure = ((CookBookData.Model.Measure)readRecipeMeasure).name;
                            var theAmnount = Int32.Parse(amount);

                            // update list
                            var recipeIngredientItem = new CookBookData.RecipeIngredientItem
                            {
                                Id = ((CookBookData.Model.Ingredient)readRecipeIngredient).Id,
                                ingredientName = ((CookBookData.Model.Ingredient)readRecipeIngredient).name,
                                //measure = ((CookBookData.Model.Measure)readRecipeMeasure).name,
                                amount = Int32.Parse(amount)
                            };

                            this._recipeIngredientItems.Add(recipeIngredientItem);
                        }
                    }
                }
                else if (this.dbActions.AddRecipeIngredient(new CookBookData.Model.RecipeIngredient { recipeId = recipeId, ingredientId = trueSelectedRecipeIngredient.Id, measureId = selectedRecipeMeasure.Id, amount = Int32.Parse(amount) }))
                {
                    Console.WriteLine("Recipe Ingredient added");
                    if (MessageBox.Show("Recipe Ingredient added", "Recipe Ingredient added", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK) == MessageBoxResult.OK)
                    {
                        // update the recipe-ingredient list view
                        //var readRecipeIngredient = this.dbActions.ReadRecipeIngredient(new CookBookData.Model.RecipeIngredient { recipeId = recipeId, ingredientId = trueSelectedRecipeIngredient.Id });
                        var readRecipeIngredient = this.dbActions.ReadIngredient(new CookBookData.Model.Ingredient { name = trueSelectedRecipeIngredient.name });
                        var readRecipeMeasure = this.dbActions.ReadMeasure(new CookBookData.Model.Measure { Id = selectedRecipeMeasure.Id });

                        //var readIngredientName = this.dbActions.ReadIngredient()

                        var theId = ((CookBookData.Model.Ingredient)readRecipeIngredient).Id;
                        var theIngName = ((CookBookData.Model.Ingredient)readRecipeIngredient).name;
                        var theMeasure = ((CookBookData.Model.Measure)readRecipeMeasure).name;
                        var theAmnount = Int32.Parse(amount);

                        // update list
                        var recipeIngredientItem = new CookBookData.RecipeIngredientItem
                        {
                            Id = ((CookBookData.Model.Ingredient)readRecipeIngredient).Id,
                            ingredientName = ((CookBookData.Model.Ingredient)readRecipeIngredient).name,
                            measure = ((CookBookData.Model.Measure)readRecipeMeasure).name,
                            amount = Int32.Parse(amount)
                        };

                        this._recipeIngredientItems.Add(recipeIngredientItem);
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