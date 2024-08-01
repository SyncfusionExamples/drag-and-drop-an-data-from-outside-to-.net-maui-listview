using Syncfusion.Maui.ListView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragDropSample
{
    public class GridDropBehavior:Behavior<Grid>
    {
        public ListViewExt ListView { get; set; }
        private ViewModel viewModel;
        private DropGestureRecognizer dropGestureRecognizer;
        private int dropIndex;
        private double prevPosition;
        private double nextPosition;
        protected override void OnAttachedTo(Grid bindable)
        {
            base.OnAttachedTo(bindable);
            
            dropGestureRecognizer = new DropGestureRecognizer();
            dropGestureRecognizer.DragOver += OnDragOver;
            dropGestureRecognizer.Drop += OnDrop;
            bindable.GestureRecognizers.Add(dropGestureRecognizer);

            this.ListView.Loaded += OnListViewLoaded;
            
        }

        private void OnListViewLoaded(object? sender, ListViewLoadedEventArgs e)
        {
            viewModel = this.ListView.BindingContext as ViewModel;
        }

        private void OnDrop(object? sender, DropEventArgs e)
        {
            this.ListView.FindEndItemIndex(prevPosition, nextPosition, out dropIndex);
            if (this.viewModel.DraggedItem != null)
            {
                var item = this.viewModel.DraggedItem;
                this.viewModel!.DragContactsInfo!.Remove(item);

                this.viewModel!.ContactsInfo!.Insert(dropIndex, item);
            }
        }

        private void OnDragOver(object? sender, DragEventArgs e)
        {
            var dragViewUI = (sender as DropGestureRecognizer)!.Parent as View;
            Point? currentDragPosition = e.GetPosition(this.ListView);
            this.ListView.GetPositionFrombounds(currentDragPosition, dragViewUI!.Bounds, out prevPosition, out nextPosition);
            this.ListView.PerformAutoScroll(prevPosition, nextPosition);
        }

        protected override void OnDetachingFrom(Grid bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.GestureRecognizers.Remove(dropGestureRecognizer);
            dropGestureRecognizer.DragOver -= OnDragOver;
            dropGestureRecognizer.Drop -= OnDrop;
            dropGestureRecognizer = null;
            this.ListView.Loaded -= OnListViewLoaded;
            this.ListView = null;
            this.viewModel = null;
        }
    }

    public class GridDragBehavior : Behavior<Grid>
    {
        public ListViewExt ListView { get; set; }
        private ViewModel viewModel;
        DragGestureRecognizer dragGestureRecognizer;
        protected override void OnAttachedTo(Grid bindable)
        {
            base.OnAttachedTo(bindable);

            dragGestureRecognizer = new DragGestureRecognizer();
            dragGestureRecognizer.DragStarting += OnDragStarting;
            bindable.GestureRecognizers.Add(dragGestureRecognizer);
            this.ListView.Loaded += OnListViewLoaded;          
        }
        private void OnListViewLoaded(object? sender, ListViewLoadedEventArgs e)
        {
            viewModel = this.ListView.BindingContext as ViewModel;
        }

        private void OnDragStarting(object? sender, DragStartingEventArgs e)
        {
            this.viewModel.DraggedItem = (sender as DragGestureRecognizer)!.BindingContext as Model;
        }

        protected override void OnDetachingFrom(Grid bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.GestureRecognizers.Remove(dragGestureRecognizer);
            dragGestureRecognizer.DragStarting -= OnDragStarting;
            dragGestureRecognizer = null;
            this.ListView.Loaded -= OnListViewLoaded;
            this.ListView = null;
            this.viewModel = null;
        }
    }
}
