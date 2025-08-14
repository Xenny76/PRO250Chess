using System.Drawing;
using System.Windows.Forms;

namespace WinChess
{
    public class FormMenu : Form
    {
        public FormMenu()
        {
            Text = "WinChess — Select Game";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(360, 160);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            var btnClassic = new Button { Text = "Classic Chess", Width = 150, Height = 50, Left = 30, Top = 40 };
            var btn960 = new Button { Text = "Chess960", Width = 150, Height = 50, Left = 180, Top = 40 };

            btnClassic.Click += (_, __) => StartGame(true);
            btn960.Click += (_, __) => StartGame(false);

            Controls.AddRange(new Control[] { btnClassic, btn960 });
        }

        private void StartGame(bool isClassic)
        {
            using (var main = new FormMain(isClassic))
            {
                Hide();
                main.ShowDialog(this);
                Show();
            }
        }
    }
}
