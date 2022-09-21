using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio1;
using negocio;

namespace pokemon.ado
{
    public partial class frmPokemons : Form
    {
        
        private List<Pokemon> listapokemon;
        public frmPokemons()
        {
            InitializeComponent();
        }

        private void frmPokemons_Load(object sender, EventArgs e)
        {   
                cargar();
                cboCampo.Items.Add("Numero");
                cboCampo.Items.Add("Nombre");
                cboCampo.Items.Add("Descripcion");   
        }
 

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxPokemon.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxPokemon.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvPokemons.CurrentRow != null)
            {
                Pokemon seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.UrlImagen);
            }
            
        }
        private void cargar()
        {
            try
            {
                PokemonNegocio negocio = new PokemonNegocio();
                listapokemon = negocio.listar();
                dgvPokemons.DataSource = listapokemon;
                cargarImagen(listapokemon[0].UrlImagen);
                ocultarColumna();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AltaPokemon alta = new AltaPokemon();
            alta.ShowDialog();

            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Pokemon Seleccionado;
            Seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
            AltaPokemon modificar = new AltaPokemon(Seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            eliminar();   
        }

        private void btnEliminiarLogico_Click(object sender, EventArgs e)
        {
            eliminar(true);
        }
        private void eliminar(bool logico = false)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            Pokemon seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Seguro desea eliminar este registro?", "Eliminar registro", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    if (logico)
                    {
                        seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
                        negocio.EliminarLogico(seleccionado.Id);
                    }
                    else
                    {
                        seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
                        negocio.eliminar(seleccionado.Id);
                    }
                    cargar();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ocultarColumna()
        {
            dgvPokemons.Columns["UrlImagen"].Visible = false;
            dgvPokemons.Columns["Id"].Visible = false;
            
        }
        private bool validarFiltro()
        {
            if(cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Porfavor ingrese un campo");
                return true;
            }
            if(cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Ingrese un criterio");
                return true;
            }
            return false;
        }
        private bool soloNumeros(String cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                {
                    return false;
                }
            }
            return true;
        }
        private bool soloLetras(String cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsLetter(caracter)))
                {
                    return false;
                }
            }
            return true;
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            {
                if (validarFiltro())
                {
                    return;
                }
                if(cboCampo.SelectedItem.ToString() == "Numero")
                {
                    if (string.IsNullOrEmpty(txtFiltro.Text))
                    {
                        MessageBox.Show("Porfavor ingrese un numero");
                        return;
                    }
                    if (!(soloNumeros(txtFiltro.Text)))
                    {
                        MessageBox.Show("Solo numeros porfavor");
                        return;
                    }
                }
                if(cboCampo.SelectedItem.ToString() == "Nombre" ||cboCampo.SelectedItem.ToString() == "Descripcion")
                {
                    if (string.IsNullOrEmpty(txtFiltro.Text))
                    {
                        MessageBox.Show("Ingrese una letra porfavor");
                        return;
                    }
                    if (!(soloLetras(txtFiltro.Text)))
                    {
                        MessageBox.Show("Ingrese solo letras porfavor");
                        return;
                    }
                }

                    string campo = cboCampo.SelectedItem.ToString();
                    string criterio = cboCriterio.SelectedItem.ToString();
                    string filtro = txtFiltro.Text;
                    dgvPokemons.DataSource = negocio.filtrar(campo, criterio, filtro);
                    ocultarColumna();
            }
          

        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            List<Pokemon> listaFiltrada;
            string Filtro = txtBuscar.Text;
            if (Filtro.Length >= 2)
            {
                listaFiltrada = listapokemon.FindAll(x => x.Nombre.ToUpper().Contains(Filtro.ToUpper()) || x.Tipo.Descripcion.ToUpper().Contains(Filtro.ToUpper()));
               
            }
            else
            {
                listaFiltrada = listapokemon;
            }
            dgvPokemons.DataSource = null;
            dgvPokemons.DataSource = listaFiltrada;
            ocultarColumna();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            try
            {
                if (opcion == "Numero")
                {
                    cboCriterio.Items.Clear();
                    cboCriterio.Items.Add("Mayor a");
                    cboCriterio.Items.Add("Menor a");
                    cboCriterio.Items.Add("Igual a");

                }
                else
                {
                    cboCriterio.Items.Clear();
                    cboCriterio.Items.Add("Comienza con");
                    cboCriterio.Items.Add("Termina con");
                    cboCriterio.Items.Add("Contiene");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnvolver_Click(object sender, EventArgs e)
        {
            dgvPokemons.DataSource = null;
            dgvPokemons.DataSource = listapokemon;
            ocultarColumna();
        }
    }
}
