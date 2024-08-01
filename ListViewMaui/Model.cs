using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragDropSample
{
    public class Model : INotifyPropertyChanged
    {
        private string? contactName;
        private ImageSource image;
        private bool isAnimated;
        public Model()
        {
        }

        public string? ContactName
        {
            get { return this.contactName; }
            set
            {
                this.contactName = value;
                RaisePropertyChanged("ContactName");
            }
        }

        public ImageSource ContactImage
        {
            get { return this.image; }
            set
            {
                if (value != null)
                {
                    this.image = value;
                    this.RaisePropertyChanged("ContactImage");
                }
            }
        }
        public bool IsAnimated
        {
            get { return isAnimated; }
            set
            {
                this.isAnimated = value;
                this.RaisePropertyChanged("IsAnimated");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void RaisePropertyChanged(String name)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

    }
}
