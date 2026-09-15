# How to perform drag and drop an data from outside into .NET MAUI ListView (SfListView)?
This demo explains about how to perform dragging and dropping the data from outside into .NET MAUI ListView (SfListView).

## Sample

```xaml
<Grid RowDefinitions="60,1,*">
<StackLayout
    x:Name="stackView"
    BindableLayout.ItemsSource="{Binding DragContactsInfo}"
    Orientation="Horizontal">
    <BindableLayout.ItemTemplate>
        <DataTemplate>
            <Grid ColumnDefinitions="56">
                <Grid.Behaviors>
                    <local:GridDragBehavior ListView="{x:Reference listView}" />
                </Grid.Behaviors>
                <Image
                    Grid.Column="0"
                    HeightRequest="40"
                    HorizontalOptions="Center"
                    Source="{Binding ContactImage}"
                    VerticalOptions="Center"
                    WidthRequest="40" />
            </Grid>
        </DataTemplate>
    </BindableLayout.ItemTemplate>
</StackLayout>
<BoxView
    Grid.Row="1"
    BackgroundColor="LightGray"
    HeightRequest="1" />
<local:ListViewExt
    x:Name="listView"
    Grid.Row="2"
    DragStartMode="OnHold"
    ItemSize="56"
    ItemsSource="{Binding ContactsInfo}">
    <local:ListViewExt.ItemTemplate>
        <DataTemplate>
            ...
        </DataTemplate>
    </local:ListViewExt.ItemTemplate>
</local:ListViewExt>
```

