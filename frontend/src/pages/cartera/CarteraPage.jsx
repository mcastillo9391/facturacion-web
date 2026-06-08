import {
  useEffect,
  useState
} from "react";

import { usePagination } from "../../hooks/usePagination";
import Pagination from "../../components/Pagination";

import {
  obtenerCartera
} from "../../services/carteraService";

export default function CarteraPage() {
  const [facturas, setFacturas] = useState([]);
  const [busqueda, setBusqueda] = useState("");

  const cargarDatos = async () => {
    const data = await obtenerCartera();
    setFacturas(data);
  };

  useEffect(() => {
    cargarDatos();
  }, []);

  const facturasFiltradas = facturas.filter(
    (f) =>
      f.cliente
        ?.toLowerCase()
        .includes(busqueda.toLowerCase()) ||
      f.facturaId
        ?.toString()
        .includes(busqueda)
  );

  const {
    page,
    totalPages,
    paginatedData,
    nextPage,
    prevPage,
    goToPage,
    setPage,
  } = usePagination(facturasFiltradas, 10);

  useEffect(() => {
    setPage(1);
  }, [busqueda, setPage]);

  const totalCartera = facturasFiltradas.reduce(
    (acc, item) => acc + Number(item.saldo || 0),
    0
  );

  return (
    <div className="page-container">

      {/* HERO */}
      <div className="page-hero">
        <div>
          <h1>💰 Cartera</h1>

          <p>
            Facturas pendientes por cobrar
          </p>
        </div>

        <div>
          <h2>
            $
            {totalCartera.toLocaleString()}
          </h2>
        </div>
      </div>

      {/* BUSCADOR */}
      <div className="search-container">
        <input
          type="text"
          placeholder="Buscar cliente o factura..."
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
                <th>Factura</th>
                <th>Cliente</th>
                <th>Fecha</th>
                <th>Valor</th>
                <th>Abonado</th>
                <th>Saldo</th>
                <th>Estado</th>
              </tr>
            </thead>

            <tbody>

              {paginatedData.length === 0 ? (
                <tr>
                  <td
                    colSpan="7"
                    style={{
                      textAlign: "center"
                    }}
                  >
                    No existen registros
                  </td>
                </tr>
              ) : (
                paginatedData.map((f) => (

                  <tr
                    key={f.facturaId}
                  >
                    <td>
                      {f.facturaId}
                    </td>

                    <td>
                      {f.cliente}
                    </td>

                    <td>
                      {new Date(
                        f.fechaFactura
                      ).toLocaleDateString()}
                    </td>

                    <td>
                      $
                      {Number(
                        f.valorFactura
                      ).toLocaleString()}
                    </td>

                    <td>
                      $
                      {Number(
                        f.valorAbonado
                      ).toLocaleString()}
                    </td>

                    <td>
                      $
                      {Number(
                        f.saldo
                      ).toLocaleString()}
                    </td>

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