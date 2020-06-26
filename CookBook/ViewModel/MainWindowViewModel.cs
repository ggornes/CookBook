using CookBook.Serialiser;
using CookBook.ViewModel.Interfaces;
using CookBookData.Model;
using CookBookData.Model.DbActions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookBook.ViewModel
{
    public class MainWindowViewModel : IViewModel, INotifyPropertyChanged
    {
        private IViewModel _currentViewModel;
        public IViewModel CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }
        private List<IViewModel> _viewModels;
        public List<IViewModel> ViewModels
        {
            get
            {
                if (_viewModels == null)
                {
                    _viewModels = new List<IViewModel>();
                }
                return _viewModels;
            }
        }

        private void ChangeViewModel(IViewModel viewModel)
        {
            if (!ViewModels.Contains(viewModel))
            {
                ViewModels.Add(viewModel);
            }
            CurrentViewModel = ViewModels.FirstOrDefault(vm => vm == viewModel);
        }

        public ICommand RecipesCommand { get; set; }
        public ICommand IngredientsCommand { get; set; }
        public ICommand MeasuresCommand { get; set; }
        public ICommand ExportCommand { get; set; }
        public ICommand ImportCommand { get; set; }
        private DbActions dbActions {get; set;}

        //public IAsyncCommand RecipesCommand { get; set; }


        public MainWindowViewModel(DbActions dbActions)
        {
            this.dbActions = dbActions;

            MeasuresCommand = new RelayCommand(OpenMeasures);
            IngredientsCommand = new RelayCommand(OpenIngredients);
            RecipesCommand = new RelayCommand(OpenRecipes);

            ExportCommand = new RelayCommand(Export);
            ImportCommand = new RelayCommand(Import);

            ViewModels.Add(new IngredientsViewModel());
            ViewModels.Add(new RecipeViewModel());
            // add all



            CurrentViewModel = ViewModels[1]; // select one default (the first one)

            // Mediator.Suscribe();

        }

        public void Export(object obj)
        {
            var recipes = this.dbActions.BrowseRecipes();
            List<CookBookData.Model.Recipe> recipesList = new List<Recipe>
                (
                    recipes.Select(data =>
                    {
                        var recipe = (Recipe)data;
                        return new Recipe
                        {
                            Id = recipe.Id,
                            name = recipe.name,
                            prepTime = recipe.prepTime
                        };
                    })
                );

            var ingredients = this.dbActions.BrowseIngredients();
            var ingredientsList = new List<Ingredient>
                (
                    ingredients.Select(data =>
                    {
                        var ingredient = (Ingredient)data;
                        return new Ingredient
                        {
                            Id = ingredient.Id,
                            name = ingredient.name
                        };
                    })
                );

            var measures = this.dbActions.BrowseMeasures();
            var measureList = new List<Measure>
                (
                    measures.Select(data =>
                    {
                        var measure = (Measure)data;
                        return new Measure
                        {
                            Id = measure.Id,
                            name = measure.name
                        };
                    })
                );

            var recipeIngredients = this.dbActions.BrowseRecipeIngredients();
            var riList = new List<RecipeIngredient>
                (
                    recipeIngredients.Select(data =>
                    {
                        var ri = (RecipeIngredient)data;
                        return new RecipeIngredient
                        {
                            Id = ri.Id,
                            recipeId = ri.recipeId,
                            ingredientId = ri.ingredientId,
                            measureId = ri.measureId,
                            amount = ri.amount,
                        };
                    })
                );

            var recipeSteps = this.dbActions.BrowseRecipeSteps();
            var rsList = new List<RecipeStep>
                (
                    recipeSteps.Select(data =>
                    {
                        var rs = (RecipeStep)data;
                        return new RecipeStep
                        {
                            Id = rs.Id,
                            recipeId = rs.recipeId,
                            stepNumber = rs.stepNumber,
                            stepInstructions = rs.stepInstructions
                        };
                    })
                );

            var exportDialog = new SaveFileDialog
            {
                Title = "Export as binary file",
                FileName = "CookBookData",
                Filter = "Binary files (*.bin)|*.bin",
                DefaultExt = ".bin"
            };

            if (exportDialog.ShowDialog() == true)
            {
                CookBook.Serialiser.Serialiser s = new Serialiser.Serialiser(exportDialog.FileName, recipesList, ingredientsList, measureList, riList, rsList);

                if (s.Serialise())
                {
                    MessageBox.Show($"Successfully saved data to {exportDialog.FileName}", "Saved data", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                }
                else
                {
                    MessageBox.Show($"Could not serialise data to {exportDialog.FileName}. Ensure that the data is not corrupted and the program has sufficient permissions for this operation.", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            }
        }

        public void Import(object obj)
        {
            Console.WriteLine("Import");
            var importDialog = new SaveFileDialog
            {
                Title = "Import binary file",
                FileName = "CookBookData",
                Filter = "Binary files (*.bin)|*.bin",
                DefaultExt = ".bin"
            };

            if (importDialog.ShowDialog() == true)
            {
                Deserialiser d = new Deserialiser(importDialog.FileName);

                List<Recipe> dRecipes;
                List<Ingredient> dIngredients;
                List<Measure> dMeasures;
                List<RecipeIngredient> dRecipeIngredients;
                List<RecipeStep> dRecipeSteps;

                if (d.Deserialise())
                {
                    dRecipes = d.DeserialisedRecipes;
                    dIngredients = d.DeserialisedIngredients;
                    dMeasures = d.DeserialisedMeasures;
                    dRecipeIngredients = d.DeserialisedRecipeIngredients;
                    dRecipeSteps = d.DeserialisedRecipeSteps;

                    // ToDo: clear DB

                    foreach (var measure in dMeasures)
                    {
                        this.dbActions.AddMeasure(new Measure { name = measure.name });
                    }

                    foreach (var ingredient in dIngredients)
                    {
                        this.dbActions.AddIngredient(new Ingredient { name = ingredient.name });
                    }

                    foreach (var recipe in dRecipes)
                    {
                        this.dbActions.AddRecipe(new Recipe { name = recipe.name, prepTime = recipe.prepTime });
                    }

                    // ToDo: recipeSteps and RecipeIngredients

                    // ToDo: update views

                    MessageBox.Show("Data imported successfully", "Data imported", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else
                {
                    MessageBox.Show("Could not deserialise data", "Deserialisation error", MessageBoxButton.OK, MessageBoxImage.Error);
                }



            }
        }

        public void OpenMeasures(object obj)
        {
            CurrentViewModel = new MeasuresViewModel();
        }

        public void OpenIngredients(object obj)
        {
            CurrentViewModel = new IngredientsViewModel();
        }

        public void OpenRecipes(object obj)
        {
            CurrentViewModel = new RecipeViewModel();
        }







        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        //public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
