using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace BMB.ViewModels
{
    public partial class PickerViewModel : BaseViewModel
    {
        [ObservableProperty]
        private Country? _country;

        [ObservableProperty]
        private ObservableCollection<Country> _selectedCountries;

        public ObservableCollection<Country> Countries { get; set; } = new();

        public static string CountryDisplayProperty => nameof(Country.Name);

        public PickerViewModel()
        {
            Countries.Add(new Country { Name = "USA" });
            Countries.Add(new Country { Name = "Canada" });
            Countries.Add(new Country { Name = "Germany" });
            Countries.Add(new Country { Name = "Japan" });

            SelectedCountries = new ObservableCollection<Country>();
        }

   

        [RelayCommand]
        private Task Reset()
        {
            Country = null;
            SelectedCountries.Clear();

            return Task.CompletedTask;
        }

        [RelayCommand]
        private async Task ShowSelectedItems()
        {
            var message = SelectedCountries.Count == 0
                ? "No countries selected."
                : string.Join(", ", SelectedCountries.Select(c => c.Name));


            await Shell.Current.DisplayAlert("Saved", $"Selected:  {message}", "OK");
        }

        [RelayCommand]
        private Task CountriesChanged()
        {
            return Task.CompletedTask;
        }
    }


}
