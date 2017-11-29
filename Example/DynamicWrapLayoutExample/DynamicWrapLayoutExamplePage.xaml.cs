using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DynamicWrapLayoutExample
{
    public partial class DynamicWrapLayoutExamplePage : ContentPage
    {
        private ObservableCollection<string> _items;

        public ObservableCollection<string> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
        public DynamicWrapLayoutExamplePage()
        {
            InitializeComponent();
            BindingContext = this;
            Items = new ObservableCollection<string>();

            for (var i = 0; i < 40; i++)
                Items.Add(i.ToString());
        }
    }
}
