import { useEffect, useState } from "react";

import { usePagination } from "../../hooks/usePagination";
import Pagination from "../../components/Pagination";

import LoadingOverlay
  from "../../components/LoadingOverlay";

import {
  obtenerPagos,
  anularPago,
} from "../../services/pagoService";

export default function PagosPage() {
  const [pagos, setPagos] = useState([]);
  const [busqueda, setBusqueda] = useState("");

  const cargarPagos = async () => {
    const data = await obtenerPagos();
    setPagos(data);
  };

  useEffect(() => {
    cargarPagos();
  }, []);

  const pagosFiltrados = pagos.filter(
    (p) =>
      p.cliente
        ?.toLowerCase()
        .includes(busqueda.toLowerCase()) ||
      p.idPago?.toString().includes(busqueda)
  );

  const {
    page,
    totalPages,
    paginatedData,
    nextPage,
    prevPage,
    goToPage,
    setPage,
  } = usePagination(pagosFiltrados, 10);

  useEffect(() => {
    setPage(1);
  }, [busqueda, setPage]);
  
  const [Desactivando, setDesactivando] =
    useState(false);
  const anular = async (idPago) => {
    const confirmar = window.confirm(
      "¿Desea anular este pago?"
    );

    if (!confirmar) return;

    try {
      if (Desactivando) return;
      setDesactivando(true);

      await anularPago(idPago);

      alert("Pago anulado correctamente");

      cargarPagos();
    } catch (error) {
      alert(
        error?.response?.data?.message ||
          "No fue posible anular el pago"
      );
    } finally {

      setDesactivando(false);

    }
  };

  const totalPagos = pagosFiltrados
    .filter((p) => !p.anulado)
    .reduce(
      (acum, pago) =>
        acum + Number(pago.valorPago || 0),
      0
    );

  return (
    <div className="page-container">
      
      {Desactivando && (
        <LoadingOverlay
          mensaje="Anulando pago..."
        />
      )}
      
      {/* HERO */}
      <div className="page-hero">
        <div>
          <h1>💳 Historial de Pagos</h1>
          <p>
            Consulta y administración de pagos
            registrados
          </p>
        </div>

        <div>
          <h2>
            $
            {totalPagos.toLocaleString()}
          </h2>
        </div>
      </div>

      {/* BUSCADOR */}
      <div className="search-container">
        <input
          type="text"
          placeholder="Buscar cliente o ID de pago..."
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
                <th>ID</th>
                <th>Cliente</th>
                <th>Fecha</th>
                <th>Valor</th>
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
                      textAlign: "center",
                    }}
                  >
                    No existen pagos registrados
                  </td>
                </tr>
              ) : (
                paginatedData.map((pago) => (
                  <tr key={pago.idPago}>
                    <td>{pago.idPago}</td>

                    <td>{pago.cliente}</td>

                    <td>
                      {new Date(
                        pago.fechaPago
                      ).toLocaleString()}
                    </td>

                    <td>
                      $
                      {Number(
                        pago.valorPago
                      ).toLocaleString()}
                    </td>

                    <td>
                      <span
                        className={`badge ${
                          pago.anulado
                            ? "danger"
                            : "success"
                        }`}
                      >
                        {pago.anulado
                          ? "ANULADO"
                          : "ACTIVO"}
                      </span>
                    </td>

                    <td>
                      {!pago.anulado && (
                        <button
                          className="btn-danger"
                          onClick={() =>
                            anular(
                              pago.idPago
                            )
                          }
                        >
                          Anular
                        </button>
                      )}
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
    </div>
  );
}