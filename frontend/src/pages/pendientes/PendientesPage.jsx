import { useEffect, useState } from "react";
import { obtenerClientes } from "../../services/clienteService";
import { obtenerProductos } from "../../services/productoService";
import { obtenerUsuarios } from "../../services/usuarioService";
import { usePagination } from "../../hooks/usePagination";
import Pagination from "../../components/Pagination";
import LoadingOverlay
  from "../../components/LoadingOverlay";

import {
  obtenerPendientes,
  crearPendiente,
  obtenerPendiente,
  agregarProductoPendiente,
  eliminarProductoPendiente,
  cancelarPendiente,
} from "../../services/pendienteVentaService";
import { generarFactura } from "../../services/facturaService";

export default function PendientesPage() {
  const [clientes, setClientes] = useState([]);
  const [productos, setProductos] = useState([]);
  const [usuarios, setUsuarios] = useState([]);
  const [usuarioAsig, setUsuarioAsig] = useState("");
  const [pendientes, setPendientes] = useState([]);
  const ESTADOS = ["TODOS", "ABIERTO", "FACTURADA", "CANCELADA"];

  const [estadoFiltro, setEstadoFiltro] = useState("TODOS");

  const [clienteId, setClienteId] = useState("");
  const [pendienteId, setPendienteId] = useState(null);
  const [pendiente, setPendiente] = useState(null);

  const [productoId, setProductoId] = useState("");
  const [cantidad, setCantidad] = useState(1);
  const [descuento, setDescuento] = useState(0);

  useEffect(() => {
    cargarDatos();
  }, []);

  const cargarDatos = async () => {
    const clientesData = await obtenerClientes();
    const productosData = await obtenerProductos();
    const pendientesData = await obtenerPendientes();
    const usuariosData = await obtenerUsuarios();

    setClientes(
      clientesData.filter((c) => c.activo)
    );

    setProductos(
      productosData.filter((p) => p.activo)
    );

    setUsuarios(
      usuariosData.filter((u) => u.activo)
    );

    setPendientes(pendientesData);
  };

  const cargarPendiente = async (id) => {
    const data = await obtenerPendiente(id);
    setPendiente(data);
    setPendienteId(id);
  };

  const contarPorEstado = (estado) => {
    if (estado === "TODOS") return pendientes.length;

    return pendientes.filter(
        (p) => p.estado?.toString().trim().toUpperCase() === estado
    ).length;
    };

    const normalizar = (texto) =>
    texto?.toString().trim().toUpperCase();

    const pendientesFiltrados = pendientes.filter((p) => {
    if (estadoFiltro === "TODOS") return true;

    return normalizar(p.estado) === estadoFiltro;
    });

  const {
    page,
    totalPages,
    paginatedData,
    nextPage,
    prevPage,
    goToPage,
    setPage
  } = usePagination(pendientesFiltrados, 10);

    useEffect(() => {
    setPage(1);
    }, [estadoFiltro]);

  const crearNuevoPendiente = async () => {
    if (!clienteId) return alert("Seleccione un cliente");

    const result = await crearPendiente({
      clienteId: parseInt(clienteId),
    });

    await cargarPendiente(result.pendienteVentaId);
    await cargarDatos();
  };

  const [Agregando, setAgregando] =
  useState(false);
  const agregarProducto = async () => {
    if (!productoId) return alert("Seleccione un producto");
      if (Agregando) return;
      try {
            setAgregando(true);
            await agregarProductoPendiente(pendienteId, {
                productoId: parseInt(productoId),
                cantidad: parseInt(cantidad),
                descuento: parseFloat(descuento),
            });

            await cargarPendiente(pendienteId);
            await cargarDatos();

            setProductoId("");
            setCantidad(1);
            setDescuento(0);
    } catch (error) {
        const data = error.response?.data;

            const mensaje = data
                ?.split("\n")[0]
                ?.replace("System.Exception:", "")
                ?.trim();

            alert(mensaje || "Ocurrió un error"); 
    } finally {

      setAgregando(false);

    }
  };

  const eliminarProducto = async (detalleId) => {
    if (!window.confirm("¿Eliminar producto?")) return;
        try {
            await eliminarProductoPendiente(detalleId);

            await cargarPendiente(pendienteId);
            await cargarDatos();

            alert("Producto eliminado correctamente");
        } catch (error) {
            const data = error.response?.data;

            const mensaje = data
                ?.split("\n")[0]
                ?.replace("System.Exception:", "")
                ?.trim();

            alert(mensaje || "Ocurrió un error");
        }
    };

  const [facturando, setFacturando] =
  useState(false);

  //window.confirm("¿Generar factura?")
  const facturar = async (id) => {
    if (!window.confirm("¿Generar factura? \n\nVerifique que el pedido esté completo y el usuario esté asignado antes de facturar.")) {
      return;
    }
    if (facturando) return;
    try {
      setFacturando(true);
      const resultado =
        await generarFactura(
          id,
          {
            usuarioAsig:
              usuarioAsig !== "" && usuarioAsig != null
                ? parseInt(usuarioAsig)
                : null
          }
        );

      alert(
        `Factura #${resultado.facturaId} generada`
      );

      setUsuarioAsig("");

      setPendiente(null);
      setPendienteId(null);

      await cargarDatos();

    } catch (error) {

      const mensaje =
        error?.response?.data
          ?.split("\n")[0]
          ?.replace("System.Exception:", "")
          ?.trim();

      alert(
        mensaje ||
        "No fue posible generar la factura"
      );
    } finally {

      setFacturando(false);

    }
  };

  const [cancelando, setCancelando] =
  useState(false);

  const cancelar = async (id) => {
    if (!window.confirm("¿Cancelar pendiente?")) return;
    if (facturando) return;
    try{
      setCancelando(true);
      
      await cancelarPendiente(id);

      if (pendienteId === id) {
        setPendiente(null);
        setPendienteId(null);
      }

      await cargarDatos();
    } catch (error) {

      const mensaje =
        error?.response?.data
          ?.split("\n")[0]
          ?.replace("System.Exception:", "")
          ?.trim();

      alert(
        mensaje ||
        "No fue posible cancelar pendiente"
      );
    } finally {

      setCancelando(false);

    }
  };

  return (
    <div className="page-container">
      {facturando && (
        <LoadingOverlay
          mensaje="Generando factura..."
        />
      )}
      {Agregando && (
        <LoadingOverlay
          mensaje="Agregando producto..."
        />
      )}
      {cancelando && (
        <LoadingOverlay
          mensaje="Cancelando pendiente..."
        />
      )}
      <div className="page-hero">
        <div>
          <h1>Pendientes de Venta</h1>
          <p>
            Gestión de pedidos y facturación
          </p>
        </div>
      </div>

      <div style={{ marginBottom: 20 }}>
        <select
          value={clienteId}
          onChange={(e) => setClienteId(e.target.value)}
        >
          <option value="">Seleccione cliente</option>
          {clientes.map((c) => (
            <option key={c.idCli} value={c.idCli}>
              {c.nombre}
            </option>
          ))}
        </select>

        <button onClick={crearNuevoPendiente}>Nuevo Pendiente</button>
      </div>

      <h4>Filtrar por estado:</h4>
          <div className="status-grid">
            {ESTADOS.map((estado) => {

              const activo =
                estadoFiltro === estado;

              return (
                <div
                  key={estado}
                  className={`status-card ${
                    activo ? "active" : ""
                  }`}
                  onClick={() => {
                    setEstadoFiltro(estado);
                    setPage(1);
                  }}
                >
                  <h4>{estado}</h4>

                  <span>
                    {contarPorEstado(estado)}
                  </span>
                </div>
              );
            })}
          </div>
        <div className="table-container">
          <div className="table-scroll">
            <table>
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Cliente</th>
                  <th>Estado</th>
                  <th>Total</th>
                  <th>Acciones</th>
                </tr>
              </thead>

              <tbody>
                {paginatedData.map((p) => (
                  <tr key={p.pendienteVentaId}>
                    <td>{p.pendienteVentaId}</td>
                    <td>{p.cliente}</td>
                    <td>{p.estado}</td>
                    <td>{p.total}</td>
                    <td>
                      {p.estado?.toString().trim().toUpperCase() === "ABIERTO" ? (
                          <>
                          <button onClick={() => cargarPendiente(p.pendienteVentaId)}>
                              Editar
                          </button>

                          <button
                            disabled={facturando}
                            onClick={() =>
                              facturar(p.pendienteVentaId)
                            }
                          >
                            {
                              facturando
                                ? "Facturando..."
                                : "Facturar"
                            }
                          </button>

                          <button
                            disabled={cancelando}
                            onClick={() =>
                              cancelar(p.pendienteVentaId)
                            }
                          >
                            {
                              cancelando
                                ? "Cancelando..."
                                : "Cancelar"
                            }
                          </button>
                          </>
                      ) : (
                          <button onClick={() => cargarPendiente(p.pendienteVentaId)}>
                          Ver
                          </button>
                      )}
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
      {pendiente && (
        <div className="detail-card">
          <h3>Pendiente #{pendiente.pendienteVentaId}</h3>

          <h4>Cliente: {pendiente.cliente}</h4>
          <div
            style={{
              display: "grid",
              gridTemplateColumns: "1fr",
              gap: "10px",
              marginBottom: "20px"
            }}
          >
            <label>
              Asignar factura a:
            </label>

            <select
              value={usuarioAsig}
              onChange={(e) =>
                setUsuarioAsig(
                  e.target.value
                )
              }
            >
              <option value="">
                Usuario que factura
              </option>

              {usuarios.map((u) => (
                <option
                  key={u.usuarioId}
                  value={u.usuarioId}
                >
                  {u.nombre}
                </option>
              ))}
            </select>
          </div>
          
          <div
            style={{
              display: "grid",
              gridTemplateColumns:
                "2fr 1fr 1fr auto",
              gap: "12px",
              marginBottom: "20px"
            }}
          >
            <select
              value={productoId}
              onChange={(e) => setProductoId(e.target.value)}
            >
              <option value="">Producto</option>
              {productos.map((p) => (
                <option key={p.idProducto} value={p.idProducto}>
                  {p.nombre}
                </option>
              ))}
            </select>

            <input
              type="number"
              value={cantidad}
              onChange={(e) => setCantidad(e.target.value)}
            />

            <input
              type="number"
              value={descuento}
              onChange={(e) => setDescuento(e.target.value)}
            />

            <button onClick={agregarProducto}>Agregar Producto</button>
          </div>

          <table>
            <thead>
              <tr>
                <th>Producto</th>
                <th>Cantidad</th>
                <th>Valor</th>
                <th>Descuento</th>
                <th>Total</th>
                <th>Acción</th>
              </tr>
            </thead>

            <tbody>
              {pendiente.detalles.map((d) => (
                <tr key={d.detallePendienteVentaId}>
                  <td>{d.producto}</td>
                  <td>{d.cantidad}</td>
                  <td>{d.valorUnitario}</td>
                  <td>{d.descuento}</td>
                  <td>{d.total}</td>
                  <td>
                    <button
                      onClick={() =>
                        eliminarProducto(d.detallePendienteVentaId)
                      }
                    >
                      Eliminar
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

          <h3>Total: {pendiente.total}</h3>
        </div>
      )}
    </div>
  );
}