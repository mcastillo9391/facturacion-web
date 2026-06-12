import {
useEffect,
useState
} from "react";

import { usePagination } from "../../hooks/usePagination";
import Pagination from "../../components/Pagination";

import LoadingOverlay
  from "../../components/LoadingOverlay";

import {
obtenerProductos,
crearProducto,
actualizarProducto,
eliminarProducto
} from "../../services/productoService";

export default function ProductosPage() {
const [productos,
setProductos] =
useState([]);

const [busqueda,
setBusqueda] =
useState("");

const [mostrarFormulario,
setMostrarFormulario] =
useState(false);

const [editando,
setEditando] =
useState(null);

const [formulario,
setFormulario] =
useState({
nombre: "",
valorCosto: "",
valorVenta: ""
});

const cargarProductos =
async () => {
const data =
await obtenerProductos();

  setProductos(data);
};

useEffect(() => {
cargarProductos();
}, []);

const limpiarFormulario =
() => {
setFormulario({
nombre: "",
valorCosto: "",
valorVenta: ""
});

  setEditando(null);
};

const abrirNuevo =
() => {
limpiarFormulario();
setMostrarFormulario(true);
};

const abrirEditar =
(producto) => {
setFormulario({
nombre:
producto.nombre,
valorCosto:
producto.valorCosto,
valorVenta:
producto.valorVenta
});

  setEditando(
    producto.idProducto
  );

  setMostrarFormulario(true);
};

const [Agregando, setAgregando] =
  useState(false);
const guardar =
async (e) => {
  e.preventDefault();

  try {
    if (Agregando) return;
    setAgregando(true);
    if (editando) {

      await actualizarProducto(
        editando,
        {
          ...formulario,
          activo: true
        }
      );

    } else {

      await crearProducto(
        formulario
      );

    }

    setMostrarFormulario(
      false
    );

    limpiarFormulario();

    cargarProductos();

  } catch {
    alert(
      "Error al guardar producto"
    );
  } finally {

    setAgregando(false);

  }
};

const [Desactivando, setDesactivando] =
  useState(false);
const desactivar =
async (id) => {
  try{

    if (Desactivando) return;
    setDesactivando(true);
    if (
      !window.confirm(
        "¿Desea desactivar este producto?"
      )
    ) {
      return;
    }

    await eliminarProducto(id);

    cargarProductos();
  } catch {
    alert(
      "Error desactivando el producto"
    );
  } finally {

    setDesactivando(false);

  }
};

const productosFiltrados =
productos.filter((p) =>
p.nombre
.toLowerCase()
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
} = usePagination(productosFiltrados, 10);

return ( <div className="page-container">

  {Desactivando && (
    <LoadingOverlay
      mensaje="Desactivando producto..."
    />
  )}
  {Agregando && (
    <LoadingOverlay
      mensaje="Agregando producto..."
    />
  )}
  <div className="page-hero">
    <div>
      <h1>Productos</h1>
      <p>
        Catálogo de productos
      </p>
    </div>

    <button onClick={abrirNuevo}>
      Nuevo Producto
    </button>
  </div>

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
              e.target.value
          })
        }
        required
      />

      <input
        type="number"
        step="0.01"
        placeholder="Costo"
        value={
          formulario.valorCosto
        }
        onChange={(e) =>
          setFormulario({
            ...formulario,
            valorCosto:
              e.target.value
          })
        }
        required
      />

      <input
        type="number"
        step="0.01"
        placeholder="Venta"
        value={
          formulario.valorVenta
        }
        onChange={(e) =>
          setFormulario({
            ...formulario,
            valorVenta:
              e.target.value
          })
        }
        required
      />

      <div>

        <button
          type="submit"
        >
          Guardar
        </button>

        <button
          type="button"
          onClick={() =>
            setMostrarFormulario(
              false
            )
          }
        >
          Cancelar
        </button>

      </div>

    </form>

  )}

  <div className="search-container">
    <input
      type="text"
      placeholder="Buscar producto..."
      value={busqueda}
      onChange={(e) =>
        setBusqueda(
          e.target.value
        )
      }
    />
  </div>
  <div className="table-container">
    <div className="table-scroll">
      <table>

        <thead>

          <tr>
            <th>ID</th>
            <th>Nombre</th>
            <th>Costo</th>
            <th>Venta</th>
            <th>Activo</th>
            <th>Acciones</th>
          </tr>

        </thead>

        <tbody>

          {paginatedData.map(
            (producto) => (

              <tr
                key={
                  producto.idProducto
                }
              >

                <td>
                  {
                    producto.idProducto
                  }
                </td>

                <td>
                  {
                    producto.nombre
                  }
                </td>

                <td>
                  $
                  {Number(
                    producto.valorCosto
                  ).toLocaleString()}
                </td>

                <td>
                  $
                  {Number(
                    producto.valorVenta
                  ).toLocaleString()}
                </td>

                <td>
                  {producto.activo
                    ? "Sí"
                    : "No"}
                </td>

                <td>

                  <button
                    onClick={() =>
                      abrirEditar(
                        producto
                      )
                    }
                  >
                    Editar
                  </button>

                  <button
                    onClick={() =>
                      desactivar(
                        producto.idProducto
                      )
                    }
                  >
                    Desactivar
                  </button>

                </td>

              </tr>

            )
          )}

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
