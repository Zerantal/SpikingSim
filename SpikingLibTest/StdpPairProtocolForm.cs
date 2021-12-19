using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Diagnostics.Contracts;

using ZedGraph;
using SpikingLibrary;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace SpikingLibTest
{
    [ContractVerification(false)]
    [SuppressMessage("ReSharper", "UnusedVariable")]
    public partial class StdpPairProtocolForm : Form
    {
        // Pair protocol values
        private const double PairProtocolTimeInterval1 = -10;  // time between pre and post synaptic spikes (in ms)
        private const double PairProtocolTimeInterval2 = 10;
        private PointPairList _pairList1;
        private PointPairList _pairList2;                
        
        // quadruplet protocol parameters
        private const double TMin = -100; // in ms
        private const double TMax = 100; // in ms
        private PointPairList _quadrupletList1;
        private PointPairList _quadrupletList2;

        // pre-post-pre triplet parameters
        private readonly double[,] _prepostpreTriplets = {{5, -5}, {10, -10}, {15, -5}, {5, -15}};
        private PointPairList _prepostpreTripletList;

        // post-pre-post triplet parameters
        private readonly double[,] _postprepostTriplets = {{-5, 5}, {-10, 10}, {-5, 15}, {-15, 5}};
        private PointPairList _postprepostTripletList;
        
        public StdpPairProtocolForm()
        {
            InitializeComponent();

            SetupGraphs();

            PairProtocol.UpdateEvent += UpdateGraphs;
            QuadrupletProtocol.UpdateEvent += UpdateGraphs;
            PrePostPreTipletProtocol.UpdateEvent += UpdateGraphs;
            PostPrePostTripletProtocol.UpdateEvent += UpdateGraphs;

            
            PairProtocol p1 = new PairProtocol(_pairList1, PairProtocolTimeInterval1);
            PairProtocol p2 = new PairProtocol(_pairList2, PairProtocolTimeInterval2);
            QuadrupletProtocol q1 = new QuadrupletProtocol(_quadrupletList1, TMin, -5);
            QuadrupletProtocol q2 = new QuadrupletProtocol(_quadrupletList2, 5, TMax);
            PrePostPreTipletProtocol t1 = new PrePostPreTipletProtocol(_prepostpreTripletList, _prepostpreTriplets);
            PostPrePostTripletProtocol t2 = new PostPrePostTripletProtocol(_postprepostTripletList, _postprepostTriplets);
        }

        private void UpdateGraphs(object sender, EventArgs e)
        {
            zgc.AxisChange();
            zgc.Refresh();            
        }

        private void SetupGraphs()
        {
            MasterPane master = zgc.MasterPane;
            master.PaneList.Clear();
            master.Margin.All = 10;
         
            // setup pair pane
            GraphPane pairPane = new GraphPane();
            pairPane.XAxis.Title.Text = "ρ [Hz]";
            pairPane.XAxis.Scale.Min = 0;
            pairPane.XAxis.Scale.Max = PairProtocol.RhoMax;
            pairPane.YAxis.Title.Text = "Δw";
            _pairList1 = new PointPairList();
            _pairList2 = new PointPairList();
            LineItem c1 = pairPane.AddCurve("Δt = " + PairProtocolTimeInterval1, _pairList1, Color.Blue, SymbolType.None);
            c1.Symbol.IsVisible = false;
            c1.Line.Style = DashStyle.Dash;
            c1.Line.Width = 2;
            LineItem c2 = pairPane.AddCurve("Δt = " + PairProtocolTimeInterval2, _pairList2, Color.Blue, SymbolType.None);
            c2.Symbol.IsVisible = false;
            c2.Line.Width = 2;
            
            // setup quadruplet pair pane
            GraphPane quadrupletPane = new GraphPane();
            quadrupletPane.XAxis.Title.Text = "T [ms]";
            quadrupletPane.XAxis.Scale.Min = TMin;
            quadrupletPane.XAxis.Scale.Max = TMax;
            quadrupletPane.YAxis.Title.Text = "Δw";
            _quadrupletList1 = new PointPairList();
            _quadrupletList2 = new PointPairList();
            c1 = quadrupletPane.AddCurve("", _quadrupletList1, Color.Blue, SymbolType.None);
            c1.Symbol.IsVisible = false;
            c1.Line.Width = 2;
            c2 = quadrupletPane.AddCurve("", _quadrupletList2, Color.Blue, SymbolType.None);
            c2.Symbol.IsVisible = false;
            c2.Line.Width = 2;
            
            // setup triplet pane (pre-post-pre)
            GraphPane tripletPane1 = new GraphPane();
            tripletPane1.XAxis.Title.Text = "(Δt1, Δt2) [ms]";
            tripletPane1.YAxis.Title.Text = "Δw";
            _prepostpreTripletList = new PointPairList();
            tripletPane1.AddBar("", _prepostpreTripletList, Color.Blue);
            string[] str1 = new string[_prepostpreTriplets.GetLength(0)];
            for (int i = 0; i < str1.Length; i++)
                str1[i] = "(" + _prepostpreTriplets[i,0] + "," + _prepostpreTriplets[i,1] + ")";
            tripletPane1.XAxis.Scale.TextLabels = str1;
            tripletPane1.XAxis.Type = AxisType.Text;

            // setup triple pane (post-pre-post)
            GraphPane tripletPane2 = new GraphPane();
            tripletPane2.XAxis.Title.Text = "(Δt1, Δt2) [ms]";
            tripletPane2.YAxis.Title.Text = "Δw";
            _postprepostTripletList = new PointPairList();
            tripletPane2.AddBar("", _postprepostTripletList, Color.Blue);
            string[] str2 = new string[_postprepostTriplets.GetLength(0)];
            for (int i = 0; i < str2.Length; i++)
                str2[i] = "(" + _postprepostTriplets[i, 0] + "," + _postprepostTriplets[i, 1] + ")";
            tripletPane2.XAxis.Scale.TextLabels = str2;
            tripletPane2.XAxis.Type = AxisType.Text;
            
            master.Add( pairPane );
            master.Add( quadrupletPane );
            master.Add( tripletPane1);
            master.Add( tripletPane2);            

            zgc.AxisChange();

            using (Graphics g = CreateGraphics())
            {
                master.SetLayout(g, PaneLayout.SquareColPreferred);
            }            
        }

        private void StdpTripletTestForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SpikingNetEngine.Stop();
        }
    }
}