```c#
public class ListViewExt : SfListView
{
    private const int scrollDelay = 100;

    private double ScrollVelocity = 50;

    private double scrollMargin = 15;

    private bool? isMovingUpOrLeft = null;

    internal ListViewScrollView scrollView;
    internal VisualContainer visualContainer;
    internal double TotalExtent;
    internal bool IsDown;
    public ListViewExt()
    {
        this.scrollView = this.GetScrollView();
        this.Loaded += ListViewExt_Loaded;
    }

    private void ListViewExt_Loaded(object? sender, ListViewLoadedEventArgs e)
    {
        visualContainer = this.GetVisualContainer();
        var extent = (double)visualContainer.GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name == "TotalExtent").GetValue(visualContainer);
        TotalExtent = extent;
    }

    public bool IsAutoScrolling
    {
        get; set;
    }

    public async void StartScrolling(double scrollVelocity)
    {
        this.ScrollVelocity = scrollVelocity;

        if (!this.IsAutoScrolling)
        {
            this.IsAutoScrolling = true;
            await this.AutoScrollAsync();
        }
    }

    public void StopScrolling()
    {
        this.ScrollVelocity = 0;
        this.IsAutoScrolling = false;
    }
    private async Task AutoScrollAsync()
    {
        while (
            !(
            this.ScrollVelocity == 0
            || this.ScrollVelocity > 0 && this.IsScrolledToBottom()
            || this.ScrollVelocity < 0 && this.IsScrolledToTop()
            )
        )
        {
            await this.scrollView.ScrollToAsync(0, this.CalculateNextScrollY(), false);
            await Task.Delay(scrollDelay);
        }

        this.StopScrolling();
    }

    private double CalculateNextScrollY()
    {
        return this.scrollView.ScrollY + this.ScrollVelocity;
    }

    private bool IsScrolledToBottom()
    {
        return this.scrollView.ScrollY
            >= this.scrollView.ContentSize.Height - this.scrollView.Bounds.Height;
    }

    private bool IsScrolledToTop()
    {
        return this.scrollView.ScrollY <= 0;
    }
    internal bool HasStickyHeader()
    {
        return this.HeaderTemplate != null && this.IsStickyHeader && this.IsScrollingEnabled;
    }
    private bool HasStickyGroupHeader()
    {
        return this.IsStickyGroupHeader && this.DataSource!.Groups.Count > 0 && this.IsScrollingEnabled;
    }

    private bool HasScrollableHeader()
    {
        return this.HeaderTemplate != null && (!this.IsStickyHeader || !this.IsScrollingEnabled);
    }

    private bool HasScrollableFooter()
    {
        return this.FooterTemplate != null && (!IsStickyFooter || !IsScrollingEnabled);
    }

    private bool HasGroups()
    {
        return this.DataSource.Groups.Count > 0;
    }


    internal bool CanAutoScroll(double start, double end, ref bool isDown)
    {
        var scrollOffset = (double)visualContainer.GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name == "ScrollOffset").GetValue(visualContainer);
        var doubleSpan = this.visualContainer!.ScrollRows!.GetClipPoints(ScrollAxisRegion.Body, false);
        Point scrollBounds;
        if (doubleSpan.IsEmpty)
        {
            scrollBounds = Point.Zero;
        }
        else
        {
            scrollBounds = new Point(doubleSpan.Start, doubleSpan.End);
        }
        var endViewPosition = Math.Round(scrollBounds.Y + scrollOffset);
        var startViewPosition = Math.Round(scrollBounds.X + scrollOffset);

        if (end > (endViewPosition - this.scrollMargin) && endViewPosition < this.TotalExtent)
        {
            isDown = true;
            return true;
        }
        else if (startViewPosition > scrollBounds.X && start < (startViewPosition + this.scrollMargin))
        {
            isDown = false;
            return true;
        }
        return false;
    }

    internal void FindEndItemIndex(double prevPosition, double nextPosition, out int endItemIndex)
    {
        endItemIndex = 0;
        var offset = (double)visualContainer.GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name == "ScrollOffset").GetValue(visualContainer);
        if (prevPosition < 0)
        {
            endItemIndex = 0;
            return;
        }
        var scrollRows = visualContainer.ScrollRows;
        if (HasStickyGroupHeader() && visualContainer != null && visualContainer.ScrollRows != null)
        {
            var firstVisibleLine = visualContainer.ScrollRows.GetVisibleLineAtPoint(0);
            if (firstVisibleLine != null && firstVisibleLine.Size > prevPosition)
            {
                endItemIndex = 0;
                return;
            }
        }

        if (this.isMovingUpOrLeft == null || (bool)this.isMovingUpOrLeft)
        {
            var prevLine = this.visualContainer!.ScrollRows!.GetVisibleLineAtPoint(prevPosition, false, false);
            if (prevLine != null)
            {
                var count = 0;
                var methodInfo = (MethodInfo)this.ItemsLayout!.GetType().GetRuntimeMethods().FirstOrDefault(x => x.Name == "GetItemIndex");//.Invoke(this.ItemsLayout, new object[] { prevLine.LineIndex, count });
                var prevIndex = (int)methodInfo.Invoke(this.ItemsLayout, new object[] { prevLine.LineIndex, count });
                if (this.IsAutoScrolling)
                {
                    var headerCount = this.HasScrollableHeader() ? 1 : 0;
                    var firstRecordIndex = headerCount + (this.HasGroups() ? 1 : 0);
                    if (prevLine.LineIndex < firstRecordIndex)
                    {
                        endItemIndex = firstRecordIndex - headerCount;
                        this.isMovingUpOrLeft = true;
                        return;
                    }
                }
                if (prevIndex >= 0 && prevIndex < endItemIndex)
                {
                    if (this.HasGroups() && prevIndex == 0)
                    {
                        this.isMovingUpOrLeft = true;
                        endItemIndex = prevIndex + 1;
                        return;
                    }
                    else
                    {
                        var layoutItemPosition = (prevLine.Size / 2) + prevLine.Origin;
                        var position = (prevPosition - (prevLine.Size / 2)) <= layoutItemPosition;
                        if (this.IsAutoScrolling || position)
                        {
                            this.isMovingUpOrLeft = true;
                            endItemIndex = prevIndex;
                            return;
                        }
                    }
                }
            }
        }

        if (nextPosition > this.TotalExtent)
        {
            return;
        }

        if (this.isMovingUpOrLeft == null || !(bool)this.isMovingUpOrLeft)
        {
            var nextLine = this.visualContainer!.ScrollRows!.GetVisibleLineAtPoint(nextPosition, false, false);
            if (nextLine != null)
            {
                var count = 0;
                var methodInfo = (MethodInfo)this.ItemsLayout!.GetType().GetRuntimeMethods().FirstOrDefault(x => x.Name == "GetItemIndex");//.Invoke(this.ItemsLayout, new object[] { prevLine.LineIndex, count });
                var nextIndex = (int)methodInfo.Invoke(this.ItemsLayout, new object[] { nextLine.LineIndex, count });
                if (nextIndex >= 0 && nextIndex > endItemIndex)
                {
                    var footerCount = this.HasScrollableFooter() ? 1 : 0;
                    var loadMoreCount = this.LoadMoreOption != LoadMoreOption.None ? 1 : 0;
                    var rowCount = (int)visualContainer.GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name == "RowCount").GetValue(visualContainer);
                    var lastRecordIndex = (rowCount - (footerCount + loadMoreCount)) - 1;

                    if (nextLine.LineIndex > lastRecordIndex)
                    {
                        var headerCount = this.HasScrollableHeader() ? 1 : 0;
                        this.isMovingUpOrLeft = false;
                        endItemIndex = lastRecordIndex - headerCount;
                    }
                    else
                    {
                        var layoutItemPosition = (nextLine.Size / 2) + nextLine.Origin;
                        var position = (nextPosition + (nextLine.Size / 2)) >= layoutItemPosition;
                        if (this.IsAutoScrolling || position)
                        {
                            this.isMovingUpOrLeft = false;
                            endItemIndex = nextIndex;
                            return;
                        }
                    }
                }

            }
        }
    }
}
```

## Requirements to run the demo

* [Visual Studio 2017](https://visualstudio.microsoft.com/downloads/) or [Visual Studio for Mac](https://visualstudio.microsoft.com/vs/mac/)
* Xamarin add-ons for Visual Studio (available via the Visual Studio installer).

## Troubleshooting

### Path too long exception

If you are facing path too long exception when building this example project, close Visual Studio and rename the repository to short and build the project.
