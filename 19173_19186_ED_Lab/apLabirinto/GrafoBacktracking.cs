using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Drawing;
using System.Collections;
using System.Threading.Tasks;

namespace apLabirinto
{
    class GrafoBacktracking : IComparable<GrafoBacktracking>
    {
        int linhas, colunas;
        char[,] matriz;

        public GrafoBacktracking(int l, int c)
        {
            this.linhas = l; 
            this.linhas = l; 
            this.colunas = c;
            this.matriz = new char[l, c];
         }

        // construtor que recebe o nome do arquivo onde está armazenado o labirinto de caractéres
        public GrafoBacktracking(string nomeArquivo)
        {
            // instancia-se o StreamReader p/ ler o txt (passado por parametro) que armazena o labirinto
            var arquivo = new StreamReader(nomeArquivo);

            // lê-se as duas primeiras linhas do arquivo
            //a 1º sendo o nº de linhas 
            // e a 2º sendo o nº de colunas
            linhas = int.Parse(arquivo.ReadLine().ToString()); 
            colunas = int.Parse(arquivo.ReadLine().ToString());

            matriz = new char[linhas, colunas]; // instancia-se a matriz labirinto com os nºs lidos anteriormente
            for (int l = 0; l < linhas; l++)
            {
                string linha = arquivo.ReadLine(); // lê-se a linha inteira
                for (int c = 0; c < colunas; c++)
                    matriz[l, c] = char.Parse(linha.Substring(c, 1)); // pega-se um caracter por vez e aramazena na matriz
            }
            arquivo.Close();
            // fecha o arquivo de leitura
        }

        // propriedades dos atributos da classe (gets e sets)
        public char[,] Matriz { get => matriz; set => matriz = value; }
        public int Linhas { get => linhas; set => linhas = value; }
        public int Colunas { get => colunas; set => colunas = value; }

        public void Exibir(DataGridView dgv)
        {
            // setta as linhas e coluna do dgv de acordo com os atributos da classe e o tamanho da matriz armazenada
            dgv.RowCount = linhas;
            dgv.ColumnCount = colunas;
      
            for (int l = 0; l < linhas; l++)
            {
                dgv.Rows[l].Height = dgv.Height/linhas; // labirinto responsivo
                for (int c = 0; c < colunas; c++)
                {
                    dgv.Columns[c].Width = dgv.Width / colunas;// labirinto responsivo

                    switch(matriz[l, c])
                    {
                        case '#': // se for parede, pinta-se a celula do dgv de preto
                            dgv[c, l].Style.BackColor = Color.Black;
                            break;
                        case 'S': // se for o destino final, pinta-se a celula do dgv de verde
                            dgv[c, l].Style.BackColor = Color.Green;
                            break;
                        case '.': // se for caminho andado, pinta-se a celula do dgv de branco
                            dgv[c, l].Style.BackColor = Color.White;
                            break;
                        case ' ': // caso seja apenas via livre do labirinto, pinta-se a celula do dgv de cinza
                            dgv[c, l].Style.BackColor = Color.DarkGray;
                            break;
                    }
                }
            }

            dgv[1, 1].Style.BackColor = Color.Red; // pinta-se a posição inicial do labirinto de vermelho
            dgv.ClearSelection();
        }

