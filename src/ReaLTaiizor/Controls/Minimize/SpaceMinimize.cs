﻿#region Imports

using System.Drawing;
using ReaLTaiizor.Util;
using System.Windows.Forms;

#endregion

namespace ReaLTaiizor.Controls
{
    #region SpaceMinimize

    public class SpaceMinimize : SpaceControl // A Hide Button for the App
    {
        public FormWindowState WindowState { get; set; }

        private bool _DefaultLocation = true;
        public bool DefaultLocation
        {
            get => _DefaultLocation;
            set
            {
                _DefaultLocation = value;
                Invalidate();
            }
        }

        private bool _DefaultAnchor = true;
        public bool DefaultAnchor
        {
            get => _DefaultAnchor;
            set
            {
                _DefaultAnchor = value;
                Invalidate();
            }
        }

        public SpaceMinimize()
        {
            SetColor("DownGradient1", 140, 138, 27); // Basic Gradients Used to Shade the Button
            SetColor("DownGradient2", 180, 196, 114); // The Gradients are reversed, depending on if Button is Pressed or not
            SetColor("NoneGradient1", 50, 50, 50);
            SetColor("NoneGradient2", 42, 42, 42);
            SetColor("ClickedGradient1", 204, 201, 35);
            SetColor("ClickedGradient2", 140, 138, 27);
            SetColor("Text", 254, 254, 254); // The Color for the Text
            SetColor("Border1", 35, 35, 35); // The Inside Border
            SetColor("Border2", 42, 42, 42); // The Outside Border
            Cursor = Cursors.Hand;
            Size = new Size(23, 21);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            Text = "_";

            if (DefaultLocation)
            {
                Location = new Point(Parent.Width - (Width * 3) - 5, 3);
            }

            if (DefaultAnchor)
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right;
            }
        }

        private Color C1; // Set up Simple Colors
        private Color C2;
        private Color C3;
        private Color C4;
        private Color C5;
        private Color C6;
        private SolidBrush B1; // A Brush to use text
        private Pen P1; // A Pen used to create borders
        private Pen P2;

        protected override void ColorHook()
        {
            C1 = GetColor("DownGradient1"); // Get the Colors for the Button Shading
            C2 = GetColor("DownGradient2");
            C3 = GetColor("NoneGradient1");
            C4 = GetColor("NoneGradient2");
            C5 = GetColor("ClickedGradient1");
            C6 = GetColor("ClickedGradient2");
            B1 = new SolidBrush(GetColor("Text")); // Set up Color for the Text
            P1 = new Pen(GetColor("Border1")); // Get and create the borders for the Buttons
            P2 = new Pen(GetColor("Border2"));
        }

        protected override void PaintHook()
        {
            if (State == MouseStateSpace.Over)
            { // Used to see if button is Hovered over
                DrawGradient(C1, C2, ClientRectangle, 90f); // if button is hovered over
            }
            else if (State == MouseStateSpace.Down)
            {
                DrawGradient(C6, C5, ClientRectangle, 90f);
                Parent.FindForm().WindowState = FormWindowState.Minimized;
            }
            else
            {
                DrawGradient(C3, C4, ClientRectangle, 90f); // else change the shading
            }

            DrawText(B1, HorizontalAlignment.Center, 0, 0); // Draw the Text Smack dab in the middle of the button
            DrawBorders(P1, 1); // Create the Inner Border
            DrawBorders(P2); // Create the Outer Border
            DrawCorners(BackColor); // Draw the Corners
        }

    }

    #endregion
}