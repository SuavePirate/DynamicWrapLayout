using System;
using Xamarin.Forms;

namespace SuaveControls.DynamicWrapLayout
{
    /// <summary>
    /// Layout data.
    /// </summary>
    struct LayoutData
    {
        /// <summary>
        /// Gets the visible child count.
        /// </summary>
        /// <value>The visible child count.</value>
        public int VisibleChildCount { get; private set; }

        /// <summary>
        /// Gets the size of the cell.
        /// </summary>
        /// <value>The size of the cell.</value>
        public Size CellSize { get; private set; }

        /// <summary>
        /// Gets the rows.
        /// </summary>
        /// <value>The rows.</value>
        public int Rows { get; private set; }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <value>The columns.</value>
        public int Columns { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SuaveControls.LayoutData"/> struct.
        /// </summary>
        /// <param name="visibleChildCount">Visible child count.</param>
        /// <param name="cellSize">Cell size.</param>
        /// <param name="rows">Rows.</param>
        /// <param name="columns">Columns.</param>
        public LayoutData(int visibleChildCount, Size cellSize, int rows, int columns) : this()
        {
            VisibleChildCount = visibleChildCount;
            CellSize = cellSize;
            Rows = rows;
            Columns = columns;
        }

    }
}
