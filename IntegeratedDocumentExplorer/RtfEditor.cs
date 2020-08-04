using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using C1.Win.C1Ribbon;
using System.IO;

namespace IntegeratedDocumentExplorer
{
    public partial class RtfEditor : C1RibbonForm
    {
        private TextEditor.TextEditor textEditor;
        public RtfEditor()
        {
            this.textEditor = new TextEditor.TextEditor();
            InitializeComponent();
        }
        public RtfEditor(string fileName)
        {
            this.textEditor = new TextEditor.TextEditor(fileName);
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.textEditor.Name = "RTF Editor";
            this.textEditor.Dock = DockStyle.Fill;
            this.textEditor.Size = new Size(200, 200);
            this.textEditor.Location = new Point(0, 0);
            this.Controls.Add(this.textEditor);
            // 
            // RtfEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "RtfEditor";
            this.Text = "Editor";
            //this.Load += new System.EventHandler(this.RtfEditor_Load);
            this.ResumeLayout(false);
        }

    }
}
