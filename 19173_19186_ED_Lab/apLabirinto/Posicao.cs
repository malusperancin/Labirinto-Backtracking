using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apLabirinto
{
    class Posicao : IComparable<Posicao>
    {
        int lin, col; // linha e coluna da posição na matriz/labirinto

        public Posicao(int lin, int col) // construtor com parametros para settar a linha e a coluna da posição
        {
            this.lin = lin;
            this.col = col;
        }

        public override string ToString() // escreve a linha e a coluna no seguinte formato: [linha, coluna]
        {
            return "[" + lin + "," + col + "]";
        }

        public int Linha // propriedade de linha (get e set)
        {
            get => lin;
            set => lin = value;
        }

        public int Coluna // propriedade de coluna (get e set)
        {
            get => col;
            set => col = value;
        }

        public int CompareTo(Posicao outro)   // para compatibilizar com ListaSimples e NoLista
        {
            return 0;
        }
    }
}

