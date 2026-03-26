using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    class Utilisateur
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }

        public override string ToString()
        {
            return $"Nom: {this.Name}, Nom d'utilisateur: {this.UserName}, Email: {this.Email}";
        }
    }
}
