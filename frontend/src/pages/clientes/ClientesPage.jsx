import { useEffect, useState } from "react";
import { usePagination } from "../../hooks/usePagination";
import Pagination from "../../components/Pagination";
import {
  obtenerClientes,
  crearCliente,
  actualizarCliente,
  eliminarCliente,
} from "../../services/clienteService";

export default function ClientesPage() {
  const [clientes, setClientes] = useState([]);
  const [busqueda, setBusqueda] = useState("");
  const [mostrarFormulario, setMostrarFormulario] = useState(false);
  const [editando, setEditando] = useState(null);

  const [formulario, setFormulario] = useState({
    identificacion: "",
    nombre: "",
    direccion: "",
    telefono: "",
  });

  const cargarClientes = async () => {
    const data = await obtenerClientes();
    setClientes(data);
  };

  useEffect(() => {
    cargarClientes();
  }, []);

  const limpiarFormulario = () => {
    setFormulario({
      identificacion: "",
      nombre: "",
      direccion: "",
      telefono: "",
    });
    setEditando(null);
  };

  const abrirNuevo = () => {
    limpiarFormulario();
    setMostrarFormulario(true);
  };

  const abrirEditar = (cliente) => {
    setFormulario({
      identificacion: cliente.identificacion,
      nombre: cliente.nombre,
      direccion: cliente.direccion,
      telefono: cliente.telefono,
    });
    setEditando(cliente.idCli);
    setMostrarFormulario(true);
  };

  const guardar = async (e) => {
    e.preventDefault();
    try {
      if (editando) {
        await actualizarCliente(editando, { ...formulario, activo: true });
      } else {
        await crearCliente(formulario);
      }
      setMostrarFormulario(false);
      limpiarFormulario();
      cargarClientes();
    } catch {
      alert("Error al guardar");
    }
  };

  const desactivar = async (id) => {
    if (!window.confirm("¿Desea desactivar este cliente?")) return;
    await eliminarCliente(id);
    cargarClientes();
  };

  const clientesFiltrados = clientes.filter((c) =>
    c.nombre.toLowerCase().includes(busqueda.toLowerCase())
  );
  
  const {
    page,
    totalPages,
    paginatedData,
    nextPage,
    prevPage,
    goToPage,
    setPage
  } = usePagination(clientesFiltrados, 10);

    useEffect(() => {
    setPage(1);
    }, [busqueda]);

  return (
    <div className="page-container">
      <div className="page-hero">
        <div>
          <h1>Clientes</h1>
          <p>
            Gestión de clientes registrados
          </p>
        </div>

        <button onClick={abrirNuevo}>
          Nuevo Cliente
        </button>
      </div>

      {mostrarFormulario && (
        <form onSubmit={guardar} className="cliente-form">
          <input
            placeholder="Identificación"
            value={formulario.identificacion}
            onChange={(e) =>
              setFormulario({ ...formulario, identificacion: e.target.value })
            }
            required
          />

          <input
            placeholder="Nombre"
            value={formulario.nombre}
            onChange={(e) =>
              setFormulario({ ...formulario, nombre: e.target.value })
            }
            required
          />

          <input
            placeholder="Dirección"
            value={formulario.direccion}
            onChange={(e) =>
              setFormulario({ ...formulario, direccion: e.target.value })
            }
          />

          <input
            placeholder="Teléfono"
            value={formulario.telefono}
            onChange={(e) =>
              setFormulario({ ...formulario, telefono: e.target.value })
            }
          />

          <div>
            <button type="submit">Guardar</button>
            <button type="button" onClick={() => setMostrarFormulario(false)}>
              Cancelar
            </button>
          </div>
        </form>
      )}

      <div className="search-container">
        <input
          type="text"
          placeholder="Buscar cliente..."
          value={busqueda}
          onChange={(e) => setBusqueda(e.target.value)}
        />
      </div>
      <div className="table-container">
        <div className="table-scroll">
          <table>
            <thead>
              <tr>
                <th>ID</th>
                <th>Identificación</th>
                <th>Nombre</th>
                <th>Dirección</th>
                <th>Teléfono</th>
                <th>Activo</th>
                <th>Acciones</th>
              </tr>
            </thead>

            <tbody>
              {paginatedData.map((cliente) => (
                <tr key={cliente.idCli}>
                  <td>{cliente.idCli}</td>
                  <td>{cliente.identificacion}</td>
                  <td>{cliente.nombre}</td>
                  <td>{cliente.direccion}</td>
                  <td>{cliente.telefono}</td>
                  <td>{cliente.activo ? "Sí" : "No"}</td>
                  <td>
                    <button onClick={() => abrirEditar(cliente)}>Editar</button>
                    <button onClick={() => desactivar(cliente.idCli)}>
                      Desactivar
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
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