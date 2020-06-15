﻿using CookBook.View;
using CookBook.ViewModel.Interfaces;
using CookBookData;
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
    public class RecipeViewModel: IViewModel, INotifyPropertyChanged
    {
        private DbActions dbActions;

        // Recipe - Ingredient List View
        // The Source is binded to the selectedRecipe
        private ObservableCollection<RecipeIngredientItem> _ingredientItems { get; set; }
        public ObservableCollection<RecipeIngredientItem> ingredientItems
        { 
            get { return _ingredientItems; }
            set { if(_ingredientItems != value)
                    {
                    _ingredientItems = value;
                        OnPropertyChanged();
                    
                    // Make Edit Recipe-Ingredients controls visible if an element of the ListView is selected
                    if(ingredientItems.Count() == 0)
                    {
                        ShowControlsRecipeIngredientSelected = false;
                    }
                }
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

        // Recipe - Step Listview
        private ObservableCollection<RecipeStep> _steps;
        public ObservableCollection<RecipeStep> steps
        {
            get { return _steps; }
            set { if(_steps != value)
                    {
                        _steps = value;
                        OnPropertyChanged();
                    Console.WriteLine("steps {0}", steps.Count());
                    if (steps.Count() == 0)
                    {
                        ShowControlsRecipeStepSelected = false;
                    }
                    }
                }
        }

        private bool _showControls;

        public bool ShowControls
        {
            get { return _showControls; }
            set
            {
                if(_showControls != value)
                {
                    _showControls = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _showControlsRecipeStepSelected;
        public bool ShowControlsRecipeStepSelected
        {
            get { return _showControlsRecipeStepSelected; }
            set
            {
                if ( _showControlsRecipeStepSelected != value)
                {
                    _showControlsRecipeStepSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _showControlsRecipeIngredientSelected;
        public bool ShowControlsRecipeIngredientSelected
        {
            get { return _showControlsRecipeIngredientSelected; }
            set
            {
                if (_showControlsRecipeIngredientSelected != value)
                {
                    _showControlsRecipeIngredientSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        // Recipe List View
        public ObservableCollection<Recipe> recipeItems { get; set; }

        public ObservableCollection<RecipeIngredientItem> recipeIngredientItems { get; set; }

        private Recipe _selectedRecipe;

        public Recipe selectedRecipe
        {
            get { return _selectedRecipe; }
            set
            {
                _selectedRecipe = value;
                OnPropertyChanged();

                
                if (selectedRecipe != null)
                {
                    ShowControls = true;

                    // read steps and Ingredients when recipe is selected
                    var allRecipeSteps = this.dbActions.BrowseRecipeSteps(selectedRecipe.Id);
                    steps = new ObservableCollection<RecipeStep>(
                        allRecipeSteps.Select(obj2 =>
                        {
                            var recipeStep = (CookBookData.Model.RecipeStep)obj2;
                            return new RecipeStep { Id = recipeStep.Id, stepNumber = recipeStep.stepNumber, stepInstructions = recipeStep.stepInstructions };
                        })
                        );

                    // read recipeIngredients table when recipe is selected
                    var allRecipeIngredients = dbActions.BrowseRecipeIngredients(selectedRecipe.Id);

                    ingredientItems = new ObservableCollection<RecipeIngredientItem>(
                        allRecipeIngredients.Select(obj =>
                        {
                            var ing = (CookBookData.RecipeIngredientItem)obj;
                            return new RecipeIngredientItem { ingredientName = ing.ingredientName, amount = ing.amount, measure = ing.measure };
                        })
                        );

                    // Read all Ingredients
                    var allIngredients = dbActions.BrowseIngredients();

                    allIngredientItems = new ObservableCollection<Ingredient>(
                        allIngredients.Select(obj3 =>
                        {
                            var ingredient = (CookBookData.Model.Ingredient)obj3;
                            return new Ingredient { Id = ingredient.Id, name = ingredient.name };
                        })
                        );

                    // Read All Measures
                    var allMeasures = dbActions.BrowseMeasures();
                    allMeasureItems = new ObservableCollection<Measure>(
                        allMeasures.Select(obj4 =>
                        {
                            var measure = (CookBookData.Model.Measure)obj4;
                            return new Measure { Id = measure.Id, name = measure.name };
                        })
                        );
                }
            }
        }

        private RecipeStep _selectedRecipeStep;
        public RecipeStep selectedRecipeStep
        {
            get { return _selectedRecipeStep; }
            set
            {
                _selectedRecipeStep = value;
                OnPropertyChanged();

                // To make controls visible, one must select first a recipe step from
                // the ListView
                if (selectedRecipe != null)
                {
                    ShowControlsRecipeStepSelected = true;
                }
            }
        }

        // Selected from Recipe-Ingredient ListView (ObservableCollection<RecipeIngredient>)
        private RecipeIngredientItem _selectedRecipeIngredient;
        public RecipeIngredientItem selectedRecipeIngredient
        {
            get { return _selectedRecipeIngredient; }
            set
            {
                _selectedRecipeIngredient = value;
                OnPropertyChanged();
                if (selectedRecipe != null)
                {
                    Console.WriteLine(selectedRecipeIngredient.ingredientName);
                    ShowControlsRecipeIngredientSelected = true;

                    var readRecipeIngredient = this.dbActions.ReadIngredient(new CookBookData.Model.Ingredient { name = selectedRecipeIngredient.ingredientName });

                    trueSelectedRecipeIngredient = (Ingredient)readRecipeIngredient;

                    var readRecipeMeasure = this.dbActions.ReadMeasure(new CookBookData.Model.Measure { name = selectedRecipeIngredient.measure });

                    selectedRecipeMeasure = (Measure)readRecipeMeasure;

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





        public RecipeViewModel()
        {
            // Load Data Base methods for doing BREAD/CRUD operations
            dbActions = new DbActions();

            #region Button Action Binding
            EditRecipeCommand = new RelayCommand(EditRecipe);
            EditRecipeStepCommand = new RelayCommand(EditRecipeStep);
            //EditRecipeIngredientCommand = new RelayCommand(EditIRecipeIngredient);

            AddRecipeCommand = new RelayCommand(OpenAddRecipeWindow);
            AddRecipeStepCommand = new RelayCommand(OpenAddRecipeStepWindow);
            //AddRecipeIngredientCommand = new RelayCommand(OpenAddIngredientWindow);


            DeleteRecipeStepCommand = new RelayCommand(DeleteRecipeStep);
            // DeleteRecipeIngredientCommand = new RelayCommand(DeleteRecipeIngredient);
            // DeleteRecipeCommand = new RelayCommand(DeleteRecipeCommand);
            #endregion


            // Hides AddIngredient and AddStep buttons until user clicks on a recipe from the list
            _showControls = false;
            // Make Edit Controls only visible when selecting a recipeStep item from the ListView
            _showControlsRecipeStepSelected = false;


            // read form database and store all recipes in a variable
            var allRecipes = this.dbActions.BrowseRecipes();

            // update Recipe ListView
            recipeItems = new ObservableCollection<Recipe>(
                allRecipes.Select(obj =>
                {
                    var recipe = (CookBookData.Model.Recipe)obj;
                    return new Recipe { Id = recipe.Id, name = recipe.name, prepTime = recipe.prepTime };
                })
                );
        }




        #region Add 
        // Open a new Window view to Add Recipes, Recipe-Ingredients or Recipe-Steps

        public ICommand AddRecipeCommand { get; set; }
        private void OpenAddRecipeWindow(object obj)
        {
            var AddRecipeVM = new AddRecipeViewModel(dbActions, recipeItems);
            var addRecipeView = new AddRecipeView(AddRecipeVM);

            addRecipeView.Show();
        }


        public ICommand AddRecipeStepCommand { get; set; }

        private void OpenAddRecipeStepWindow(object obj)
        {
            var AddRecipeStepVM = new AddRecipeStepViewModel(dbActions, steps, selectedRecipe.Id);
            var AddRecipeStepV = new AddRecipeStepView(AddRecipeStepVM);

            AddRecipeStepV.Show();
        }


        #endregion

        #region Delete
        public ICommand DeleteRecipeStepCommand { get; set; }

        private void DeleteRecipeStep(object obj)
        {
            if (selectedRecipeStep != null)
            {
                if (MessageBox.Show("Do you want to delete the selected recipe Step?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    if (dbActions.DeleteRecipeStep(new CookBookData.Model.RecipeStep { Id = selectedRecipeStep.Id }))
                    {
                        steps.Remove(selectedRecipeStep);
                    }
                }
            }
        }

        #endregion

        #region Edit

        public ICommand EditRecipeCommand { get; set; }

        void EditRecipe(object obj)
        {
            if (selectedRecipe != null && !string.IsNullOrEmpty(selectedRecipe.name) && !string.IsNullOrWhiteSpace(selectedRecipe.name))
            {
                if (dbActions.EditRecipe(new CookBookData.Model.Recipe { Id = selectedRecipe.Id, name = selectedRecipe.name, prepTime = selectedRecipe.prepTime }))
                {
                    MessageBox.Show("Recipe updated", "Recipe updated", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                }
            }

        }

        public ICommand EditRecipeStepCommand { get; set; }

        void EditRecipeStep(object obj)
        {
            if (selectedRecipeStep != null && !string.IsNullOrEmpty(selectedRecipeStep.stepInstructions) && !string.IsNullOrWhiteSpace(selectedRecipeStep.stepInstructions))
            {
                //var test = new CookBookData.Model.RecipeStep { Id = selectedRecipeStep.Id, recipeId = selectedRecipe.Id, stepNumber = selectedRecipeStep.stepNumber, stepInstructions = selectedRecipeStep.stepInstructions };

                if (dbActions.EditRecipeStep(new CookBookData.Model.RecipeStep { Id = selectedRecipeStep.Id, recipeId = selectedRecipe.Id, stepNumber = selectedRecipeStep.stepNumber, stepInstructions = selectedRecipeStep.stepInstructions }))
                {
                    MessageBox.Show("Recipe Step updated", "Recipe Step updated", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                }
            }
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
