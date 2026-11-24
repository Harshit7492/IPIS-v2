//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace IPISv2
//{
//    public partial class ucKeyboard : UserControl
//    {
//        public enum KeyboardLanguage
//        {
//            Hindi,
//            Bengali,
//            Gujarati
//        }
//        private bool isDragging = false;
//        private Point dragStartPoint; 
//        public KeyboardLanguage CurrentLanguage { get; private set; }
//        public TextBox TargetTextBox { get; set; }
//        public ucKeyboard()
//        {
//            InitializeComponent();
//            BuildKeyboard(KeyboardLanguage.Hindi);
//            this.Size = new Size(680, 220);

//        }
//        private List<string[]> GetKeys(KeyboardLanguage lang)
//        {
//            switch (lang)
//            {
//                case KeyboardLanguage.Bengali:
//                    return new List<string[]>
//            {
//                new string[] { "१","२","३","४","५","६","७","८","९","०" }, 
//                new string[] { "अ","आ","इ","ई","उ","ऊ","ए","ऐ","ओ","औ","अं","अः" }, 
//                new string[] { "क","ख","ग","घ","ङ","च","छ","ज","झ","ञ" },
//                new string[] { "ट","ठ","ड","ढ","ण","त","थ","द","ध","न" },
//                new string[] { "प","फ","ब","भ","म","य","र","ल","व" },
//                new string[] { "श","ष","स","ह","क्ष","त्र","ज्ञ" },
//                new string[] { "ा","ि","ी","ु","ू","े","ै","ो","ौ","ं","ः","्" }
//            };

//                case KeyboardLanguage.Gujarati:
//                    return new List<string[]>
//            {
//                new string[] { "१","२","३","४","५","६","७","८","९","०" },
//                new string[] { "अ","आ","इ","ई","उ","ऊ","ए","ऐ","ओ","औ","अं","अः" },
//                new string[] { "क","ख","ग","घ","ङ","च","छ","ज","झ","ञ" },
//                new string[] { "ट","ठ","ड","ढ","ण","त","थ","द","ध","न" },
//                new string[] { "प","फ","ब","भ","म","य","र","ल","व" },
//                new string[] { "श","ष","स","ह","क्ष","त्र","ज्ञ" },
//                new string[] { "ा","ि","ी","ु","ू","े","ै","ो","ौ","ं","ः","्" }
//            };

//                default:
//                    return new List<string[]>
//            {
//                new string[] { "१","२","३","४","५","६","७","८","९","०" },
//                new string[] { "अ","आ","इ","ई","उ","ऊ","ए","ऐ","ओ","औ","अं","अः" },
//                new string[] { "क","ख","ग","घ","ङ","च","छ","ज","झ","ञ" },
//                new string[] { "ट","ठ","ड","ढ","ण","त","थ","द","ध","न" },
//                new string[] { "प","फ","ब","भ","म","य","र","ल","व" },
//                new string[] { "श","ष","स","ह","क्ष","त्र","ज्ञ" },
//                new string[] { "ा","ि","ी","ु","ू","े","ै","ो","ौ","ं","ः","्" }
//            };
//            }
//        }
//        public void BuildKeyboard(KeyboardLanguage lang)
//        {
//            CurrentLanguage = lang;
//            var keys = GetKeys(lang);
//            this.BackColor = Color.White;
//            TableLayoutPanel main = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                RowCount = 2
//            };
//            main.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
//            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

//            Panel header = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.FromArgb(76, 175, 80)
//            };

//            Label title = new Label
//            {
//                Text = $"{lang} Keyboard",
//                ForeColor = Color.White,
//                Font = new Font("Segoe UI", 10, FontStyle.Bold),
//                AutoSize = true,
//                Location = new Point(10, 10)
//            };

//            Button btnClose = new Button
//            {
//                Text = "✖",
//                ForeColor = Color.White,
//                FlatStyle = FlatStyle.Flat,
//                Width = 40,
//                Dock = DockStyle.Right
//            };
//            btnClose.FlatAppearance.BorderSize = 0;
//            btnClose.Click += (s, e) => this.FindForm()?.Hide();
//            header.MouseDown += Header_MouseDown;
//            header.MouseMove += Header_MouseMove;
//            header.MouseUp += Header_MouseUp;
//            header.Controls.Add(title);
//            header.Controls.Add(btnClose);

