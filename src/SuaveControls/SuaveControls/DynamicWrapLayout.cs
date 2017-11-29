using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace SuaveControls
{
    /// <summary>
    /// Dynamic wrap layout. Used for creating a data bound list of items and templates
    /// that stack horizontally but wrap when done.
    /// Be careful using this with large amounts of data since there is no real view recycling
    /// This will likely be in a nuget package to the public since nothing like it exists as is
    /// </summary>
    public class DynamicWrapLayout : Layout<View>
    {
        private Dictionary<Size, LayoutData> _layoutDataCache = new Dictionary<Size, LayoutData>();

        #region Bindable Properties
        /// <summary>
        /// The column spacing property.
        /// </summary>
        public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create(
            nameof(ColumnSpacing),
            typeof(double),
            typeof(DynamicWrapLayout),
            5.0,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                ((DynamicWrapLayout)bindable).InvalidateLayout();
            });

        /// <summary>
        /// The row spacing property.
        /// </summary>
        public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create(
            nameof(RowSpacing),
            typeof(double),
            typeof(DynamicWrapLayout),
            5.0,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                ((DynamicWrapLayout)bindable).InvalidateLayout();
            });

        /// <summary>
        /// The items source property.
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IEnumerable<object>),
            typeof(DynamicWrapLayout),
            null);

        /// <summary>
        /// The item template property.
        /// </summary>
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
            nameof(ItemTemplate),
            typeof(DataTemplate),
            typeof(DynamicWrapLayout),
            null);

        /// <summary>
        /// The vertical content alignment property.
        /// </summary>
        public static readonly BindableProperty VerticalContentAlignmentProperty = BindableProperty.Create(
            nameof(VerticalContentAlignment),
            typeof(DynamicGridViewContentAlignment),
            typeof(DynamicWrapLayout),
            DynamicGridViewContentAlignment.Default);

        /// <summary>
        /// The horizontal content alignment property.
        /// </summary>
        public static readonly BindableProperty HorizontalContentAlignmentProperty = BindableProperty.Create(
            nameof(HorizontalContentAlignment),
            typeof(DynamicGridViewContentAlignment),
            typeof(DynamicWrapLayout),
            DynamicGridViewContentAlignment.Default);

        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the horizontal content alignment.
        /// </summary>
        /// <value>The horizontal content alignment.</value>
        public DynamicGridViewContentAlignment HorizontalContentAlignment
        {
            get { return (DynamicGridViewContentAlignment)GetValue(HorizontalContentAlignmentProperty); }
            set { SetValue(HorizontalContentAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the vertical content alignment.
        /// </summary>
        /// <value>The vertical content alignment.</value>
        public DynamicGridViewContentAlignment VerticalContentAlignment
        {
            get { return (DynamicGridViewContentAlignment)GetValue(VerticalContentAlignmentProperty); }
            set { SetValue(VerticalContentAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the item template.
        /// </summary>
        /// <value>The item template.</value>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the column spacing.
        /// </summary>
        /// <value>The column spacing.</value>
        public double ColumnSpacing
        {
            set { SetValue(ColumnSpacingProperty, value); }
            get { return (double)GetValue(ColumnSpacingProperty); }
        }

        /// <summary>
        /// Gets or sets the row spacing.
        /// </summary>
        /// <value>The row spacing.</value>
        public double RowSpacing
        {
            set { SetValue(RowSpacingProperty, value); }
            get { return (double)GetValue(RowSpacingProperty); }
        }

        #endregion

        /// <summary>
        /// Called when measuring for layout
        /// </summary>
        /// <returns>The measure.</returns>
        /// <param name="widthConstraint">Width constraint.</param>
        /// <param name="heightConstraint">Height constraint.</param>
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            LayoutData layoutData = GetLayoutData(widthConstraint, heightConstraint);
            if (layoutData.VisibleChildCount == 0)
            {
                return new SizeRequest();
            }

            Size totalSize = new Size(layoutData.CellSize.Width * layoutData.Columns + ColumnSpacing * (layoutData.Columns - 1),
                                      layoutData.CellSize.Height * layoutData.Rows + RowSpacing * (layoutData.Rows - 1));
            return new SizeRequest(totalSize);
        }

        /// <summary>
        /// Layouts the children. This is where the magic happens
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            LayoutData layoutData = GetLayoutData(width, height);

            if (layoutData.VisibleChildCount == 0)
            {
                return;
            }

            double xChild = x;
            double yChild = y;
            int row = 0;
            int column = 0;

            foreach (View child in Children)
            {
                if (!child.IsVisible)
                {
                    continue;
                }

                LayoutChildIntoBoundingRegion(child, new Rectangle(new Point(xChild, yChild), layoutData.CellSize));

                if (++column == layoutData.Columns)
                {
                    column = 0;
                    row++;
                    xChild = x;
                    yChild += RowSpacing + layoutData.CellSize.Height;
                }
                else
                {
                    xChild += ColumnSpacing + layoutData.CellSize.Width;
                }
            }
        }

        /// <summary>
        /// Gets the layout data.
        /// </summary>
        /// <returns>The layout data.</returns>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        private LayoutData GetLayoutData(double width, double height)
        {
            Size size = new Size(width, height);

            // Check if cached information is available.
            if (_layoutDataCache.ContainsKey(size))
            {
                return _layoutDataCache[size];
            }

            int visibleChildCount = 0;
            Size maxChildSize = new Size();
            int rows = 0;
            int columns = 0;
            LayoutData layoutData = new LayoutData();

            // Enumerate through all the children.
            foreach (View child in Children)
            {
                // Skip invisible children.
                if (!child.IsVisible)
                    continue;

                // Count the visible children.
                visibleChildCount++;

                // Get the child's requested size.
                SizeRequest childSizeRequest = child.Measure(Double.PositiveInfinity, Double.PositiveInfinity);

                // Accumulate the maximum child size.
                maxChildSize.Width = Math.Max(maxChildSize.Width, childSizeRequest.Request.Width);
                maxChildSize.Height = Math.Max(maxChildSize.Height, childSizeRequest.Request.Height);
            }

            if (visibleChildCount != 0)
            {
                // Calculate the number of rows and columns.
                if (Double.IsPositiveInfinity(width))
                {
                    columns = visibleChildCount;
                    rows = 1;
                }
                else
                {
                    columns = (int)((width + ColumnSpacing) / (maxChildSize.Width + ColumnSpacing));
                    columns = Math.Max(1, columns);
                    rows = (visibleChildCount + columns - 1) / columns;
                }

                // Now maximize the cell size based on the layout size.
                Size cellSize = new Size();

                if (Double.IsPositiveInfinity(width))
                {
                    cellSize.Width = maxChildSize.Width;
                }
                else
                {
                    cellSize.Width = (width - ColumnSpacing * (columns - 1)) / columns;
                }

                if (Double.IsPositiveInfinity(height))
                {
                    cellSize.Height = maxChildSize.Height;
                }
                else
                {
                    cellSize.Height = (height - RowSpacing * (rows - 1)) / rows;
                }

                layoutData = new LayoutData(visibleChildCount, cellSize, rows, columns);
            }

            _layoutDataCache.Add(size, layoutData);
            return layoutData;
        }

        /// <summary>
        /// Creates the grid.
        /// </summary>
        private void CreateGrid()
        {
            // Check for data
            if (ItemsSource == null || ItemsSource.Count() == 0)
            {
                return;
            }

            CreateCells();
        }

        /// <summary>
        /// Creates the cell view.
        /// </summary>
        /// <returns>The cell view.</returns>
        /// <param name="item">Item.</param>
        private View CreateCellView(object item)
        {
            var view = (View)ItemTemplate.CreateContent();
            var bindableObject = (BindableObject)view;

            if (bindableObject != null)
            {
                bindableObject.BindingContext = item;
            }

            return view;
        }

        /// <summary>
        /// Creates the cells.
        /// </summary>
        private void CreateCells()
        {
            foreach (var item in ItemsSource)
            {
                Children.Add(CreateCellView(item));
            }
        }

        /// <summary>
        /// Invalidates the layout.
        /// </summary>
        protected override void InvalidateLayout()
        {
            base.InvalidateLayout();

            // Discard all layout information for children added or removed.
            _layoutDataCache.Clear();
        }

        /// <summary>
        /// Called when child object is invalidated
        /// </summary>
        protected override void OnChildMeasureInvalidated()
        {
            base.OnChildMeasureInvalidated();

            // Discard all layout information for child size changed.
            _layoutDataCache.Clear();
        }


        /// <summary>
        /// Called when the binding context changes
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            CreateGrid();
        }

        /// <summary>
        /// Called when the item source property changes
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == ItemsSourceProperty.PropertyName)
            {
                CreateGrid();
            }

            base.OnPropertyChanged(propertyName);
        }
    }
}
