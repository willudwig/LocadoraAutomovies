﻿using FluentValidation.Results;
using LocadoraAutomoveis.WinFormsApp.Modulo_GrupoVeiculo;
using LocadoraVeiculos.Dominio.Modulo_GrupoVeiculo;
using LocadoraVeiculos.Dominio.Modulo_Plano;
using Microsoft.VisualBasic;
using System;
using System.Windows.Forms;

namespace LocadoraAutomoveis.WinFormsApp.Modulo_Plano
{
    public partial class TelaCadastroPlano : Form 
    {
        private Plano plano;

        public Func<Plano, ValidationResult> GravarRegistro
        {
            get; set;
        }

        public Plano Plano
        {
            get
            {
                return plano;
            }
            set
            {
                plano = value;

                if (tabControlPlano.SelectedTab == tabControlPlano.TabPages[0])
                {
                    tbValorDiario_Diario.Text = plano.ValorDiario.ToString();
                }
                

                //tbDescricao.Text = plano.Nome;
                //tbValordiario.Text = plano.Salario.ToString();
                //tbData.Text = plano.DataAdmissao.ToString();
                //tbCidade.Text = plano.Cidade;
                //cbUF.Text = plano.Estado;
                //tbLogin.Text = plano.Login;
                //tbSenha.Text = plano.Senha;
                //cbPerfil.Text = plano.Perfil;
            }
        }

        public TelaCadastroPlano()
        {
            InitializeComponent();
        }

        private void TelaCadasrtoPlano_Load(object sender, EventArgs e)
        {
            FormPrincipal.Instancia.AtualizarRodape("");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //plano.Nome = tbNome.Text;
            //plano.Salario = float.Parse(tbSalario.Text);
            //plano.DataAdmissao = Convert.ToDateTime(tbData.Text);
            //plano.Cidade = tbCidade.Text;
            //plano.Estado = cbUF.Text;
            //plano.Login = tbLogin.Text;
            //plano.Senha = tbSenha.Text;
            //plano.Perfil = cbPerfil.Text;

            ValidationResult resultadoValidacao = GravarRegistro(plano);

            if (resultadoValidacao.IsValid == false)
            {
                string erro = resultadoValidacao.Errors[0].ErrorMessage;

                FormPrincipal.Instancia.AtualizarRodape(erro);

                DialogResult = DialogResult.None;
            }

        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            //tbNome.Clear();
            //tbSalario.Clear();
            //tbData.Text = "01/01/1753";
            //tbCidade.Clear();
            //tbLogin.Clear();
            //tbSenha.Clear();

            //tbNome.Focus();
        }

        private void TelaCadasrtoPlano_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormPrincipal.Instancia.AtualizarRodape("");
        }

        private static void ImpedirLetrasCharEspeciais(KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != ',')
            {
                e.Handled = true;
            }
        }

        private static KeyPressEventArgs ImpedirNumeroECharsEspeciaisTexBox(KeyPressEventArgs e)
        {
            if ((Strings.Asc(e.KeyChar) >= 48 & Strings.Asc(e.KeyChar) <= 57))
            {
                e.Handled = true;
            }

            if (!(char.IsLetter(e.KeyChar) || char.IsControl(e.KeyChar) || char.IsWhiteSpace(e.KeyChar)))
            {
                e.Handled = true;
            }

            return e;
        }

        private void btnAddGrupo_Click(object sender, EventArgs e)
        {
            var controladorGrupo = new ControladorGrupoVeiculo();

            controladorGrupo.Inserir();

            ObterItensGrupoVeiculo();
        }

        private void ObterItensGrupoVeiculo()
        {
            ////var servicoGrupo = new ServicoGrupo();
            ////var nomes = servicoGrupo.SelecionarTodos();

            //foreach (GrupoVeiculo gv in nomes)
            //{
            //    cbGrupo.Items.Add(gv.Nome);
            //}
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void LimparCamposDiario()
        {
            tbValorDiario_Diario.Clear();
            tbValorKmRodado_Diario.Clear();
        }

        private void LimparCampoLivre()
        {
            tbValorDiario_Livre.Clear();
        }
        private void LimparCamposControlado()
        {
            tbValorDiario_Controlado.Clear();
            tbKmRodado_Controlado.Clear();
            tbLimiteQuilometragem.Clear();
        }
    }
}
