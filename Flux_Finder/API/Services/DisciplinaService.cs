using System;
using System.Collections.Generic;
using Data;
using Entities;

namespace API.Services
{
        
    public class DisciplinaService
    {
        private readonly DataBase _dataBase;
        public List<List<int>> listaAdjacencias; //O indice da lista + 1 representa o Id da disciplina.
        public List<int> listaAdjacenciasReversa ;//O indice da lista + 1 representa o Id da disciplina. 
        public Queue<int> S ;// Fila que irá armazenar os nós que não recebem arestas.

        public DisciplinaService(DataBase dataBase)
        {
            this._dataBase = dataBase;
        }

        public void GetTopologia(List<int> materiasRealizadas)
        {
            //Montagem da Lista de Adjacencias e da lista de Adjacencias reversa.    
            GenerateLISTAS(_dataBase);
            //Retirando do grafo todas as materias que o estudante ja realizou
            if (materiasRealizadas.Count != 0){
                MateriasRealizadas(materiasRealizadas);
            }
        }
    
    
        private void GenerateLISTAS(DataBase dataBase)
        {
            this.listaAdjacencias = new List<List<int>>();
            this.listaAdjacenciasReversa= new List<int>();
            this.S =new Queue<int>();


            foreach (Disciplina disciplina in dataBase.DisciplinasCadastradas)
            {
                //Crio o nó na listaAdjacencias e na listaAdjacenciasReversa
                this.listaAdjacencias.Add(new List<int>());
                this.listaAdjacenciasReversa.Add(0);

                var pendencias = disciplina.Pendencias;                
                if (pendencias.Count == 0){
                    //Se a disciplina não tem pré requisitos significa que na lista reversa nao tem arestas entao inserimos na fila
                    this.S.Enqueue(disciplina.Id);
                    continue;
                }
                foreach(var pendenciaId in pendencias){  
                    this.listaAdjacencias[pendenciaId - 1].Add(disciplina.Id);
                    this.listaAdjacenciasReversa[disciplina.Id - 1]++;
                }
            } 
        }
    

        private void MateriasRealizadas(List<int> materiasRealizadas) 
            {
                foreach(var idMateria in materiasRealizadas){
                        var listaAdjacenciaNo = this.listaAdjacencias[idMateria - 1];
                            
                        foreach (int disciplinaId in listaAdjacenciaNo){
                            var result = -- this.listaAdjacenciasReversa[disciplinaId - 1];
                            if (result== 0)
                                this.S.Enqueue(disciplinaId);
                        }
                    }
            }
}
    }