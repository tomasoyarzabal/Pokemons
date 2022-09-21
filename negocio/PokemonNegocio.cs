﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio1;

namespace negocio
{
    public class PokemonNegocio
    {
        public List<Pokemon> listar()
        {
            List<Pokemon> lista = new List<Pokemon>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=POKEDEX_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, IdTipo, IdDebilidad, P.Id From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo And D.Id = P.IdDebilidad and P.Activo = 1";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)lector["Id"];
                    aux.Numero = lector.GetInt32(0);
                    aux.Nombre = (string)lector["Nombre"];
                    aux.Descripcion = (string)lector["Descripcion"];
                    aux.Tipo = new Elemento();
                    aux.Tipo.Id = (int)lector["IdTipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Id = (int)lector["IdDebilidad"];


                    //Vlidacion de columna NULL.
                    if (!(lector["UrlImagen"]is DBNull))
                    aux.UrlImagen = (string)lector["UrlImagen"];


                    aux.Tipo = new Elemento();
                    aux.Tipo.Descripcion = (string)lector["Tipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Descripcion = (string)lector["Debilidad"];

                    lista.Add(aux);
                }

                conexion.Close();
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void agregar(Pokemon nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("insert into POKEMONS (Numero, Nombre, Descripcion, Activo, IdTipo, IdDebilidad,UrlImagen) values (" + nuevo.Numero+",'" + nuevo.Nombre + "','" + nuevo.Descripcion + "',1, @IdTipo, @IdDebilidad, @UrlImagen)");
                datos.setearParametros("IdTipo", nuevo.Tipo.Id);
                datos.setearParametros("IdDebilidad", nuevo.Debilidad.Id);
                datos.setearParametros("UrlImagen", nuevo.UrlImagen);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificar (Pokemon poke)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update POKEMONS set Numero = @numero, Nombre = @nombre , Descripcion = @descripcion, UrlImagen = @imagen, IdTipo = @IdTipo ,IdDebilidad = @IdDebilidad WHERE Id = @Id");
                datos.setearParametros("@numero", poke.Numero);
                datos.setearParametros("@nombre", poke.Nombre);
                datos.setearParametros("@descripcion", poke.Descripcion);
                datos.setearParametros("@imagen", poke.UrlImagen);
                datos.setearParametros("@IdTipo", poke.Tipo.Id);
                datos.setearParametros("@IdDebilidad", poke.Debilidad.Id);
                datos.setearParametros("@Id", poke.Id);

                datos.EjecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();

            }
        }

        public void eliminar (int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("delete from pokemons where id = @id");
                datos.setearParametros("@id", id);
                datos.EjecutarAccion();
            }
            catch ( Exception ex)
            {

                throw ex;
            }
        }

        public List<Pokemon> filtrar(string campo, string criterio, string filtro)
        {
            List<Pokemon> lista = new List<Pokemon>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, IdTipo, IdDebilidad, P.Id From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo And D.Id = P.IdDebilidad and P.Activo = 1 and ";
                switch (campo)
                {
                    case "Numero":
                        {
                            switch (criterio)
                            {
                                
                                case "Mayor a":
                                    consulta += "Numero >" + filtro;
                                    break;
                                case "Menor a":
                                    consulta += "Numero <" + filtro;
                                    break;

                                default:
                                    consulta += "Numero =" + filtro;
                                    break;
                            }
                            break;
                        }
                    case "Nombre":
                        {
                            switch (criterio)
                            {
                                case "Comienza con":
                                    consulta += "Nombre like '" + filtro + "%'";
                                    break;
                                case "Termina con":
                                    consulta += "Nombre like '%" + filtro + "'";
                                    break;
                                default:
                                    consulta += "Nombre like '%" + filtro + "%'";
                                    break;
                            }
                            break;
                        }
                    case "Descripcion":
                        {
                            switch (criterio)
                            {
                                case "Comienza con":
                                    consulta += "P.Descripcion like '" + filtro + "%'";
                                    break;
                                case "Termina con":
                                    consulta += "P.Descripcion like '%" + filtro + "'";
                                    break;
                                default:
                                    consulta += "P.Descripcion like '%" + filtro + "%'";
                                    break;
                            }
                        }
                        break;
                }
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Numero = datos.Lector.GetInt32(0);
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Tipo = new Elemento();
                    aux.Tipo.Id = (int)datos.Lector["IdTipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Id = (int)datos.Lector["IdDebilidad"];



                    if (!(datos.Lector["UrlImagen"] is DBNull))
                        aux.UrlImagen = (string)datos.Lector["UrlImagen"];


                    aux.Tipo = new Elemento();
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        

        public void EliminarLogico(int id)
        {

            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("update POKEMONS set Activo = 0 where id = @id");
                datos.setearParametros("@id",id);
                datos.EjecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
    }

}
