using FluentResults;
using FluentValidation.Results;
using LocadoraVeiculos.Dominio.Modulo_Cliente;
using LocadoraVeiculos.Infra.BancoDados.Modulo_Cliente;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LocadoraAutomoveis.Aplicacao.Modulo_Cliente
{
    public class ServicoCliente
    {
        readonly RepositorioClienteEmBancoDados repositorioCliente;
        ValidadorCliente validadorCliente;

        public ServicoCliente(RepositorioClienteEmBancoDados repositorioCliente)
        {
            this.repositorioCliente = repositorioCliente;
        }

        public Result<Cliente> Inserir(Cliente cliente)
        {
            Log.Logger.Debug("Tentando inserir Cliente... {@cliente}", cliente);

            var resultadoValidacao = Validar(cliente);

            if (resultadoValidacao.IsFailed)
            {
                foreach (var erro in resultadoValidacao.Errors)
                {
                    Log.Logger.Warning("Falha ao tentar inserir Cliente. {ClienteId} -> Motivo: {erro}", cliente.Id, erro.Message);
                }

                return Result.Fail(resultadoValidacao.Errors);
            }

            try
            {
                repositorioCliente.Inserir(cliente);

                Log.Logger.Information("Cliente inserido com sucesso. {@cliente}", cliente);

                return Result.Ok(cliente);
            }
            catch (Exception ex)
            {
                string msgErro = "Falha ao tentar inserir Cliente.";

                Log.Logger.Error(ex, msgErro + "{ClienteId}", cliente.Id);

                return Result.Fail(msgErro);
            }

            return resultadoValidacao;
        }

        public Result<Cliente> Editar(Cliente cliente)
        {
            Log.Logger.Debug("Tentando editar Cliente... {@cliente}", cliente);

            var resultadoValidacao = Validar(cliente);

            if (resultadoValidacao.IsFailed)
            {
                foreach (var erro in resultadoValidacao.Errors)
                {
                    Log.Logger.Warning("Falha ao tentar editar Cliente. {ClienteId} -> Motivo: {erro}", cliente.Id, erro.Message);
                }

                return Result.Fail(resultadoValidacao.Errors);
            }

            try
            {
                repositorioCliente.Editar(cliente);

                Log.Logger.Information("Cliente editado com sucesso. {@cliente}", cliente);

                return Result.Ok(cliente);
            }
            catch (Exception ex)
            {
                string msgErro = "Falha ao tentar editar Cliente";

                Log.Logger.Error(ex, msgErro + "{ClienteId}", cliente.Id);

                return Result.Fail(msgErro);
            }
        }

        public Result Excluir(Cliente cliente)
        {
            Log.Logger.Debug("Tentando excluir Cliente... {@cliente}", cliente);

            try
            {
                repositorioCliente.Excluir(cliente);

                Log.Logger.Information("Cliente excluído com sucesso. {@cliente}", cliente);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                string msgErro = "Falha ao tentar excluir Cliente.";

                Log.Logger.Error(ex, msgErro + "{ClienteId}", cliente.Id);

                return Result.Fail(msgErro);
            }
        }

        public Result<List<Cliente>> SelecionarTodos()
        {
            try
            {
                return Result.Ok(repositorioCliente.SelecionarTodos());
            }
            catch (Exception ex)
            {
                string msgErro = "falha no sistema ao tentar selecioanr todos os clientes";

                Log.Logger.Error(ex, msgErro);

                return Result.Fail(msgErro);
            }
        }

        public Result<Cliente> SelecionarPorId(Guid id)
        {
            try
            {
                return Result.Ok(repositorioCliente.SelecionarPorId(id));
            }
            catch (Exception ex)
            {
                string msgErro = "Falha no sistema ao tentar selecionar cliente";

                Log.Logger.Error(ex, msgErro + "{ClienteId}", id);

                return Result.Fail(msgErro);
            }
        }


        #region privates

        private bool NomeDuplicado(Cliente cliente)
        {
            repositorioCliente.Sql_selecao_por_parametro = @"SELECT * FROM TBCLIENTE WHERE NOME = @NOMECLIENTE";
            repositorioCliente.PropriedadeParametro = "NOMECLIENTE";

            var clienteEncontrado = repositorioCliente.SelecionarPorParametro(repositorioCliente.PropriedadeParametro, cliente);

            return clienteEncontrado != null &&
                   clienteEncontrado.Nome.Equals(cliente.Nome) &&
                  !clienteEncontrado.Id.Equals(cliente.Id);
        }

        private bool CnpjDuplicado(Cliente cliente)
        {
            repositorioCliente.Sql_selecao_por_parametro = @"SELECT * FROM TBCLIENTE WHERE CNPJ = @CNPJCLIENTE";
            repositorioCliente.PropriedadeParametro = "CNPJCLIENTE";

            var clienteEncontrado = repositorioCliente.SelecionarPorParametro(repositorioCliente.PropriedadeParametro, cliente);

            return clienteEncontrado != null &&
                   clienteEncontrado.Cnpj != "-" &&
                   clienteEncontrado.Cnpj.Equals(cliente.Cnpj) &&
                  !clienteEncontrado.Id.Equals(cliente.Id);
        }

        private bool CpfDuplicado(Cliente cliente)
        {
            repositorioCliente.Sql_selecao_por_parametro = @"SELECT * FROM TBCLIENTE WHERE CPF = @CPFCLIENTE";
            repositorioCliente.PropriedadeParametro = "CPFCLIENTE";

            var clienteEncontrado = repositorioCliente.SelecionarPorParametro(repositorioCliente.PropriedadeParametro, cliente);

            return clienteEncontrado != null &&
                   clienteEncontrado.Cpf != "-" &&
                   clienteEncontrado.Cpf.Equals(cliente.Cpf) &&
                  !clienteEncontrado.Id.Equals(cliente.Id);
        }

        private Result Validar(Cliente cliente)
        {
            var validadorCliente = new ValidadorCliente();

            var resultadoValidacao = validadorCliente.Validate(cliente);

            List<Error> erros = new List<Error>(); //FluentResult

            foreach (ValidationFailure item in resultadoValidacao.Errors) //FluentValidation 
            {
                erros.Add(new Error(item.ErrorMessage));
            }

            if (NomeDuplicado(cliente))
                resultadoValidacao.Errors.Add(new ValidationFailure("Nome", "'Nome' duplicado"));

            if (CpfDuplicado(cliente))
                resultadoValidacao.Errors.Add(new ValidationFailure("Cpf", "'Cpf' duplicado"));

            if (CnpjDuplicado(cliente))
                resultadoValidacao.Errors.Add(new ValidationFailure("Cnpj", "'Cnpj' duplicado"));

            if (erros.Any())
                return Result.Fail(erros);

            return Result.Ok();
        }

        #endregion
    }
    
}
