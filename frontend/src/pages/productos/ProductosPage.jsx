import {
useEffect,
useState
} from "react";

import { usePagination } from "../../hooks/usePagination";
import Pagination from "../../components/Pagination";

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

const guardar =
async (e) => {
e.preventDefault();

  try {

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
  }
};

const desactivar =
async (id) => {

  if (
    !window.confirm(
      "¿Desea desactivar este producto?"
    )
  ) {
    return;
  }

  await eliminarProducto(id);

  cargarProductos();
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

  <div className="page-header">

    <h2>
      Productos
    </h2>

    <button
      onClick={abrirNuevo}
    >
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
