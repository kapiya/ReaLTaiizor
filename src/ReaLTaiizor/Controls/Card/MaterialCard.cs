﻿#region Imports

using System;
using System.Drawing;
using ReaLTaiizor.Util;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using static ReaLTaiizor.Helper.MaterialDrawHelper;

#endregion

namespace ReaLTaiizor.Controls
{
    #region MaterialCard

    public class MaterialCard : System.Windows.Forms.Panel, MaterialControlI
    {
        [Browsable(false)]
        public int Depth { get; set; }

        [Browsable(false)]
        public MaterialManager SkinManager => MaterialManager.Instance;

        [Browsable(false)]
        public MaterialMouseState MouseState { get; set; }

        public MaterialCard()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            Paint += new PaintEventHandler(paintControl);
            BackColor = SkinManager.BackgroundColor;
            ForeColor = SkinManager.TextHighEmphasisColor;
            Margin = new Padding(SkinManager.FORM_PADDING);
            Padding = new Padding(SkinManager.FORM_PADDING);
        }

        private void drawShadowOnParent(object sender, PaintEventArgs e)
        {
            if (Parent == null)
            {
                RemoveShadowPaintEvent((Control)sender, drawShadowOnParent);
                return;
            }

            // paint shadow on parent
            Graphics gp = e.Graphics;
            Rectangle rect = new Rectangle(Location, ClientRectangle.Size);
            gp.SmoothingMode = SmoothingMode.AntiAlias;
            DrawSquareShadow(gp, rect);
        }

        protected override void InitLayout()
        {
            LocationChanged += (sender, e) => { Parent?.Invalidate(); };
            ForeColor = SkinManager.TextHighEmphasisColor;
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent != null)
            {
                AddShadowPaintEvent(Parent, drawShadowOnParent);
            }

            if (_oldParent != null)
            {
                RemoveShadowPaintEvent(_oldParent, drawShadowOnParent);
            }

            _oldParent = Parent;
        }

        private Control _oldParent;

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Parent == null)
            {
                return;
            }

            if (Visible)
            {
                AddShadowPaintEvent(Parent, drawShadowOnParent);
            }
            else
            {
                RemoveShadowPaintEvent(Parent, drawShadowOnParent);
            }
        }

        private bool _shadowDrawEventSubscribed = false;

        private void AddShadowPaintEvent(Control control, PaintEventHandler shadowPaintEvent)
        {
            if (_shadowDrawEventSubscribed)
            {
                return;
            }

            control.Paint += shadowPaintEvent;
            control.Invalidate();
            _shadowDrawEventSubscribed = true;
        }

        private void RemoveShadowPaintEvent(Control control, PaintEventHandler shadowPaintEvent)
        {
            if (!_shadowDrawEventSubscribed)
            {
                return;
            }

            control.Paint -= shadowPaintEvent;
            control.Invalidate();
            _shadowDrawEventSubscribed = false;
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            BackColor = SkinManager.BackgroundColor;
        }

        private void paintControl(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.Clear(Parent.BackColor);

            // card rectangle path
            RectangleF cardRectF = new RectangleF(ClientRectangle.Location, ClientRectangle.Size);
            cardRectF.X -= 0.5f;
            cardRectF.Y -= 0.5f;
            GraphicsPath cardPath = CreateRoundRect(cardRectF, 4);

            // button shadow (blend with form shadow)
            DrawSquareShadow(g, ClientRectangle);

            // Draw card
            using (SolidBrush normalBrush = new SolidBrush(BackColor))
            {
                g.FillPath(normalBrush, cardPath);
            }
        }
    }

    #endregion
}