using System;
using System.Windows.Forms;
using System.Threading;

class PilhaLista<Dado> : ListaSimples<Dado>, IStack<Dado>, IComparable<PilhaLista<Dado>>
                        where Dado : IComparable<Dado>

{

    public Dado Desempilhar()
    {
        if (EstaVazia)
            throw new PilhaVaziaException("pilha vazia!");

        Dado valor = default(Dado);
        NoLista<Dado> pri = base.Primeiro;
        NoLista<Dado> ant = null;
        base.RemoverNo(ref pri, ref ant);
        if (base.Primeiro != null)
            valor = base.Primeiro.Info;

        return valor;
    }

    public void Empilhar(Dado elemento)
    {
        base.InserirAntesDoInicio
            (
                new NoLista<Dado>(elemento, null)
            );
    }

    new public bool EstaVazia
    {
        get => base.EstaVazia;
    }

    public Dado OTopo()
    {
        if (EstaVazia)
            throw new PilhaVaziaException("pilha vazia!");

        return base.Primeiro.Info;
    }

    public int Tamanho { get => base.QuantosNos; }


    public void Exibir(DataGridView dgv)
    {
        dgv.ColumnCount = Tamanho;
        dgv.RowCount = 2;
        for (int j = 0; j < dgv.ColumnCount; j++)
            dgv[j, 0].Value = "";

        var auxiliar = new PilhaLista<Dado>();
        int i = 0;
        while (!this.EstaVazia)
        {
            dgv[i++, 0].Value = this.OTopo();
            Thread.Sleep(300);
            Application.DoEvents();
            auxiliar.Empilhar(this.Desempilhar());
        }

        while (!auxiliar.EstaVazia)
            this.Empilhar(auxiliar.Desempilhar());
    }

    public int CompareTo(PilhaLista<Dado> outro)   // para compatibilizar com ListaSimples e NoLista
    {
        return 0;
    }

    public PilhaLista<Dado> Copia()
    {
        var copia = new PilhaLista<Dado>();
        Dado[] aux = new Dado[this.Tamanho];

        for (int i = 0; !this.EstaVazia; i++)
        {
            aux[i] = this.OTopo();
            this.Desempilhar();
        }
           

        for (int i = aux.Length - 1; i >= 0; i--)
        {
            this.Empilhar(aux[i]);
            copia.Empilhar(aux[i]);
        }

        return copia;
    }
}
