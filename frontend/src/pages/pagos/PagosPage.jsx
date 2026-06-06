import {
  useEffect,
  useState
} from "react";

import { usePagination } from "../../hooks/usePagination";
import Pagination from "../../components/Pagination";

import {
  obtenerPagos,
  anularPago
} from "../../services/pagoService";

export default function PagosPage() {
  const [pagos, setPagos] =
    useState([]);
  
  const [busqueda, setBusqueda] = useState("");

  const cargarPagos =
    async () => {
      const data =
        await obtenerPagos();

      setPagos(data);
    };

  useEffect(() => {
    cargarPagos();
  }, []);

  const pagosFiltrados = pagos.filter(
    (p) =>
        p.cliente?.toLowerCase().includes(busqueda.toLowerCase()) ||
        p.idPago?.toString().includes(busqueda)
    );

    const {
        page,
        totalPages,
        paginatedData,
        nextPage,
        prevPage,
        goToPage,
        setPage
    } = usePagination(pagosFiltrados, 10);

    
    useEffect(() => {
    setPage(1);
    }, [busqueda]);
  const anular =
    async (idPago) => {
      const confirmar =
        window.confirm(
          "¿Desea anular este pago?"
        );

      if (!confirmar)
        return;

      try {
        await anularPago(idPago);

        alert(
          "Pago anulado correctamente"
        );

        cargarPagos();
      } catch (error) {
        alert(
          error?.response?.data?.message ||
          "No fue posible anular el pago"
        );
      }
    };

  return (
    <div className="page-container">
      <h2>
        Historial de Pagos
      </h2>
        <input
            type="text"
            placeholder="Buscar cliente o ID pago..."
            value={busqueda}
            onChange={(e) => setBusqueda(e.target.value)}
        />
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
          {paginatedData.map(
            (pago) => (
              <tr
                key={pago.idPago}
              >
                <td>
                  {pago.idPago}
                </td>

                <td>
                  {pago.cliente}
                </td>

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
                  {pago.anulado
                    ? "ANULADO"
                    : "ACTIVO"}
                </td>

                <td>
                  {!pago.anulado && (
                    <button
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