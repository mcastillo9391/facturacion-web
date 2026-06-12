import { useEffect, useState } from "react";

import {
  obtenerUsuarios,
  crearUsuario,
  actualizarUsuario,
  desactivarUsuario,
  obtenerRoles,
} from "../../services/usuarioService";

import LoadingOverlay
  from "../../components/LoadingOverlay";

import { usePagination } from "../../hooks/usePagination";
import Pagination from "../../components/Pagination";

export default function UsuariosPage() {
  const [usuarios, setUsuarios] = useState([]);
  const [roles, setRoles] = useState([]);

  const [busqueda, setBusqueda] = useState("");

  const [mostrarFormulario, setMostrarFormulario] =
    useState(false);

  const [editando, setEditando] =
    useState(null);

  const [formulario, setFormulario] =
    useState({
      nombre: "",
      username: "",
      password: "",
      rolId: 1,
      activo: true,
    });

  const cargarDatos = async () => {
    try {
      const usuariosData =
        await obtenerUsuarios();

      const rolesData =
        await obtenerRoles();

      setUsuarios(usuariosData);
      setRoles(rolesData);
    } catch (error) {
      console.error(error);

      alert(
        "Error cargando usuarios"
      );
    }
  };

  useEffect(() => {
    cargarDatos();
  }, []);

  const limpiarFormulario = () => {
    setFormulario({
      nombre: "",
      username: "",
      password: "",
      rolId: 1,
      activo: true,
    });

    setEditando(null);
  };

  const abrirNuevo = () => {
    limpiarFormulario();
    setMostrarFormulario(true);
  };

  const abrirEditar = (usuario) => {
    const rolEncontrado =
      roles.find(
        (r) =>
          r.nombre === usuario.rol
      );

    setFormulario({
      nombre: usuario.nombre,
      username: usuario.username,
      password: "",
      rolId:
        rolEncontrado?.rolId || 1,
      activo: usuario.activo,
    });

    setEditando(
      usuario.usuarioId
    );

    setMostrarFormulario(true);
  };
  
  const [Agregando, setAgregando] =
    useState(false);
  const guardar = async (e) => {
    e.preventDefault();

    try {
      // Validando para efecto de espera
      if (Agregando) return;
      //Activando modo espera
      setAgregando(true);

      if (editando) {
        await actualizarUsuario(
          editando,
          {
            nombre:
              formulario.nombre,
            username:
              formulario.username,
            rolId: parseInt(
              formulario.rolId
            ),
            activo:
              formulario.activo,
          }
        );
      } else {
        await crearUsuario({
          nombre:
            formulario.nombre,
          username:
            formulario.username,
          password:
            formulario.password,
          rolId: parseInt(
            formulario.rolId
          ),
          activo: true,
        });
      }

      setMostrarFormulario(false);

      limpiarFormulario();

      cargarDatos();
    } catch (error) {
      console.error(error);

      alert(
        "Error al guardar usuario"
      );
    } finally {
      // Finalizando modo espera
      setAgregando(false);

    }
  };
  
  const [Desactivando, setDesactivando] =
    useState(false);
  const desactivar = async (id) => {
    if (
      !window.confirm(
        "¿Desea desactivar este usuario?"
      )
    ) {
      return;
    }

    try {
      // Validando para modo espera
      if (Desactivando) return;
      // Activando modo espera
      setDesactivando(true);

      // Desactivando...
      await desactivarUsuario(id);

      cargarDatos();
    } catch {
      alert(
        "Error al desactivar usuario"
      );
    } finally {

      setDesactivando(false);

    }
  };

  const usuariosFiltrados =
    usuarios.filter(
      (u) =>
        u.nombre
          ?.toLowerCase()
          .includes(
            busqueda.toLowerCase()
          ) ||
        u.username
          ?.toLowerCase()
          .includes(
            busqueda.toLowerCase()
          )
    );

  const {
    page,
    totalPages,
    paginatedData,
    nextPage,
    prevPage,
    goToPage,
    setPage,
  } = usePagination(
    usuariosFiltrados,
    10
  );

  useEffect(() => {
    setPage(1);
  }, [busqueda, setPage]);

  return (
    <div className="page-container">
  
      {Desactivando && (
        <LoadingOverlay
          mensaje="Desactivando usuario..."
        />
      )}
      {Agregando && (
        <LoadingOverlay
          mensaje="Agregando usuario..."
        />
      )}

      {/* HERO */}
      <div className="page-header">

        <h2>
          👤 Usuarios
        </h2>

        <button
          className="btn-primary"
          onClick={abrirNuevo}
        >
          Nuevo Usuario
        </button>

      </div>

      {/* FORMULARIO */}
      {mostrarFormulario && (
        <form
          onSubmit={guardar}
          className="cliente-form"
        >
          <input
            placeholder="Nombre"
            value={
              formulario.nombre
            }
            onChange={(e) =>
              setFormulario({
                ...formulario,
                nombre:
                  e.target.value,
              })
            }
            required
          />

          <input
            placeholder="Usuario"
            value={
              formulario.username
            }
            onChange={(e) =>
              setFormulario({
                ...formulario,
                username:
                  e.target.value,
              })
            }
            required
          />

          {!editando && (
            <input
              type="password"
              placeholder="Contraseña"
              value={
                formulario.password
              }
              onChange={(e) =>
                setFormulario({
                  ...formulario,
                  password:
                    e.target.value,
                })
              }
              required
            />
          )}

          <select
            value={
              formulario.rolId
            }
            onChange={(e) =>
              setFormulario({
                ...formulario,
                rolId:
                  e.target.value,
              })
            }
          >
            {roles.map((rol) => (
              <option
                key={rol.rolId}
                value={rol.rolId}
              >
                {rol.nombre}
              </option>
            ))}
          </select>

          <div>
            <button
              type="submit"
              className="btn-success"
            >
              Guardar
            </button>

            <button
              type="button"
              className="btn-danger"
              onClick={() => {
                limpiarFormulario();
                setMostrarFormulario(
                  false
                );
              }}
            >
              Cancelar
            </button>
          </div>
        </form>
      )}

      {/* BUSCADOR */}
      <div className="search-container">
        <input
          type="text"
          placeholder="Buscar usuario..."
          value={busqueda}
          onChange={(e) =>
            setBusqueda(
              e.target.value
            )
          }
        />
      </div>

      {/* TABLA */}
      <div className="table-container">
        <div className="table-scroll">

          <table>

            <thead>
              <tr>
                <th>ID</th>
                <th>Nombre</th>
                <th>Usuario</th>
                <th>Rol</th>
                <th>Estado</th>
                <th>Acciones</th>
              </tr>
            </thead>

            <tbody>

              {paginatedData.length === 0 ? (
                <tr>
                  <td
                    colSpan="6"
                    style={{
                      textAlign:
                        "center",
                    }}
                  >
                    No existen usuarios
                  </td>
                </tr>
              ) : (
                paginatedData.map(
                  (usuario) => (
                    <tr
                      key={
                        usuario.usuarioId
                      }
                    >
                      <td>
                        {
                          usuario.usuarioId
                        }
                      </td>

                      <td>
                        {
                          usuario.nombre
                        }
                      </td>

                      <td>
                        {
                          usuario.username
                        }
                      </td>

                      <td>
                        {usuario.rol}
                      </td>

                      <td>
                        <span
                          className={`badge ${
                            usuario.activo
                              ? "success"
                              : "danger"
                          }`}
                        >
                          {usuario.activo
                            ? "ACTIVO"
                            : "INACTIVO"}
                        </span>
                      </td>

                      <td>
                        <div className="actions">

                          <button
                            className="btn-view"
                            onClick={() =>
                              abrirEditar(
                                usuario
                              )
                            }
                          >
                            Editar
                          </button>

                          {usuario.activo && (
                            <button
                              className="btn-danger"
                              onClick={() =>
                                desactivar(
                                  usuario.usuarioId
                                )
                              }
                            >
                              Desactivar
                            </button>
                          )}

                        </div>
                      </td>
                    </tr>
                  )
                )
              )}

            </tbody>

          </table>

        </div>
      </div>

      {/* PAGINACIÓN */}
      <Pagination
        page={page}
        totalPages={totalPages}
        nextPage={nextPage}
        prevPage={prevPage}
        goToPage={goToPage}
      />

    </div>
  );
}