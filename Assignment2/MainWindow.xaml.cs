using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Variables to store visibility state of tables
        static bool NOThidden = true, customtable = false;
        //TextBlock variables for Custom Truth Table
        static TextBlock F, operands;
        static TextBlock[] Func, TruthValues;        
        static int NoOfRows = 0;
        public MainWindow()
        {
            InitializeComponent();            
            SetVal(0);
        }
        private char BoolToChar(bool b)
        {
            return b ? 'T' : 'F';
        }
        private bool IntToBool(int x)
        {
            return x == 0 ? false : true;
        }
        private void SetResultType(int TrueCount, int n)    
        {
            if (TrueCount == 0)
            {
                ResultType.Text = "Type : Contradiction";
            }
            else if (TrueCount == n)
            {
                ResultType.Text = "Type : Tautology";
            }
            else
            {
                ResultType.Text = "Type : Contingency";
            }
        }
        private char GetVal(int v1, int v2, int connective)
        {
            bool var1 = IntToBool(v1), var2 = IntToBool(v2);
            switch (connective)
            {
                case 0: return BoolToChar(var1 & var2);
                case 1: return BoolToChar(var1 | var2);
                case 2: return BoolToChar(var1 ^ var2);
                case 3: return BoolToChar(!(var1 & var2));
                case 4: return BoolToChar(!(var1 | var2));
                case 5: return BoolToChar(!var1 | var2);
                case 6: return BoolToChar(!(var1 ^ var2));

                default: return 'X';
            }
        }
        private void SetSymbol(int connective)
        {
            switch (connective)
            {
                case 0: ConnectiveSymbol.Text = "A . B"; break;
                case 1: ConnectiveSymbol.Text = "A + B"; break;
                case 2: ConnectiveSymbol.Text = "A XOR B"; break;
                case 3: ConnectiveSymbol.Text = "A NAND B"; break;
                case 4: ConnectiveSymbol.Text = "A NOR B"; break;
                case 5: ConnectiveSymbol.Text = "A -> B"; break;
                case 6: ConnectiveSymbol.Text = "A <-> B"; break;
                default: break;

            }
        }
        private void SetVal(int connective)
        {
            SetSymbol(connective);
            TextBlock[] Val = { Val0, Val1, Val2, Val3 };
            char temp;
            int TrueCount = 0;
            for (int i = 1, count = 0; i >= 0; i--)
            {
                for (int j = 1; j >= 0; j--)
                {
                    temp = GetVal(i, j, connective);
                    if(temp=='T')
                    {
                        Val[count].Foreground = Brushes.LimeGreen;
                        TrueCount++;
                    }
                    else
                    {
                        Val[count].Foreground = Brushes.Red;
                    }
                    Val[count++].Text = (GetVal(i, j, connective)).ToString();
                }
            }
            Foreground = Brushes.Black;
            SetResultType(TrueCount, 4);
        }
        private void NOTUnhide()    //Hides Current Table and unhides NOT Truth Table
        {
            TextBlock[] TruthTable = { Table0, Table1, Table2, Table3, Table4, ConnectiveSymbol, Val0, Val1, Val2, Val3 };
            TextBlock[] NotTable = { NOTTable0, NOTTable1, NOTTable2, NOTTable3, NOTTable4, NOTTable5 };
            foreach (TextBlock t in TruthTable)
            {
                t.Visibility = Visibility.Collapsed;
            }
            NOTTable4.Foreground = Brushes.Red;
            NOTTable4.Text = BoolToChar(!true).ToString();
            NOTTable5.Foreground = Brushes.LimeGreen;
            NOTTable5.Text = BoolToChar(!false).ToString();
            Foreground = Brushes.Black;
            ResultType.Text = "Type : Contingency";
            foreach (TextBlock t in NotTable)
            {
                t.Visibility = Visibility.Visible;
            }
            NOThidden = false;
        }
        private void NOThide()  //Hides NOT Truth Table and unhides original Truth Table
        {
            TextBlock[] TruthTable = { Table0, Table1, Table2, Table3, Table4, ConnectiveSymbol, Val0, Val1, Val2, Val3 };
            TextBlock[] NotTable = { NOTTable0, NOTTable1, NOTTable2, NOTTable3, NOTTable4, NOTTable5 };
            foreach (TextBlock t in TruthTable)
            {
                t.Visibility = Visibility.Visible;
            }
            foreach (TextBlock t in NotTable)
            {
                t.Visibility = Visibility.Collapsed;
            }
            NOThidden = true;
        }
        private void HideTable()    //Hides all Tables for Custom Table
        {
            TextBlock[] Table = { Table0, Table1, Table2, Table3, Table4, ConnectiveSymbol, Val0, Val1, Val2, Val3, NOTTable0, NOTTable1, NOTTable2, NOTTable3, NOTTable4, NOTTable5 };
            foreach (TextBlock t in Table)
            {
                t.Visibility = Visibility.Collapsed;
            }
            NOThidden = true;
        }
        private void UnhideTable()  //Unhides Table
        {
            TextBlock[] TruthTable = { Table0, Table1, Table2, Table3, Table4, ConnectiveSymbol, Val0, Val1, Val2, Val3 };
            foreach (TextBlock t in TruthTable)
            {
                t.Visibility = Visibility.Visible;
            }
            NOThidden = true;
        }
        private void DeleteCustomTable()    //Deletes Custom Table
        {
            MainGrid.Children.Remove(F);
            MainGrid.Children.Remove(operands);
            for (int i = 0; i < NoOfRows; i++)
            {
                MainGrid.Children.Remove(Func[i]);
                MainGrid.Children.Remove(TruthValues[i]);
            }
            customtable = false;
        }
        
        private void ShowCustomTable(Boolean B) //Creates Custom Table
        {
            //Declaration
            int TrueCount=0,n,i,j,temp,x=B.opnds.Length,fsize=17;
            fsize -= (B.opnds.Length < 10 ? B.opnds.Length : 10);

            n = NoOfRows = Convert.ToInt32(Math.Pow(2, B.opnds.Length));
            bool[] result = new bool[n];
            bool[] b = new bool[x];
            //Creating TextBlock for Truth Table Operands
            Thickness T = new Thickness(170, 60, 0, 0);
            operands = new TextBlock();
            F = new TextBlock();
            TruthValues = new TextBlock[n];
            Func = new TextBlock[n];
            customtable = true;
            F.Text = "F";
            F.Width = 30;
            operands.Text = "";
            F.TextWrapping = operands.TextWrapping = TextWrapping.Wrap;
            F.FontSize = operands.FontSize = fsize;

            
            F.FontWeight = operands.FontWeight = FontWeights.Bold;
            F.HorizontalAlignment = operands.HorizontalAlignment = HorizontalAlignment.Center;
            F.VerticalAlignment = operands.VerticalAlignment = VerticalAlignment.Top;
            operands.Margin = T;
            T.Left = 355;
            F.Margin = T;
            for (i = 0; i < B.opnds.Length; i++)
            {
                operands.Text += B.opnds[i].ToString() + "   ";
            }
            //Creating Text Blocks for Truth Values
            T.Top = 70;
            T.Left = 170;

            //Calculating Truth Values and Function
            for (i = n - 1; i >= 0; i--)
            {
                temp = i;
                for (j = x - 1; j >= 0; j--)
                {
                    if (Math.Pow(2, j) <= temp)
                    {
                        b[x - 1 - j] = true;
                        temp -= Convert.ToInt32(Math.Pow(2, j));
                    }
                    else
                    {
                        b[x - 1 - j] = false;
                    }
                }
                TruthValues[i] = new TextBlock();
                TruthValues[i].Text = "";                
                TruthValues[i].Height = 20;
                TruthValues[i].FontSize = fsize;
                TruthValues[i].FontWeight = FontWeights.Bold;
                TruthValues[i].HorizontalAlignment = HorizontalAlignment.Center;
                TruthValues[i].VerticalAlignment = VerticalAlignment.Top;
                TruthValues[i].TextWrapping = TextWrapping.Wrap;
                T.Top += 20;
                TruthValues[i].Margin = T;
                for (j = 0; j < x; j++)
                {
                    TruthValues[i].Text += (b[j] ? "T   " : "F   ");
                }                
            }
            if (B.GetResult())
            {
                HideTable();
                //Creating Text Blocks for Result                    
                T.Left = 355;
                T.Top = 70;
                MainGrid.Children.Add(operands);
                MainGrid.Children.Add(F);
                for (i = 0; i < n; i++)
                {
                    T.Top += 20;
                    Func[i] = new TextBlock();
                    if (B.result[i])
                    {
                        Func[i].Foreground = Brushes.LimeGreen;
                        Func[i].Text = "T";
                        TrueCount++;
                    }
                    else
                    {
                        Func[i].Foreground = Brushes.Red;
                        Func[i].Text = "F";
                    }
                    Func[i].Width = 30;
                    Func[i].Height = 20;
                    Func[i].FontSize = fsize;
                    Func[i].HorizontalAlignment = HorizontalAlignment.Center;
                    Func[i].VerticalAlignment = VerticalAlignment.Top;
                    Func[i].Margin = T;
                    Func[i].TextWrapping = TextWrapping.Wrap;
                    Func[i].FontWeight = FontWeights.Bold;
                    MainGrid.Children.Add(TruthValues[i]);
                    MainGrid.Children.Add(Func[i]);
                }
                Foreground = Brushes.Black;
                SetResultType(TrueCount, n);
            }
            else
            {
                MessageBox.Show("Invalid Expression");
            }
        }
        private void SelectButton_Click(object sender, RoutedEventArgs e)//Function for Selection Button click
        {
            if (EqualButton.IsChecked == true)
            {
                string S1, S2;
                S1 = InputBox.Text;
                S2 = InputBox2.Text;
                int x = Boolean.IsEquivalent(S1, S2);                
                if (x == 0)
                {                    
                    EquivalenceBlock.Visibility = Visibility.Visible;
                    EquivalenceBlock.Foreground = Brushes.Red;
                    EquivalenceBlock.Text = "Expressions are not Logicaly Equivalent";
                }
                else if (x == 1)
                {
                    EquivalenceBlock.Visibility = Visibility.Visible;
                    EquivalenceBlock.Foreground = Brushes.LimeGreen;
                    EquivalenceBlock.Text = "Expressions are Logicaly Equivalent";
                }
                else if (x == -1)
                {
                    EquivalenceBlock.Visibility = Visibility.Collapsed;
                    MessageBox.Show("Invalid Expression 1");
                }
                else
                {
                    EquivalenceBlock.Visibility = Visibility.Collapsed;
                    MessageBox.Show("Invalid Expression 2");
                }
            }
            else if (CustomButton.IsChecked == true)
            {
                Boolean B = new Boolean();
                B.exp = InputBox.Text;
                if (customtable)
                {
                    DeleteCustomTable();
                }
                if (B.ConvertExp())
                {
                    ShowCustomTable(B);
                }
                else
                {
                    MessageBox.Show("Invalid Expression");
                }
            }
            else
            {
                if (customtable)
                {
                    DeleteCustomTable();
                    UnhideTable();
                }
            } 
            if(SimplifyButton.IsChecked==true)
            {
               if(!Simplify())
               {
                    MessageBox.Show("Invalid Expression");
               }
            }
            if (NOTButton.IsChecked == true)
            {
                if (NOThidden)
                {
                    NOTUnhide();
                }
            }
            else if (!NOThidden)
            {
                NOThide();
            }
            if (ANDButton.IsChecked == true)
            {
                SetVal(0);
            }
            else if (ORButton.IsChecked == true)
            {
                SetVal(1);
            }
            else if (XORButton.IsChecked == true)
            {
                SetVal(2);
            }
            else if (NANDButton.IsChecked == true)
            {
                SetVal(3);
            }
            else if (NORButton.IsChecked == true)
            {
                SetVal(4);
            }
            else if (IMPButton.IsChecked == true)
            {
                SetVal(5);
            }
            else if (BICONButton.IsChecked == true)
            {
                SetVal(6);
            }
        }

        private bool Simplify() 
        {
            Boolean B = new Boolean();
            B.exp = InputBox.Text;           
            
            if(B.ConvertExp())
            {
                int x = B.opnds.Length,n=Convert.ToInt32(Math.Pow(2,x));
                bool[] result = new bool[n];
                bool[] b = new bool[x];
                if (B.GetResult())
                {
                    string temp = B.Simplify(B.result, x);
                    if (temp == "T")
                    {
                        SimplifiedOutput.Foreground = Brushes.LimeGreen;
                    }
                    else if (temp == "F")
                    {
                        SimplifiedOutput.Foreground = Brushes.Red;
                    }  
                    else
                    {
                        SimplifiedOutput.Foreground = Brushes.Black;
                    }
                    SimplifiedOutput.Text = temp;
                    
                    return true;
                }
                return false;
            }
            return false;
        }

        private void Custom_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = InputBlock.Visibility = InstructionsBlock.Visibility = Visibility.Visible;
            InputBox2.Visibility = InputBlock2.Visibility = SimplifiedOutput.Visibility = EquivalenceBlock.Visibility = Visibility.Collapsed;
            SelectButton.Content = "View Truth Table";            
        }
        private void Custom_Click2(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = InputBlock.Visibility = InstructionsBlock.Visibility = SimplifiedOutput.Visibility  = Visibility.Visible;
            InputBox2.Visibility = InputBlock2.Visibility = EquivalenceBlock.Visibility = Visibility.Collapsed;
            SelectButton.Content = "Simplify Expression (POS)";
        }
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = InputBlock.Visibility = InstructionsBlock.Visibility = InputBox2.Visibility = InputBlock2.Visibility = SimplifiedOutput.Visibility = EquivalenceBlock.Visibility = Visibility.Collapsed;

            SelectButton.Content = "View Truth Table";
        }
        private void Equal_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = InputBlock.Visibility = InstructionsBlock.Visibility = InputBox2.Visibility = InputBlock2.Visibility = Visibility.Visible;
            SimplifiedOutput.Visibility = Visibility.Collapsed;
            SelectButton.Content = "Check Equivalence";
        }
    }
    //Class to store,evaluate,convert,simplify and check logical equivalence of expressions
    class Boolean
    {
        public string exp, optr = "", opnd = "", opnds = "";
        public bool[] result;

        bool isoperand(char ch)
        {
            return char.IsLetter(ch);
        }
        bool isoptr(char ch)
        {
            switch (ch)
            {
                case '!':
                case '~':
                case '+':
                case '|':
                case '&':
                case '.':
                case ')':
                case '<':
                case '-':
                case '^': return true;
                default: return false;
            }
        }
        bool isnot(char ch)
        {
            if (ch == '!' || ch == '~')
            {
                return true;
            }
            return false;
        }
        string RemoveSpaces(string exp)
        {
            for (int i = 0; i < exp.Length; i++)
            {
                if (exp[i] == ' ')
                {
                    exp = exp.Remove(i, 1);
                }
            }
            return exp;
        }

        string RemoveNot(string exp)
        {
            for (int i = 0, count = 0; i < exp.Length; i++)
            {
                if (isnot(exp[i]))
                {
                    count = 1;
                    i++;
                    while (isnot(exp[i]))
                    {
                        count++;
                        if (count % 2 == 0)
                        {
                            count = 0;
                            exp = exp.Remove(i - 1, 2);
                        }
                    }
                }
            }
            return exp;
        }

        void SearchOperand(char ch)
        {
            if (ch == 'T' || ch == 'F')
            {
                return;
            }
            foreach (char c in opnds)
            {
                if (c == ch)
                {
                    return;
                }
            }
            opnds += ch;
        }
        public bool ConvertExp()    //Converts expression into suitable form and checks for validity of expression
        {
            exp = "(" + exp + ")";
            int i, CountL = 0, CountR = 0;
            exp = RemoveSpaces(exp);
            exp = RemoveNot(exp);            
            for (i = 0; i < exp.Length; i++)
            {
                if (isnot(exp[i]))
                {
                    if (isoperand(exp[i + 1]) || exp[i + 1] == '(')
                    {
                        exp = exp.Insert(i, "T");
                        i++;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (exp[i] == '(')
                {
                    CountL++;
                }
                else if (exp[i] == ')')
                {
                    CountR++;
                }
                else if (exp[i] == '-')
                {
                    if (exp.Length - i < 3)
                    {
                        return false;
                    }
                    if (exp[i + 1] == '>' && (isoperand(exp[i + 2]) || isnot(exp[i + 2])))
                    {
                        exp = exp.Remove(i + 1, 1);
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (exp[i] == '<')
                {
                    if (exp.Length - i < 4)
                    {
                        return false;
                    }
                    if (exp[i + 1] == '-' && exp[i + 2] == '>' && (isoperand(exp[i + 3]) || isnot(exp[i + 3])))
                    {
                        exp = exp.Remove(i + 1, 2);
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (isoperand(exp[i]))
                {
                    SearchOperand(exp[i]);
                }
            }
            if (CountL != CountR)
            {
                return false;
            }
            return true;
        }
        int GetIndex(char ch)
        {
            if (ch == 'T') return -1;
            if (ch == 'F') return -2;
            for (int i = 0; i < opnds.Length; i++)
            {
                if (opnds[i] == ch)
                {
                    return i;
                }
            }
            return -3;
        }
        void PerformOperation(bool[] b)
        {
            char v1 = opnd[opnd.Length - 2], v2 = opnd[opnd.Length - 1];
            bool b1 = true, b2 = true;
            opnd = opnd.Remove(opnd.Length - 2, 2);
            int x = GetIndex(v1);
            if (x == -1) b1 = true;
            else if (x == -2) b1 = false;            
            else b1 = b[x];
            x = GetIndex(v2);
            if (x == -1) b2 = true;
            else if (x == -2) b2 = false;            
            else b2 = b[x];
            switch (optr[optr.Length - 1])
            {
                case '~':
                case '!': b1 = !b2; break;
                case '&':
                case '.': b1 = b1 & b2; break;
                case '|':
                case '+': b1 = b1 | b2; break;
                case '^': b1 = b1 ^ b2; break;
                case '<': b1 = !(b1 ^ b2); break;
                case '-': b1 = !b1 | b2; break;
                default: break;
            }
            if (b1) opnd += 'T';
            else opnd += 'F';
            optr = optr.Remove(optr.Length - 1);
        }
        int getprecedence(char connective)
        {
            switch (connective)
            {
                case '~':
                case '!': return 5;
                case '&':
                case '.': return 4;
                case '|':
                case '+': return 2;
                case '^': return 3;
                case '<': return 0;
                case '-': return 1;
                default: return -1;
            }
        }
        public int EvaluateBool(bool[] b)   //Function for evaluating expression
        {
            int i;
            if(exp.Length==3)
            {
                int x = GetIndex(exp[1]);
                if (x == -1) return 1;
                else if (x == -2) return  0;
                else return b[0]?1:0;
            }
            for (i = 0; i < exp.Length; i++)
            {
                if (isoperand(exp[i]))
                {
                    if (isoptr(exp[i + 1]))
                    {
                        opnd += exp[i];
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (exp[i] == '(')
                {
                    if (isoperand(exp[i + 1]) || exp[i + 1] == '(')
                    {
                        optr += '(';
                    }
                    else
                    {
                        return -2;
                    }
                }
                else if (exp[i] == ')')
                {
                    if (!(isoperand(exp[i - 1]) || exp[i - 1] == ')'))
                    {                        
                        return -3;
                    }
                    while (optr[optr.Length - 1] != '(')
                    {
                        PerformOperation(b);                        
                    }
                    optr = optr.Remove(optr.Length - 1);
                }
                else
                {
                    if (!(exp[i + 1] == ')' || isoperand(exp[i + 1]) || exp[i + 1] == '('))
                    {
                        return -4;
                    }
                    while (getprecedence(optr[optr.Length - 1]) >= getprecedence(exp[i]))
                    {
                        PerformOperation(b);
                    }
                    optr += exp[i];
                }                
            }
            if (opnd[0] == 'T') return 1;
            return 0;
        }
        public bool GetResult() //stores all possible truth values in result[]
        {
            int x = opnds.Length, k = 0, n = Convert.ToInt32(Math.Pow(2, x)), res, i, temp;
            result = new bool[n];
            bool[] b = new bool[x];
            for (i = n - 1; i >= 0; i--)
            {
                temp = i;
                for (int j = x - 1; j >= 0; j--)
                {
                    if (Math.Pow(2, j) <= temp)
                    {
                        b[x - 1 - j] = true;
                        temp -= Convert.ToInt32(Math.Pow(2, j));
                    }
                    else
                    {
                        b[x - 1 - j] = false;
                    }
                }

                res = EvaluateBool(b);
                if (res == 0) result[k++] = false;
                else if (res == 1) result[k++] = true;
                else
                {
                    return false;
                }
                opnd = "";
            }
            if (i < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Code for Expression Simplification
        string ToBoolString(int a, int x)
        {
            string result = "";
            for (int i = 0; i < x; i++)
            {
                result = (a % 2).ToString() + result;
                a /= 2;
            }
            return result;
        }
        bool IsGreyCode(string a, string b)
        {
            int count = 0;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) count++;
            }
            return count == 1 ? true : false;
        }
        string RemoveBit(string a, string b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                {
                    a = a.Remove(i, 1);
                    a = a.Insert(i, "-");
                    break;
                }
            }
            return a;
        }
        void Sort(ref int[] a, int n)
        {
            int[] onecount = new int[n];
            int temp, i, j, min;
            for (i = 0; i < n; i++)
            {
                onecount[i] = 0;
                temp = a[i];
                while (temp != 0)
                {
                    if (temp % 2 != 0) onecount[i]++;
                    temp /= 2;
                }
            }
            int pos;
            for (i = 0; i < n; i++)
            {
                min = onecount[i];
                pos = i;
                for (j = i + 1; j < n; j++)
                {
                    if (onecount[j] < min)
                    {
                        min = onecount[j];
                        pos = j;
                    }
                }
                temp = a[i];
                a[i] = a[pos];
                a[pos] = temp;
                temp = onecount[i];
                onecount[i] = onecount[pos];
                onecount[pos] = temp;
            }
        }
        bool IsMinterm(List<string> minterms, string term)
        {
            foreach (string s in minterms)
            {
                if (term == s) return true;
            }
            return false;
        }
        public string Simplify(bool[] r, int x)
        {
            string final = "";
            
            int i, j, k = 0, n = Convert.ToInt32(Math.Pow(2, x)), p;
            int[] a = new int[n];
            for (i = 0; i < n; i++)
            {
                if (r[i])
                {
                    a[k++] = i;
                }
            }
            if(k==0)
            {
                return "F";
            }
            else if(k==n)
            {
                return "T";
            }
            if (x == 1)
            {
                return opnds;
            }
            Sort(ref a, k);
            string[] s = new string[k];
            for (i = 0; i < k; i++)
            {
                s[i] = ToBoolString(a[i], x);
            }
            List<string> minterms = new List<string>();
            List<string> Finalminterms = new List<string>();
            string tempS;
            for (i = 0; i < k; i++)
            {
                minterms.Add(s[i]);
            }

            for (i = 0; i < x; i++)
            {
                bool[] IsChecked = new bool[minterms.Count];
                for (j = 0; j < minterms.Count; IsChecked[j++] = false) ;
                k = minterms.Count;
                for (j = 0; j < k - 1; j++)
                {
                    for (int l = j + 1; l < k; l++)
                    {
                        if (IsGreyCode(minterms[j], minterms[l]))
                        {
                            tempS = RemoveBit(minterms[j], minterms[l]);
                            IsChecked[j] = IsChecked[l] = true;
                            if (!IsMinterm(minterms, tempS))
                            {
                                minterms.Add(tempS);
                            }
                        }
                    }
                }
                p = 0;
                for (j = 0; j < k; j++)
                {
                    if (IsChecked[j])
                    {
                        minterms.Remove(minterms[p]);

                    }
                    else if (!IsMinterm(Finalminterms, minterms[p]))
                    {
                        Finalminterms.Add(minterms[p]);
                        p++;
                    }
                }
            }
            for (j = 0; j < Finalminterms.Count; j++)
            {
                string S = Finalminterms[j];                
                bool dot = false;
                for (i = 0; i < S.Length; i++)
                {
                    if (S[i] == '0')
                    {
                        if (dot)
                        {
                            final += ".";
                        }
                        final += opnds[i];
                        dot = true;
                    }
                    else if (S[i] == '1')
                    {
                        if (dot)
                        {
                            final += ".";
                        }
                        final += "!" + opnds[i];
                        dot = true;
                    }
                }
                if (j < Finalminterms.Count - 1)
                {
                    final += "+";
                }

            }
            return final;
        }      
        
        static public int IsEquivalent(string exp1, string exp2) //Function to check logical equivalence of two expressions
        {
            Boolean B1 = new Boolean();
            Boolean B2 = new Boolean();
            B1.exp = exp1;
            B2.exp = exp2;
            if(!B1.ConvertExp())
            {
                return -1;
            }
            if (!B2.ConvertExp())
            {
                return -2;
            }            
            if(!B1.GetResult())
            {
                return -1;
            }
            if (!B2.GetResult())
            {
                return -2;
            }            
            B1.exp = B1.Simplify(B1.result, B1.opnds.Length);
            B2.exp = B2.Simplify(B2.result, B2.opnds.Length);
            if (B1.exp == B2.exp)
            {
                return 1;
            }
            B1.ConvertExp();
            B2.ConvertExp();
            if (B1.opnds!=B2.opnds)
            {                
                return 0;
            }
            int n = Convert.ToInt32(Math.Pow(2, B1.opnds.Length));
            
            for (int i = 0; i < n; i++)
            {
                if (B1.result[i] != B2.result[i])
                {
                    return 0;
                }
            }
            return 1;                        
        }
    }
}
