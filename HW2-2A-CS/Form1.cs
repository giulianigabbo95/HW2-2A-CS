using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;

namespace HW2_2A_VB
{
    public partial class Finestra
    {
        public Finestra()
        {
            InitializeComponent();
        }

        public Random R = new Random();
        private List<HW2_2A_VB.Persona> Persone = new List<HW2_2A_VB.Persona>();

        private void GeneraPersone_Click(object sender, EventArgs e)
        {
            int PersoneCount = 100;
            double AltezzaMin = 100d;
            double AltezzaMax = 200d;
            this.SpazioStampa.AppendText("DATASET: Età e Altezza delle persone" + Environment.NewLine + Environment.NewLine);
            this.SpazioStampa.AppendText("Totale: " + Environment.NewLine + Environment.NewLine);
            for (int i = 1, loopTo = PersoneCount; i <= loopTo; i++)
            {
                double RandomAltezza = Math.Round(AltezzaMin + (AltezzaMax - AltezzaMin) * R.NextDouble(), 2);
                int RandomEta = R.Next(10, 100);
                var p = new HW2_2A_VB.Persona();
                p.Altezza = RandomAltezza;
                p.Eta = RandomEta;
                Persone.Add(p);
                this.SpazioStampa.AppendText(("Persona" + i).PadRight(10) + RandomAltezza.ToString().PadRight(10) + RandomEta + Environment.NewLine);
            }
        }

        private void MediaNormale_Click(object sender, EventArgs e)
        {
            int Somma = 0;
            foreach (HW2_2A_VB.Persona p in Persone)
                Somma += p.Eta;
            double Media = Somma / (double)Persone.Count;
            this.SpazioStampa.AppendText(" " + Environment.NewLine + Environment.NewLine);
            this.SpazioStampa.AppendText("Media Normale Eta: " + Media + Environment.NewLine);
        }

        private void MediaOnline_Click(object sender, EventArgs e)
        {
            double MediaAttuale = 0d;
            int PersonaAttuale = 0;
            foreach (HW2_2A_VB.Persona p in Persone)
            {
                PersonaAttuale += 1;
                MediaAttuale = MediaAttuale + ((double)p.Eta - MediaAttuale);
                this.SpazioStampa.AppendText("Media Online Eta: " + MediaAttuale + Environment.NewLine);
            }

            this.SpazioStampa.AppendText(" " + Environment.NewLine + Environment.NewLine);
            this.SpazioStampa.AppendText("Media Online Definitiva Eta: " + MediaAttuale + Environment.NewLine);
        }

        private void Frequenza_Click(object sender, EventArgs e)
        {
            var FrequenzaDistribuzione = new SortedDictionary<int, HW2_2A_VB.FrequenzaEta>();
            foreach (HW2_2A_VB.Persona p in Persone)
            {
                if (FrequenzaDistribuzione.ContainsKey(p.Eta))
                {
                    FrequenzaDistribuzione[p.Eta].Count += 1;
                }
                else
                {
                    FrequenzaDistribuzione.Add(p.Eta, new HW2_2A_VB.FrequenzaEta());
                }

                this.SpazioStampa.AppendText(" " + Environment.NewLine + Environment.NewLine);
                this.SpazioStampa.AppendText("Distribuzione Eta: " + Environment.NewLine + Environment.NewLine);
                this.SpazioStampa.AppendText("Eta".PadRight(7) + "Numero".PadRight(7) + "Freq".PadRight(7) + "PErc".PadRight(7) + Environment.NewLine);
                foreach (KeyValuePair<int, HW2_2A_VB.FrequenzaEta> kvp in FrequenzaDistribuzione)
                    this.SpazioStampa.AppendText(kvp.Key.ToString().PadRight(7) + kvp.Value.Count.ToString.PadRight(7) + kvp.Value.FrequenzaRelativa.ToString("0.##").PadRight(7) + kvp.Value.Count.ToString.PadRight(7) + " % " + Environment.NewLine);
            }

            double GrandezzaIntervallo = 10d;
            double PuntoFine = 170d;
            var Intervallo_0 = new HW2_2A_VB.Interval();
            Intervallo_0.LowerEnd = PuntoFine;
            Intervallo_0.UpperEnd = Intervallo_0.LowerEnd + GrandezzaIntervallo;
            var ListaIntervalli = new List<HW2_2A_VB.Interval>();
            ListaIntervalli.Add(Intervallo_0);
            foreach (HW2_2A_VB.Persona p in Persone)
            {
                bool PersoneAllocate = false;
                foreach (var Interval in ListaIntervalli)
                {
                    if (p.Altezza > Interval.LowerEnd && p.Altezza <= Interval.UpperEnd)
                    {
                        Interval.Count += 1;
                        PersoneAllocate = true;
                        break;
                    }
                }

                if (PersoneAllocate == true)
                    continue;
                if (p.Altezza <= ListaIntervalli[0].LowerEnd)
                {
                    do
                    {
                        var NewLeftInterval = new HW2_2A_VB.Interval();
                        NewLeftInterval.UpperEnd = ListaIntervalli[0].LowerEnd;
                        NewLeftInterval.LowerEnd = NewLeftInterval.UpperEnd - ListaIntervalliSize;
                        ListaIntervalli.Insert(0, NewLeftInterval);
                        if (p.Altezza > NewLeftInterval.LowerEnd && p.Altezza <= NewLeftInterval.UpperEnd)
                        {
                            NewLeftInterval.Count += 1;
                            break;
                        }
                    }
                    while (true);
                }
                else if (p.Altezza > ListaIntervalli[ListaIntervalli.Count - 1].UpperEnd)
                {
                    do
                    {
                        var NewRightInterval = new HW2_2A_VB.Interval();
                        NewRightInterval.LowerEnd = ListaIntervalli[ListaIntervalli.Count - 1].UpperEnd;
                        NewRightInterval.UpperEnd = NewRightInterval.LowerEnd + IntervalSize;
                        ListaIntervalli.Add(NewRightInterval);
                        if (p.Altezza > NewRightInterval.LowerEnd && p.Altezza <= NewRightInterval.UpperEnd)
                        {
                            NewRightInterval.Count += 1;
                            break;
                        }
                    }
                    while (true);
                }
                else
                {
                    throw new Exception("Not Expected Occurence");
                }
            }

            this.SpazioStampa.AppendText(" " + Environment.NewLine + Environment.NewLine);
            this.SpazioStampa.AppendText("Distribuzione Altezza: " + Environment.NewLine + Environment.NewLine);
            this.SpazioStampa.AppendText("Altezza".PadRight(7) + "Numero" + Environment.NewLine);
            var SommaNumero = default(int);
            foreach (HW2_2A_VB.Interval Interval in ListaIntervalli)
            {
                SommaNumero += Interval.Count;
                this.SpazioStampa.AppendText(("(" + Interval.LowerEnd + " - " + Interval.UpperEnd + ")").PadRight(7) + Interval.Count + Constants.vbCrLf);
            }

            this.SpazioStampa.AppendText(Constants.vbCrLf + Constants.vbCrLf + "Somma Numero: " + SommaNumero + Constants.vbCrLf);
        }
    }
}