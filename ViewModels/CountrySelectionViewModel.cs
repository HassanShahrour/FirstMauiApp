using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BMB.ViewModels
{
    public partial class CountrySelectionViewModel
    {
        public ObservableCollection<Country> Countries { get; set; } = new();
        public ICommand SaveCommand { get; }

        public CountrySelectionViewModel()
        {
            Countries.Add(new Country { Name = "USA" });
            Countries.Add(new Country { Name = "Canada" });
            Countries.Add(new Country { Name = "Germany" });
            Countries.Add(new Country { Name = "Japan" });

            SaveCommand = new Command(SaveSelection);
        }

        private void SaveSelection()
        {
            var selectedCountries = Countries.Where(c => c.IsSelected).Select(c => c.Name).ToList();

            Shell.Current.DisplayAlert("Saved", $"Selected: {string.Join(", ", selectedCountries)}", "OK");
        }
    }

    public class Country
    {
        public string? Name { get; set; }
        public bool IsSelected { get; set; }
    }

}
