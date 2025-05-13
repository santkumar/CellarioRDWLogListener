using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CellarioRDWLogListener
{
    public class ConsoleRedirector : TextWriter
    {
        private readonly RichTextBox outputBox;
        private readonly Form form;

        public ConsoleRedirector(RichTextBox box, Form form)
        {
            outputBox = box;
            this.form = form;
        }

        public override void Write(char value)
        {
            form.Invoke((MethodInvoker)(() =>
            {
                outputBox.AppendText(value.ToString());
            }));
        }

        public override void Write(string value)
        {
            form.Invoke((MethodInvoker)(() =>
            {
                outputBox.AppendText(value);
            }));
        }

        public override Encoding Encoding => Encoding.UTF8;
    }
}
