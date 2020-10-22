using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apLabirinto
{
    class Movimento : IComparable<Movimento>
    {
		private Posicao origem;  // onde estou, para onde vou
		private int direcao;

		public Movimento()
        {
			origem.Coluna = 1;
			origem.Linha = 1;
			direcao = 0;
        }

		public Movimento(Posicao or, int dir)
		{
			origem = or;
			direcao = dir;
		}
		public Posicao Origem
		{
			get => origem;
			set => origem = value;
		}

        // não foi necessário

		//public Posicao Destino
		//{
		//	get => destino;
		//	set => destino = value;
		//}

		public int Direcao
		{
			get => direcao;
			set => direcao = value;
		}
		public override String ToString()
		{
			return origem+" para "+direcao;
		}

		public int CompareTo(Movimento outro)   // para compatibilizar com ListaSimples e NoLista
		{
			return 0;
		}
	}
}
