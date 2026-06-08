import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import {
  obtenerFacturas,
  obtenerFactura
} from "../../services/facturaService";

import { usePagination } from "../../hooks/usePagination";
import Pagination from "../../components/Pagination";

export default function FacturasPage() {
  const [facturas, setFacturas] = useState([]);
  const [facturaDetalle, setFacturaDetalle] = useState(null);
  const [busqueda, setBusqueda] = useState("");

  const navigate = useNavigate();

  // =========================
  // CARGA DE DATOS
  // =========================
  const cargarFacturas = async () => {
    const data = await obtenerFacturas();
    setFacturas(data);
  };

  useEffect(() => {
    cargarFacturas();
  }, []);

  // =========================
  // DETALLE FACTURA
  // =========================
  const verDetalle = async (codigo) => {
    const data = await obtenerFactura(codigo);
    setFacturaDetalle(data);
  };

  // =========================
  // FILTRO GLOBAL
  // =========================
  const facturasFiltradas = facturas.filter((f) =>
    `${f.codigo} ${f.cliente}`
      .toLowerCase()
      .includes(busqueda.toLowerCase())
  );

  const totalFacturas = facturas.length;

  const totalPagadas =
    facturas.filter(
      f => f.estado === "PAGADA"
    ).length;

  const totalPendientes =
    facturas.filter(
      f => f.estado !== "PAGADA"
    ).length;

  const carteraTotal =
    facturas.reduce(
      (acc, f) => acc + Number(f.saldo || 0),
      0
    );
  // =========================
  // PAGINACIÓN
  // =========================
  const {
    page,
    totalPages,
    paginatedData,
    nextPage,
    prevPage,
    goToPage,
    setPage
  } = usePagination(facturasFiltradas, 8);

    useEffect(() => {
    setPage(1);
    }, [busqueda]);

  return (
    <div className="facturas-container">

      {/* HEADER */}
      <div className="page-hero">
        <div>
          <h1>Facturas</h1>
          <p>
            Consulta y gestión de facturación
          </p>
        </div>
      </div>

      <div className="cards-grid">

      <div className="card">
        <h3>Total Facturas</h3>
        <p>{totalFacturas}</p>
      </div>

      <div className="card">
        <h3>Facturas Pagadas</h3>
        <p>{totalPagadas}</p>
      </div>

      <div className="card">
        <h3>Facturas Pendientes</h3>
        <p>{totalPendientes}</p>
      </div>

      <div className="card">
        <h3>Cartera</h3>
        <p>
          $
          {carteraTotal.toLocaleString()}
        </p>
      </div>

    </div>

      {/* BUSCADOR */}
      <div
        style={{
          marginBottom: "20px"
        }}
      >
          <input
            className="search-input"
            type="text"
            placeholder="Buscar factura o cliente..."
            value={busqueda}
            onChange={(e) =>
              setBusqueda(e.target.value)
            }
          />
      </div>
      
      {/* TABLA */}
      <div className="table-container">
        <div className="table-scroll">  
          <table>
            <thead>
              <tr>
                <th>Código</th>
                <th>Cliente</th>
                <th>Fecha</th>
                <th>Valor</th>
                <th>Abonado</th>
                <th>Saldo</th>
                <th>Estado</th>
                <th>Acciones</th>
              </tr>
            </thead>

            <tbody>
              {paginatedData.length === 0 ? (
                <tr>
                  <td colSpan="8" style={{ textAlign: "center" }}>
                    No hay facturas
                  </td>
                </tr>
              ) : (
                paginatedData.map((f) => (
                  <tr key={f.codigo}>
                    <td>{f.codigo}</td>
                    <td>{f.cliente}</td>
                    <td>
                      {new Date(f.fechaGen).toLocaleDateString()}
                    </td>
                    <td>${f.valorFactura.toLocaleString()}</td>
                    <td>${f.valorAbonado.toLocaleString()}</td>
                    <td>${f.saldo.toLocaleString()}</td>

                    <td>
                      <span
                        className={`badge ${
                          f.estado === "PAGADA"
                            ? "success"
                            : "warning"
                        }`}
                      >
                        {f.estado}
                      </span>
                    </td>

                    <td>
                      <div className="actions">

                        <button
                          className="btn-view"
                          onClick={() => verDetalle(f.codigo)}
                        >
                          Ver
                        </button>

                        <button
                          className="btn-success"
                          onClick={() =>
                            navigate(`/facturas/${f.codigo}`)
                          }
                        >
                          Abrir
                        </button>

                      </div>
                    </td>
                  </tr>
                ))
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

      {/* DETALLE */}
      {facturaDetalle && (
        <div className="detail-card">

          <h3>Factura #{facturaDetalle.codigo}</h3>

          <div className="cards-grid">

            <div className="card">
              <h3>Cliente</h3>
              <p>{facturaDetalle.cliente}</p>
            </div>

            <div className="card">
              <h3>Estado</h3>
              <p>{facturaDetalle.estado}</p>
            </div>

            <div className="card">
              <h3>Valor</h3>
              <p>
                $
                {facturaDetalle.valorFactura.toLocaleString()}
              </p>
            </div>

            <div className="card">
              <h3>Saldo</h3>
              <p>
                $
                {facturaDetalle.saldo.toLocaleString()}
              </p>
            </div>

          </div>

          {/* DETALLE ITEMS */}
          <table>
            <thead>
              <tr>
                <th>Producto</th>
                <th>Cantidad</th>
                <th>Valor</th>
                <th>Descuento</th>
                <th>Total</th>
              </tr>
            </thead>

            <tbody>
              {facturaDetalle.detalles.map((d) => (
                <tr key={d.codigo}>
                  <td>{d.producto}</td>
                  <td>{d.cantidad}</td>
                  <td>${d.valorUnitario.toLocaleString()}</td>
                  <td>${d.descuento.toLocaleString()}</td>
                  <td>${d.total.toLocaleString()}</td>
                </tr>
              ))}
            </tbody>
          </table>

          <br />

          <button
            className="btn-danger"
            onClick={() => setFacturaDetalle(null)}
          >
            Cerrar
          </button>

        </div>
      )}

    </div>
  );
}