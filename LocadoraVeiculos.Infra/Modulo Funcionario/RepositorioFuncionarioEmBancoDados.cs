﻿using LocadoraVeiculos.Dominio.Modulo_Funcionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocadoraVeiculos.Infra.BancoDados.Modulo_Funcionario
{
    public class RepositorioFuncionarioEmBancoDados : ConexaoBancoDados<Funcionario>, IRepositorio<Funcionario>
    {
    }
}
