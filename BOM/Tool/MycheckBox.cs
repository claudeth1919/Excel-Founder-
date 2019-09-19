using System;
using System.Drawing;
using System.Windows.Forms;
using BOM.Model;
class MyCheckBox : CheckBox
{
    public MaterialOrder material;

    public MyCheckBox(MaterialOrder material)
    {
        this.TextAlign = ContentAlignment.MiddleRight;
        this.material = material;
        this.Checked = true;
    }

    protected override void OnCheckedChanged(EventArgs e)
    {
        base.OnCheckedChanged(e);
        material.IsSelected = this.Checked;
    }
    protected override void OnCheckStateChanged(EventArgs e)
    {
        base.OnCheckStateChanged(e);
        material.IsSelected = this.Checked;
    }
    public override bool AutoSize
    {
        get { return base.AutoSize; }
        set { base.AutoSize = false; }
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        int h = this.ClientSize.Height - 2;
        Rectangle rc = new Rectangle(new Point(0, 1), new Size(h, h));
        ControlPaint.DrawCheckBox(e.Graphics, rc,
            this.Checked ? ButtonState.Checked : ButtonState.Normal);
    }
}