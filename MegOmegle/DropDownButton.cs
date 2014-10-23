/* DropDownButton.cs
 * 
 * A custom button that supports a context menu.
 * 
 */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MegOmegle
{
    public class DropDownButton : Button
    {
        private const int ARROW_WIDTH = 8;
        private const int LINE_PADDING = 7;
        private string btnText;
        private bool arrowEnabled;

        //Store text this way to custom align it later
        public string ButtonText
        {
            get { return btnText; }
            set
            {
                btnText = value;
                Invalidate();
            }
        }

        [DefaultValue(null)]
        public ContextMenuStrip ButtonMenu { get; set; }

        [DefaultValue(false)]
        public bool ArrowEnabled
        {
            get { return arrowEnabled; }
            set
            {
                arrowEnabled = value;
                Invalidate();
            }
        }

        private Rectangle getArrowBox()
        {
            //Rectangle bounding dropdown part of button
            return new Rectangle(ClientRectangle.Width - ARROW_WIDTH - 2 * LINE_PADDING, 0,
                                 ARROW_WIDTH + 2 * LINE_PADDING - 1, ClientRectangle.Height - 1);            
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            //Open context menu if arrow clicked
            if (ButtonMenu != null && e.Button == MouseButtons.Left && arrowEnabled && getArrowBox().Contains(e.Location))
                ButtonMenu.Show(this, e.Location);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Brush b = Enabled ? SystemBrushes.ControlText : SystemBrushes.ButtonShadow;
            SizeF size = e.Graphics.MeasureString(btnText, base.Font);

            //Draw arrow if context menu
            if (arrowEnabled)
            {
                Rectangle arrowBox = getArrowBox();

                //Align text in non-arrow part of button
                
                PointF location = new PointF(arrowBox.X / 2 - size.Width / 2 + 2, arrowBox.Height / 2 - size.Height / 2 + 1);
                e.Graphics.DrawString(btnText, base.Font, b, location);

                //Arrow
                Point[] arrowPoints = new Point[] { new Point(arrowBox.X+LINE_PADDING, ClientRectangle.Height/2 - 1),
                                           new Point(arrowBox.X+LINE_PADDING+ARROW_WIDTH, ClientRectangle.Height/2 - 1),
                                           new Point(arrowBox.X+LINE_PADDING + ARROW_WIDTH/2, ClientRectangle.Height/2 - 1 + ARROW_WIDTH/2) };
                e.Graphics.FillPolygon(b, arrowPoints);

                //Separator line
                e.Graphics.DrawLine(SystemPens.ButtonShadow,
                                    new Point(arrowBox.X, LINE_PADDING),
                                    new Point(arrowBox.X, ClientRectangle.Height - LINE_PADDING));
            }
            else
            {
                //Center text
                PointF location = new PointF(ClientRectangle.Width/2 - size.Width/2 + 1, ClientRectangle.Height/2 - size.Height/2 + 1);
                e.Graphics.DrawString(btnText, base.Font, b, location);
            }
        }

        /// <summary>
        /// Returnes the index of the first checked item in the menu.
        /// </summary>
        /// <returns>The checked item index.</returns>
        public int getCheckedIndex()
        {
            if (ButtonMenu != null)
            {
                for (int i = 0; i < ButtonMenu.Items.Count; i++)
                {
                    if (((ToolStripMenuItem)ButtonMenu.Items[i]).Checked)
                        return i;
                }
            }
            return -1;
        }
    }
}
