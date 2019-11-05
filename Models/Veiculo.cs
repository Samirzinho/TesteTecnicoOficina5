using System;
using locadora.banco;
using Newtonsoft.Json;

namespace locadora.models
{
    public class Veiculo
    {
        public string Placa { get; set; }
        public string Marca { get; set; }
        public int Ano { get; set; }
        public string Descricao { get; set; }
        public bool Vendido { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        
        [JsonIgnore]
        public AppDb Db { get; set; }

        public Veiculo(AppDb db = null)
        {
            Db = db;
        }

        
    }
}
