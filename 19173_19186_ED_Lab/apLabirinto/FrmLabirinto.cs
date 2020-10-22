using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apLabirinto
{
    public partial class FrmLabirinto : Form
    {
        GrafoBacktracking oGrafo; // variável para armazenar o labirinto a ser lido do txt + métodos para resolução
        ArrayList mapas; // arraylist que armazenará todos os grafos com as matrizes personalizadas com cada caminho encontrado
        string arq = null; 

        public FrmLabirinto()
        {
            InitializeComponent();
        }

        private void btnAbrirArquivo_Click(object sender, EventArgs e)
        {
            if (dlgAbrir.ShowDialog() == DialogResult.OK) // abre o explorador de arquivos para o usuário escolher o txt com o labirinto
            {
                arq = dlgAbrir.FileName;
                oGrafo = new GrafoBacktracking(arq); // passa nome do arquivo por parametro na instanciação do Grafo
                oGrafo.Exibir(dgvLabirinto); // exibe o labirinto proveniente do arquivo e armazenado em oGrafo no dgv da esquerda
            }
        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if(arq == null || arq.Equals("")) // caso o usuário não tenha escolhido um arquivo ainda
                MessageBox.Show("Nenhum arquivo foi selecionado ainda! Clique em [Abrir Aquivo].");
            else
            {
                mapas = new ArrayList();
                var pilhaCaminhos = oGrafo.BuscarCaminhos(dgvLabirinto, ref mapas);
                // armazena na PilhaLista "pilhaCaminhos" todos os caminhos que forem encontrados no labirinto do oGrafo
                // e passando como parametro o dgv da esquerda (para exibir os passos) e o ref mapas, um arrayList, que volta
                // com todos os caminhos encontrados em forma de pilha de movimentos, para ser, aqui, exibido no dgv da direita

                if (pilhaCaminhos.EstaVazia) // se a função retornar vazia
                    MessageBox.Show("Não achou caminho... Tente outro labirinto!"); // alerta o usuário

                else // há algum caminho possível
                {
                    MessageBox.Show("Achou caminho!");
                    dgvCaminhos.RowCount = pilhaCaminhos.QuantosNos; // seta o nº de linhas do dgv da direita com o nº de caminhos encontrados
                    int colunas = 0;
                    for (int l = 0; !pilhaCaminhos.EstaVazia; l++) // percorre a pilha de caminhos encontrados
                    {
                        var pilhaMovimentos = pilhaCaminhos.OTopo(); // recupera o primeiro movimento
                        int cols = pilhaMovimentos.QuantosNos; // seta o nº de colunas do dgv da direita com o nº de movimentos máximo entre todos os caminhos encontrados
                        if (cols > colunas)
                            dgvCaminhos.ColumnCount = cols;
                        
                        for (int c = 0; !pilhaMovimentos.EstaVazia; c++) // percorre a pilha de movimentos dentro do l-ésimo caminho de pilhaCaminhos
                        {
                            dgvCaminhos.Columns[c].Width = 30;
                            var mov = pilhaMovimentos.OTopo(); // armazena o c-ésimo movimento da pilhaMovimentos
                            dgvCaminhos[c, l].Value = mov.ToString(); // exibe a linha e a coluna do movimento + a direção deste
                            pilhaMovimentos.Desempilhar(); // desimpilha de pilhaMovimentos, para, quando retomar o loop, pegar o próximo movimento
                        }

                        // ao acabar de exibir o l-ésimo caminho, este é desimpilhado para, ao retomar o loop, ser possível acessar o próximo caminho 
                        pilhaCaminhos.Desempilhar();
                    }
                }
                dgvCaminhos.ClearSelection();
            }
        }

        private void dgvCaminhos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // ao clicar em alguma linha do dgv de caminhos (da direita)
            int linha = e.RowIndex; // armazena-se o nº da linha
            ((GrafoBacktracking)mapas[linha]).Exibir(dgvLabirinto); 
            // e exibe, no dgvLabirinto (da esquerda), o mapa com o caminho selecionado
        }
    }
}
