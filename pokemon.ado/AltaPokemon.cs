using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio1;
using negocio;

namespace pokemon.ado
{
    public partial class AltaPokemon : Form
    {
        private Pokemon pokemon = null;
        private OpenFileDialog archivo = null;
        public AltaPokemon()
        {
            InitializeComponent();
        }
        public AltaPokemon(Pokemon pokemon)
        {
            InitializeComponent();
            this.pokemon = pokemon;
            Text = "Modificar Pokemon";

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {

            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                if (pokemon == null)
                    pokemon = new Pokemon();
                pokemon.Numero = int.Parse(textBox1.Text);
                pokemon.Nombre = textBox2.Text;
                pokemon.Descripcion = textBox3.Text;
                pokemon.Tipo = (Elemento)cboTipo.SelectedItem;
                pokemon.Debilidad = (Elemento)cboDebilidad.SelectedItem;
                pokemon.UrlImagen = (string)textBox4.Text;



                if (pokemon.Id != 0)
                {
                    negocio.modificar(pokemon);
                    MessageBox.Show("modificado exitosamente");
                }
                else
                {
                    negocio.agregar(pokemon);
                    MessageBox.Show("agregado exitosamente");
                }
                //Guardar imagen al aceptar.
                if(archivo != null && !(textBox4.Text.ToUpper().Contains("HTTP")))
                File.Copy(archivo.FileName, ConfigurationManager.AppSettings["image-folder"] + archivo.SafeFileName);
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AltaPokemon_Load(object sender, EventArgs e)
        {
            ElementoNegocio elementoNegocio = new ElementoNegocio();
            try
            {
                cboTipo.DataSource = elementoNegocio.listar();
                cboTipo.ValueMember = "Id";
                cboTipo.DisplayMember = "Descripcion";
                cboDebilidad.DataSource = elementoNegocio.listar();
                cboDebilidad.ValueMember = "Id";
                cboDebilidad.DisplayMember = "Descripcion";
                if (pokemon != null)
                {
                    textBox1.Text = pokemon.Numero.ToString();
                    textBox2.Text = pokemon.Nombre;
                    textBox3.Text = pokemon.Descripcion;
                    textBox4.Text = pokemon.UrlImagen;
                    cargarImagen(pokemon.UrlImagen);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            cargarImagen(textBox4.Text);
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

        private void btnlevantarimagen_Click(object sender, EventArgs e)
        {
            try
            {
                archivo = new OpenFileDialog();
                archivo.Filter = "jpg|*.jpg";

                if (archivo.ShowDialog() == DialogResult.OK)
                {
                    textBox4.Text = archivo.FileName;
                    cargarImagen(archivo.FileName);

                    //guardar imagen

                    //File.Copy(archivo.FileName, ConfigurationManager.AppSettings["image-folder"]+ archivo.SafeFileName);
                    
                }
            }
            
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
}