        public PilhaLista<PilhaLista<Movimento>> BuscarCaminhos(DataGridView dgvLabirinto, ref ArrayList grafos )
        {
            var ret = new PilhaLista<PilhaLista<Movimento>>(); // pilha de pilha de movimentos == pilha de caminhos
            var copiaMatriz = this.matriz; // faz-se uma cópia da matriz/labirinto original, sem caminhos
            int i = 1, j = 1, direcao = 0; // inicia-se as variáveis de i (linha), j (coluna) e a direção
            PilhaLista<Movimento> pilha = new PilhaLista<Movimento>(); // inicia-se a pilha para armazenar o caminho atual
            bool naoTemSaida = false; // variável para verificar se ainda há caminho a ser encontrado

            while (!naoTemSaida)
            {
                bool achouCaminho = false;
                naoTemSaida = false;

                // setta cada posicao dos vetores de linha e coluna de tal forma que cada posição de lin e col somadas 
                // à linha e à coluna de uma matriz, representa uma "rosa dos ventos", iniciando na direção vertical para cima
                int[] lin = { -1, -1, 0, 1, 1, 1, 0, -1 };
                int[] col = { 0, 1, 1, 1, 0, -1, -1, -1 };

                while (!naoTemSaida && !achouCaminho) // enquanto ainda houver caminho a ser traçado para achar o 'S'
                {
                    for (; direcao < 8; direcao++) // verificamos a possibilidade de andar em x direação a partir da posicao atual (i, j)
                    {
                        // nova potencial posição
                        int Inovo = i + lin[direcao];
                        int Jnovo = j + col[direcao];

                        Posicao pos = new Posicao(Inovo, Jnovo);

                        // verifica se não há parede e se já não foi passamos por essa posição
                        if (matriz[Inovo, Jnovo] != '#' && matriz[Inovo, Jnovo] != '.') 
                        {
                            //achou caminho
                            Posicao posAnt = new Posicao(i, j);
                            Movimento mov = new Movimento(posAnt, direcao);
                            matriz[i, j] = '.';

                            // atualiza os valores de i e j com a nova posição
                            i = Inovo;
                            j = Jnovo;

                            pilha.Empilhar(mov); // empilha movimento da pilha que representa o caminho
                            
                            achouCaminho = matriz[i, j] == 'S'; // verifica se já chegamos ao final do labirinto

                            if(!achouCaminho)
                                ExibirUmPasso(true);
                            direcao = 0; // reinicia a variável de direção
                            break;// sai do for 
                        }
                    }
                    if (!achouCaminho && !pilha.EstaVazia && direcao == 8) // n achou caminho
                    {
                        // desimpilha-se o movimento da posição atual, armazena o anterior, aumenta 1 na sua direção 
                        // e volta para o loop do while

                        matriz[i, j] = ' ';
                        Movimento mov = pilha.OTopo();
                        ExibirUmPasso(false); // deixa de exibir esse passo errado que foi desempilhado
                        pilha.Desempilhar();

                        i = j = 1; // sempre setta i e j com 1
                        
                        if (mov != null) // se, depois de desempilhar, ainda houver movimentos
                        {
                            i = mov.Origem.Linha; // corrigimos o valor do i e j para os deste movimento
                            j = mov.Origem.Coluna;
                            direcao = mov.Direcao+1;
                        }
                    }
                    naoTemSaida = (i == 1 && j == 1 && pilha.EstaVazia && direcao == 8);
                    // se percorrermos todo o labirinto, significa q voltamos p/ posição inicial com o valor de direcao
                    // sendo 8 (máx.), portanto, já não há mais caminhos a serem traçados
                }

                if (pilha == null || pilha.EstaVazia) // se não achar nenhum caminho, sai do loop do while
                    break;
                ret.Empilhar(pilha.Copia()); // empilha o caminho encontrado na variável de retorno
                grafos.Add(this.Copia()); // adiciona esta instância do grafoBacktracking ao arrayList de grafos
                i = pilha.OTopo().Origem.Linha; // setta o i e j com os valores de i e j do movimento anterior ao último que encontrou o 'S'S
                j = pilha.OTopo().Origem.Coluna;
                direcao = pilha.OTopo().Direcao+1; // setta a próxima direção
                pilha.Desempilhar(); // tira o último movimento da pilha do caminho para, quando voltar ao loop, podermos buscar caminhos diferentes ao(s) já encontrado(s)
            }

            this.matriz = copiaMatriz; // setta o valor da matriz deste grafo para a original, lida do txt (já que a alteramos no meio do processo)

            return ret; // retorna a pilha dos caminhos encontrados

            void ExibirUmPasso(bool andar)
            {
                if (andar) // se é para adicionar e exibir um passo
                    try { dgvLabirinto[j, i].Style.BackColor = Color.White; } catch (Exception) { }
                else // se for para retroceder a posição
                    try { dgvLabirinto[j, i].Style.BackColor = Color.DarkGray; } catch (Exception) { }

                Thread.Sleep(100); // delay de 0,1s para o usuário ver a mudança no dgv
                Application.DoEvents(); // atualiza o dgv 
            }
        }

        public int CompareTo(GrafoBacktracking outro)   // para compatibilizar com ListaSimples e NoLista
        {
            return 0;
        }

        public GrafoBacktracking Copia() // esta função retorna um clone do grafo que o chamar
        {
            var copia = new GrafoBacktracking(this.linhas, this.colunas); // instancia um grafo com o mesmo número de linhas e colunas de this

            for (int l = 0; l<this.linhas; l++) // copia todos os valores de cada posição da matriz de this para a matriz de copia
                for (int c = 0; c < this.colunas; c++)
                    copia.matriz[l, c] = this.matriz[l, c]; 

            return copia; // retorna um grafoBacktracking idêntico ao this
        }

    }

}
