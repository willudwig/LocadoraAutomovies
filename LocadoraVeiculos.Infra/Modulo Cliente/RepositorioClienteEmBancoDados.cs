﻿using FluentValidation.Results;
using LocadoraVeiculos.Dominio.Modulo_Cliente;
using LocadoraVeiculos.Infra.BancoDados.Compartilhado;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace LocadoraVeiculos.Infra.BancoDados.Modulo_Cliente
{
    public class RepositorioClienteEmBancoDados : RepositorioBase<Cliente, MapeadorCliente, ValidadorCliente>
    {
        protected override string sql_insercao => @"INSERT INTO TBCLIENTE 
                                                   (
                                                        [NOME],    
                                                        [CPF],
                                                        [CNPJ],   
                                                        [ENDERECO],
                                                        [TIPOCLIENTE],
                                                        [CNH],
                                                        [EMAIL],
                                                        [TELEFONE]  
                                                   )
                                                   VALUES
                                                   (
                                                        @NOME,    
                                                        @CPF,
                                                        @CNPJ,   
                                                        @ENDERECO,
                                                        @TIPOCLIENTE,
                                                        @CNH,
                                                        @EMAIL,
                                                        @TELEFONE

                                                   );SELECT SCOPE_IDENTITY();";

        protected override string sql_edicao => @"UPDATE [TBCLIENTE] SET 

                                                        [NOME] = @NOME,
                                                        [CPF] = @CPF,
                                                        [CNPJ] = @CNPJ,
                                                        [ENDERECO] = @ENDERECO,
                                                        [TIPOCLIENTE] = @TIPOCLIENTE,
                                                        [CNH] = @CNH,
                                                        [EMAIL] = @EMAIL,
                                                        [TELEFONE] = @TELEFONE

                                                WHERE
		                                                ID = @ID";

        protected override string sql_exclusao => @"DELETE FROM TBCLIENTE WHERE ID = @ID;";
        protected override string sql_selecao_por_id => @"SELECT * FROM TBCLIENTE";

        protected override string sql_selecao_todos => @"SELECT * FROM TBCLIENTE";


        protected override bool VerificarDuplicidade(Cliente entidade)
        {
            var clientes = SelecionarTodos();

            foreach (Cliente c in clientes)
            {
                if (entidade.Cpf != "-" && c.Cpf == entidade.Cpf  ) 
                    return true;

                if (entidade.Cnpj != "-" && c.Cnpj == entidade.Cnpj)
                    return true;

                if (c.Cnh == entidade.Cnh) 
                    return true;
            }

            return false;
        }
    }
}
