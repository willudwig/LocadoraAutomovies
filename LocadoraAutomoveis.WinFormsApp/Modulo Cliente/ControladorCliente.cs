using FluentResults;
using LocadoraAutomoveis.Aplicacao.Modulo_Cliente;
using LocadoraAutomoveis.WinFormsApp.Compartilhado;
using LocadoraVeiculos.Dominio.Modulo_Cliente;
using System;
using System.Collections.Generic;

using System.Windows.Forms;

namespace LocadoraAutomoveis.WinFormsApp.Modulo_Cliente
{
    public class ControladorCliente : ControladorBase
    {
        readonly ServicoCliente servicoCliente;
        ClienteControl tabelaClientes;

        public ControladorCliente(ServicoCliente servicoCliente)
        {
            this.servicoCliente = servicoCliente;
        }

        public override void Inserir()
        {
            TelaCadastroCliente tela = new()
            {
                Cliente = new(),

                GravarRegistro = servicoCliente.Inserir
            };

            DialogResult resultado = tela.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                CarregarClientes();
            }
        }

        public override void Editar()
        {
            Cliente selecionado = null;

            Result<Cliente> resultadoResult = ObtemClienteSelecionado();

            if (resultadoResult.IsSuccess)
            {
                selecionado = resultadoResult.Value;

                if (selecionado.Id == Guid.Empty)
                {
                    MessageBox.Show("Selecione um cliente primeiro",
                    "Edição de Cliente", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                TelaCadastroCliente tela = new()
                {
                    Cliente = selecionado,

                    GravarRegistro = servicoCliente.Editar
                };

                DialogResult resultado = tela.ShowDialog();

                if (resultado == DialogResult.OK)
                {
                    CarregarClientes();
                }
            }
            else if (resultadoResult.IsFailed)
            {
                MessageBox.Show(resultadoResult.Errors[0].Message,
                   "Edição de Cliente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        
        public override void Excluir()
        {
            Cliente selecionado = null;

            Result<Cliente> resultadoResult = ObtemClienteSelecionado();

            if (resultadoResult.IsSuccess)
            {
                selecionado = resultadoResult.Value;

                if (selecionado.Id == Guid.Empty)
                {
                    MessageBox.Show("Selecione um cliente primeiro",
                    "Exclusão de Clientes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                DialogResult resultado = MessageBox.Show("Deseja realmente excluir o cliente?",
                    "Exclusão de Cliente", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (resultado == DialogResult.OK)
                {
                    servicoCliente.Excluir(selecionado);

                    CarregarClientes();
                }
            }
            else if (resultadoResult.IsFailed)
            {
                MessageBox.Show(resultadoResult.Errors[0].Message,
                   "Excluir de Cliente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        public override ConfiguracaoToolStripBase ObtemConfiguracaoToolStrip()
        {
            return new ConfiguracaoStripCliente();
        }

        public override UserControl ObtemListagem()
        {
            if (tabelaClientes == null)
                tabelaClientes = new ClienteControl();

            CarregarClientes();

            return tabelaClientes;
        }

        private void CarregarClientes()
        {
            Result<List<Cliente>> resultado = servicoCliente.SelecionarTodos();

            if (resultado.IsSuccess)
            {
                List<Cliente> clientes = resultado.Value;

                tabelaClientes.AtualizarRegistros(clientes);

                FormPrincipal.Instancia.AtualizarRodape($"Visualizando {clientes.Count} cliente(s)");
            }
            else
            {
                MessageBox.Show(resultado.Errors[0].Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Result<Cliente> ObtemClienteSelecionado()
        {
            var numero = tabelaClientes.ObtemNumerClienteSelecionado();

            return servicoCliente.SelecionarPorId(numero);
        }
    }
}