//            TableLayoutPanel body = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                RowCount = keys.Count + 1
//            };

//            for (int i = 0; i < keys.Count + 1; i++)
//                body.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / (keys.Count + 1)));

//            foreach (var row in keys)
//            {
//                TableLayoutPanel rowPanel = new TableLayoutPanel
//                {
//                    Dock = DockStyle.Fill,
//                    ColumnCount = row.Length,
//                    Margin = new Padding(1),
//                    Padding = new Padding(0)
//                };


//                for (int i = 0; i < row.Length; i++)
//                {
//                    rowPanel.ColumnStyles.Add(
//                        new ColumnStyle(SizeType.Percent, 100f / row.Length)
//                    );

//                    var btn = CreateStyledButton(row[i]);
//                    btn.Dock = DockStyle.Fill;

//                    rowPanel.Controls.Add(btn, i, 0);
//                }

//                body.Controls.Add(rowPanel);
//            }

//            TableLayoutPanel lastRow = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                ColumnCount = 2
//            };

//            lastRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75));
//            lastRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));

//            Button btnSpace = CreateStyledButton("SPACE");
//            btnSpace.Dock = DockStyle.Fill;
//            btnSpace.Click += (s, e) => InsertText(" ");

//            Button btnBack = CreateStyledButton("⌫");
//            btnBack.Dock = DockStyle.Fill;
//            btnBack.Click += BtnBack_Click;

//            lastRow.Controls.Add(btnSpace, 0, 0);
//            lastRow.Controls.Add(btnBack, 1, 0);

//            body.Controls.Add(lastRow);

//            main.Controls.Add(header, 0, 0);
//            main.Controls.Add(body, 0, 1);

//            this.Controls.Clear();
//            this.Controls.Add(main); 
//        }
//        private Button CreateStyledButton(string text)
//        {
//            Button btn = new Button
//            {
//                Text = text,
//                Width = 40,
//                Height = 38,
//                Margin = new Padding(1),
//                Font = new Font("Nirmala UI", 11, FontStyle.Bold),
//                BackColor = Color.White,
//                FlatStyle = FlatStyle.Flat
//            };

//            btn.FlatAppearance.BorderColor = Color.LightGray;
//            btn.FlatAppearance.BorderSize = 1;

//            btn.MouseEnter += (s, e) => btn.BackColor = Color.LightBlue;
//            btn.MouseLeave += (s, e) => btn.BackColor = Color.White;

//            if (text != "SPACE" && text != "⌫")
//                btn.Click += (s, e) => InsertText(text);

//            return btn;
//        }
//        private void InsertText(string text)
//        {
//            if (TargetTextBox == null) return;

//            int pos = TargetTextBox.SelectionStart;

//            TargetTextBox.Text = TargetTextBox.Text.Insert(pos, text);
//            TargetTextBox.SelectionStart = pos + text.Length;
//            TargetTextBox.Focus();
//        }
//        private void BtnBack_Click(object sender, EventArgs e)
//        {
//            if (TargetTextBox == null) return;

//            int pos = TargetTextBox.SelectionStart;

//            if (pos > 0)
//            {
//                TargetTextBox.Text = TargetTextBox.Text.Remove(pos - 1, 1);
//                TargetTextBox.SelectionStart = pos - 1;
//            }
//        }
//        private void Header_MouseDown(object sender, MouseEventArgs e)
//        {
//            isDragging = true;
//            dragStartPoint = e.Location;
//        }
//        private void Header_MouseMove(object sender, MouseEventArgs e)
//        {
//            if (isDragging)
//            {
//                Form form = this.FindForm();
//                if (form != null)
//                {
//                    form.Location = new Point(
//                        form.Left + e.X - dragStartPoint.X,
//                        form.Top + e.Y - dragStartPoint.Y
//                    );
//                }
//            }
//        }
//        private void Header_MouseUp(object sender, MouseEventArgs e)
//        {
//            isDragging = false;
//        }

