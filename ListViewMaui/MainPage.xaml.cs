using System.Collections.ObjectModel;

namespace DragDropSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.listView.ItemGenerator = new ItemGeneratorExt(this.listView);
        }
    }
}