//    }
//}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPISv2
{
    public partial class ucKeyboard : UserControl
    {
        private Form keyboardForm;
        private ucKeyboard keyboardControl;
        //Code change on 16-06-2026.
        public enum KeyboardLanguage
        {
            Hindi,
            Assamese,
            Bengali,
            Gujarati,
            Kannada,
            Malayalam,
            Marathi,
            Odia,
            Punjabi,
            Tamil,
            Telugu,

            Bodo,
            Dogri,
            Kashmiri,
            Konkani,
            Maithili,
            Manipuri,
            Nepali,
            Sanskrit,
            Santali,
            Sindhi,
            Urdu
        }

        private bool isDragging = false;
        private Point dragStartPoint;
        public KeyboardLanguage CurrentLanguage { get; private set; }
        public TextBox TargetTextBox { get; set; }
        public ucKeyboard()
        {
            InitializeComponent();
            //BuildKeyboard(KeyboardLanguage.Hindi);
            this.Size = new Size(1000, 420);

        }
        private List<string[]> GetKeys(KeyboardLanguage lang)
        {
            switch (lang)
            {
                case KeyboardLanguage.Bengali:
                    return new List<string[]>
                    {
                        new[] { "১","২","৩","৪","৫","৬","৭","৮","৯","০" },
                        new[] { "অ","আ","ই","ঈ","উ","ঊ","এ","ঐ","ও","ঔ","ং","ঃ" },
                        new[] { "ক","খ","গ","ঘ","ঙ","চ","ছ","জ","ঝ","ঞ" },
                        new[] { "ট","ঠ","ড","ঢ","ণ","ত","থ","দ","ধ","ন" },
                        new[] { "প","ফ","ব","ভ","ম","য","র","ল","শ","ষ" },
                        new[] { "স","হ","ড়","ঢ়","য়","ৎ","ক্ষ","জ্ঞ" },
                        new[] { "া","ি","ী","ু","ূ","ে","ৈ","ো","ৌ","্","ঁ" }
                    };

                case KeyboardLanguage.Gujarati:
                    return new List<string[]>
                    {
                        new[] { "૧","૨","૩","૪","૫","૬","૭","૮","૯","૦" },
                        new[] { "અ","આ","ઇ","ઈ","ઉ","ઊ","એ","ઐ","ઓ","ઔ","અં","અઃ" },
                        new[] { "ક","ખ","ગ","ઘ","ઙ","ચ","છ","જ","ઝ","ઞ" },
                        new[] { "ટ","ઠ","ડ","ઢ","ણ","ત","થ","દ","ધ","ન" },
                        new[] { "પ","ફ","બ","ભ","મ","ય","ર","લ","વ" },
                        new[] { "શ","ષ","સ","હ","ળ","ક્ષ","જ્ઞ" },
                        new[] { "ા","િ","ી","ુ","ૂ","ે","ૈ","ો","ૌ","્","ં","ઃ" }

                    };
                case KeyboardLanguage.Marathi:
                    return new List<string[]>
                    {
                        new[] { "१","२","३","४","५","६","७","८","९","०" },
                        new[] { "अ","आ","इ","ई","उ","ऊ","ए","ऐ","ओ","औ","अं","अः" },
                        new[] { "क","ख","ग","घ","ङ","च","छ","ज","झ","ञ" },
                        new[] { "ट","ठ","ड","ढ","ण","त","थ","द","ध","न" },
                        new[] { "प","फ","ब","भ","म","य","र","ल","व" },
                        new[] { "श","ष","स","ह","ळ","क्ष","ज्ञ" },
                        new[] { "ा","ि","ी","ु","ू","े","ै","ो","ौ","ं","ः","्" }
                    };
                case KeyboardLanguage.Assamese:
                    return new List<string[]>
                    {
                        new[] { "১","২","৩","৪","৫","৬","৭","৮","৯","০" },
                        new[] { "অ","আ","ই","ঈ","উ","ঊ","এ","ঐ","ও","ঔ","ং","ঃ" },
                        new[] { "ক","খ","গ","ঘ","ঙ","চ","ছ","জ","ঝ","ঞ" },
                        new[] { "ট","ঠ","ড","ঢ","ণ","ত","থ","দ","ধ","ন" },
                        new[] { "প","ফ","ব","ভ","ম","য","ৰ","ল","শ","ষ" },
                        new[] { "স","হ","ড়","ঢ়","য়","ৎ","ক্ষ","জ্ঞ" },
                        new[] { "া","ি","ী","ু","ূ","ে","ৈ","ো","ৌ","্","ঁ" }
                    };
                case KeyboardLanguage.Punjabi:
                    return new List<string[]>
                    {
                        new[] { "੧","੨","੩","੪","੫","੬","੭","੮","੯","੦" },
                        new[] { "ਅ","ਆ","ਇ","ਈ","ਉ","ਊ","ਏ","ਐ","ਓ","ਔ","ਅੰ","ਅਃ" },
                        new[] { "ਕ","ਖ","ਗ","ਘ","ਙ","ਚ","ਛ","ਜ","ਝ","ਞ" },
                        new[] { "ਟ","ਠ","ਡ","ਢ","ਣ","ਤ","ਥ","ਦ","ਧ","ਨ" },
                        new[] { "ਪ","ਫ","ਬ","ਭ","ਮ","ਯ","ਰ","ਲ","ਵ" },
                        new[] { "ਸ਼","਷","ਸ","ਹ","ੜ","ਖ਼","ਗ਼" },
                        new[] { "ਾ","ਿ","ੀ","ੁ","ੂ","ੇ","ੈ","ੋ","ੌ","ਂ","ੱ","੍" }
                    };
                case KeyboardLanguage.Odia:
                    return new List<string[]>
                    {
                        new[] { "୧","୨","୩","୪","୫","୬","୭","୮","୯","୦" },
                        new[] { "ଅ","ଆ","ଇ","ଈ","ଉ","ଊ","ଏ","ଐ","ଓ","ଔ","ଂ","ଃ" },
                        new[] { "କ","ଖ","ଗ","ଘ","ଙ","ଚ","ଛ","ଜ","ଝ","ଞ" },
                        new[] { "ଟ","ଠ","ଡ","ଢ","ଣ","ତ","ଥ","ଦ","ଧ","ନ" },
                        new[] { "ପ","ଫ","ବ","ଭ","ମ","ଯ","ର","ଲ","ୱ" },
                        new[] { "ଶ","ଷ","ସ","ହ","ଳ","କ୍ଷ","ଜ୍ଞ" },
                        new[] { "ା","ି","ୀ","ୁ","ୂ","େ","ୈ","ୋ","ୌ","ଂ","ଃ","୍" }
                    };
                case KeyboardLanguage.Tamil:
                    return new List<string[]>
                    {
                        new[] { "௧","௨","௩","௪","௫","௬","௭","௮","௯","௦" },
                        new[] { "அ","ஆ","இ","ஈ","உ","ஊ","எ","ஏ","ஐ","ஒ","ஓ","ஔ" },
                        new[] { "க","ங","ச","ஜ","ஞ","ட","ண","த","ந","ன" },
                        new[] { "ப","ம","ய","ர","ல","வ","ழ","ள","ற" },
                        new[] { "ஷ","ஸ","ஹ","க்ஷ","ஃ" },
                        new[] { "ா","ி","ீ","ு","ூ","ெ","ே","ை","ொ","ோ","ௌ","்" }
                    };
                case KeyboardLanguage.Telugu:
                    return new List<string[]>
                    {
                        new[] { "౧","౨","౩","౪","౫","౬","౭","౮","౯","౦" },
                        new[] { "అ","ఆ","ఇ","ఈ","ఉ","ఊ","ఎ","ఏ","ఐ","ఒ","ఓ","ఔ" },
                        new[] { "క","ఖ","గ","ఘ","ఙ","చ","ఛ","జ","ఝ","ఞ" },
                        new[] { "ట","ఠ","డ","ఢ","ణ","త","థ","ద","ధ","న" },
                        new[] { "ప","ఫ","బ","భ","మ","య","ర","ల","వ" },
                        new[] { "శ","ష","స","హ","ళ","క్ష","జ్ఞ" },
                        new[] { "ా","ి","ీ","ు","ూ","ె","ే","ై","ొ","ో","ౌ","్" }
                    };
                case KeyboardLanguage.Kannada:
                    return new List<string[]>
                    {
                        new[] { "೧","೨","೩","೪","೫","೬","೭","೮","೯","೦" },
                        new[] { "ಅ","ಆ","ಇ","ಈ","ಉ","ಊ","ಎ","ಏ","ಐ","ಒ","ಓ","ಔ" },
                        new[] { "ಕ","ಖ","ಗ","ಘ","ಙ","ಚ","ಛ","ಜ","ಝ","ಞ" },
                        new[] { "ಟ","ಠ","ಡ","ಢ","ಣ","ತ","ಥ","ದ","ಧ","ನ" },
                        new[] { "ಪ","ಫ","ಬ","ಭ","ಮ","ಯ","ರ","ಲ","ವ" },
                        new[] { "ಶ","ಷ","ಸ","ಹ","ಳ","ಕ್ಷ","ಜ್ಞ" },
                        new[] { "ಾ","ಿ","ೀ","ು","ೂ","ೆ","ೇ","ೈ","ೊ","ೋ","ೌ","್" }
                    };
                case KeyboardLanguage.Malayalam:
                    return new List<string[]>
                    {
                        new[] { "൧","൨","൩","൪","൫","൬","൭","൮","൯","൦" },
                        new[] { "അ","ആ","ഇ","ഈ","ഉ","ഊ","എ","ഏ","ഐ","ഒ","ഓ","ഔ" },
                        new[] { "ക","ഖ","ഗ","ഘ","ങ","ച","ഛ","ജ","ഝ","ഞ" },
                        new[] { "ട","ഠ","ഡ","ഢ","ണ","ത","ഥ","ദ","ധ","ന" },
                        new[] { "പ","ഫ","ബ","ഭ","മ","യ","ര","ല","വ" },
                        new[] { "ശ","ഷ","സ","ഹ","ള","ക്ഷ","ജ്ഞ" },
                        new[] { "ാ","ി","ീ","ു","ൂ","െ","േ","ൈ","ൊ","ോ","ൌ","്" }

                    };
                case KeyboardLanguage.Bodo:
                    return new List<string[]>
    {
        new[] { "१","२","३","४","५","६","७","८","९","०" },
        new[] { "अ","आ","इ","ई","उ","ऊ","ए","ऐ","ओ","औ","अं","अः" },
        new[] { "क","ख","ग","घ","ङ","च","छ","ज","झ","ञ" },
        new[] { "ट","ठ","ड","ढ","ण","त","थ","द","ध","न" },
        new[] { "प","फ","ब","भ","म","य","र","ल","व" },
        new[] { "श","ष","स","ह","क्ष","त्र","ज्ञ" },
        new[] { "ा","ि","ी","ु","ू","े","ै","ो","ौ","ं","ः","्" }
    };
                case KeyboardLanguage.Dogri:
                    return new List<string[]>
    {
        new[] { "१","२","३","४","५","६","७","८","९","०" },
        new[] { "अ","आ","इ","ई","उ","ऊ","ए","ऐ","ओ","औ","अं","अः" },
        new[] { "क","ख","ग","घ","ङ","च","छ","ज","झ","ञ" },
        new[] { "ट","ठ","ड","ढ","ण","त","थ","द","ध","न" },
        new[] { "प","फ","ब","भ","म","य","र","ल","व" },
        new[] { "श","ष","स","ह","क्ष","त्र","ज्ञ" },
        new[] { "ा","ि","ी","ु","ू","े","ै","ो","ौ","ं","ः","्" }
    };
                case KeyboardLanguage.Konkani:
                    return new List<string[]>
    {
        new[] { "१","२","३","४","५","६","७","८","९","०" },
        new[] { "अ","आ","इ","ई","उ","ऊ","ए","ऐ","ओ","औ","अं","अः" },
        new[] { "क","ख","ग","घ","ङ","च","छ","ज","झ","ञ" },
        new[] { "ट","ठ","ड","ढ","ण","त","थ","द","ध","न" },
        new[] { "प","फ","ब","भ","म","य","र","ल","व" },
        new[] { "श","ष","स","ह","क्ष","त्र","ज्ञ" },
        new[] { "ा","ि","ी","ु","ू","े","ै","ो","ौ","ं","ः","्" }
    };
                case KeyboardLanguage.Maithili:
                    return new List<string[]>
    {
        new[] { "१","२","३","४","५","६","७","८","९","०" },
        new[] { "अ","आ","इ","ई","उ","ऊ","ए","ऐ","ओ","औ","अं","अः" },
        new[] { "क","ख","ग","घ","ङ","च","छ","ज","झ","ञ" },
        new[] { "ट","ठ","ड","ढ","ण","त","थ","द","ध","न" },
        new[] { "प","फ","ब","भ","म","य","र","ल","व" },
        new[] { "श","ष","स","ह","क्ष","त्र","ज्ञ" },
        new[] { "ा","ि","ी","ु","ू","े","ै","ो","ौ","ं","ः","्" }
    };
                case KeyboardLanguage.Nepali:
                    return new List<string[]>
    {
        new[] { "१","२","३","४","५","६","७","८","९","०" },
        new[] { "अ","आ","इ","ई","उ","ऊ","ए","ऐ","ओ","औ","अं","अः" },
        new[] { "क","ख","ग","घ","ङ","च","छ","ज","झ","ञ" },
        new[] { "ट","ठ","ड","ढ","ण","त","थ","द","ध","न" },
        new[] { "प","फ","ब","भ","म","य","र","ल","व" },
        new[] { "श","ष","स","ह","क्ष","त्र","ज्ञ" },
        new[] { "ा","ि","ी","ु","ू","े","ै","ो","ौ","ं","ः","्" }
    };
                case KeyboardLanguage.Sanskrit:
                    return new List<string[]>
    {
        new[] { "१","२","३","४","५","६","७","८","९","०" },
        new[] { "अ","आ","इ","ई","उ","ऊ","ऋ","ॠ","ए","ऐ","ओ","औ" },
        new[] { "क","ख","ग","घ","ङ","च","छ","ज","झ","ञ" },
        new[] { "ट","ठ","ड","ढ","ण","त","थ","द","ध","न" },
        new[] { "प","फ","ब","भ","म","य","र","ल","व" },
        new[] { "श","ष","स","ह","क्ष","त्र","ज्ञ" },
        new[] { "ा","ि","ी","ु","ू","ृ","ॄ","े","ै","ो","ौ","्" }
    };
                case KeyboardLanguage.Urdu:
                    return new List<string[]>
    {
        new[] { "۱","۲","۳","۴","۵","۶","۷","۸","۹","۰" },
        new[] { "ا","آ","ب","پ","ت","ٹ","ث","ج","چ","ح","خ" },
        new[] { "د","ڈ","ذ","ر","ڑ","ز","ژ","س","ش","ص" },
        new[] { "ض","ط","ظ","ع","غ","ف","ق","ک","گ","ل" },
        new[] { "م","ن","ں","و","ہ","ھ","ء","ی","ے" },
        new[] { "َ","ِ","ُ","ّ","ْ","ٓ","ٔ" }
    };
                case KeyboardLanguage.Kashmiri:
                    return new List<string[]>
    {
        new[] { "۱","۲","۳","۴","۵","۶","۷","۸","۹","۰" },
        new[] { "ا","آ","ب","پ","ت","ٹ","ث","ج","چ","ح","خ" },
        new[] { "د","ڈ","ذ","ر","ڑ","ز","ژ","س","ش","ص" },
        new[] { "ض","ط","ظ","ع","غ","ف","ق","ک","گ","ل" },
        new[] { "م","ن","ں","و","ہ","ھ","ء","ی","ے" },
        new[] { "َ","ِ","ُ","ّ","ْ","ٓ","ٔ" }
    };
                case KeyboardLanguage.Sindhi:
                    return new List<string[]>
    {
        new[] { "۱","۲","۳","۴","۵","۶","۷","۸","۹","۰" },
        new[] { "ا","آ","ب","ڀ","ت","ٿ","ج","ڄ","ح","خ" },
        new[] { "د","ڌ","ذ","ر","ڙ","ز","س","ش","ص","ض" },
        new[] { "ط","ظ","ع","غ","ف","ق","ڪ","گ","ل","م" },
        new[] { "ن","ڻ","و","ه","ء","ي","ئ" },
        new[] { "َ","ِ","ُ","ّ","ْ" }
    };


                default:
                    return new List<string[]>
                    {
                        new string[] { "१","२","३","४","५","६","७","८","९","०" },
                        new string[] { "अ","आ","इ","ई","उ","ऊ","ए","ऐ","ओ","औ","अं","अः" },
                        new string[] { "क","ख","ग","घ","ङ","च","छ","ज","झ","ञ" },
                        new string[] { "ट","ठ","ड","ढ","ण","त","थ","द","ध","न" },
                        new string[] { "प","फ","ब","भ","म","य","र","ल","व" },
                        new string[] { "श","ष","स","ह","क्ष","त्र","ज्ञ" },
                        new string[] { "ा","ि","ी","ु","ू","े","ै","ो","ौ","ं","ः","्" }
                    };
            }
        }
        public void BuildKeyboard(KeyboardLanguage lang)
        {
            //MessageBox.Show("BuildKeyboard : " + lang.ToString());
            Text = lang.ToString();
            this.Controls.Clear();
            CurrentLanguage = lang;
            var keys = GetKeys(lang);
            this.BackColor = Color.White;
            TableLayoutPanel main = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 1
            };

            Label title = new Label
            {
                Text = lang.ToString(),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(8, 8)
            };

            TableLayoutPanel body = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = keys.Count + 1
            };

            for (int i = 0; i < keys.Count + 1; i++)
                body.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / (keys.Count + 1)));

            foreach (var row in keys)
            {
                TableLayoutPanel rowPanel = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = row.Length,
                    Margin = new Padding(2),
                    Padding = new Padding(0)
                };


                for (int i = 0; i < row.Length; i++)
                {
                    rowPanel.ColumnStyles.Add(
                        new ColumnStyle(SizeType.Percent, 100f / row.Length)
                    );

                    var btn = CreateStyledButton(row[i]);

                    btn.Dock = DockStyle.Fill;

                    rowPanel.Controls.Add(btn, i, 0);
                }

                body.Controls.Add(rowPanel);
            }

            TableLayoutPanel lastRow = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2
            };

            lastRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75));
            lastRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));

            Button btnSpace = CreateStyledButton("SPACE");
            btnSpace.Dock = DockStyle.Fill;
            btnSpace.Click += (s, e) => InsertText(" ");

            Button btnBack = CreateStyledButton("⌫");
            btnBack.Dock = DockStyle.Fill;
            btnBack.Click += BtnBack_Click;

            lastRow.Controls.Add(btnSpace, 0, 0);
            lastRow.Controls.Add(btnBack, 1, 0);

            body.Controls.Add(lastRow);


            main.Controls.Add(body, 0, 1);

            this.Controls.Clear();
            this.Controls.Add(main);

        }
        private Button CreateStyledButton(string text)
        {
            Button btn = new Button
            {
                Text = text,
                Width = 60,
                Height = 35,
                Margin = new Padding(2),
                Font = new Font("Nirmala UI", 12, FontStyle.Bold),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btn.FlatAppearance.BorderColor = Color.LightGray;
            btn.FlatAppearance.BorderSize = 1;

            btn.MouseEnter += (s, e) => btn.BackColor = Color.LightBlue;
            btn.MouseLeave += (s, e) => btn.BackColor = Color.White;

            if (text != "SPACE" && text != "⌫")
                btn.Click += (s, e) => InsertText(text);

            return btn;
        }
        private void InsertText(string text)
        {
            if (TargetTextBox == null || TargetTextBox.IsDisposed) return;

            int pos = TargetTextBox.SelectionStart;

            TargetTextBox.Text = TargetTextBox.Text.Insert(pos, text);

            TargetTextBox.SelectionStart = pos + text.Length;
            TargetTextBox.Focus();
        }
        private void BtnBack_Click(object sender, EventArgs e)
        {
            if (TargetTextBox == null) return;

            int pos = TargetTextBox.SelectionStart;

            if (pos > 0)
            {
                TargetTextBox.Text = TargetTextBox.Text.Remove(pos - 1, 1);
                TargetTextBox.SelectionStart = pos - 1;
            }
        }
        private void Header_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            dragStartPoint = e.Location;
        }
        private void Header_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Form form = this.FindForm();
                if (form != null)
                {
                    form.Location = new Point(
                        form.Left + e.X - dragStartPoint.X,
                        form.Top + e.Y - dragStartPoint.Y
                    );
                }
            }
        }
        private void Header_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void ucKeyboard_Load(object sender, EventArgs e)
        {

        }
    }
}
